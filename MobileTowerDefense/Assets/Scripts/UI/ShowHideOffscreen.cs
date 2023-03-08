using System;
using UnityEngine;

namespace Mtd.UI {
  [RequireComponent(typeof(RectTransform))]
  public class ShowHideOffscreen: MonoBehaviour {
    RectTransform _rectTransform;
    Vector2 _initialPosition;
    float _size = 100;

    public bool IsHidden { get; private set; }

    void Awake() {
      _rectTransform = GetComponent<RectTransform>();
    }

    void Start() {
      // trying to check initial position and width in Awake() result in inaccurate values. i
      // believe this is because Unity UI components are not guaranteed to be finished resizing
      // when Awake() is called. it makes sense - the layout needs to be dynamically calculated and
      // it has to happen somewhere. probably best that these objects don't get special treatment
      // just for being part of Unity's main library.
      _initialPosition = _rectTransform.anchoredPosition;
      _size = _rectTransform.rect.width;
      Hide();
    }

    public void Show() {
      _rectTransform.anchoredPosition = _initialPosition;
      IsHidden = false;
    }

    public void Hide() {
      _rectTransform.anchoredPosition = _initialPosition + Vector2.left * _size;
      IsHidden = true;
    }
  }
}
