using System;
using System.Collections;
using System.Collections.Generic;
using Mtd.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mtd {
  /**
   * This class takes care of input processing that the new input system can't do on its own.
   *
   * If pinch zoom breaks, it might be because of one of these things:
   * - Touch1Press was moved below Touch1Position in the Zoom action map
   * - execution order of action maps is not determined by ordering in action map editor
   */
  [RequireComponent(typeof(MtdInput))]
  public class MtdInputActionsAdapter: MonoBehaviour, MtdInputActions.IMainActions {
    Camera _camera;
    MtdInput _mtdInput;
    MtdInputActions _mtdInputActions;

    bool _dragInitialized = false;
    Vector2 _dragPreviousPosition;

    bool _pinchZoomInitialized = false;
    Vector2 _pinchZoomPreviousPositionDelta;
    Vector2 _touch0Position;
    Vector2 _touch1Position;

    [SerializeField] float _zoomScaling = 0.003f;
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
      if (_dragInitialized) {
        _mtdInput.Drag.Invoke(nextTouch0Position);
      }
      _touch0Position = nextTouch0Position;
    }

    public void OnTouch1Press(InputAction.CallbackContext context) {
      if (context.phase == InputActionPhase.Canceled) {
        _pinchZoomInitialized = false;
      }
    }

    public void OnTouch1Position(InputAction.CallbackContext context) {
      _touch1Position = context.ReadValue<Vector2>();
      if (!_pinchZoomInitialized) {
        _pinchZoomPreviousPositionDelta = _touch1Position - _touch0Position;
        _pinchZoomInitialized = true;
        return;
      }
      var currentPositionDelta = _touch1Position - _touch0Position;
      var fingerDistanceDelta = currentPositionDelta.magnitude - _pinchZoomPreviousPositionDelta.magnitude;
      OnPinchZoom(fingerDistanceDelta);
      _pinchZoomPreviousPositionDelta = currentPositionDelta;
    }

    /** this is different from OnTap because OnTap is bound to tap (quicker than press) */
    public void OnDragPress(InputAction.CallbackContext context) {
      if (context.phase == InputActionPhase.Started) {
        _dragInitialized = true;
        _mtdInput.DragStart.Invoke(_touch0Position);
      }
      else if (context.phase == InputActionPhase.Canceled) {
        _mtdInput.DragEnd.Invoke(_touch0Position);
        _dragInitialized = false;
      }
    }
  }
}
