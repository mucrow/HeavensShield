using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  public class ScenarioSelectionManager: MonoBehaviour {
    void Start() {
      StartCoroutine(HackyShowMainMenu());
    }

    // TODO we don't need a coroutine here. after I get LeanTween to handle UI open animations,
    //      I'll remove Hide() from ShowHideOffscreen#Start and just have managers of each
    //      individual scene (e.g., MainMenuManager) show and hide the appropriate UIs.
    IEnumerator HackyShowMainMenu() {
      yield return new WaitForSeconds(1.1f / 60f);
      Globals.UI.ScenarioSelection.Show();
    }

    void OnDestroy() {
      Globals.UI.ScenarioSelection.Hide();
    }
  }
}
