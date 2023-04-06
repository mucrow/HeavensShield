using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Mtd {
  public class ScenarioManager: MonoBehaviour {
    List<EnemyController> _livingEnemies = new List<EnemyController>();
    bool _enemySpawningComplete = false;

    public bool IsPaused { get; private set; } = false;
    public float BattleSpeed { get; private set; } = 1f;
    float[] _battleSpeeds = new float[] {1f, 2f, 3f};

    void Awake() {
      Globals.ScenarioManager.Register(this);
    }

    void Start() {
      Time.timeScale = 1f;
      Globals.UI.HUD.ShowInstant();
      Globals.UI.ScenarioUI.UpdateLeftSideButtons(this);
      Globals.UI.ScenarioLeftSideButtons.ShowInstant();
    }

    void OnDestroy() {
      Globals.UI.UnitSelector.CloseInstant();
      Globals.UI.HUD.HideInstant();
      Globals.UI.ScenarioLeftSideButtons.HideInstant();
      Globals.ScenarioManager.Unregister(this);
      Time.timeScale = 1f;
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
      enemy.SetScenarioManager(this);
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
        Debug.Log("u won! (showing win banner)");

        // show win banner
        await Task.Delay(1000);

        Debug.Log("showing score dialog");

        Globals.UI.UnitSelector.CloseInstant();

        // bring up score dialog
        // play animation that converts gold/score/tower health into political capital
        // buttons to play next scenario, replay the current scenario, or go back to main menu
        Globals.PlayerAgent.With(playerAgent => {
          Globals.LoadedScenario.With(loadedScenario => {
            var saveData = Globals.GameManager.SaveData;
            // TODO this is stubbed with a dummy value
            float towerHealth = 100f;
            saveData.Game.PoliticalCapital += playerAgent.Money / 300f;
            saveData.Game.PoliticalCapital += playerAgent.Score / 100f;
            saveData.Game.PoliticalCapital += towerHealth / 2f;
            saveData.Game.UnlockScenarios(loadedScenario.Unlocks.ToArray());
            if (saveData.Game.NextStoryScenarioID == loadedScenario.ID) {
              saveData.Game.NextStoryScenarioID += 1;
            }
          });
        });
        Debug.Log("Political Capital: " + Globals.GameManager.SaveData.Game.PoliticalCapital);
        Debug.Log("Unlocked Scenarios: " + string.Join(", ", Globals.GameManager.SaveData.Game.UnlockedScenarioIDs));

        Globals.GameManager.WriteSaveData();
        Debug.Log("writing save data");

        await Task.Delay(1000);
        Debug.Log("going back to main menu");

        Globals.GameManager.LoadMainMenuScene();
      }
    }
  }
}
