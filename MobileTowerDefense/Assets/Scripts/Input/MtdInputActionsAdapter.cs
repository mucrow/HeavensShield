using System;
using System.Collections;
using System.Collections.Generic;
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
  public class MtdInputActionsAdapter: MonoBehaviour, MtdInputActions.ISimpleActions, MtdInputActions.IZoomActions {
    MtdInput _mtdInput;
    MtdInputActions _mtdInputActions;

    bool _pinchZoomInitialized = false;
    Vector2 _pinchZoomPreviousPositionDelta;
    Vector2 _touch0Position;
    Vector2 _touch1Position;

    [SerializeField] float _zoomScaling = 0.001f;
    [SerializeField] float _scrollZoomScaling = 1f;
    [SerializeField] float _pinchZoomScaling = 1f;

    public void StartInputActionsAdapter(MtdInput mtdInput) {
      _mtdInput = mtdInput;
      _mtdInputActions = new MtdInputActions();
      _mtdInputActions.Simple.SetCallbacks(this);
      _mtdInputActions.Simple.Enable();
      _mtdInputActions.Zoom.SetCallbacks(this);
      _mtdInputActions.Zoom.Enable();
    }

    public void OnClick(InputAction.CallbackContext context) {
      _mtdInput.Click.Invoke(context.action.IsPressed());
    }

    public void OnPoint(InputAction.CallbackContext context) {
      _mtdInput.Point.Invoke(context.ReadValue<Vector2>());
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

    public void OnTouch0Position(InputAction.CallbackContext context) {
      _touch0Position = context.ReadValue<Vector2>();
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
  }
}
