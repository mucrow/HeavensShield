using System;
using UnityEngine;
using UnityEngine.UI;
namespace Mtd.UI {
  public class SpeedButton : MonoBehaviour {
    [SerializeField] Sprite[] _speedSprites;

    [SerializeField] Image _buttonIcon;

    public void UpdateSpeedState(float speed) {
      float speedClamped = Mathf.Clamp(speed, 1f, 3f);
      int spriteIndex = Mathf.RoundToInt(speedClamped) - 1;
      _buttonIcon.sprite = _speedSprites[spriteIndex];
    }
  }
}
