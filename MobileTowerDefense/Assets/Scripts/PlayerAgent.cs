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
    [SerializeField] MtdInput _mtdInput;

    GameObject _itemBeingPlaced;

    bool _ignoreCurrentDrag = false;
    Vector2 _dragStartScreenPos;
    Vector3 _dragStartCameraWorldPos;

    void Start() {
      _mtdInput.DragStart.AddListener(OnDragStart);
      _mtdInput.Drag.AddListener(OnDrag);
      _mtdInput.Tap.AddListener(OnTap);
      _mtdInput.Zoom.AddListener(OnZoom);
    }

    void OnDragStart(Vector2 screenPos) {
      _ignoreCurrentDrag = Globals.UI.DoesUICoverScreenPoint(screenPos);
      if (!_ignoreCurrentDrag) {
        _dragStartScreenPos = screenPos;
        _dragStartCameraWorldPos = Globals.Camera.GetPosition();
      }
    }

    void OnDrag(Vector2 screenPos) {
      if (_ignoreCurrentDrag) {
        return;
      }
      // we need to calculate the world positions here because the dragging changes them.
      var dragStartWorldPos = Globals.Camera.ScreenToWorldPoint(_dragStartScreenPos);
      var dragCurrentWorldPos = Globals.Camera.ScreenToWorldPoint(screenPos);
      var worldPosDelta = dragCurrentWorldPos - dragStartWorldPos;
      Globals.Camera.SetPosition(_dragStartCameraWorldPos + -1f * worldPosDelta);
    }

    void OnTap(ScreenAndWorldPoint point) {
      // is this really the right way to do this?
      var ui = Globals.UI;
      if (!ui.DoesUICoverScreenPoint(point.Screen)) {
        var halfTile = new Vector3(0.5f, 0.5f, 0f);
        var bottomLeftCornerOfTappedTile = new Vector3(Mathf.Floor(point.World.x), Mathf.Floor(point.World.y), point.World.z);
        var pointCenteredToTile = bottomLeftCornerOfTappedTile + halfTile;
        var unitSelector = ui.UnitSelector;
        if (unitSelector.IsHidden) {
          unitSelector.StartUnitSelection(pointCenteredToTile);
        }
        else {
          unitSelector.CancelUnitSelection();
        }
      }
    }

    void OnZoom(float amount) {
      Globals.Camera.ChangeZoomLevel(amount);
    }
  }
}
