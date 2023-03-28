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

    void Awake() {
      Globals.PlayerAgent.Register.AddListener(OnPlayerAgentRegister);
      Globals.PlayerAgent.Unregister.AddListener(OnPlayerAgentUnregister);
    }

    void OnPlayerAgentRegister(PlayerAgent playerAgent) {
      playerAgent.MoneyChange.AddListener(UpdateMoney);
      playerAgent.ScoreChange.AddListener(UpdateScore);
    }

    void OnPlayerAgentUnregister(PlayerAgent playerAgent) {
      playerAgent.MoneyChange.RemoveListener(UpdateMoney);
      playerAgent.ScoreChange.RemoveListener(UpdateScore);
    }

    public void UpdateMoney(int money) {
      _moneyText.text = money.ToString("N0");
    }

    public void UpdateScore(int score) {
      _scoreText.text = score.ToString("N0");
    }

    public void Show() {
      _showHideOffscreen.ShowInstant();
    }

    public void Hide() {
      _showHideOffscreen.HideInstant();
    }
  }
}
