using System;
using UnityEngine;

namespace Mtd {
  public class ScenarioSelectionManager: MonoBehaviour {
    [SerializeField] AudioClip _music;

    void Start() {
      Globals.AudioManager.PlayMusic(_music);
      Globals.UI.EnsureReady();
      Globals.UI.ScenarioSelection.RefreshUI();
      Globals.UI.ScenarioSelection.ShowInstant();
    }

    void OnDestroy() {
      Globals.UI.ScenarioSelection.HideInstant();
    }
  }
}
