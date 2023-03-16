using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  public class MainMenuManager: MonoBehaviour {
    void Start() {
      Globals.UI.MainMenu.OnClickScenarioSelection.AddListener(LoadScenarioSelectionScene);
      Globals.UI.MainMenu.OnClickBack.AddListener(LoadStartMenuScene);
      StartCoroutine(HackyShowMainMenu());
    }

    void OnDestroy() {
      Globals.UI.MainMenu.OnClickScenarioSelection.RemoveListener(LoadScenarioSelectionScene);
      Globals.UI.MainMenu.OnClickBack.RemoveListener(LoadStartMenuScene);
      Globals.UI.MainMenu.Hide();
    }

    // TODO we don't need a coroutine here. after I get LeanTween to handle UI open animations,
    //      I'll remove Hide() from ShowHideOffscreen#Start and just have managers of each
    //      individual scene (e.g., MainMenuManager) show and hide the appropriate UIs.
    IEnumerator HackyShowMainMenu() {
      yield return new WaitForSeconds(1.1f / 60f);
      Globals.UI.MainMenu.Show();
    }

    void LoadStartMenuScene() {
      Globals.GameManager.LoadScene("Scenes/StartMenu");
    }

    void LoadScenarioSelectionScene() {
      Globals.GameManager.LoadScene("Scenes/ScenarioSelection");
    }
  }
}
