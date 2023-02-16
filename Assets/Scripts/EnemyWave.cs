using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  public class EnemyWave: MonoBehaviour {
    [SerializeField] GameObject _enemyPrefab;
    public GameObject EnemyPrefab => _enemyPrefab;

    [SerializeField] int _enemyCount = 5;
    public int EnemyCount => _enemyCount;

    [SerializeField] float _timeBetweenEnemies = 1.5f;
    public float TimeBetweenEnemies => _timeBetweenEnemies;
  }
}
