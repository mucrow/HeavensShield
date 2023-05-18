using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mtd {
  public class EnemyController: MonoBehaviour {
    [SerializeField] Rigidbody2D _rigidbody2D;

    // TODO consider renaming these to "default speed"
    [SerializeField] float _speed = 2f;
    public float Speed => _speed;

    Path _path;
    ScenarioManager _scenarioManager;

    int _pathIndex = 0;

    // TODO consider renaming these to "default max health"
    [SerializeField] int _maxHealth = 50;
    public int MaxHealth => _maxHealth;
    int _health;

    [SerializeField] int _moneyOnKill = 17;
    [SerializeField] int _scoreOnKill = 21;

    [SerializeField] int _defaultEnemyCount = 10;
    public int DefaultEnemyCount => _defaultEnemyCount;
    [SerializeField] float _defaultWaitTimeBetweenEach = 1.5f;
    public float DefaultWaitTimeBetweenEach => _defaultWaitTimeBetweenEach;
    [SerializeField] float _defaultWaitTimeBeforeWave = 5f;
    public float DefaultWaitTimeBeforeWave => _defaultWaitTimeBeforeWave;
    [SerializeField] float _defaultWaitTimeAfterWave = 10f;
    public float DefaultWaitTimeAfterWave => _defaultWaitTimeAfterWave;

    [SerializeField] HealthBar _healthBar;

    void Awake() {
      _health = _maxHealth;
    }

    void Update() {
      float healthFraction = (float) _health / (float) _maxHealth;
      _healthBar.SetCurrentHealth(healthFraction);
    }

    void FixedUpdate() {
      MoveTowardNextWaypoint();
    }

    public void SetPath(Path path, int pathIndex) {
      _path = path;
      _pathIndex = pathIndex;
    }

    public void SetScenarioManager(ScenarioManager scenarioManager) {
      _scenarioManager = scenarioManager;
    }

    /**
     * Intended for use when an enemy is first spawned. Sets enemy health to max health after
     * modification.
     */
    public void SetMaxHealth(int newMaxHealth) {
      _maxHealth = newMaxHealth;
      _health = newMaxHealth;
    }
    
    /**
     * Intended for use when an enemy is first spawned. Does not update velocity.
     */
    public void SetSpeed(float newSpeed) {
      _speed = newSpeed;
    }

    public void ReceiveDamage(int amount) {
      _health -= amount;
      if (_health <= 0) {
        _health = 0;
        Globals.PlayerAgent.With(playerAgent => {
          playerAgent.AddMoney(_moneyOnKill);
          playerAgent.AddToScore(_scoreOnKill);
        });
        _scenarioManager.NotifyEnemyDestroyed(this);
        Destroy(gameObject);
      }
    }

    void MoveTowardNextWaypoint() {
      if (_path == null) {
        return;
      }

      if (_pathIndex >= _path.WaypointCount()) {
        OnReachEndOfPath();
        return;
      }

      Vector3 currentPos = transform.position;
      Vector3 nextWaypoint = _path.GetWaypoint(_pathIndex);
      if ((nextWaypoint - currentPos).magnitude < 0.05f) {
        _pathIndex += 1;
        MoveTowardNextWaypoint();
        return;
      }

      // float dt = Time.deltaTime;
      // transform.position = Vector3.MoveTowards(currentPos, nextWaypoint, _speed * dt);
      _rigidbody2D.velocity = (nextWaypoint - currentPos).normalized * _speed;
    }

    void OnReachEndOfPath() {
      _rigidbody2D.velocity = Vector2.zero;
      _path = null;
    }
  }
}
