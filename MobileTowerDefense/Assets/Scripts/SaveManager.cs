using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Mtd {
  public class SaveManager: MonoBehaviour {
    string _saveFilePath = System.IO.Path.Combine(Application.persistentDataPath, "SaveData.json");

    public async Task<SaveData> ReadSaveData() {
      string jsonString = await System.IO.File.ReadAllTextAsync(_saveFilePath);
      return JsonUtility.FromJson<SaveData>(jsonString);
    }

    public async Task WriteSaveData(SaveData saveData) {
      string jsonString = JsonUtility.ToJson(saveData, true);
      await System.IO.File.WriteAllTextAsync(_saveFilePath, jsonString);
    }
  }
}
