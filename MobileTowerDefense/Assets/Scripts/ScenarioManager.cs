using System;
using System.Collections;
using System.Collections.Generic;
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

    public void CheckIfPlayerWon() {
      if (_enemySpawningComplete && _livingEnemies.Count == 0) {
        Debug.Log("u won!");
      }
    }
  }
}
