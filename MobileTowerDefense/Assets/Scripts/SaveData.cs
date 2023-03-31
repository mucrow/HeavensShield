using System.Collections.Generic;

namespace Mtd {
  [System.Serializable]
  public class SaveData {
    public SettingsSaveData Settings = new SettingsSaveData();
    public GameSaveData Game = new GameSaveData();

    [System.Serializable]
    public class SettingsSaveData {
      public AudioSettingsSaveData Audio = new AudioSettingsSaveData();
      public UISettingsSaveData UI = new UISettingsSaveData();
    }

    [System.Serializable]
    public class AudioSettingsSaveData {
      public int MasterVolume = 7;
      public int SoundEffectsVolume = 7;
      public int MusicVolume = 7;
    }

    [System.Serializable]
    public class UISettingsSaveData {
      public float Scale = 1f;
    }

    [System.Serializable]
    public class GameSaveData {
      public float PoliticalCapital;
      public List<int> UnlockedScenarioIDs;
      public List<int> UnlockedUnitIDs;
    }
  }
}
