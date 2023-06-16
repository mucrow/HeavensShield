using System.Collections;
using System.Collections.Generic;
using Mtd.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Mtd {
  public class Globals: MonoBehaviour {
    /** Never null during or after Start() */
    public static AudioManager AudioManager;

    /** Never null during or after Start() */
    public static CameraController Camera;

    /** Never null during or after Start() */
    public static GameManager GameManager;

    /** Never null during or after Start() */
    public static MtdInput Input;

    public static readonly Proxy<PlayerAgent> PlayerAgent = new Proxy<PlayerAgent>();
    public static readonly Proxy<OrderedScenarioInfo> LoadedScenario = new Proxy<OrderedScenarioInfo>();
    public static readonly Proxy<ScenarioManager> ScenarioManager = new Proxy<ScenarioManager>();

    public static ScenarioSceneQuitTarget ScenarioSceneQuitTarget = ScenarioSceneQuitTarget.MainMenu;

    /** Never null during or after Start() */
    public static ScenarioOrder ScenarioOrder;

    /** Never null during or after Start() */
    public static MtdUI UI;

    static Globals _instance;

    [SerializeField] AudioManager _audioManager;
    [SerializeField] CameraController _camera;
    [SerializeField] GameManager _gameManager;
    [SerializeField] MtdInput _input;
    [SerializeField] ScenarioOrder _scenarioOrder;
    [SerializeField] MtdUI _ui;

    PlayerAgent _playerAgent;

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
      AudioManager = _audioManager;
      Camera = _camera;
      GameManager = _gameManager;
      Input = _input;
      ScenarioOrder = _scenarioOrder;
      UI = _ui;
    }
  }
}
