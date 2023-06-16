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

      /** A drag event is in progress. DragStart has been invoked. */
      InProgress,
    }
    DragState _dragState = DragState.None;
    Vector2 _dragStartPosition;
    double _dragStartTime;
    bool _currentDragMetThreshold = false;
    /**
     * Minimum world distance dragged when camera orthographic size is 5.
     *
     * Drag events actually happen even if this threshold isn't met. This threshold exists for the
     * purposes of preventing tap events on fast drags.
     */
    [FormerlySerializedAs("_dragThresholdLength")]
    [SerializeField] float _dragThresholdLengthAtCameraSize5 = 1f / 8f;

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
        _currentDragMetThreshold = false;
      }
      else if (context.phase == InputActionPhase.Canceled) {
        _mtdInput.DragEnd.Invoke(_touch0Position);
        _dragState = DragState.None;

        if (!_currentDragMetThreshold) {
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
        if (!_currentDragMetThreshold) {
          CheckCurrentDragMetThreshold(nextTouch0Position);
        }
        _mtdInput.Drag.Invoke(nextTouch0Position);
      }
      else if (_dragState == DragState.TouchStart) {
        _dragStartPosition = nextTouch0Position;
        _mtdInput.DragStart.Invoke(_dragStartPosition);
        _dragState = DragState.InProgress;
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

    void CheckCurrentDragMetThreshold(Vector2 nextTouch0Position) {
      var dragStartPoint = ScreenAndWorldPoint.FromScreenPoint(_camera, _dragStartPosition);
      var currentTouchPoint = ScreenAndWorldPoint.FromScreenPoint(_camera, nextTouch0Position);

      var dragThresholdLength = (_camera.orthographicSize / 5f) * _dragThresholdLengthAtCameraSize5;
      var dragLength = (currentTouchPoint.World - dragStartPoint.World).magnitude;

      if (dragLength >= dragThresholdLength) {
        _currentDragMetThreshold = true;
      }
    }
  }
}
