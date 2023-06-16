using UnityEngine;
using UnityEngine.UI;

namespace Mtd.UI {
  public class PauseButton : MonoBehaviour {
    [SerializeField] Color _pausedColor;
    [SerializeField] Color _unpausedColor;

    [SerializeField] Image _buttonIcon;

    public void UpdatePausedState(bool isPaused) {
      _buttonIcon.color = isPaused ? _pausedColor : _unpausedColor;
    }
  }
}
