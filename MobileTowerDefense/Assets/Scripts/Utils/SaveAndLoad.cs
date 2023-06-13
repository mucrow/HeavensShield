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

    /** Deletes existing save data and returns newly initialized save data. */
    public static ClearSaveDataResult ClearSaveData() {
      try {
        var saveFilePath = GetSaveFilePath();
        if (System.IO.File.Exists(saveFilePath)) {
          System.IO.File.Delete(saveFilePath);
          return ClearSaveDataResult.DataCleared();
        }
        return ClearSaveDataResult.NoDataToClear();
      }
      catch (Exception e) {
        return ClearSaveDataResult.ExceptionThrown(e);
      }
    }

    static string GetSaveFilePath() {
      return System.IO.Path.Combine(Application.persistentDataPath, "SaveData.json");
    }

    public class ClearSaveDataResult {
      public ClearSaveDataResultCode Code;
      public SaveData NewSaveData;
      public Exception Exception;

      private ClearSaveDataResult() {}

      public static ClearSaveDataResult DataCleared() {
        return new ClearSaveDataResult() {
          Code = ClearSaveDataResultCode.DataCleared,
          NewSaveData = new SaveData(),
          Exception = null,
        };
      }

      public static ClearSaveDataResult ExceptionThrown(Exception exception) {
        return new ClearSaveDataResult() {
          Code = ClearSaveDataResultCode.ExceptionThrown,
          NewSaveData = null,
          Exception = exception,
        };
      }

      public static ClearSaveDataResult NoDataToClear() {
        return new ClearSaveDataResult() {
          Code = ClearSaveDataResultCode.NoDataToClear,
          NewSaveData = new SaveData(),
          Exception = null,
        };
      }
    }

    public enum ClearSaveDataResultCode {
      DataCleared,
      ExceptionThrown,
      NoDataToClear,
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
