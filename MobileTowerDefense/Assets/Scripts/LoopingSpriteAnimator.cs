using UnityEngine;

namespace Mtd {
  public class LoopingSpriteAnimator: MonoBehaviour {
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Sprite[] _animationFrames;
    [SerializeField] float _timePerFrame = 0.3f;

    float _timeToNextFrame;
    int _currentFrame;

    void Awake() {
      ResetAnimation();
    }

    void Update() {
      _timeToNextFrame -= Time.deltaTime;
      if (_timeToNextFrame < 0f) {
        _currentFrame = (_currentFrame + 1) % _animationFrames.Length;
        UpdateSprite();
        _timeToNextFrame += _timePerFrame;
      }
    }

    public void SetAnimationFrames(Sprite[] frames, bool resetAnimation = true) {
      SetAnimationFrames(frames, _timePerFrame, resetAnimation);
    }

    public void SetAnimationFrames(Sprite[] frames, float timePerFrame, bool resetAnimation = true) {
      _animationFrames = frames;
      SetTimePerFrame(timePerFrame);
      if (resetAnimation) {
        ResetAnimation();
      }
    }

    public void SetTimePerFrame(float t) {
      _timePerFrame = t;
    }

    public void ResetAnimation() {
      _currentFrame = 0;
      _timeToNextFrame = _timePerFrame;
      UpdateSprite();
    }

    void UpdateSprite() {
      _spriteRenderer.sprite = _animationFrames[_currentFrame];
    }
  }
}
