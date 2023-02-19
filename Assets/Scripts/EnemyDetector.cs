using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  public class EnemyDetector: MonoBehaviour {
    List<EnemyController> _enemiesInRange = new List<EnemyController>();
    public List<EnemyController> EnemiesInRange => _enemiesInRange;

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
