using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  public class EnemySpawnManager: MonoBehaviour {
    [SerializeField] ScenarioManager _scenarioManager;
    [SerializeField] Transform _enemyFolder;
    [SerializeField] GameObject _enemyPathTriggerPrefab;
    [SerializeField] Transform _enemyPathTriggersFolder;

    [SerializeField] Path _enemyPath;

    [Header("Use -1 if you don't want to override.")]
    [SerializeField] float _overrideWaitTimeBeforeFirstWave = 15f;
    [Header("Use -1 if you don't want to override.")]
    [SerializeField] float _overrideWaitTimeAfterLastWave = 2f;
    [SerializeField] EnemyWave[] _enemyWaves;

    [SerializeField] float _startingHealthScaling = 1f;
    [SerializeField] float _healthScalingPerMinute = 0.75f;
    [SerializeField] float _maxHealthScaling = 20f;
    float _healthScaling;

    [Header("uh...don't actually do speed scaling")]
    [SerializeField] float _startingSpeedScaling = 1f;
    [Header("enemies can get stuck at waypoints if")]
    [SerializeField] float _speedScalingPerMinute = 0f;
    [Header("they're moving too fast. youve been warned")]
    [SerializeField] float _maxSpeedScaling = 1f;
    float _speedScaling;

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
      _healthScaling = _startingHealthScaling;
      _speedScaling = _startingSpeedScaling;
      SetupEnemyPathTriggers();
    }

    void Update() {
      if (_state == State.FinishedAllWaves) {
        return;
      }
      float dt = Time.deltaTime;
      _timer -= dt;
      if (_state != State.NotStarted) {
        UpdateScaling(dt);
      }

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

    void UpdateScaling(float dt) {
      var healthScalingPerSecond = _healthScalingPerMinute / 60f;
      _healthScaling = Mathf.Min(_healthScaling + healthScalingPerSecond * dt, _maxHealthScaling);
      var speedScalingPerSecond = _speedScalingPerMinute / 60f;
      _speedScaling = Mathf.Min(_speedScaling + speedScalingPerSecond * dt, _maxSpeedScaling);
    }

    // TODO maybe this should be in ScenarioManager
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
        // TODO this feels gross (same if-condition is a few lines down)
        //      i should really add a StartingWave state to make this tidy
        if (_currentEnemyIndex < _currentWave.EnemyCount) {
          SpawnEnemy();
          _currentEnemyIndex += 1;
        }

        if (_currentEnemyIndex < _currentWave.EnemyCount) {
          _timer += _currentWave.WaitTimeBetweenEnemies;
        }
        else {
          bool isLastWave = _currentWaveIndex + 1 >= _enemyWaves.Length;
          float waitTime = _currentWave.WaitTimeAfterLastEnemy;
          if (isLastWave && _overrideWaitTimeAfterLastWave >= 0) {
            waitTime = _overrideWaitTimeAfterLastWave;
          }
          _timer += waitTime;

          _state = State.FinishingWave;
        }
      }
    }

    void StartWave(int waveNumber) {
      _currentWaveIndex = waveNumber;
      if (_currentWaveIndex < _enemyWaves.Length) {
        _currentWave = _enemyWaves[_currentWaveIndex];
        // TODO gross
        if (_currentWave.DisableWave) {
          StartWave(_currentWaveIndex + 1);
          return;
        }
        _currentEnemyIndex = 0;

        float waitTime = _currentWave.WaitTimeBeforeFirstEnemy;
        if (waveNumber == 0 && _overrideWaitTimeBeforeFirstWave >= 0) {
          waitTime = _overrideWaitTimeBeforeFirstWave;
        }
        _timer += waitTime;

        _state = State.SpawningWave;
      }
      else {
        _state = State.FinishedAllWaves;
        _scenarioManager.NotifyEnemySpawningComplete();
      }
    }

    void SpawnEnemy() {
      var startPosition = _enemyPath.GetWaypoint(0);
      SpawnEnemy(_currentWave, startPosition, _enemyPath, 1);
    }

    void SpawnEnemy(EnemyWave wave, Vector3 startPosition, Path path, int pathIndex) {
      GameObject newEnemyObject = Instantiate(wave.EnemyPrefab, startPosition, Quaternion.identity, _enemyFolder);
      var enemy = newEnemyObject.GetComponent<EnemyController>();
      enemy.SetMaxHealth(Mathf.RoundToInt(wave.EnemyMaxHealth * _healthScaling));
      enemy.SetSpeed(wave.EnemySpeed * _speedScaling);
      enemy.SetPath(path, pathIndex);
      _scenarioManager.NotifyEnemySpawned(enemy);
    }
  }
}
