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

    [SerializeField] TMP_Text _heading;
    [SerializeField] TMP_Text _baseScoreField;
    [SerializeField] TMP_Text _towerHPField;
    [SerializeField] TMP_Text _towerHPBonusField;
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
      _heading.text = heading;

      _baseScoreField.text = FormatScoreBonus(score);

      _towerHPField.text = FormatTowerHP(towerHP);
      _towerHPBonusField.text = FormatScoreBonus(towerHPBonus);

      _moneyField.text = FormatMoney(money);
      _moneyBonusField.text = FormatScoreBonus(moneyBonus);

      _totalScoreField.text = FormatTotalScore(totalScore);
    }

    string FormatScoreBonus(float score) {
      return "+" + Utils.Utils.FormatNumberWithCommas(score);
    }

    string FormatTowerHP(float towerHP) {
      return Utils.Utils.FormatNumberWithCommas(towerHP) + "HP";
    }

    string FormatMoney(float money) {
      return Utils.Utils.FormatNumberWithCommas(money);
    }

    string FormatTotalScore(float score) {
      return Utils.Utils.FormatNumberWithCommas(score);
    }
  }
}
