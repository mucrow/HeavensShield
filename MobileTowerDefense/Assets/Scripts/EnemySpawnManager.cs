using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  public class EnemySpawnManager: MonoBehaviour {
    [SerializeField] Transform _enemyFolder;
    [SerializeField] GameObject _enemyPathTriggerPrefab;
    [SerializeField] Transform _enemyPathTriggersFolder;

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

    void Awake() {
      SetupEnemyPathTriggers();
    }

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

    void SetupEnemyPathTriggers() {
      var waypointCount = _enemyPath.WaypointCount();
      if (waypointCount < 2) {
        return;
      }
      var previousWaypoint = _enemyPath.GetWaypoint(0);
      for (int i = 1; i < waypointCount; ++i) {
        var currentWaypoint = _enemyPath.GetWaypoint(i);
        CreateTriggerBetweenWaypoints(previousWaypoint, currentWaypoint);
        previousWaypoint = currentWaypoint;
      }
    }

    void CreateTriggerBetweenWaypoints(Vector3 a, Vector3 b) {
      // we expect two continuous waypoints to differ in exactly one coordinate. in other words,
      // there are only two valid cases for a and b:
      // - a and b have the same X and different Ys
      // - a and b have the same Y and different Xs
      //
      if ((a.x == b.x) == (a.y == b.y)) {
        Debug.LogWarning("Vector between waypoints has an angle other than 0, 90, 180, or 270. EnemyPathTrigger will likely be wrong.");
      }
      var position = new Vector3((a.x + b.x) / 2, (a.y + b.y) / 2, 0);
      var colliderSize = new Vector2(Mathf.Abs(b.x - a.x) + 1, Mathf.Abs(b.y - a.y) + 1);
      CreateTriggerWithPositionAndSize(position, colliderSize);
    }

    void CreateTriggerWithPositionAndSize(Vector3 position, Vector2 size) {
      var newTrigger = Instantiate(
        _enemyPathTriggerPrefab,
        position,
        Quaternion.identity,
        _enemyPathTriggersFolder
      );
      newTrigger.GetComponent<BoxCollider2D>().size = size;
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
