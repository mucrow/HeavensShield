using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

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
      Globals.GameManager.LoadMainMenuScene();
    }

    public async void QuitToMainMenuEH() {
      await QuitToMainMenu();
    }

    public async Task RestartScenario() {
      await HideScenarioMenu();
      Globals.GameManager.ReloadScenario();
    }

    public async void RestartScenarioEH() {
      await RestartScenario();
    }

    public void ToggleScenarioPaused() {
      Globals.ScenarioManager.With(scenarioManager => {
        scenarioManager.ToggleScenarioPaused();
      });
    }

    public void CycleBattleSpeed() {
      Globals.ScenarioManager.With(scenarioManager => {
        scenarioManager.CycleBattleSpeed();
      });
    }
  }
}
