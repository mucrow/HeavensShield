using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Mtd.UI {
  public class ScenarioUI: MonoBehaviour {
    public async Task ShowScenarioMenu() {
      await Globals.UI.ScenarioMenu.Show();
    }

    public async void ShowScenarioMenuEH() {
      await ShowScenarioMenu();
    }

    public async Task HideScenarioMenu() {
      await Globals.UI.ScenarioMenu.Hide();
    }

    public async void HideScenarioMenuEH() {
      await HideScenarioMenu();
    }

    public async Task QuitToMainMenu() {
      await HideScenarioMenu();
      Globals.GameManager.LoadScene("Scenes/MainMenu");
    }

    public async void QuitToMainMenuEH() {
      await QuitToMainMenu();
    }
  }
}
