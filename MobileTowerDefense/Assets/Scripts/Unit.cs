using System;
using System.Collections;
using System.Collections.Generic;
using Mtd.Utils;
using UnityEngine;

namespace Mtd {
  public class Unit: MonoBehaviour {
    [SerializeField] Sprite _idleSprite;
    [SerializeField] Sprite _attackSprite1;
    [SerializeField] Sprite _attackSprite2;

    [SerializeField] EnemyDetector _enemyDetector;
    [SerializeField] SpriteRenderer _spriteRenderer;

    [SerializeField] int _damage = 10;
    [SerializeField] float _speed = 20.0f;

    [Header("If a unit does combo damage, the second strike on an enemy does double damage, and all subsequent strikes on that enemy do triple damage.")]
    [SerializeField] bool _doesComboDamage = false;

    [SerializeField] GameObject _projectilePrefab;
    [SerializeField] bool _createProjectileOnTopOfEnemy = false;
    [SerializeField] Vector3 _projectileStartingPosition = Vector3.zero;
    [SerializeField] float _projectileSpeed = 5f;
    [SerializeField] ProjectileRotationType _projectileRotationType = ProjectileRotationType.Arrow;

    public float Range => _enemyDetector.Range;

    float _timeBetweenActions = 10000000f;

    float _actionCooldownTimer = 0f;

    float _attackFrame1Length = 10f / 60f;
    float _attackFrame2Length = 20f / 60f;
    float _animationTimer = 0f;

    int _comboCount = 1;
    EnemyController _enemyBeingCombod;

    void Awake() {
      _timeBetweenActions = UnitSpeed.ToTimeBetweenActions(_speed);
    }

    void Update() {
      UpdateSprite();

      if (_actionCooldownTimer > 0f) {
        _actionCooldownTimer -= Time.deltaTime;
        return;
      }

      if (_enemyDetector.EnemiesInRange.Count > 0) {
        AttackEnemy(_enemyDetector.EnemiesInRange[0]);
      }
    }

    void AttackEnemy(EnemyController enemy) {
      if (_doesComboDamage) {
        if (enemy == _enemyBeingCombod) {
          _comboCount += 1;
        }
        else {
          _comboCount = 1;
        }
      }

      StartAttackAnimation(enemy);
      CreateProjectile(enemy);

      _actionCooldownTimer += _timeBetweenActions;
      _enemyBeingCombod = enemy;
    }

    enum ProjectileRotationType { None, Slash, Arrow }

    void CreateProjectile(EnemyController enemy) {
      var startingPosition = GetProjectileStartingPosition(enemy);

      var projectileObject = Instantiate(_projectilePrefab, startingPosition, Quaternion.identity);
      var projectile = projectileObject.GetComponent<Projectile>();

      int damage = Mathf.FloorToInt(_damage * Mathf.Min(_comboCount, 10f));
      var positionDelta = enemy.transform.position - transform.position;
      var velocity = (Vector2)positionDelta.normalized * _projectileSpeed;

      float spriteRotation = GetProjectileSpriteRotation(projectile, enemy);
      projectile.Init(damage, velocity, spriteRotation, enemy);
    }

    Vector3 GetProjectileStartingPosition(EnemyController enemy) {
      if (_createProjectileOnTopOfEnemy) {
        return enemy.transform.position;
      }
      return transform.position + _projectileStartingPosition;
    }

    float GetProjectileSpriteRotation(Projectile projectile, EnemyController enemy) {
      if (_projectileRotationType == ProjectileRotationType.Arrow) {
        var positionDelta = projectile.transform.position - enemy.transform.position;
        return Vector2.SignedAngle(Vector2.right, positionDelta);
      }
      if (_projectileRotationType == ProjectileRotationType.Slash) {
        return enemy.transform.position.x > transform.position.x ? 45f : 135f;
      }
      return 0f;
    }

    void StartAttackAnimation(EnemyController target) {
      _spriteRenderer.flipX = target.transform.position.x > transform.position.x;
      _spriteRenderer.sprite = _attackSprite1;
      _animationTimer = _attackFrame1Length;
    }

    void UpdateSprite() {
      if (_spriteRenderer.sprite == _idleSprite) {
        return;
      }
      _animationTimer -= Time.deltaTime;
      if (_animationTimer <= 0f) {
        if (_spriteRenderer.sprite == _attackSprite1) {
          _spriteRenderer.sprite = _attackSprite2;
          _animationTimer += _attackFrame2Length;
        }
        else if (_spriteRenderer.sprite == _attackSprite2) {
          _spriteRenderer.sprite = _idleSprite;
        }
      }
    }
  }
}
