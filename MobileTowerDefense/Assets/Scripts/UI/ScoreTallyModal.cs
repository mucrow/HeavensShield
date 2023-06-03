using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Mtd.UI {
  public class ScoreTallyModal: MonoBehaviour {
    [SerializeField] ShowHideOffscreen _showHideOffscreen;

    [SerializeField] TMP_Text _towerHPField;
    [SerializeField] TMP_Text _towerHPBonusField;
    [SerializeField] TMP_Text _scoreField;
    [SerializeField] TMP_Text _moneyField;
    [SerializeField] TMP_Text _moneyBonusField;
    [SerializeField] TMP_Text _totalScoreField;

    public Func<Task> Show => _showHideOffscreen.Show;
    public Func<Task> Hide => _showHideOffscreen.Hide;
    public UnityAction ShowInstant => _showHideOffscreen.ShowInstant;
    public UnityAction HideInstant => _showHideOffscreen.HideInstant;

    public void SetText(
      string heading,
      float towerHP,
      float towerHPBonus,
      float score,
      float money,
      float moneyBonus,
      float totalScore
    ) {
      _towerHPField.text = FormatTowerHP(towerHP);
      _towerHPBonusField.text = FormatScore(towerHPBonus);
      _scoreField.text = FormatScore(score);
      _moneyField.text = FormatMoney(money);
      _moneyBonusField.text = FormatScore(moneyBonus);
      _totalScoreField.text = FormatScore(totalScore);
    }

    string FormatTowerHP(float towerHP) {
      return Utils.Utils.FormatNumberWithCommas(towerHP) + "HP";
    }

    string FormatScore(float score) {
      return Utils.Utils.FormatNumberWithCommas(score);
    }

    string FormatMoney(float money) {
      return Utils.Utils.FormatNumberWithCommas(money);
    }
  }
}
