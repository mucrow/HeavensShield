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

    public float Range => _enemyDetector.Range;

    float _timeBetweenActions = 10000000f;

    float _actionCooldownTimer = 0f;

    float _attackFrame1Length = 10f / 60f;
    float _attackFrame2Length = 20f / 60f;
    float _animationTimer = 0f;

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
        var enemy = _enemyDetector.EnemiesInRange[0];
        enemy.ReceiveDamage(_damage);
        StartAttackAnimation(enemy);
        _actionCooldownTimer += _timeBetweenActions;
      }
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
