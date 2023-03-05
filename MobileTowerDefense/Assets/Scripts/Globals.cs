using System.Collections;
using System.Collections.Generic;
using Mtd.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mtd {
  public class Globals: MonoBehaviour {
    /** Never null during or after Start() */
    public static CameraController Camera;

    /** Never null during or after Start() */
    public static MtdUI UI;

    static Globals _instance;

    [SerializeField] CameraController _camera;
    [SerializeField] MtdUI _ui;

    void Awake() {
      if (_instance == null) {
        DontDestroyOnLoad(gameObject);
        _instance = this;
        DoGlobalsAwake();
      }
      else {
        Destroy(gameObject);
      }
    }

    /** the part of Awake() that only runs if we are the singleton instance of Globals */
    void DoGlobalsAwake() {
      ExposeFields();
    }

    void ExposeFields() {
      Camera = _camera;
      UI = _ui;
    }
  }
}
