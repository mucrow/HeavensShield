using System;
using Mtd.UI;
using UnityEngine;

namespace Mtd {
  public class SimpleUISceneManager: MonoBehaviour {
    [SerializeField] UIObjectKey[] _uiObjectKeys;

    void Start() {
      Globals.UI.EnsureReady();
      foreach (UIObjectKey uiObjectKey in _uiObjectKeys) {
        var component = Globals.UI.GetObjectByKey(uiObjectKey);
        component.Show();
      }
    }

    void OnDestroy() {
      foreach (UIObjectKey uiObjectKey in _uiObjectKeys) {
        var component = Globals.UI.GetObjectByKey(uiObjectKey);
        component.Hide();
      }
    }
  }
}
