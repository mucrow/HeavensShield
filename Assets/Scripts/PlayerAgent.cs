using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

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

    public void OnClick(InputAction.CallbackContext context) {
      Debug.Log(context.phase);
    }

    // void OnClick(InputValue value) {
    //   if (value.isPressed) {
    //     // TODO figure out why value.isPressed is true twice on my (android) phone's touchscreen
    //     // left click press / touch start
    //     // Debug.Log("OnClick isPressed=true");
    //     if (!_itemBeingPlaced) {
    //       _itemBeingPlaced = Instantiate(_coffeeMugPrefab, _pointPosition, Quaternion.identity);
    //     }
    //   }
    //   else {
    //     // Debug.Log("OnClick isPressed=false");
    //     // left click release / touch end
    //     _itemBeingPlaced = null;
    //   }
    // }
    //
    // void OnClick2(InputValue value) {
    //   try {
    //     var touch = value.Get<Touch>();
    //     Debug.Log("Able to Get<Touch>(): " + touch);
    //   }
    //   catch {}
    //
    //   try {
    //     var obj = value.Get();
    //     Debug.Log(obj);
    //   }
    //   catch {}
    //
    //   if (value.isPressed) {
    //     Debug.Log("OnClick2 isPressed=true");
    //   }
    //   else {
    //     Debug.Log("OnClick2 isPressed=false");
    //   }
    // }
    //
    // void OnPoint(InputValue value) {
    //   var screenPos = value.Get<Vector2>();
    //   _pointPosition = Camera.main.ScreenToWorldPoint(screenPos);
    //   _pointPosition.z = 0;
    // }
  }
}
