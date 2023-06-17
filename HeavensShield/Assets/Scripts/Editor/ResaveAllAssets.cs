// from https://forum.unity.com/threads/serialized-variable-lost-its-value-after-removing-formerserializedas.621055/#post-4160230

using UnityEditor;
using UnityEngine;

namespace Mtd.Editor {
  public class ResaveAllAssets: UnityEditor.Editor {
    [MenuItem("Assets/Custom Scripts/Resave All Assets (Use New Property Names)")]
    static void Execute() {
      string[] assets = AssetDatabase.FindAssets("");

      Debug.Log($"Re-saving {assets.Length} assets");
      foreach (string guid in assets) {
        string path = AssetDatabase.GUIDToAssetPath(guid);
        var asset = AssetDatabase.LoadAssetAtPath<Object>(path);
        EditorUtility.SetDirty(asset);
      }

      AssetDatabase.SaveAssets();
    }
  }
}
