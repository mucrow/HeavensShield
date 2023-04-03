using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Mtd.UI {
  public class ChapterListScenario: MonoBehaviour {
    [SerializeField] TMP_Text _text;

    OrderedScenarioInfo _info;

    public void InitFromScenarioInfo(OrderedScenarioInfo info) {
      gameObject.name = info.Name;
      _text.text = info.Name;
      _info = info;
    }

    public void LoadAssociatedScenario() {
      if (_info == null) {
        Debug.LogWarning("Tried to load scenario from ChapterListScenario (button) but scene path not set (use InitFromScenarioInfo(...))");
        return;
      }
      Globals.GameManager.LoadScenario(_info);
    }
  }
}
