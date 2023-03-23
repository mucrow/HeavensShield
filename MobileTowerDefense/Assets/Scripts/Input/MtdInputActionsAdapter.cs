using System;
using System.Collections;
using System.Collections.Generic;
using Mtd.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Mtd {
  /** This class takes care of input processing that the new input system can't do on its own. */
  [RequireComponent(typeof(MtdInput))]
  public class MtdInputActionsAdapter: MonoBehaviour, MtdInputActions.IMainActions {
    Camera _camera;
    MtdInput _mtdInput;
    MtdInputActions _mtdInputActions;

    enum DragState {
      None,

      /**
       * A touch input was pressed but not yet released. This is a transient state that exists so
       * we can poll the drag start position the next time Touch0Position is fired.
       */
      TouchStart,

      /**
       * A touch input was pressed and not yet released, AND the drag start position has been set.
       * The high-level DragStart event has not yet been invoked for MTD components because the
       * player has not dragged their finger very far (yet).
       *
       * Consumers of the DragStart & Drag events should compute the delta between the DragStart
       * position and Drag event positions in order to compensate for the time delay that the
       * BelowThreshold state causes.
       */
      BelowThreshold,

      /** A drag event is in progress. DragStart has been invoked. */
      InProgress,
    }
    DragState _dragState = DragState.None;
    Vector2 _dragStartPosition;
    double _dragStartTime;
    /** Minimum world distance dragged when camera orthographic size is 5. */
    [FormerlySerializedAs("_dragThresholdLength")]
    [SerializeField] float _dragThresholdLengthAtCameraSize5 = 0.25f;

    enum PinchZoomState { None, Starting, InProgress }
    PinchZoomState _pinchZoomState = PinchZoomState.None;
    Vector2 _pinchZoomPreviousPositionDelta;
    Vector2 _touch0Position;
    Vector2 _touch1Position;

    [SerializeField] float _zoomScaling = 0.0015f;
    [SerializeField] float _scrollZoomScaling = 1f;
    [SerializeField] float _pinchZoomScaling = 1f;

    [SerializeField] double _tapPressReleaseTimeLimit = 0.4;

    void Start() {
      _camera = Camera.main;
    }

    public void StartInputActionsAdapter(MtdInput mtdInput) {
      _mtdInput = mtdInput;
      _mtdInputActions = new MtdInputActions();
      _mtdInputActions.Main.SetCallbacks(this);
      _mtdInputActions.Main.Enable();
    }

    void OnZoom(float amount) {
      _mtdInput.Zoom.Invoke(amount * _zoomScaling);
    }

    public void OnMouseScrollZoom(InputAction.CallbackContext context) {
      var value = context.ReadValue<Vector2>();
      OnZoom(value.y * _scrollZoomScaling);
    }

    void OnPinchZoom(float fingerDistanceDelta) {
      OnZoom(fingerDistanceDelta * _pinchZoomScaling);
    }

    public void OnTouch0Press(InputAction.CallbackContext context) {
      if (context.phase == InputActionPhase.Started) {
        _dragState = DragState.TouchStart;
        _dragStartTime = Time.realtimeSinceStartupAsDouble;
      }
      else if (context.phase == InputActionPhase.Canceled) {
        if (_dragState == DragState.InProgress) {
          _mtdInput.DragEnd.Invoke(_touch0Position);
          _dragState = DragState.None;
        }
        else {
          // it's important that the below code lives in this else block - we only want a tap event
          // if a drag event was never started.

          // TODO rename these "dragFoo" variables so they more generically cover both drag and tap
          double dragEndTime = Time.realtimeSinceStartupAsDouble;
          double dragTime = dragEndTime - _dragStartTime;
          if (dragTime < _tapPressReleaseTimeLimit) {
            var screenAndWorldPoint = ScreenAndWorldPoint.FromScreenPoint(_camera, _touch0Position);
            _mtdInput.Tap.Invoke(screenAndWorldPoint);
          }
        }
      }
    }

    /** Primary touch position OR mouse position. */
    public void OnTouch0Position(InputAction.CallbackContext context) {
      var nextTouch0Position = context.ReadValue<Vector2>();
      if (_dragState == DragState.InProgress) {
        _mtdInput.Drag.Invoke(nextTouch0Position);
      }
      else if (_dragState == DragState.BelowThreshold) {
        var dragStartPoint = ScreenAndWorldPoint.FromScreenPoint(_camera, _dragStartPosition);
        var currentTouchPoint = ScreenAndWorldPoint.FromScreenPoint(_camera, nextTouch0Position);

        var dragThresholdLength = (5f / _camera.orthographicSize) * _dragThresholdLengthAtCameraSize5;
        var dragLength = (currentTouchPoint.World - dragStartPoint.World).magnitude;

        if (dragLength >= dragThresholdLength) {
          _mtdInput.DragStart.Invoke(_dragStartPosition);
          _dragState = DragState.InProgress;
        }
      }
      else if (_dragState == DragState.TouchStart) {
        _dragStartPosition = nextTouch0Position;
        _dragState = DragState.BelowThreshold;
      }
      _touch0Position = nextTouch0Position;
    }

    public void OnTouch1Press(InputAction.CallbackContext context) {
      if (context.phase == InputActionPhase.Canceled) {
        _pinchZoomState = PinchZoomState.None;
      }
    }

    public void OnTouch1Position(InputAction.CallbackContext context) {
      _touch1Position = context.ReadValue<Vector2>();
      if (_pinchZoomState == PinchZoomState.InProgress) {
        var currentPositionDelta = _touch1Position - _touch0Position;
        var fingerDistanceDelta = currentPositionDelta.magnitude - _pinchZoomPreviousPositionDelta.magnitude;
        OnPinchZoom(fingerDistanceDelta);
        _pinchZoomPreviousPositionDelta = currentPositionDelta;
      }
      else if (_pinchZoomState == PinchZoomState.Starting) {
        _pinchZoomPreviousPositionDelta = _touch1Position - _touch0Position;
        _pinchZoomState = PinchZoomState.InProgress;
      }
      else {
        _pinchZoomState = PinchZoomState.Starting;
      }
    }
  }
}
