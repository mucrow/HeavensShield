using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Mtd.UI {
  public class ChapterListScenario: MonoBehaviour {
    [SerializeField] TMP_Text _text;

    string _path;

    void InitFromNameAndPath(string scenarioName, string scenarioScenePath) {
      gameObject.name = scenarioName;
      _text.text = scenarioName;
      _path = scenarioScenePath;
    }

    public void InitFromScenarioInfo(ScenarioInfo info) {
      InitFromNameAndPath(info.Name, info.Path);
    }

    public void InitFromScenarioInfo(OrderedScenarioInfo info) {
      InitFromNameAndPath(info.Name, info.Path);
    }

    public void LoadAssociatedScenario() {
      if (string.IsNullOrEmpty(_path)) {
        Debug.LogWarning("Tried to load scenario from ChapterListScenario (button) but scene path not set (use InitFromScenarioInfo(...))");
        return;
      }
      Globals.GameManager.LoadScene("Scenes/Scenarios/" + _path);
    }
  }
}
