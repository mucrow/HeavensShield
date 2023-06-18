//source: https://forum.unity.com/threads/canvashelper-resizes-a-recttransform-to-iphone-xs-safe-area.521107

using System.Collections.Generic;
using Mtd.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Mtd.UI {
  [RequireComponent(typeof(Canvas))]
  public class CanvasHelper: MonoBehaviour {
    static readonly List<CanvasHelper> _helpers = new List<CanvasHelper>();

    public static readonly UnityEvent OnResolutionOrOrientationChanged = new UnityEvent();

    static bool _screenChangeVarsInitialized = false;
    static ScreenOrientation _lastOrientation = ScreenOrientation.LandscapeLeft;
    static Vector2 _lastResolution = Vector2.zero;
    static Rect _lastSafeArea = Rect.zero;

    Canvas _canvas;
    RectTransform _rectTransform;
    RectTransform _safeAreaTransform;

    void Awake() {
      if (!_helpers.Contains(this)) {
        _helpers.Add(this);
      }

      _canvas = GetComponent<Canvas>();
      _rectTransform = GetComponent<RectTransform>();

      _safeAreaTransform = transform.Find("SafeArea") as RectTransform;

      if (!_screenChangeVarsInitialized) {
        _lastOrientation = Screen.orientation;
        _lastResolution.x = Screen.width;
        _lastResolution.y = Screen.height;
        _lastSafeArea = Screen.safeArea;
        Debug.Log("SafeArea: " + _lastSafeArea);

        _screenChangeVarsInitialized = true;
      }

      ApplySafeArea();
    }

    void Update() {
      if (_helpers[0] != this) {
        return;
      }

      if (Application.isMobilePlatform && Screen.orientation != _lastOrientation) {
        OrientationChanged();
      }

      if (Screen.safeArea != _lastSafeArea) {
        SafeAreaChanged();
      }

      if (Screen.width != _lastResolution.x || Screen.height != _lastResolution.y) {
        ResolutionChanged();
      }
    }

    void ApplySafeArea() {
      if (_safeAreaTransform == null) {
        return;
      }

      var safeArea = Screen.safeArea;

      var anchorMin = safeArea.position;
      var anchorMax = safeArea.position + safeArea.size;
      anchorMin.x /= _canvas.pixelRect.width;
      anchorMin.y /= _canvas.pixelRect.height;
      anchorMax.x /= _canvas.pixelRect.width;
      anchorMax.y /= _canvas.pixelRect.height;

      _safeAreaTransform.anchorMin = anchorMin;
      _safeAreaTransform.anchorMax = anchorMax;
    }

    void OnDestroy() {
      if (_helpers != null && _helpers.Contains(this)) {
        _helpers.Remove(this);
      }
    }

    static void OrientationChanged() {
      //Debug.Log("Orientation changed from " + lastOrientation + " to " + Screen.orientation + " at " + Time.time);

      _lastOrientation = Screen.orientation;
      _lastResolution.x = Screen.width;
      _lastResolution.y = Screen.height;

      OnResolutionOrOrientationChanged.Invoke();
    }

    static void ResolutionChanged() {
      Debug.Log("Resolution changed from " + _lastResolution + " to (" + Screen.width + ", " + Screen.height + ") at " + Time.time);

      _lastResolution.x = Screen.width;
      _lastResolution.y = Screen.height;

      OnResolutionOrOrientationChanged.Invoke();
    }

    static void SafeAreaChanged() {
      Debug.Log("Safe Area changed from " + _lastSafeArea + " to " + Screen.safeArea.size + " at " + Time.time);

      _lastSafeArea = Screen.safeArea;

      for (int i = 0; i < _helpers.Count; i++) {
        _helpers[i].ApplySafeArea();
      }
    }

    public SafeAreaPadding GetSafeAreaPadding() {
      var ret = new SafeAreaPadding();

      var safeAreaRect = _safeAreaTransform.rect;
      var safeAreaSize = safeAreaRect.size;

      ret.Left = (int) safeAreaRect.x;
      ret.Bottom = (int) safeAreaRect.y;
      ret.Right = (int) (_lastResolution.x - (ret.Left + safeAreaSize.x));
      ret.Top = (int) (_lastResolution.y - (ret.Bottom + safeAreaSize.y));

      return ret;
    }
  }
}
