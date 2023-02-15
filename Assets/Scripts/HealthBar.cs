using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  public class HealthBar: MonoBehaviour {
    [SerializeField] GameObject _currentHealth;

    /** sets health via a float where 0 is 0% health and 1 is 100% health */
    public void SetCurrentHealth(float fraction) {
      float xOffset = -0.5f * (1f - fraction);
      _currentHealth.transform.localScale = new Vector3(fraction, 1, 1);
      _currentHealth.transform.localPosition = new Vector3(xOffset, 0, 0);
    }
  }
}
