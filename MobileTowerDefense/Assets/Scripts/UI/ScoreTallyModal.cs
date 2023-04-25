using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Mtd.UI {
  public class ScoreTallyModal: MonoBehaviour {
    [SerializeField] ShowHideOffscreen _showHideOffscreen;

    [SerializeField] TMP_Text _towerHPField;
    [SerializeField] TMP_Text _towerPCField;
    [SerializeField] TMP_Text _scoreField;
    [SerializeField] TMP_Text _scorePCField;
    [SerializeField] TMP_Text _moneyField;
    [SerializeField] TMP_Text _moneyPCField;

    public Func<Task> Show => _showHideOffscreen.Show;
    public Func<Task> Hide => _showHideOffscreen.Hide;
    public UnityAction ShowInstant => _showHideOffscreen.ShowInstant;
    public UnityAction HideInstant => _showHideOffscreen.HideInstant;

    public void SetEarnings(float towerHP, float towerPC, float score, float scorePC, float money, float moneyPC) {
      _towerHPField.text = FormatTowerHP(towerHP);
      _towerPCField.text = FormatPC(towerPC);
      _scoreField.text = FormatScore(score);
      _scorePCField.text = FormatPC(scorePC);
      _moneyField.text = FormatMoney(money);
      _moneyPCField.text = FormatPC(moneyPC);
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

    string FormatPC(float pc) {
      return Utils.Utils.FormatNumberWithCommas(pc);
    }
  }
}
