using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  public class Unit: MonoBehaviour {
    [SerializeField] EnemyDetector _enemyDetector;

    int _damage = 10;
    float _speed = 20.0f;

    float _timeBetweenActions = 10000000f;

    float _actionCooldownTimer = 0f;

    void Awake() {
      _timeBetweenActions = CalculateTimeBetweenActions(_speed);
    }

    void Update() {
      if (_actionCooldownTimer > 0f) {
        _actionCooldownTimer -= Time.deltaTime;
        return;
      }

      if (_enemyDetector.EnemiesInRange.Count > 0) {
        Debug.Log("here!");
        var enemy = _enemyDetector.EnemiesInRange[^1];
        enemy.ReceiveDamage(_damage);
        _actionCooldownTimer += _timeBetweenActions;
      }
    }

    float CalculateTimeBetweenActions(float speed) {
      float minSpeed = 1f;
      float maxTimeBetweenActions = 2f;
      float maxSpeed = 200f;
      float minTimeBetweenActions = 1f / 20f;

      float speedRange = maxSpeed - minSpeed;
      float timeBetweenActionsRange = maxTimeBetweenActions - minTimeBetweenActions;

      float pointInSpeedRange = (Mathf.Clamp(speed, minSpeed, maxSpeed) - minSpeed) / speedRange;
      float pointInTimeRange = Mathf.Pow(1 - pointInSpeedRange, 2);

      return pointInTimeRange * timeBetweenActionsRange + minTimeBetweenActions;
    }
  }
}
