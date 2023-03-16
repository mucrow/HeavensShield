using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Mtd.UI {
  public class StartMenu: MonoBehaviour {
    [SerializeField] Button _playButton;
    public UnityEvent OnClickPlay => _playButton.onClick;

    [SerializeField] Button _settingsButton;
    public UnityEvent OnClickSettings => _settingsButton.onClick;

    [SerializeField] ShowHideOffscreen _showHideOffscreen;
    public UnityAction Show => _showHideOffscreen.Show;
    public UnityAction Hide => _showHideOffscreen.Hide;
  }
}
