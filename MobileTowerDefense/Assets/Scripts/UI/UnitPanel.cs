using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Mtd.UI {
  public class UnitPanel: MonoBehaviour {
    RectTransform _rectTransform;

    [SerializeField] float _bookmarkWidth = 32f;
    [SerializeField] TMP_Text _openCloseButtonTextComponent;

    Vector2 _initialPosition;
    bool _isHidden = false;
    float _width;

    void Awake() {
      _rectTransform = GetComponent<RectTransform>();
      _initialPosition = _rectTransform.anchoredPosition;
      _width = _rectTransform.rect.width;
    }

    public void OnOpenCloseButtonClick() {
      _isHidden = !_isHidden;
      if (_isHidden) {
        _rectTransform.anchoredPosition = _initialPosition + Vector2.left * (_width - _bookmarkWidth);
        _openCloseButtonTextComponent.text = ">";
      }
      else {
        _rectTransform.anchoredPosition = _initialPosition;
        _openCloseButtonTextComponent.text = "<";
      }
    }
  }
}
