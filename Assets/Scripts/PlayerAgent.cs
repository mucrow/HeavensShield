using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Mtd {
  public class PlayerAgent: MonoBehaviour {
    [SerializeField] GameObject _coffeeMugPrefab;

    Camera _camera;

    Vector3 _pointPosition;
    GameObject _itemBeingPlaced;

    void Start() {
      _camera = Camera.main;
    }

    void Update() {
      if (_itemBeingPlaced != null) {
        _itemBeingPlaced.transform.position = _pointPosition;
      }
    }

    public void OnClick(InputAction.CallbackContext context) {
      // there are two called to OnClick per touchscreen touch start, i dont know why
      // (there is a third call when the touch ends as well)
      if (context.action.IsPressed()) {
        if (!_itemBeingPlaced) {
          _itemBeingPlaced = Instantiate(_coffeeMugPrefab, _pointPosition, Quaternion.identity);
        }
      }
      else {
        _itemBeingPlaced = null;
      }
    }

    public void OnPoint(InputAction.CallbackContext context) {
      var screenPos = context.ReadValue<Vector2>();
      _pointPosition = _camera.ScreenToWorldPoint(screenPos);
      _pointPosition.z = 0;
    }
  }
}
