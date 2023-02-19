using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace Mtd {
  public class PlayerAgent: MonoBehaviour {
    [SerializeField] GameObject _coffeeMugPrefab;

    Vector3 _pointPosition;
    GameObject _itemBeingPlaced;

    void Update() {
      if (_itemBeingPlaced != null) {
        _itemBeingPlaced.transform.position = _pointPosition;
      }
    }

    void OnClick(InputValue value) {
      if (value.isPressed) {
        // TODO figure out why value.isPressed is true twice on my (android) phone's touchscreen
        // left click press / touch start
        if (!_itemBeingPlaced) {
          _itemBeingPlaced = Instantiate(_coffeeMugPrefab, _pointPosition, Quaternion.identity);
        }
      }
      else {
        // left click release / touch end
        _itemBeingPlaced = null;
      }
    }

    void OnPoint(InputValue value) {
      var screenPos = value.Get<Vector2>();
      _pointPosition = Camera.main.ScreenToWorldPoint(screenPos);
      _pointPosition.z = 0;
    }
  }
}
