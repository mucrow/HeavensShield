using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Properties;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mtd.UI {
  public class MtdUI: MonoBehaviour {
    [SerializeField] GameObject _eventSystemPrefab;
    [SerializeField] GraphicRaycaster _uiGraphicRaycaster;

    [SerializeField] HUD _hud;
    public HUD HUD => _hud;

    [SerializeField] UnitSelector _unitSelector;
    public UnitSelector UnitSelector => _unitSelector;

    [SerializeField] ShowHideUIElement _scenarioLeftSideButtons;
    public ShowHideUIElement ScenarioLeftSideButtons => _scenarioLeftSideButtons;

    [SerializeField] ShowHideUIElement _scenarioTapToStartOverlay;
    public ShowHideUIElement ScenarioTapToStartOverlay => _scenarioTapToStartOverlay;

    [SerializeField] ScenarioUI _scenarioUI;
    public ScenarioUI ScenarioUI => _scenarioUI;

    [SerializeField] ShowHideUIElement _scenarioMenu;
    public ShowHideUIElement ScenarioMenu => _scenarioMenu;

    [SerializeField] ShowHideUIElement _victoryBanner;
    public ShowHideUIElement VictoryBanner => _victoryBanner;

    [SerializeField] ScoreTallyModal _scoreTallyModal;
    public ScoreTallyModal ScoreTallyModal => _scoreTallyModal;

    [SerializeField] ShowHideUIElement _defeatBanner;
    public ShowHideUIElement DefeatBanner => _defeatBanner;

    [SerializeField] ScenarioSelection _scenarioSelection;
    public ScenarioSelection ScenarioSelection => _scenarioSelection;

    [SerializeField] TMP_Text _startMenuVersionText;
    public TMP_Text StartMenuVersionText => _startMenuVersionText;

    [SerializeField] TMP_Text _settingsMenuMusicVolumeText;
    public TMP_Text SettingsMenuMusicVolumeText => _settingsMenuMusicVolumeText;
    [SerializeField] TMP_Text _settingsMenuSoundEffectsVolumeText;
    public TMP_Text SettingsMenuSoundEffectsVolumeText => _settingsMenuSoundEffectsVolumeText;

    [SerializeField] ShowHideUIElement _startMenu;
    [SerializeField] ShowHideUIElement _settingsMenu;
    [SerializeField] ShowHideUIElement _mainMenu;

    EventSystem _eventSystem;
    bool _isWholeUIReady = false;

    void Start() {
      var eventSystemObject = Instantiate(_eventSystemPrefab, Vector3.zero, Quaternion.identity, transform);
      _eventSystem = eventSystemObject.GetComponent<EventSystem>();
      EnsureReady();
    }

    /** Check if the UI currently covers the given screen point. */
    public bool DoesUICoverScreenPoint(Vector2 point) {
      var pointerEventData = new PointerEventData(_eventSystem) {
        position = point
      };
      List<RaycastResult> results = new List<RaycastResult>();
      _uiGraphicRaycaster.Raycast(pointerEventData, results);
      return results.Count > 0;
    }

    /**
     * Ensures the UI is ready for interaction.
     *
     * Usually called in Start(), cannot be called in Awake().
     *
     * The first call to this method is slow, but after the first call completes successfully,
     * subsequent calls are cheap.
     */
    public void EnsureReady() {
      if (_isWholeUIReady) {
        return;
      }
      var components = FindObjectsOfType<MonoBehaviour>().OfType<IEnsureReady>();
      foreach (IEnsureReady component in components) {
        component.EnsureReady();
      }
      _isWholeUIReady = true;
    }

    public ShowHideUIElement GetObjectByKey(UIObjectKey key) {
      switch (key) {
        case UIObjectKey.StartMenu:
          return _startMenu;
        case UIObjectKey.SettingsMenu:
          return _settingsMenu;
        case UIObjectKey.MainMenu:
          return _mainMenu;
      }
      throw new Exception("Unhandled UIObjectKey " + key);
    }
  }

  public enum UIObjectKey {
    StartMenu = 0,
    SettingsMenu = 1,
    MainMenu = 2,
  }
}
