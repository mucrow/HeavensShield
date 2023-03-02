using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  [RequireComponent(typeof(Camera))]
  public class CameraController: MonoBehaviour {
    [SerializeField] float _sizeOfClosestZoom = 2.5f;
    [SerializeField] float _sizeOfFarthestZoom = 50f;

    Camera _camera;

    void Awake() {
      _camera = GetComponent<Camera>();
      SetZoomLevel(_camera.orthographicSize);
    }

    public void ChangeZoom(float amount) {
      SetZoomLevel(_camera.orthographicSize + amount);
    }

    public void SetZoomLevel(float level) {
      _camera.orthographicSize = Mathf.Clamp(level, _sizeOfClosestZoom, _sizeOfFarthestZoom);
    }
  }
}
