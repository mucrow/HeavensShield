using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Mtd {
  public class ScenarioManager: MonoBehaviour {
    List<EnemyController> _livingEnemies = new List<EnemyController>();
    bool _enemySpawningComplete = false;

    void Start() {
      Globals.UI.HUD.ShowInstant();
      Globals.UI.ScenarioLeftSideButtons.ShowInstant();
    }

    void OnDestroy() {
      Globals.UI.ScenarioLeftSideButtons.HideInstant();
      Globals.UI.HUD.HideInstant();
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

        // bring up score dialog
        // play animation that converts gold/score/tower health into political capital
        // buttons to play next scenario, replay the current scenario, or go back to main menu
        Globals.PlayerAgent.With(playerAgent => {
          // TODO this is stubbed with a dummy value
          float towerHealth = 100f;
          // TODO this is stubbed with a dummy value
          var unlockedScenarioIDs = new List<int>() { 1 };

          Globals.GameManager.SaveData.Game.PoliticalCapital += playerAgent.Money / 300f;
          Globals.GameManager.SaveData.Game.PoliticalCapital += playerAgent.Score / 100f;
          Globals.GameManager.SaveData.Game.PoliticalCapital += towerHealth / 2f;
          Globals.GameManager.SaveData.Game.UnlockedScenarioIDs.AddRange(unlockedScenarioIDs);
        });
        Debug.Log("Political Capital: " + Globals.GameManager.SaveData.Game.PoliticalCapital);
        Debug.Log("Unlocked Scenarios: " + string.Join(", ", Globals.GameManager.SaveData.Game.UnlockedScenarioIDs));

        await Globals.GameManager.WriteSaveData();
        Debug.Log("writing save data");

        await Task.Delay(1000);
        Debug.Log("going back to main menu");

        Globals.GameManager.LoadScene("Scenes/MainMenu");
      }
    }
  }
}
