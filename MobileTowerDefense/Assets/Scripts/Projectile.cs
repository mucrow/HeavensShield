using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  public class Projectile: MonoBehaviour {
    [SerializeField] Rigidbody2D _rigidbody2D;

    [SerializeField] AudioClip _impactSound;

    int _damage;

    public void Init(int damage, float speed, Vector3 creatorPosition, Vector3 targetPosition, float spriteRotation) {
      // float angle = Vector2.SignedAngle(creatorPosition, targetPosition);
      _damage = damage;
      transform.rotation = Quaternion.Euler(0f, 0f, spriteRotation);
      Vector2 positionDelta = targetPosition - creatorPosition;
      _rigidbody2D.velocity = positionDelta.normalized * speed;
    }

    void OnTriggerEnter2D(Collider2D other) {
      if (other.CompareTag("Enemy")) {
        Globals.AudioManager.PlaySoundEffect(_impactSound);
        var enemy = other.GetComponent<EnemyController>();
        enemy.ReceiveDamage(_damage);
        Destroy(gameObject);
      }
    }
  }
}
