using System;
using Mtd.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Mtd {
  public class SimpleUISceneManager: MonoBehaviour {
    [SerializeField] AudioClip _music;
    [SerializeField] bool _shouldLoop = true;
    [SerializeField] UIObjectKey[] _uiObjectKeys;

    [SerializeField] UnityEvent _onUIReady;

    void Start() {
      if (_music) {
        Globals.AudioManager.PlayMusic(_music, _shouldLoop);
      }
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
