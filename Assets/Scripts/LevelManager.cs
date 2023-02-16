using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  public class LevelManager: MonoBehaviour {
    [SerializeField] Transform _enemyFolder;

    [SerializeField] Path _enemyPath;
    [SerializeField] EnemyWave[] _enemyWaves;

    int _currentWaveIndex = 0;
    int _currentEnemyIndex = 0;

    float _spawnTimer;

    void Update() {
      DoSpawning();
    }

    void DoSpawning() {
      _spawnTimer -= Time.deltaTime;
      if (_spawnTimer <= 0f) {
        SpawnEnemy();
        _spawnTimer += _enemyWaves[_currentWaveIndex].TimeBetweenEnemies;
      }
    }

    void SpawnEnemy() {
      var currentWave = _enemyWaves[_currentWaveIndex];
      var prefab = currentWave.EnemyPrefab;
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
