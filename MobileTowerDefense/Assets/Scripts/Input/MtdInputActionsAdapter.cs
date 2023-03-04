using System;
using System.Collections;
using System.Collections.Generic;
using Mtd.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mtd {
  /** This class takes care of input processing that the new input system can't do on its own. */
  [RequireComponent(typeof(MtdInput))]
  public class MtdInputActionsAdapter: MonoBehaviour, MtdInputActions.IMainActions {
    Camera _camera;
    MtdInput _mtdInput;
    MtdInputActions _mtdInputActions;

    enum DragState { None, Starting, InProgress }
    DragState _dragState = DragState.None;
    Vector2 _dragPreviousPosition;

    enum PinchZoomState { None, Starting, InProgress }
    PinchZoomState _pinchZoomState = PinchZoomState.None;
    Vector2 _pinchZoomPreviousPositionDelta;
    Vector2 _touch0Position;
    Vector2 _touch1Position;

    [SerializeField] float _zoomScaling = 0.0015f;
    [SerializeField] float _scrollZoomScaling = 1f;
    [SerializeField] float _pinchZoomScaling = 1f;

    void Start() {
      _camera = Camera.main;
    }

    public void StartInputActionsAdapter(MtdInput mtdInput) {
      _mtdInput = mtdInput;
      _mtdInputActions = new MtdInputActions();
      _mtdInputActions.Main.SetCallbacks(this);
      _mtdInputActions.Main.Enable();
    }

    public void OnTap(InputAction.CallbackContext context) {
      if (context.phase == InputActionPhase.Performed) {
        var point = ScreenAndWorldPoint.FromScreenPoint(_camera, _touch0Position);
        _mtdInput.Tap.Invoke(point);
      }
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

    /** Primary touch position OR mouse position. */
    public void OnTouch0Position(InputAction.CallbackContext context) {
      var nextTouch0Position = context.ReadValue<Vector2>();
      if (_dragState == DragState.InProgress) {
        _mtdInput.Drag.Invoke(nextTouch0Position);
      }
      else if (_dragState == DragState.Starting) {
        _mtdInput.DragStart.Invoke(nextTouch0Position);
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

    /** this is different from OnTap because OnTap is bound to tap (quicker than press) */
    public void OnDragPress(InputAction.CallbackContext context) {
      if (context.phase == InputActionPhase.Started) {
        _dragState = DragState.Starting;
      }
      else if (context.phase == InputActionPhase.Canceled) {
        _mtdInput.DragEnd.Invoke(_touch0Position);
        _dragState = DragState.None;
      }
    }
  }
}
