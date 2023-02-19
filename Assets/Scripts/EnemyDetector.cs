using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  public class EnemyDetector: MonoBehaviour {
    List<EnemyController> _enemiesInRange = new List<EnemyController>();
    public List<EnemyController> EnemiesInRange => _enemiesInRange;

    void OnTriggerEnter2D(Collider2D other) {
      Debug.Log("trigger enter");
      if (other.gameObject.CompareTag("Enemy")) {
        Debug.Log("enemy detected - adding to list");
        _enemiesInRange.Add(other.gameObject.GetComponent<EnemyController>());
      }
    }

    void OnTriggerExit2D(Collider2D other) {
      Debug.Log("trigger exit");
      if (other.gameObject.CompareTag("Enemy")) {
        Debug.Log("enemy detected - removing from list");
        _enemiesInRange.Remove(other.gameObject.GetComponent<EnemyController>());
      }
    }
  }
}
