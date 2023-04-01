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
        SaveAndLoad.TryWriteSaveData(_saveData);
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

    public void LoadScene(string sceneName) {
      SceneManager.LoadScene(sceneName);
    }
  }
}
