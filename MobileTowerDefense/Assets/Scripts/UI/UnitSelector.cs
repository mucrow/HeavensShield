using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Mtd.UI {
  public class UnitSelector: MonoBehaviour {
    RectTransform _rectTransform;

    Vector2 _initialPosition = Vector2.zero;
    bool _isHidden = false;
    float _width = 100f;

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
      _width = _rectTransform.rect.width;
      Hide();
    }

    public void OnOpenCloseButtonClick() {
      if (_isHidden) {
        Show();
      }
      else {
        Hide();
      }
    }

    public void Show() {
      _rectTransform.anchoredPosition = _initialPosition;
      _isHidden = false;
    }

    public void Hide() {
      _rectTransform.anchoredPosition = _initialPosition + Vector2.left * _width;
      _isHidden = true;
    }
  }
}