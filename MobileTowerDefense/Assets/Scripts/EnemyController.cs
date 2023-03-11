using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mtd {
  public class EnemyController: MonoBehaviour {
    [SerializeField] Rigidbody2D _rigidbody2D;

    [SerializeField] float _speed = 2f;

    [SerializeField] Path _path;

    int _pathIndex = 0;

    [SerializeField] int _maxHealth = 100;
    int _health;

    [SerializeField] int _moneyOnKill = 1;
    [SerializeField] int _scoreOnKill = 1;

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

    public void ReceiveDamage(int amount) {
      _health -= amount;
      if (_health <= 0) {
        _health = 0;
        Globals.Player.AddMoney(_moneyOnKill);
        Globals.Player.AddToScore(_scoreOnKill);
        Destroy(gameObject);
      }
    }

    public void SetPath(Path path, int pathIndex) {
      _path = path;
      _pathIndex = pathIndex;
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
