using System;
using System.Collections;
using System.Collections.Generic;
using Mtd.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Mtd {
  public class PlayerAgent: MonoBehaviour {
    [SerializeField] GameObject _coffeeMugPrefab;
    [SerializeField] EventSystem _eventSystem;
    [SerializeField] GraphicRaycaster _uiGraphicRaycaster;
    [SerializeField] MtdInput _mtdInput;

    Camera _rawCamera;
    CameraController _camera;

    ScreenAndWorldPoint _pointEventPosition;
    GameObject _itemBeingPlaced;

    bool _ignoreCurrentDrag = false;

    void Awake() {
      _pointEventPosition = ScreenAndWorldPoint.Zero();
    }

    void Start() {
      _rawCamera = Camera.main;
      _camera = _rawCamera.GetComponent<CameraController>();
      _mtdInput.DragStart.AddListener(OnDragStart);
      _mtdInput.Drag.AddListener(OnDrag);
      _mtdInput.Tap.AddListener(OnTap);
      _mtdInput.Zoom.AddListener(OnZoom);
    }

    void Update() {
      if (_itemBeingPlaced != null) {
        _itemBeingPlaced.transform.position = _pointEventPosition.World;
      }
    }

    void OnDragStart(ScreenAndWorldPoint point) {
      _ignoreCurrentDrag = DoesPointHitUI(point);
    }

    void OnDrag(Vector3 delta) {
      if (!_ignoreCurrentDrag) {
        // _rawCamera.transform.position = _rawCamera.transform.position + delta;
      }
    }

    void OnTap(ScreenAndWorldPoint point) {
      // // there are two called to OnClick per touchscreen touch start, i don't know why
      // // (there is a third call when the touch ends as well)
      // if (isPressed) {
      //   if (!_itemBeingPlaced) {
      //     if (!DoesPointHitUI(point)) {
      //       _itemBeingPlaced = Instantiate(_coffeeMugPrefab, point.World, Quaternion.identity);
      //     }
      //   }
      // }
      // else {
      //   _itemBeingPlaced = null;
      // }
    }

    void OnZoom(float amount) {
      _camera.ChangeZoomLevel(amount);
    }

    // is this really the right way to do this?
    bool DoesPointHitUI(ScreenAndWorldPoint point) {
      var pointerEventData = new PointerEventData(_eventSystem) {
        position = point.Screen
      };
      List<RaycastResult> results = new List<RaycastResult>();
      _uiGraphicRaycaster.Raycast(pointerEventData, results);
      return results.Count > 0;
    }
  }
}
