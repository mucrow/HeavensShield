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
    public static GameManager GameManager;

    /** Never null during or after Start() in a level, but probably null in the start menu. */
    public static PlayerAgent Player;

    /** Never null during or after Start() */
    public static MtdUI UI;

    static Globals _instance;

    [SerializeField] CameraController _camera;
    [SerializeField] GameManager _gameManager;
    [SerializeField] PlayerAgent _player;
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
      GameManager = _gameManager;
      Player = _player;
      UI = _ui;
    }
  }
}
