using System;
using System.Threading.Tasks;
using Mtd.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Mtd.UI {
  [RequireComponent(typeof(RectTransform))]
  public class ShowHideUIElement: UIBehaviour, IEnsureReady {
    [FormerlySerializedAs("_edge")] [SerializeField] HideMode _hideMode;

    [Header("Can be null unless Hide Mode is \"Fade\".")]
    [SerializeField] CanvasGroup _canvasGroup;

    [SerializeField] float _tweenTime = 0.1f;

    RectTransform _rectTransform;
    bool _isReady = false;
    Vector2 _initialPosition;
    Vector2 _size = new Vector2(16, 16);

    public bool IsHidden { get; private set; }

    protected override void Awake() {
      _rectTransform = GetComponent<RectTransform>();

      if (_hideMode == HideMode.Fade && _canvasGroup == null) {
        Debug.LogError("You must set the Canvas Group property when Hide Mode is \"Fade\".", this);
        Debug.LogError("Changing Hide Mode to \"Slide Down\".", this);
        _hideMode = HideMode.SlideDown;
      }
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
        HideInstant();
      }
      else {
        ShowInstant();
      }
    }

    public void ShowInstant() {
      if (_hideMode == HideMode.Fade) {
        SetCanvasAlpha(1f);
      }
      else {
        _rectTransform.anchoredPosition = _initialPosition;
      }
      IsHidden = false;
    }

    public void HideInstant() {
      if (_hideMode == HideMode.Fade) {
        SetCanvasAlpha(0f);
      }
      else {
        var hiddenPosition = GetHiddenPosition();
        _rectTransform.anchoredPosition = hiddenPosition;
      }
      IsHidden = true;
    }

    public Task Show() {
      var tcs = new TaskCompletionSource<bool>();

      LTDescr tween;
      if (_hideMode == HideMode.Fade) {
        tween = TweenCanvasAlpha(1f, _tweenTime);
      }
      else {
        var hiddenPosition = GetHiddenPosition();
        tween = LeanTween.move(_rectTransform, _initialPosition, _tweenTime);
      }

      tween.setOnComplete(() => {
        tcs.SetResult(true);
      }).setIgnoreTimeScale(true);

      IsHidden = false;
      return tcs.Task;
    }

    public Task Hide() {
      var tcs = new TaskCompletionSource<bool>();

      LTDescr tween;
      if (_hideMode == HideMode.Fade) {
        tween = TweenCanvasAlpha(0f, _tweenTime);
      }
      else {
        var hiddenPosition = GetHiddenPosition();
        tween = LeanTween.move(_rectTransform, hiddenPosition, _tweenTime);
      }

      tween.setOnComplete(() => {
        tcs.SetResult(true);
      }).setIgnoreTimeScale(true);
      IsHidden = true;
      return tcs.Task;
    }

    public async void ShowEH() {
      await Show();
    }

    public async void HideEH() {
      await Hide();
    }

    Vector2 GetHiddenPosition() {
      var direction = _hideMode.ToVector2();
      var relevantDimension = GetRelevantDimension();
      return _initialPosition + direction * relevantDimension;
    }

    public float GetRelevantDimension() {
      if (_hideMode == HideMode.Fade) {
        return 0f;
      }
      if (_hideMode == HideMode.SlideLeft || _hideMode == HideMode.SlideRight) {
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

    LTDescr TweenCanvasAlpha(float alpha, float time) {
      return ChangeCanvasAlphaHelper(alpha, true, time);
    }

    void SetCanvasAlpha(float alpha) {
      ChangeCanvasAlphaHelper(alpha, false, 0f);
    }

    /**
     * A helper method that helps me avoid forgetting to update _canvasGroup.blocksRaycasts. Also
     * ensures I don't forget to make the tween use the "ease out circular" curve.
     *
     * If alpha is 0, _canvasGroup.blocksRaycasts is set to false.
     * Otherwise, _canvasGroup.blocksRaycasts is set to true.
     *
     * If time is greater than 0, an LTDescr is returned.
     * Otherwise, null is returned.
     */
    LTDescr ChangeCanvasAlphaHelper(float alpha, bool returnLTDescr, float time) {
      _canvasGroup.blocksRaycasts = alpha != 0;
      if (returnLTDescr) {
        return LeanTween.alphaCanvas(_canvasGroup, alpha, time).setEaseOutCirc();
      }
      _canvasGroup.alpha = alpha;
      return null;
    }
  }
}
