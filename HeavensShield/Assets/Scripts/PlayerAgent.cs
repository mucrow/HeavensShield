using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mtd.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Mtd {
  public class PlayerAgent: MonoBehaviour {
    [SerializeField] Tilemap _tilemap;
    [SerializeField] TileBase[] _validUnitPlacementTiles;

    bool _ignoreCurrentDrag = false;
    Vector2 _dragStartScreenPos;
    Vector3 _dragStartCameraWorldPos;

    int _money = 1100;
    public int Money => _money;

    int _score = 0;
    public int Score => _score;

    public readonly UnityEvent<int> MoneyChange = new UnityEvent<int>();
    public readonly UnityEvent<int> ScoreChange = new UnityEvent<int>();

    int _tapTriggerLayerMask;

    void Awake() {
      _tapTriggerLayerMask = LayerMask.GetMask("TapTrigger");
    }

    void Start() {
      Globals.PlayerAgent.Register(this);

      Globals.Input.DragStart.AddListener(OnDragStart);
      Globals.Input.Drag.AddListener(OnDrag);
      Globals.Input.Tap.AddListener(OnTap);
      Globals.Input.Zoom.AddListener(OnZoom);

      AddMoney(0);
    }

    void OnDestroy() {
      Globals.Input.DragStart.RemoveListener(OnDragStart);
      Globals.Input.Drag.RemoveListener(OnDrag);
      Globals.Input.Tap.RemoveListener(OnTap);
      Globals.Input.Zoom.RemoveListener(OnZoom);

      Globals.PlayerAgent.Unregister(this);
    }

    public void AddMoney(int amount) {
      SetMoney(_money + amount);
    }

    public void SetMoney(int amount) {
      _money = amount;
      MoneyChange.Invoke(_money);
    }

    public void AddToScore(int points) {
      _score += points;
      ScoreChange.Invoke(_score);
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
      if (!Globals.UI.DoesUICoverScreenPoint(point.Screen)) {
        HandleWorldTap(point);
      }
    }

    async void HandleWorldTap(ScreenAndWorldPoint point) {
      var unitSelector = Globals.UI.UnitSelector;
      if (!unitSelector.IsHidden) {
        await unitSelector.Close();
        return;
      }

      var worldPosition = point.World;

      if (!CanPlaceUnit(worldPosition)) {
        return;
      }

      var pointCenteredToTile = Utils.Utils.SnapPointToTileCenter(worldPosition);
      await unitSelector.Open(pointCenteredToTile);
    }

    public bool CanPlaceUnit(Vector3 worldPosition) {
      if (!_tilemap.localBounds.Contains(worldPosition)) {
        return false;
      }

      var tileCell = _tilemap.WorldToCell(worldPosition);
      var tileInfo = _tilemap.GetTile(tileCell);
      if (!_validUnitPlacementTiles.Contains(tileInfo)) {
        return false;
      }

      var tappedObject = GetTappedObject(worldPosition);
      if (tappedObject) {
        // This is where we could get unit stats, but those were taken out of scope.
        // This block is still required to prevent units from being placed on the enemy path or on
        // top of trees, etc.
        return false;
      }

      return true;
    }

    GameObject GetTappedObject(Vector3 worldPosition) {
      var result = Physics2D.OverlapPoint(worldPosition, _tapTriggerLayerMask);
      if (result) {
        return result.gameObject;
      }
      return null;
    }

    void OnZoom(float amount) {
      Globals.Camera.ChangeZoomLevel(amount);
    }
  }
}