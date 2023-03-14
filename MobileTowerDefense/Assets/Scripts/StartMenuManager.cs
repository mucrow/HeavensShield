using System;
using System.Collections;
using System.Collections.Generic;
using Mtd.UI;
using UnityEngine;

namespace Mtd {
  public class StartMenuManager: MonoBehaviour {
    StartMenu _startMenu;

    void Start() {
      _startMenu = Globals.UI.StartMenu;
      _startMenu.OnClickPlay.AddListener(OnClickPlay);
      _startMenu.Show();
    }

    void OnDestroy() {
      _startMenu.OnClickPlay.RemoveListener(OnClickPlay);
    }

    public void OnClickPlay() {
      _startMenu.Hide();
      Globals.GameManager.LoadScene("Scenes/Scenarios/0000-TestScenario");
    }
  }
}
