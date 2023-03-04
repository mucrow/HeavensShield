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

    GameObject _itemBeingPlaced;

    bool _ignoreCurrentDrag = false;
    Vector2 _dragStartScreenPos;
    Vector3 _dragStartCameraWorldPos;

    void Start() {
      _rawCamera = Camera.main;
      _camera = _rawCamera.GetComponent<CameraController>();
      _mtdInput.DragStart.AddListener(OnDragStart);
      _mtdInput.Drag.AddListener(OnDrag);
      _mtdInput.Tap.AddListener(OnTap);
      _mtdInput.Zoom.AddListener(OnZoom);
    }

    void OnDragStart(Vector2 screenPos) {
      _ignoreCurrentDrag = DoesPointHitUI(screenPos);
      if (!_ignoreCurrentDrag) {
        _dragStartScreenPos = screenPos;
        _dragStartCameraWorldPos = _rawCamera.gameObject.transform.position;
      }
    }

    void OnDrag(Vector2 screenPos) {
      if (_ignoreCurrentDrag) {
        return;
      }
      // we need to calculate the world positions here because the dragging changes them.
      var dragStartWorldPos = _rawCamera.ScreenToWorldPoint(_dragStartScreenPos);
      var dragCurrentWorldPos = _rawCamera.ScreenToWorldPoint(screenPos);
      var worldPosDelta = dragCurrentWorldPos - dragStartWorldPos;
      _rawCamera.gameObject.transform.position = _dragStartCameraWorldPos + -1f * worldPosDelta;
    }

    void OnTap(ScreenAndWorldPoint point) {
      // // there are two called to OnClick per touchscreen touch start, i don't know why
      // // (there is a third call when the touch ends as well)
      // if (isPressed) {
      //   if (!_itemBeingPlaced) {
      //     if (!DoesPointHitUI(point.Screen)) {
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
    bool DoesPointHitUI(Vector2 screenPos) {
      var pointerEventData = new PointerEventData(_eventSystem) {
        position = screenPos
      };
      List<RaycastResult> results = new List<RaycastResult>();
      _uiGraphicRaycaster.Raycast(pointerEventData, results);
      return results.Count > 0;
    }
  }
}
