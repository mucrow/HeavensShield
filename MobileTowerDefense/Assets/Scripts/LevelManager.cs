using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  public class LevelManager: MonoBehaviour {
    [SerializeField] Transform _enemyFolder;

    [SerializeField] Path _enemyPath;
    [SerializeField] EnemyWave[] _enemyWaves;

    EnemyWave _currentWave;
    int _currentWaveIndex = 0;
    int _currentEnemyIndex = 0;

    float _timer = 0f;

    enum State {
      NotStarted,
      SpawningWave,
      FinishingWave,
      FinishedAllWaves,
    }
    State _state = State.NotStarted;

    void Update() {
      if (_state == State.FinishedAllWaves) {
        return;
      }
      _timer -= Time.deltaTime;

      if (_state == State.SpawningWave) {
        DoSpawningWave();
      }
      else if (_state == State.FinishingWave) {
        DoFinishingWave();
      }
      else if (_state == State.NotStarted) {
        StartWave(0);
      }
    }

    void DoFinishingWave() {
      if (_timer <= 0f) {
        StartWave(_currentWaveIndex + 1);
      }
    }

    void DoSpawningWave() {
      if (_timer <= 0f) {
        SpawnEnemy();
        _currentEnemyIndex += 1;
        if (_currentEnemyIndex < _currentWave.EnemyCount) {
          _timer += _currentWave.WaitTimeBetweenEnemies;
        }
        else {
          _timer += _currentWave.WaitTimeAfterLastEnemy;
          _state = State.FinishingWave;
        }
      }
    }

    void StartWave(int waveNumber) {
      _currentWaveIndex = waveNumber;
      if (_currentWaveIndex < _enemyWaves.Length) {
        _currentWave = _enemyWaves[_currentWaveIndex];
        _currentEnemyIndex = 0;
        _timer += _currentWave.WaitTimeBeforeFirstEnemy;
        _state = State.SpawningWave;
      }
      else {
        _state = State.FinishedAllWaves;
      }
    }

    void SpawnEnemy() {
      var prefab = _currentWave.EnemyPrefab;
      var startPosition = _enemyPath.GetWaypoint(0);
      SpawnEnemy(prefab, startPosition, _enemyPath, 1);
    }

    void SpawnEnemy(GameObject prefab, Vector3 startPosition, Path path, int pathIndex) {
      GameObject newEnemyObject = Instantiate(prefab, startPosition, Quaternion.identity, _enemyFolder);
      var enemy = newEnemyObject.GetComponent<EnemyController>();
      enemy.SetPath(path, pathIndex);
    }
  }
}
