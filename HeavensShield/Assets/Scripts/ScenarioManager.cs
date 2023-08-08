using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace Mtd {
  public class ScenarioManager: MonoBehaviour {
    [SerializeField] AudioClip _victoryJingle;
    [SerializeField] AudioClip _bigVictoryJingle;
    [SerializeField] AudioClip _defeatJingle;

    [SerializeField] Transform _unitsGroup;
    [SerializeField] Tilemap _tilemap;
    public Transform UnitsGroup => _unitsGroup;

    [SerializeField] Tower _tower;

    List<EnemyController> _livingEnemies = new List<EnemyController>();
    bool _enemySpawningComplete = false;

    public bool IsPaused { get; private set; } = false;
    public float BattleSpeed { get; private set; } = 1f;
    float[] _battleSpeeds = new float[] {1f, 2f, 3f};

    [SerializeField] int _startingMoney = 1100;
    [SerializeField] int _startingScore = 0;

    [SerializeField] AudioClip _music;
    [SerializeField] bool _isBigBattle = false;
    [SerializeField] bool _isFinalBattle = false;

    /**
     * Is this a fake scenario set up only for the purposes of taking a promotional screenshot?
     *
     * Screenshot scenarios lock the timeScale at zero, meaning that they will not play out, even
     * while unpaused.
     *
     * To create a screenshot scenario, play the normal scenario it will be based on, create the
     * desired situation you would like to capture, and then pause the game. Copy the "Units" and
     * "Enemies" hierarchy objects and stop the running game.
     *
     * Duplicate the scenario you were playing and move the clone to the Assets\Scenarios\Debug
     * folder. Open the new scene and paste the Units and Enemies hierarchy objects. Focus the
     * ScenarioManager in the inspector and check the "Is Screenshot Scenario" checkbox. Don't
     * forget to also set the starting money and starting score fields, as well as other properties
     * in the scene, like the tower's health.
     *
     * Notes:
     * - You can click the pause button to change its look, but the scenario will not run!
     * - You can click the battle speed button to change its look, but the scenario will not run!
     * - A unit's exact animation frame will carry over when copied using the process above!
     */
    [SerializeField] bool _isScreenshotScenario = false;
    public bool IsScreenshotScenario => _isScreenshotScenario;

    void Awake() {
      Globals.ScenarioManager.Register(this);
      Globals.PlayerAgent.AddRegisterListener(OnPlayerAgentRegistered);
    }

    void Start() {
      if (_music) {
        Globals.AudioManager.PlayMusic(_music);
      }

      IsPaused = true;
      BattleSpeed = 1f;
      UpdateTimeScale();

      Globals.UI.EnsureReady();

      Globals.LoadedScenario.With(info => {
        Globals.UI.ScenarioNumberOnTapToStartOverlayText.text = FormatChapterAndScenarioNumber(info);
        Globals.UI.ScenarioNameOnTapToStartOverlayText.text = info.Name;
        Globals.UI.ScenarioNameInHUDText.text = info.Name;
      });

      Globals.UI.HUD.ShowInstant();
      Globals.UI.ScenarioUI.UpdateLeftSideButtons(this);
      Globals.UI.ScenarioLeftSideButtons.ShowInstant();
      Globals.UI.ScenarioTapToStartOverlay.ShowInstant();

      Globals.Camera.OnScenarioStart(_tilemap);
    }

    void OnDestroy() {
      Globals.UI.ScoreTallyModal.HideInstant();
      Globals.UI.DefeatBanner.HideInstant();
      Globals.UI.VictoryBanner.HideInstant();
      Globals.UI.ScenarioTapToStartOverlay.HideInstant();
      Globals.UI.ScenarioMenu.HideInstant();
      Globals.UI.UnitSelector.CloseInstant();
      Globals.UI.HUD.HideInstant();
      Globals.UI.ScenarioLeftSideButtons.HideInstant();

      Globals.Camera.OnScenarioEnd();

      Globals.ScenarioManager.Unregister(this);

      IsPaused = false;
      BattleSpeed = 1f;
      UpdateTimeScale();
    }

    void OnPlayerAgentRegistered(PlayerAgent playerAgent) {
      var startingUnits = _unitsGroup.GetComponentsInChildren<Unit>();
      var startingMoneyAfterDebugCost = _startingMoney;
      if (!_isScreenshotScenario) {
        // Do not subtract this if it is a screenshot scenario.
        startingMoneyAfterDebugCost -= startingUnits.Length * 300;
      }
      playerAgent.SetMoney(startingMoneyAfterDebugCost);
      playerAgent.SetScore(_startingScore);
      Globals.PlayerAgent.RemoveRegisterListener(OnPlayerAgentRegistered);
    }

    public void SetScenarioPaused(bool isPaused) {
      IsPaused = isPaused;
      UpdateTimeScale();
    }

    public void ToggleScenarioPaused() {
      SetScenarioPaused(!IsPaused);
    }

    public void SetBattleSpeed(float newSpeed) {
      if (!_battleSpeeds.Contains(newSpeed)) {
        Debug.LogWarning("Expected discreet battle speed of 1f, 2f, or 3f, but received " + newSpeed);
      }
      BattleSpeed = newSpeed;
      UpdateTimeScale();
    }

    /** Cycle through the available battle speeds (1x, 2x, and 3x). */
    public void CycleBattleSpeed() {
      int indexOfCurrentBattleSpeed = Array.IndexOf(_battleSpeeds, BattleSpeed);
      if (indexOfCurrentBattleSpeed >= 0) {
        int indexOfNextBattleSpeed = (indexOfCurrentBattleSpeed + 1) % _battleSpeeds.Length;
        SetBattleSpeed(_battleSpeeds[indexOfNextBattleSpeed]);
      }
      else {
        SetBattleSpeed(_battleSpeeds[0]);
      }
    }

    void UpdateTimeScale() {
      if (_isScreenshotScenario || IsPaused) {
        Time.timeScale = 0f;
      }
      else {
        Time.timeScale = BattleSpeed;
      }
    }

    public void NotifyEnemySpawned(EnemyController enemy) {
      _livingEnemies.Add(enemy);
    }

    public void NotifyEnemyDestroyed(EnemyController enemy) {
      _livingEnemies.Remove(enemy);
      CheckIfPlayerWon();
    }

    public void NotifyEnemySpawningComplete() {
      _enemySpawningComplete = true;
      CheckIfPlayerWon();
    }

    public void NotifyTowerDestroyed() {
      DoScenarioOutcome(false);
    }

    void CheckIfPlayerWon() {
      if (_enemySpawningComplete && _livingEnemies.Count == 0) {
        if (_isFinalBattle) {
          DoFinalBattleScenarioOutcome();
        }
        else {
          DoScenarioOutcome(true);
        }
      }
    }

    async void DoFinalBattleScenarioOutcome() {
      SetScenarioPaused(true);

      var saveData = Globals.GameManager.SaveData;

      Globals.PlayerAgent.With(playerAgent => {
        Globals.LoadedScenario.With(loadedScenario => {
          int towerHP = _tower.Health;
          int money = playerAgent.Money;
          int baseScore = playerAgent.Score;

          int moneyBonus = Mathf.FloorToInt(money / 100f);
          int towerHPBonus = Mathf.FloorToInt(towerHP * 2f);
          int totalScore = Mathf.FloorToInt(baseScore + moneyBonus + towerHPBonus);

          bool newHighScore = saveData.Game.RegisterScore(loadedScenario.ID, totalScore);
          if (newHighScore) {
            // TODO you can do somethin abt it..
          }

          saveData.Game.UnlockScenarios(loadedScenario.Unlocks.ToArray());

          if (saveData.Game.NextStoryScenarioID == loadedScenario.ID) {
            saveData.Game.NextStoryScenarioID += 1;
          }

          Globals.GameManager.WriteSaveData();
        });
      });

      Globals.UI.UnitSelector.CloseInstant();

      var whiteOutFadeTask = Globals.UI.WhiteOutOverlay.Show();
      var musicFadeTask = Globals.AudioManager.FadeCurrentMusic(5f);
      await Task.WhenAll(whiteOutFadeTask, musicFadeTask);

      Globals.GameManager.LoadCreditsScene();
    }

    async void DoScenarioOutcome(bool isVictory) {
      SetScenarioPaused(true);

      var jingle = ChooseJingle(isVictory);
      var saveData = Globals.GameManager.SaveData;
      var banner = isVictory ? Globals.UI.VictoryBanner : Globals.UI.DefeatBanner;

      Globals.PlayerAgent.With(playerAgent => {
        Globals.LoadedScenario.With(loadedScenario => {
          int towerHP = _tower.Health;
          int money = playerAgent.Money;
          int baseScore = playerAgent.Score;

          int moneyBonus = Mathf.FloorToInt(money / 100f);
          int towerHPBonus = Mathf.FloorToInt(towerHP * 2f);
          int totalScore = Mathf.FloorToInt(baseScore + moneyBonus + towerHPBonus);

          if (isVictory) {
            bool newHighScore = saveData.Game.RegisterScore(loadedScenario.ID, totalScore);
            if (newHighScore) {
              // TODO you can do somethin abt it..
            }

            saveData.Game.UnlockScenarios(loadedScenario.Unlocks.ToArray());

            if (saveData.Game.NextStoryScenarioID == loadedScenario.ID) {
              saveData.Game.NextStoryScenarioID += 1;
            }
          }

          Globals.GameManager.WriteSaveData();

          Globals.UI.ScoreTallyModal.SetText(isVictory, towerHP, towerHPBonus, baseScore, money, moneyBonus, totalScore);
        });
      });

      Globals.UI.UnitSelector.CloseInstant();

      bool resumeMusicAfterJingle = isVictory && !_isBigBattle;
      var jingleTask = Globals.AudioManager.PlayJingleEffect(jingle, resumeMusicAfterJingle);
      await banner.Show();
      await jingleTask;

      await Task.WhenAll(
        banner.Hide(),
        Globals.UI.ScoreTallyModal.Show()
      );
    }

    string FormatChapterAndScenarioNumber(OrderedScenarioInfo info) {
      int chapterNumber = info.ChapterID + 1;
      int scenarioNumber = info.IndexInChapter + 1;
      return "Chapter " + chapterNumber + ", Scenario " + scenarioNumber;
    }

    AudioClip ChooseJingle(bool outcomeIsVictory) {
      if (!outcomeIsVictory) {
        return _defeatJingle;
      }
      if (_isBigBattle) {
        return _bigVictoryJingle;
      }
      return _victoryJingle;
    }
  }
}
