using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Mtd {
  public class HUD: MonoBehaviour {
    [SerializeField] TMP_Text _goldText;

    public void UpdateGold(int amount) {
      _goldText.text = amount.ToString("N0");
    }
  }
}
