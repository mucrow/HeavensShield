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
  public class PlayerAgent: MonoBehaviour, MtdInputActions.IMainActions {
    [SerializeField] GameObject _coffeeMugPrefab;
    [SerializeField] EventSystem _eventSystem;
    [SerializeField] GraphicRaycaster _uiGraphicRaycaster;

    Camera _camera;
    MtdInputActions _mtdInputActions;

    PointEventPosition _pointEventPosition;
    GameObject _itemBeingPlaced;

    void Awake() {
      _mtdInputActions = new MtdInputActions();
    }

    void OnEnable() {
      _mtdInputActions.Main.SetCallbacks(this);
      _mtdInputActions.Main.Enable();
    }

    void Start() {
      _camera = Camera.main;
    }

    void Update() {
      if (_itemBeingPlaced != null) {
        _itemBeingPlaced.transform.position = _pointEventPosition.World;
      }
    }

    public void OnClick(InputAction.CallbackContext context) {
      // there are two called to OnClick per touchscreen touch start, i dont know why
      // (there is a third call when the touch ends as well)
      if (context.action.IsPressed()) {
        if (!_itemBeingPlaced) {
          if (!DidClickHitUI()) {
            _itemBeingPlaced = Instantiate(_coffeeMugPrefab, _pointEventPosition.World, Quaternion.identity);
          }
        }
      }
      else {
        _itemBeingPlaced = null;
      }
    }

    public void OnPoint(InputAction.CallbackContext context) {
      if (context.phase == InputActionPhase.Performed) {
        UpdatePointEventPosition(context.ReadValue<Vector2>());
      }
    }

    // is this really the right way to do this?
    bool DidClickHitUI() {
      var pointerEventData = new PointerEventData(_eventSystem);
      pointerEventData.position = _pointEventPosition.Screen;
      List<RaycastResult> results = new List<RaycastResult>();
      _uiGraphicRaycaster.Raycast(pointerEventData, results);
      return results.Count > 0;
    }

    void UpdatePointEventPosition(Vector2 screenPosition) {
      _pointEventPosition.Screen = screenPosition;
      _pointEventPosition.World = _camera.ScreenToWorldPoint(screenPosition);
      _pointEventPosition.World.z = 0f;
    }

    struct PointEventPosition {
      public Vector2 Screen;
      public Vector3 World;
    }
  }
}
