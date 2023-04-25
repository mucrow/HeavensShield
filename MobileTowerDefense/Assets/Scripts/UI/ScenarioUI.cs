using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mtd.UI {
  public class ScenarioUI: MonoBehaviour {
    [SerializeField] PauseButton _pauseButton;
    [SerializeField] SpeedButton _speedButton;

    bool _pauseStateBeforeOpeningScenarioMenu;

    public async Task ShowScenarioMenu() {
      Globals.ScenarioManager.With(scenarioManager => {
        _pauseStateBeforeOpeningScenarioMenu = scenarioManager.IsPaused;
        scenarioManager.SetScenarioPaused(true);
      });
      await Globals.UI.ScenarioMenu.Show();
    }

    public async void ShowScenarioMenuEH() {
      await ShowScenarioMenu();
    }

    public async Task HideScenarioMenu() {
      Globals.ScenarioManager.With(scenarioManager => {
        scenarioManager.SetScenarioPaused(_pauseStateBeforeOpeningScenarioMenu);
      });
      await Globals.UI.ScenarioMenu.Hide();
    }

    public async void HideScenarioMenuEH() {
      await HideScenarioMenu();
    }

    public async Task QuitScenario() {
      await HideScenarioMenu();
      if (Globals.ScenarioSceneQuitTarget == ScenarioSceneQuitTarget.MainMenu) {
        Globals.GameManager.LoadMainMenuScene();
      }
      else if (Globals.ScenarioSceneQuitTarget == ScenarioSceneQuitTarget.ScenarioSelection) {
        Globals.GameManager.LoadScenarioSelectionScene();
      }
      else {
        Debug.LogWarning("Unhandled ScenarioSceneQuitTarget " + Globals.ScenarioSceneQuitTarget + ". Quitting to main menu.");
        Globals.GameManager.LoadMainMenuScene();
      }
    }

    public async void QuitScenarioEH() {
      await QuitScenario();
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
        UpdateLeftSideButtons(scenarioManager);
      });
    }

    public void CycleBattleSpeed() {
      Globals.ScenarioManager.With(scenarioManager => {
        scenarioManager.CycleBattleSpeed();
        UpdateLeftSideButtons(scenarioManager);
      });
    }

    public void UpdateLeftSideButtons(ScenarioManager scenarioManager) {
      _pauseButton.UpdatePausedState(scenarioManager.IsPaused);
      _speedButton.UpdateSpeedState(scenarioManager.BattleSpeed);
    }

    public void LoadScenarioAfterCurrentlyLoadedScenario() {
      Globals.LoadedScenario.With(scenarioInfo => {
        var nextScenario = scenarioInfo.Unlocks[0];
        Globals.GameManager.LoadScenarioByID(nextScenario, Globals.ScenarioSceneQuitTarget);
      });
    }
  }
}
