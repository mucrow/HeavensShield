using System;
using Mtd.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Mtd {
  public class SimpleUISceneManager: MonoBehaviour {
    [SerializeField] AudioClip _music;
    [SerializeField] UIObjectKey[] _uiObjectKeys;

    [SerializeField] UnityEvent _onUIReady;

    void Start() {
      Globals.AudioManager.PlayMusic(_music);
      Globals.UI.EnsureReady();
      _onUIReady.Invoke();
      foreach (UIObjectKey uiObjectKey in _uiObjectKeys) {
        var component = Globals.UI.GetObjectByKey(uiObjectKey);
        component.ShowInstant();
      }
    }

    void OnDestroy() {
      foreach (UIObjectKey uiObjectKey in _uiObjectKeys) {
        var component = Globals.UI.GetObjectByKey(uiObjectKey);
        component.HideInstant();
      }
    }
  }
}
