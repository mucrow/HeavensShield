using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  public class MainMenuManager: MonoBehaviour {
    void Start() {
      Globals.UI.EnsureReady();
      Globals.UI.MainMenu.OnClickScenarioSelection.AddListener(LoadScenarioSelectionScene);
      Globals.UI.MainMenu.OnClickBack.AddListener(LoadStartMenuScene);
      Globals.UI.MainMenu.Show();
    }

    void OnDestroy() {
      Globals.UI.MainMenu.OnClickScenarioSelection.RemoveListener(LoadScenarioSelectionScene);
      Globals.UI.MainMenu.OnClickBack.RemoveListener(LoadStartMenuScene);
      Globals.UI.MainMenu.Hide();
    }

    void LoadStartMenuScene() {
      Globals.GameManager.LoadScene("Scenes/StartMenu");
    }

    void LoadScenarioSelectionScene() {
      Globals.GameManager.LoadScene("Scenes/ScenarioSelection");
    }
  }
}
