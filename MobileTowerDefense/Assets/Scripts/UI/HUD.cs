using System.Collections;
using System.Collections.Generic;
using Mtd.UI;
using TMPro;
using UnityEngine;

namespace Mtd {
  [RequireComponent(typeof(ShowHideOffscreen))]
  public class HUD: MonoBehaviour {
    [SerializeField] TMP_Text _moneyText;
    [SerializeField] TMP_Text _scoreText;
    [SerializeField] ShowHideOffscreen _showHideOffscreen;

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

    public void Show() {
      _showHideOffscreen.Show();
    }

    public void Hide() {
      _showHideOffscreen.Hide();
    }
  }
}
