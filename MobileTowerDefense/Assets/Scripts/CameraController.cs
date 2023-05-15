using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace Mtd {
  [RequireComponent(typeof(Camera))]
  public class CameraController: MonoBehaviour {
    [SerializeField] float _closestZoomSize = 2.5f;
    [SerializeField] float _farthestZoomSize = 10f;
    [SerializeField] float _zoomLevelCurve = 1.5f;

    Camera _camera;
    Bounds _limits;
    Tilemap _tilemap;
    float _zoomLevel;

    void Awake() {
      _camera = GetComponent<Camera>();
      var zoomSizeRange = _farthestZoomSize - _closestZoomSize;
      var pointInZoomSizeRange = (_camera.orthographicSize - _closestZoomSize) / zoomSizeRange;
      var clampedPointInZoomSizeRange = Mathf.Clamp01(pointInZoomSizeRange);

      var zoomLevel = 1f - Mathf.Pow(clampedPointInZoomSizeRange, 1f / _zoomLevelCurve);
      SetZoomLevel(zoomLevel);
    }

    void Update() {
      if (_tilemap) {
        Utils.Utils.DebugDrawBounds(_limits, Color.red);
      }
    }

    public Vector3 GetPosition() {
      return gameObject.transform.position;
    }

    public void SetPosition(Vector3 newPosition) {
      var x = Mathf.Clamp(newPosition.x, _limits.min.x, _limits.max.x);
      var y = Mathf.Clamp(newPosition.y, _limits.min.y, _limits.max.y);
      var limitedPosition = new Vector3(x, y, -10f);
      gameObject.transform.position = limitedPosition;
    }

    public Vector3 ScreenToWorldPoint(Vector3 screenPoint) {
      var ret = _camera.ScreenToWorldPoint(screenPoint);
      ret.z = 0;
      return ret;
    }

    public Vector3 WorldToScreenPoint(Vector3 worldPoint) {
      return _camera.WorldToScreenPoint(worldPoint);
    }

    public void ChangeZoomLevel(float amount) {
      SetZoomLevel(_zoomLevel + amount);
    }

    public void SetZoomLevel(float zoomLevel) {
      _zoomLevel = Mathf.Clamp01(zoomLevel);
      UpdateZoomSize();
    }

    public void OnScenarioStart(Tilemap tilemap) {
      _tilemap = tilemap;
      _tilemap.CompressBounds();
      UpdateCameraPositionLimits();
    }

    public void OnScenarioEnd() {
      _tilemap = null;
      UpdateCameraPositionLimits();
    }

    void UpdateZoomSize() {
      _camera.orthographicSize = Utils.Utils.MapRange(
        0f,
        1f,
        _closestZoomSize,
        _farthestZoomSize,
        1f - _zoomLevel,
        _zoomLevelCurve
      );
      UpdateCameraPositionLimits();
    }

    void UpdateCameraPositionLimits() {
      Debug.Log("");
      if (!_tilemap) {
        SetCameraPositionLimits(0f, 0f, 0f, 0f);
        return;
      }

      var viewportBounds = Utils.Utils.GetWorldBoundsInCameraView(_camera);
      Debug.Log("viewportBounds: " + viewportBounds);
      var mapBounds = _tilemap.localBounds;
      Debug.Log("mapBounds before expansion: " + mapBounds);
      mapBounds.Expand(0f * Vector3.one);
      Debug.Log("mapBounds after expansion: " + mapBounds);

      // var minX = mapBounds.min.x + viewportBounds.size.x / 2f;
      // var maxX = mapBounds.max.x - viewportBounds.size.x / 2f;
      // var minY = mapBounds.min.y + viewportBounds.size.y / 2f;
      // var maxY = mapBounds.max.y - viewportBounds.size.y / 2f;
      
      // var minX = viewportBounds.min.x + mapBounds.size.x / 2f;
      // var maxX = viewportBounds.max.x - mapBounds.size.x / 2f;
      // var minY = viewportBounds.min.y + mapBounds.size.y / 2f;
      // var maxY = viewportBounds.max.y - mapBounds.size.y / 2f;
      
      var minX = (viewportBounds.size.x / 2f - mapBounds.size.x / 2f) * -1f;
      var maxX = (viewportBounds.size.x / 2f - mapBounds.size.x / 2f) *  1f;
      var minY = (viewportBounds.size.y / 2f - mapBounds.size.y / 2f) * -1f;
      var maxY = (viewportBounds.size.y / 2f - mapBounds.size.y / 2f) *  1f;

      if (minX > maxX) {
        minX = mapBounds.center.x;
        maxX = mapBounds.center.x;
      }
      else {
        minX += mapBounds.center.x;
        maxX += mapBounds.center.x;
      }
      
      if (minY > maxY) {
        minY = mapBounds.center.y;
        maxY = mapBounds.center.y;
      }
      else {
        minY += mapBounds.center.y;
        maxY += mapBounds.center.y;
      }

      SetCameraPositionLimits(minX, minY, maxX, maxY);

      Debug.Log("Limits updated: " + _limits);
      Debug.Log("");
    }

    void SetCameraPositionLimits(float minX, float minY, float maxX, float maxY) {
      _limits.min = new Vector3(minX, minY, 0f);
      _limits.max = new Vector3(maxX, maxY, 0f);
      SetPosition(GetPosition());
    }
  }
}
