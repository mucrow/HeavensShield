using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Mtd {
  public class HUD: MonoBehaviour {
    [SerializeField] TMP_Text _moneyText;
    [SerializeField] TMP_Text _scoreText;

    void Start() {
      Globals.Player.MoneyChange.AddListener(UpdateMoney);
      Globals.Player.ScoreChange.AddListener(UpdateScore);
    }

    public void UpdateMoney(int money) {
      _moneyText.text = money.ToString("N0");
    }

    public void UpdateScore(int score) {
      _scoreText.text = score.ToString("N0");
    }
  }
}
