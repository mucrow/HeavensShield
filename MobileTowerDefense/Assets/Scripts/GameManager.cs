using System;
using System.Threading.Tasks;
using Mtd.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Mtd {
  public class GameManager: MonoBehaviour {
    SaveData _saveData;
    public SaveData SaveData => _saveData;

    void Awake() {
      var result = SaveAndLoad.TryReadSaveData();
      if (result.Code == SaveAndLoad.TryReadSaveDataResultCode.ExceptionThrown) {
        // TODO could do handling here for corrupted save data, etc.
        throw result.Exception;
      }
      else if (result.Code == SaveAndLoad.TryReadSaveDataResultCode.DoesNotExist) {
        _saveData = result.SaveData;
      }
      else if (result.Code == SaveAndLoad.TryReadSaveDataResultCode.Success) {
        _saveData = result.SaveData;
      }
      else {
        Debug.LogWarning("Unhandled TryReadSaveDataResultCode: " + result.Code);
      }
    }

    void OnDestroy() {
      if (this == Globals.GameManager) {
        WriteSaveData();
      }
    }

    public void WriteSaveData() {
      try {
        SaveAndLoad.TryWriteSaveData(_saveData);
      }
      catch (Exception e) {
        Debug.LogError("Couldn't write save data to disk");
      }
    }

    public void LoadBarracksScene() {
      LoadSceneHelper("Scenes/Barracks", null);
    }

    public void LoadMainMenuScene() {
      LoadSceneHelper("Scenes/MainMenu", null);
    }

    public void LoadScenarioSelectionScene() {
      LoadSceneHelper("Scenes/ScenarioSelection", null);
    }

    public void LoadSettingsMenuScene() {
      LoadSceneHelper("Scenes/SettingsMenu", null);
    }

    public void LoadStartMenuScene() {
      LoadSceneHelper("Scenes/StartMenu", null);
    }

    public void LoadNextStoryScenario() {
      LoadScenarioByID(_saveData.Game.NextStoryScenarioID);
    }

    public void LoadScenarioByID(int id) {
      LoadScenario(Globals.ScenarioOrder.GetScenarioByID(id));
    }

    public void LoadScenario(OrderedScenarioInfo scenarioInfo) {
      LoadSceneHelper("Scenes/Scenarios/" + scenarioInfo.Path, scenarioInfo);
    }

    public void ReloadScenario() {
      Globals.LoadedScenario.With(scenarioInfo => LoadScenario(scenarioInfo));
    }

    void LoadSceneHelper(string path, OrderedScenarioInfo scenario) {
      if (scenario != null) {
        Globals.LoadedScenario.Register(scenario);
      }
      else {
        Globals.LoadedScenario.Unregister();
      }
      SceneManager.LoadScene(path);
    }
  }
}
