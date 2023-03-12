using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  public class EnemyWave: MonoBehaviour {
    [SerializeField] GameObject _enemyPrefab;
    public GameObject EnemyPrefab => _enemyPrefab;

    [SerializeField] int _enemyCount = 5;
    public int EnemyCount => _enemyCount;

    [SerializeField] float _waitTimeBetweenEnemies = 1.5f;
    public float WaitTimeBetweenEnemies => _waitTimeBetweenEnemies;

    [SerializeField] float _waitTimeBeforeFirstEnemy = 5f;
    public float WaitTimeBeforeFirstEnemy => _waitTimeBeforeFirstEnemy;

    [SerializeField] float _waitTimeAfterLastEnemy = 0f;
    public float WaitTimeAfterLastEnemy => _waitTimeAfterLastEnemy;
  }
}
