using System;
using System.Collections;
using System.Collections.Generic;
using Mtd.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Mtd {
  public class PlayerAgent: MonoBehaviour {
    [SerializeField] MtdInput _mtdInput;

    bool _ignoreCurrentDrag = false;
    Vector2 _dragStartScreenPos;
    Vector3 _dragStartCameraWorldPos;

    int _money = 1100;
    public int Money => _money;

    public readonly UnityEvent<int> MoneyChange = new UnityEvent<int>();

    void Start() {
      _mtdInput.DragStart.AddListener(OnDragStart);
      _mtdInput.Drag.AddListener(OnDrag);
      _mtdInput.Tap.AddListener(OnTap);
      _mtdInput.Zoom.AddListener(OnZoom);

      AddMoney(0);
    }

    public void AddMoney(int amount) {
      _money += amount;
      MoneyChange.Invoke(_money);
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
        var pointCenteredToTile = Utils.Utils.SnapPointToTileCenter(point.World);
        var unitSelector = ui.UnitSelector;
        if (unitSelector.IsHidden) {
          unitSelector.Open(pointCenteredToTile);
        }
        else {
          unitSelector.Close();
        }
      }
    }

    void OnZoom(float amount) {
      Globals.Camera.ChangeZoomLevel(amount);
    }
  }
}
