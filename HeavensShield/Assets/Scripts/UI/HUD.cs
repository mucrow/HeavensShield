using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mtd.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Mtd {
  [RequireComponent(typeof(ShowHideUIElement))]
  public class HUD: MonoBehaviour {
    [SerializeField] TMP_Text _towerHealthText;
    [SerializeField] TMP_Text _moneyText;
    [SerializeField] TMP_Text _scoreText;
    [SerializeField] ShowHideUIElement _showHide;

    public Func<Task> Show => _showHide.Show;
    public Func<Task> Hide => _showHide.Hide;
    public UnityAction ShowInstant => _showHide.ShowInstant;
    public UnityAction HideInstant => _showHide.HideInstant;

    void Awake() {
      Globals.PlayerAgent.AddRegisterListener(OnPlayerAgentRegister);
      Globals.PlayerAgent.AddUnregisterListener(OnPlayerAgentUnregister);
      UpdateTowerHealth(100);
      UpdateMoney(100);
      UpdateScore(0);
    }

    void OnPlayerAgentRegister(PlayerAgent playerAgent) {
      playerAgent.MoneyChange.AddListener(UpdateMoney);
      playerAgent.ScoreChange.AddListener(UpdateScore);
    }

    void OnPlayerAgentUnregister(PlayerAgent playerAgent) {
      playerAgent.MoneyChange.RemoveListener(UpdateMoney);
      playerAgent.ScoreChange.RemoveListener(UpdateScore);
    }

    public void UpdateTowerHealth(int towerHealth) {
      _towerHealthText.text = Utils.Utils.FormatNumberWithCommas(towerHealth) + "HP";
    }

    public void UpdateMoney(int money) {
      _moneyText.text = Utils.Utils.FormatNumberWithCommas(money);
    }

    public void UpdateScore(int score) {
      _scoreText.text = Utils.Utils.FormatNumberWithCommas(score);
    }
  }
}
