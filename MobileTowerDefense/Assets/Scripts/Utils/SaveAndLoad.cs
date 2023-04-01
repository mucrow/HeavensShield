using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Mtd.Utils {
  public class SaveAndLoad {
    public static TryReadSaveDataResult TryReadSaveData() {
      var saveFilePath = GetSaveFilePath();
      if (!System.IO.File.Exists(saveFilePath)) {
        return TryReadSaveDataResult.DoesNotExist();
      }

      try {
        string jsonString = System.IO.File.ReadAllText(saveFilePath);
        var saveData = JsonUtility.FromJson<SaveData>(jsonString);
        return TryReadSaveDataResult.Success(saveData);
      }
      catch (Exception e) {
        return TryReadSaveDataResult.ExceptionThrown(e);
      }
    }

    /** This may throw an exception if we don't have permission to write to the _saveFilePath. */
    public static void TryWriteSaveData(SaveData saveData) {
      var saveFilePath = GetSaveFilePath();
      string jsonString = JsonUtility.ToJson(saveData, true);
      System.IO.File.WriteAllText(saveFilePath, jsonString);
    }

    static string GetSaveFilePath() {
      return System.IO.Path.Combine(Application.persistentDataPath, "SaveData.json");
    }

    public class TryReadSaveDataResult {
      public TryReadSaveDataResultCode Code;
      public SaveData SaveData;
      public Exception Exception;

      private TryReadSaveDataResult() {}

      public static TryReadSaveDataResult DoesNotExist() {
        return new TryReadSaveDataResult() {
          Code = TryReadSaveDataResultCode.DoesNotExist,
          SaveData = new SaveData(),
          Exception = null,
        };
      }

      public static TryReadSaveDataResult ExceptionThrown(Exception exception) {
        return new TryReadSaveDataResult() {
          Code = TryReadSaveDataResultCode.ExceptionThrown,
          SaveData = null,
          Exception = exception,
        };
      }

      public static TryReadSaveDataResult Success(SaveData saveData) {
        return new TryReadSaveDataResult() {
          Code = TryReadSaveDataResultCode.Success,
          SaveData = saveData,
          Exception = null,
        };
      }
    }

    public enum TryReadSaveDataResultCode {
      DoesNotExist,
      ExceptionThrown,
      Success
    }
  }
}
