﻿using System.Collections.Generic;

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
      public float MusicVolume = 1f;
      public float SoundEffectsVolume = 1f;
    }

    [System.Serializable]
    public class UISettingsSaveData {
      public float Scale = 1f;
    }

    [System.Serializable]
    public class GameSaveData {
      public float PoliticalCapital = 0f;

      public int NextStoryScenarioID = 0;

      // Do not convert these to HashSets. HashSets are not serialized by JsonUtility. HashSet
      // fields on an object getting serialized are ignored without warnings or errors.
      public List<int> UnlockedScenarioIDs = new List<int>() { 0 };
      public List<int> UnlockedUnitIDs = new List<int>() { 0, 1, 2 };

      public void UnlockScenarios(params int[] ids) {
        Utils.Utils.AddNewToList(UnlockedScenarioIDs, ids);
      }

      public void UnlockUnits(params int[] ids) {
        Utils.Utils.AddNewToList(UnlockedUnitIDs, ids);
      }
    }
  }
}
