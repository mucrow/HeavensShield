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
    [SerializeField] Transform _unitsGroup;
    [SerializeField] Tilemap _tilemap;
    public Transform UnitsGroup => _unitsGroup;

    List<EnemyController> _livingEnemies = new List<EnemyController>();
    bool _enemySpawningComplete = false;

    public bool IsPaused { get; private set; } = false;
    public float BattleSpeed { get; private set; } = 1f;
    float[] _battleSpeeds = new float[] {1f, 2f, 3f};

    [FormerlySerializedAs("_startingCash")] [SerializeField] int _startingMoney = 1100;

    void Awake() {
      Globals.ScenarioManager.Register(this);
      Globals.PlayerAgent.AddRegisterListener(OnPlayerAgentRegistered);
    }

    void Start() {
      IsPaused = true;
      BattleSpeed = 1f;
      UpdateTimeScale();

      Globals.UI.EnsureReady();
      Globals.UI.HUD.ShowInstant();
      Globals.UI.ScenarioUI.UpdateLeftSideButtons(this);
      Globals.UI.ScenarioLeftSideButtons.ShowInstant();
      Globals.UI.ScenarioTapToStartOverlay.ShowInstant();

      Globals.Camera.OnScenarioStart(_tilemap);
    }

    void OnDestroy() {
      Globals.UI.ScoreTallyModal.HideInstant();
      Globals.UI.VictoryBanner.HideInstant();
      Globals.UI.ScenarioTapToStartOverlay.HideInstant();
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
      playerAgent.SetMoney(_startingMoney);
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
      if (IsPaused) {
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

    async void CheckIfPlayerWon() {
      if (_enemySpawningComplete && _livingEnemies.Count == 0) {
        Globals.PlayerAgent.With(playerAgent => {
          Globals.LoadedScenario.With(loadedScenario => {
            var saveData = Globals.GameManager.SaveData;
            // TODO this is stubbed with a dummy value
            float towerHealth = 100f;

            float money = playerAgent.Money;
            float score = playerAgent.Score;

            float moneyPC = money / 100f;
            float scorePC = score / 1000f;
            float towerPC = towerHealth * 2f;

            saveData.Game.PoliticalCapital += moneyPC;
            saveData.Game.PoliticalCapital += scorePC;
            saveData.Game.PoliticalCapital += towerPC;
            saveData.Game.UnlockScenarios(loadedScenario.Unlocks.ToArray());

            if (saveData.Game.NextStoryScenarioID == loadedScenario.ID) {
              saveData.Game.NextStoryScenarioID += 1;
            }

            Globals.UI.ScoreTallyModal.SetEarnings(towerHealth, towerPC, score, scorePC, money, moneyPC);
          });
        });
        Globals.GameManager.WriteSaveData();

        Globals.UI.UnitSelector.CloseInstant();
        Globals.UI.VictoryBanner.ShowInstant();
        await Task.Delay(2000);

        await Task.WhenAll(
          Globals.UI.VictoryBanner.Hide(),
          Globals.UI.ScoreTallyModal.Show()
        );
      }
    }
  }
}
