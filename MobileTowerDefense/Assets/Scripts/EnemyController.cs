using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mtd {
  public class EnemyController: MonoBehaviour {
    [SerializeField] Rigidbody2D _rigidbody2D;
    [SerializeField] SpriteRenderer _spriteRenderer;

    // TODO consider renaming these to "default speed"
    [SerializeField] float _speed = 2f;
    public float Speed => _speed;

    Path _path;

    int _pathIndex = 0;

    // TODO consider renaming these to "default max health"
    [SerializeField] int _maxHealth = 50;
    public int MaxHealth => _maxHealth;
    int _health;

    [SerializeField] int _moneyOnKill = 17;
    [SerializeField] int _scoreOnKill = 21;

    [SerializeField] int _damage = 1;
    public int Damage => _damage;

    [SerializeField] float _durationBetweenAttacks = 0.64f;

    [SerializeField] int _defaultEnemyCount = 10;
    public int DefaultEnemyCount => _defaultEnemyCount;
    [SerializeField] float _defaultWaitTimeBetweenEach = 1.5f;
    public float DefaultWaitTimeBetweenEach => _defaultWaitTimeBetweenEach;
    [SerializeField] float _defaultWaitTimeBeforeWave = 5f;
    public float DefaultWaitTimeBeforeWave => _defaultWaitTimeBeforeWave;
    [SerializeField] float _defaultWaitTimeAfterWave = 10f;
    public float DefaultWaitTimeAfterWave => _defaultWaitTimeAfterWave;

    [SerializeField] HealthBar _healthBar;

    float _attackTimer;
    Tower _targetTower = null;

    void Awake() {
      _health = _maxHealth;
    }

    void Start() {
      UpdateHealthBar();
    }

    void Update() {
      if (_targetTower) {
        _attackTimer -= Time.deltaTime;
        if (_attackTimer <= 0f) {
          _targetTower.ReceiveDamage(_damage);
          _attackTimer += _durationBetweenAttacks;
        }
      }
    }

    void FixedUpdate() {
      MoveTowardNextWaypoint();
    }

    void OnTriggerEnter2D(Collider2D other) {
      if (other.CompareTag("Tower")) {
        _targetTower = other.GetComponent<Tower>();
        _targetTower.ReceiveDamage(_damage);
        _attackTimer = _durationBetweenAttacks;
      }
    }

    void OnTriggerExit2D(Collider2D other) {
      if (_targetTower && other.gameObject == _targetTower.gameObject) {
        _targetTower = null;
      }
    }

    public void SetPath(Path path, int pathIndex) {
      _path = path;
      _pathIndex = pathIndex;
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
      _health = Mathf.Max(_health - amount, 0);
      UpdateHealthBar();
      if (_health <= 0) {
        Globals.PlayerAgent.With(playerAgent => {
          playerAgent.AddMoney(_moneyOnKill);
          playerAgent.AddToScore(_scoreOnKill);
        });
        Globals.ScenarioManager.With(scenarioManager => {
          scenarioManager.NotifyEnemyDestroyed(this);
        });
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
      float waypointProximityThreshold = 0.05f * (_speed / 2f) * (Time.timeScale / 2f);
      if ((nextWaypoint - currentPos).magnitude < waypointProximityThreshold) {
        // transform.position = nextWaypoint;
        _pathIndex += 1;
        MoveTowardNextWaypoint();
        return;
      }

      // float dt = Time.deltaTime;
      // transform.position = Vector3.MoveTowards(currentPos, nextWaypoint, _speed * dt);
      var nextVelocity = (nextWaypoint - currentPos).normalized * _speed;
      _rigidbody2D.velocity = nextVelocity;
      if (nextVelocity.x < 0) {
        _spriteRenderer.flipX = true;
      }
    }

    void OnReachEndOfPath() {
      _rigidbody2D.velocity = Vector2.zero;
      _path = null;
    }

    float GetWaypointProximityThreshold() {
      // float speedFactor = Mathf.Max(_speed / 2f, 1f);
      // float timeFactor = Mathf.Max(Time.timeScale / 2f, 1f);
      float speedFactor = _speed / 2f;
      float timeFactor = Time.timeScale / 2f;
      return 0.05f * speedFactor * timeFactor;
    }

    void UpdateHealthBar() {
      float healthFraction = (float) _health / (float) _maxHealth;
      _healthBar.SetCurrentHealth(healthFraction);
    }
  }
}
