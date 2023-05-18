using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  public class EnemyWave : MonoBehaviour {
    [SerializeField] GameObject _enemyPrefab;
    public GameObject EnemyPrefab => _enemyPrefab;

    [SerializeField] bool _disableWave = false;
    public bool DisableWave => _disableWave;

    EnemyController _enemyComponent;
    
    void Start() {
      _enemyComponent = _enemyPrefab.GetComponent<EnemyController>();
    }
    
    [SerializeField] float _enemyCountMultiplier = 1f;
    [SerializeField] float _enemyCountIncrease = 0f;
    public int EnemyCount => (
      Mathf.FloorToInt(_enemyComponent.DefaultEnemyCount * _enemyCountMultiplier + _enemyCountIncrease)
    );

    [SerializeField] float _waitTimeBetweenEnemiesMultiplier = 1f;
    [SerializeField] float _waitTimeBetweenEnemiesIncrease = 0f;
    public float WaitTimeBetweenEnemies => (
      _enemyComponent.DefaultWaitTimeBetweenEach * _waitTimeBetweenEnemiesMultiplier + _waitTimeBetweenEnemiesIncrease
    );

    [SerializeField] float _waitTimeBeforeFirstEnemyMultiplier = 1f;
    [SerializeField] float _waitTimeBeforeFirstEnemyIncrease = 0f;
    public float WaitTimeBeforeFirstEnemy => (
      _enemyComponent.DefaultWaitTimeBeforeWave * _waitTimeBeforeFirstEnemyMultiplier + _waitTimeBeforeFirstEnemyIncrease
    );
    
    [SerializeField] float _waitTimeAfterLastEnemyMultiplier = 1f;
    [SerializeField] float _waitTimeAfterLastEnemyIncrease = 0f;
    public float WaitTimeAfterLastEnemy => (
      _enemyComponent.DefaultWaitTimeAfterWave * _waitTimeAfterLastEnemyMultiplier + _waitTimeAfterLastEnemyIncrease
    );
  }
}
