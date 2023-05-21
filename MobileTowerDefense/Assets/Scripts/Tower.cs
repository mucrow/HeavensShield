using UnityEngine;

namespace Mtd {
  public class Tower: MonoBehaviour {
    [SerializeField] int _health = 100;
    
    public void ReceiveDamage(int amount) {
      _health -= amount;
    }
  }
}
