using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mtd {
  public class EnemyController: MonoBehaviour {
    [SerializeField] float _speed = 2f;

    [SerializeField] Path _path;

    int _pathIndex = 0;

    [SerializeField] int _maxHealth = 100;
    int _health;

    [SerializeField] HealthBar _healthBar;

    void Awake() {
      _health = _maxHealth;
    }

    void Update() {
      MoveTowardNextWaypoint();

      _health -= Random.Range(1, 101) / 100;
      _health = Math.Max(_health, 0);
      float healthFraction = (float) _health / (float) _maxHealth;
      _healthBar.SetCurrentHealth(healthFraction);
    }

    public void SetPath(Path path, int pathIndex) {
      _path = path;
      _pathIndex = pathIndex;
    }

    void MoveTowardNextWaypoint() {
      if (_pathIndex >= _path.WaypointCount()) {
        return;
      }

      Vector3 currentPos = transform.position;
      Vector3 nextWaypoint = _path.GetWaypoint(_pathIndex);
      if (currentPos == nextWaypoint) {
        _pathIndex += 1;
        MoveTowardNextWaypoint();
      }

      float dt = Time.deltaTime;
      transform.position = Vector3.MoveTowards(currentPos, nextWaypoint, _speed * dt);
    }
  }
}
