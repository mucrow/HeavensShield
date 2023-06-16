using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  public class Projectile: MonoBehaviour {
    [SerializeField] Rigidbody2D _rigidbody2D;

    [SerializeField] AudioClip _impactSound;

    int _damage;
    EnemyController _target;

    bool _struckTarget = false;

    void FixedUpdate() {
      float speed = _rigidbody2D.velocity.magnitude;
      if (speed > 0f && _target != null) {
        var positionDelta = _target.transform.position - transform.position;
        _rigidbody2D.velocity = positionDelta.normalized * speed;
      }
    }

    void OnTriggerEnter2D(Collider2D other) {
      if (!_struckTarget && other.CompareTag("Enemy")) {
        Globals.AudioManager.PlaySoundEffect(_impactSound);
        var enemy = other.GetComponent<EnemyController>();
        enemy.ReceiveDamage(_damage);
        _struckTarget = true;
        Destroy(gameObject);
      }
    }

    public void Init(int damage, Vector2 velocity, float spriteRotation, EnemyController target) {
      _damage = damage;
      transform.rotation = Quaternion.Euler(0f, 0f, spriteRotation);
      _rigidbody2D.velocity = velocity;
      _target = target;
    }
  }
}
