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

    float _initialWaypointAngle;

    void Awake() {
      _health = _maxHealth;
    }

    void Update() {
      float healthFraction = (float) _health / (float) _maxHealth;
      _healthBar.SetCurrentHealth(healthFraction);
    }

    void FixedUpdate() {
      UpdateMovement();
    }

    public void SetPath(Path path, int pathIndex) {
      Debug.Log("SetPath (index=" + pathIndex + ")");
      _path = path;
      _pathIndex = pathIndex;
      SetVelocityTowardNextWaypoint();
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

    void UpdateMovement() {
      if (_path == null) {
        return;
      }

      if (_pathIndex >= _path.WaypointCount()) {
        OnReachEndOfPath();
        return;
      }

      if (_rigidbody2D.velocity == Vector2.zero) {
        SetVelocityTowardNextWaypoint();
        return;
      }

      // Vector2 currentPos = transform.position;
      // Vector2 nextWaypoint = _path.GetWaypoint(_pathIndex);
      float angle = GetAngleToNextWaypoint();
      float epsilon = 10f;
      Debug.Log("angle: " + angle);
      if (Mathf.Abs(angle - _initialWaypointAngle) > epsilon) {
        Debug.Log("Incrementing _pathIndex");
        _pathIndex += 1;
        _rigidbody2D.velocity = Vector2.zero;
        UpdateMovement();
      }
    }

    void SetVelocityTowardNextWaypoint() {
      Debug.Log("Updating velocity");
      Vector2 currentPos = transform.position;
      Vector2 nextWaypoint = _path.GetWaypoint(_pathIndex);
      _rigidbody2D.velocity = (nextWaypoint - currentPos).normalized * _speed;
      _initialWaypointAngle = GetAngleToNextWaypoint();
      Debug.Log("_initialWaypointAngle: " + _initialWaypointAngle);
    }

    void OnReachEndOfPath() {
      Debug.Log("OnReachEndOfPath");
      _rigidbody2D.velocity = Vector2.zero;
      _path = null;
    }

    float GetAngleToNextWaypoint() {
      Vector2 currentPos = transform.position;
      Vector2 nextWaypoint = _path.GetWaypoint(_pathIndex);
      return Vector2.SignedAngle(currentPos, nextWaypoint);
    }
  }
}
