using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  [RequireComponent(typeof(CircleCollider2D))]
  public class EnemyDetector: MonoBehaviour {
    [SerializeField] CircleCollider2D _circleCollider2D;

    List<EnemyController> _enemiesInRange = new List<EnemyController>();
    public List<EnemyController> EnemiesInRange => _enemiesInRange;

    public float Range => _circleCollider2D.radius;

    void OnTriggerEnter2D(Collider2D other) {
      if (other.gameObject.CompareTag("Enemy")) {
        _enemiesInRange.Add(other.gameObject.GetComponent<EnemyController>());
      }
    }

    void OnTriggerExit2D(Collider2D other) {
      if (other.gameObject.CompareTag("Enemy")) {
        _enemiesInRange.Remove(other.gameObject.GetComponent<EnemyController>());
      }
    }
  }
}
