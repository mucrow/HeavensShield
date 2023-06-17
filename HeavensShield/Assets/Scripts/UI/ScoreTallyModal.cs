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
    [FormerlySerializedAs("_showHideOffscreen")] [SerializeField] ShowHideUIElement _showHide;

    [SerializeField] TMP_Text _heading;

    [SerializeField] TMP_Text _baseScoreField;
    [SerializeField] TMP_Text _towerHPField;
    [SerializeField] TMP_Text _towerHPBonusField;
    [SerializeField] TMP_Text _moneyField;
    [SerializeField] TMP_Text _moneyBonusField;
    [SerializeField] TMP_Text _totalScoreField;

    [SerializeField] GameObject _victoryButtons;
    [SerializeField] GameObject _defeatButtons;

    public Func<Task> Show => _showHide.Show;
    public Func<Task> Hide => _showHide.Hide;
    public UnityAction ShowInstant => _showHide.ShowInstant;
    public UnityAction HideInstant => _showHide.HideInstant;

    public void SetText(
      bool isVictory,
      int towerHP,
      int towerHPBonus,
      int score,
      int money,
      int moneyBonus,
      int totalScore
    ) {
      if (isVictory) {
        _heading.text = "The tower is standing tall!";
        _victoryButtons.SetActive(true);
        _defeatButtons.SetActive(false);
      }
      else {
        _heading.text = "The tower was razed...";
        _victoryButtons.SetActive(false);
        _defeatButtons.SetActive(true);
      }

      _baseScoreField.text = FormatScoreBonus(score);

      _towerHPField.text = FormatTowerHP(towerHP);
      _towerHPBonusField.text = FormatScoreBonus(towerHPBonus);

      _moneyField.text = FormatMoney(money);
      _moneyBonusField.text = FormatScoreBonus(moneyBonus);

      _totalScoreField.text = FormatTotalScore(totalScore);
    }

    string FormatScoreBonus(int score) {
      return "+" + Utils.Utils.FormatNumberWithCommas(score);
    }

    string FormatTowerHP(int towerHP) {
      return Utils.Utils.FormatNumberWithCommas(towerHP) + "HP";
    }

    string FormatMoney(int money) {
      return Utils.Utils.FormatNumberWithCommas(money);
    }

    string FormatTotalScore(int score) {
      return Utils.Utils.FormatNumberWithCommas(score);
    }
  }
}
