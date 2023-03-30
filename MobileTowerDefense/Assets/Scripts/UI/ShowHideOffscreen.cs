using System;
using System.Threading.Tasks;
using Mtd.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mtd.UI {
  [RequireComponent(typeof(RectTransform))]
  public class ShowHideOffscreen: UIBehaviour, IEnsureReady {
    [SerializeField] Direction _edge;

    [SerializeField] float _tweenTime = 0.1f;

    RectTransform _rectTransform;
    bool _isReady = false;
    Vector2 _initialPosition;
    Vector2 _size = new Vector2(16, 16);

    public bool IsHidden { get; private set; }

    protected override void Awake() {
      _rectTransform = GetComponent<RectTransform>();
    }

    protected override void Start() {
      // trying to check initial position and width in Awake() result in inaccurate values. i
      // believe this is because Unity UI components are not guaranteed to be finished resizing
      // when Awake() is called. it makes sense - the layout needs to be dynamically calculated and
      // it has to happen somewhere. probably best that these objects don't get special treatment
      // just for being part of Unity's main library.
      EnsureReady();
    }

    protected override void OnRectTransformDimensionsChange() {
      base.OnRectTransformDimensionsChange();
      if (!_rectTransform) {
        return;
      }
      _size = _rectTransform.rect.size;
      if (IsHidden) {
        var hiddenPosition = GetHiddenPosition();
        _rectTransform.anchoredPosition = hiddenPosition;
      }
    }

    public void ShowInstant() {
      _rectTransform.anchoredPosition = _initialPosition;
      IsHidden = false;
    }

    public void HideInstant() {
      var hiddenPosition = GetHiddenPosition();
      _rectTransform.anchoredPosition = hiddenPosition;
      IsHidden = true;
    }

    public Task Show() {
      var tcs = new TaskCompletionSource<bool>();
      LeanTween.move(_rectTransform, _initialPosition, _tweenTime).setOnComplete(() => {
        tcs.SetResult(true);
      });
      IsHidden = false;
      return tcs.Task;
    }

    public Task Hide() {
      var tcs = new TaskCompletionSource<bool>();
      var hiddenPosition = GetHiddenPosition();
      LeanTween.move(_rectTransform, hiddenPosition, _tweenTime).setOnComplete(() => {
        tcs.SetResult(true);
      });
      IsHidden = true;
      return tcs.Task;
    }

    Vector2 GetHiddenPosition() {
      var direction = _edge.ToVector2();
      var relevantDimension = GetRelevantDimension();
      return _initialPosition + direction * relevantDimension;
    }

    public float GetRelevantDimension() {
      if (_edge == Direction.Left || _edge == Direction.Right) {
        return _size.x;
      }
      return _size.y;
    }

    public void EnsureReady() {
      if (_isReady) {
        return;
      }
      _initialPosition = _rectTransform.anchoredPosition;
      _size = _rectTransform.rect.size;
      HideInstant();
      _isReady = true;
    }
  }
}
