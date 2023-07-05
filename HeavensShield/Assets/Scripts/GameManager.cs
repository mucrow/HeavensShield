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
        Debug.LogError("Couldn't write save data to disk. Error: " + e);
      }
    }

    public void ClearSaveData() {
      var result = SaveAndLoad.ClearSaveData();
      if (result.Code == SaveAndLoad.ClearSaveDataResultCode.ExceptionThrown) {
        // TODO not sure what can really be done here
        throw result.Exception;
      }
      else if (result.Code == SaveAndLoad.ClearSaveDataResultCode.NoDataToClear) {
        _saveData = result.NewSaveData;
      }
      else if (result.Code == SaveAndLoad.ClearSaveDataResultCode.DataCleared) {
        _saveData = result.NewSaveData;
      }
      else {
        Debug.LogWarning("Unhandled ClearSaveDataResultCode: " + result.Code);
      }
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

    public void LoadCreditsScene() {
      LoadSceneHelper("Scenes/Credits", null);
    }

    public void LoadNextStoryScenarioFromMainMenu() {
      LoadNextStoryScenario(ScenarioSceneQuitTarget.MainMenu);
    }

    public void LoadNextStoryScenario(ScenarioSceneQuitTarget quitTarget) {
      LoadScenarioByID(_saveData.Game.NextStoryScenarioID, quitTarget);
    }

    public void LoadScenarioByID(int id, ScenarioSceneQuitTarget quitTarget) {
      LoadScenario(Globals.ScenarioOrder.GetScenarioByID(id), quitTarget);
    }

    public void OpenPrivacyPolicy() {
      Application.OpenURL("https://mucrow.github.io/heavens-shield/privacy");
    }

    public void ReloadScenario() {
      Globals.LoadedScenario.With(scenarioInfo => {
        LoadScenario(scenarioInfo, Globals.ScenarioSceneQuitTarget);
      });
    }

    public void LoadScenario(OrderedScenarioInfo scenarioInfo, ScenarioSceneQuitTarget quitTarget) {
      Globals.ScenarioSceneQuitTarget = quitTarget;
      int chapterNumber = scenarioInfo.ChapterID + 1;
      string chapterFolder = "Scenarios/Chapter" + chapterNumber + "/";
      LoadSceneHelper(chapterFolder + scenarioInfo.Path, scenarioInfo);
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
