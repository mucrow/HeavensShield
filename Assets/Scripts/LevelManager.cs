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

    float _spawnTimer;

    void Awake() {
      UpdateCurrentWave();
    }

    void Update() {
      DoSpawning();
    }

    void DoSpawning() {
      if (!_currentWave) {
        return;
      }

      _spawnTimer -= Time.deltaTime;
      if (_spawnTimer <= 0f) {
        SpawnEnemy();
        _spawnTimer += _currentWave.TimeBetweenEnemies;
        UpdateEnemyIndex();
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

    void UpdateEnemyIndex() {
      _currentEnemyIndex += 1;
      if (_currentEnemyIndex >= _currentWave.EnemyCount) {
        _currentWaveIndex += 1;
        UpdateCurrentWave();
        _currentEnemyIndex = 0;
      }
    }

    void UpdateCurrentWave() {
      if (_currentWaveIndex < _enemyWaves.Length) {
        _currentWave = _enemyWaves[_currentWaveIndex];
      }
      else {
        _currentWave = null;
      }
    }
  }
}
