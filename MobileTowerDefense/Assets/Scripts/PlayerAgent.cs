using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Mtd {
  public class PlayerAgent: MonoBehaviour, MtdInputActions.IMainActions, MtdInputActions.IPinchZoomActions {
    [SerializeField] GameObject _coffeeMugPrefab;
    [SerializeField] EventSystem _eventSystem;
    [SerializeField] GraphicRaycaster _uiGraphicRaycaster;

    Camera _rawCamera;
    CameraController _camera;
    MtdInputActions _mtdInputActions;

    PointEventPosition _pointEventPosition;
    GameObject _itemBeingPlaced;

    bool _pinchZoomInitialized = false;
    Vector2 _pinchZoomPreviousPositionDelta;
    Vector2 _touch0Position;
    Vector2 _touch1Position;

    [SerializeField] float _zoomScaling = 0.001f;
    [SerializeField] float _scrollZoomScaling = 1f;
    [SerializeField] float _pinchZoomScaling = 1f;

    void Awake() {
      _mtdInputActions = new MtdInputActions();
    }

    void OnEnable() {
      _mtdInputActions.Main.SetCallbacks(this);
      _mtdInputActions.Main.Enable();
      _mtdInputActions.PinchZoom.SetCallbacks(this);
      _mtdInputActions.PinchZoom.Enable();
    }

    void Start() {
      _rawCamera = Camera.main;
      _camera = _rawCamera.GetComponent<CameraController>();
    }

    void Update() {
      if (_itemBeingPlaced != null) {
        _itemBeingPlaced.transform.position = _pointEventPosition.World;
      }
    }

    public void OnClick(InputAction.CallbackContext context) {
      // // there are two called to OnClick per touchscreen touch start, i don't know why
      // // (there is a third call when the touch ends as well)
      // if (context.action.IsPressed()) {
      //   if (!_itemBeingPlaced) {
      //     if (!DidClickHitUI()) {
      //       _itemBeingPlaced = Instantiate(_coffeeMugPrefab, _pointEventPosition.World, Quaternion.identity);
      //     }
      //   }
      // }
      // else {
      //   _itemBeingPlaced = null;
      // }
    }

    public void OnPoint(InputAction.CallbackContext context) {
      if (context.phase == InputActionPhase.Performed) {
        UpdatePointEventPosition(context.ReadValue<Vector2>());
      }
    }

    // is this really the right way to do this?
    bool DidClickHitUI() {
      var pointerEventData = new PointerEventData(_eventSystem) {
        position = _pointEventPosition.Screen
      };
      List<RaycastResult> results = new List<RaycastResult>();
      _uiGraphicRaycaster.Raycast(pointerEventData, results);
      return results.Count > 0;
    }

    void UpdatePointEventPosition(Vector2 screenPosition) {
      _pointEventPosition.Screen = screenPosition;
      _pointEventPosition.World = _rawCamera.ScreenToWorldPoint(screenPosition);
      _pointEventPosition.World.z = 0f;
    }

    struct PointEventPosition {
      public Vector2 Screen;
      public Vector3 World;
    }

    void OnZoom(float amount) {
      _camera.ChangeZoomLevel(_zoomScaling * amount);
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

    public void OnTouch1Press(InputAction.CallbackContext context) {
      if (context.phase == InputActionPhase.Canceled) {
        _pinchZoomInitialized = false;
      }
    }
  }
}
