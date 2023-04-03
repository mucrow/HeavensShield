using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mtd.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Mtd {
  [RequireComponent(typeof(ShowHideOffscreen))]
  public class HUD: MonoBehaviour {
    [SerializeField] TMP_Text _moneyText;
    [SerializeField] TMP_Text _scoreText;
    [SerializeField] ShowHideOffscreen _showHideOffscreen;

    public Func<Task> Show => _showHideOffscreen.Show;
    public Func<Task> Hide => _showHideOffscreen.Hide;
    public UnityAction ShowInstant => _showHideOffscreen.ShowInstant;
    public UnityAction HideInstant => _showHideOffscreen.HideInstant;

    void Awake() {
      Globals.PlayerAgent.AddRegisterListener(OnPlayerAgentRegister);
      Globals.PlayerAgent.AddUnregisterListener(OnPlayerAgentUnregister);
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
  }
}
