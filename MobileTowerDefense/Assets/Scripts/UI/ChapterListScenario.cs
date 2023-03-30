using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Mtd.UI {
  public class ChapterListScenario: MonoBehaviour {
    [SerializeField] TMP_Text _text;

    ScenarioInfo _info;

    public void InitFromScenarioInfo(ScenarioInfo info) {
      _info = info;
      gameObject.name = info.Name;
      _text.text = info.Name;
    }

    public void LoadAssociatedScenario() {
      if (!_info) {
        Debug.LogWarning("Tried to load scenario from ChapterListScenario (button) but ScenarioInfo not set (use InitFromScenarioInfo(...))");
        return;
      }
      Globals.GameManager.LoadScene("Scenes/Scenarios/" + _info.Path);
    }
  }
}
