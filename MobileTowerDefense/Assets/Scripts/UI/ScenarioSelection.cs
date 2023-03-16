using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Mtd.UI {
  public class ScenarioSelection: MonoBehaviour {
    // TODO replace with dynamically loaded scenario selection. this is just dummy code
    // ---- dummy code begins
    [SerializeField] Button _firstButton;
    public UnityEvent OnClickFirstScenario => _firstButton.onClick;

    void Start() {
      _firstButton.onClick.AddListener(LoadTestScenario);
    }

    void LoadTestScenario() {
      Globals.GameManager.LoadScene("Scenes/Scenarios/0000-TestScenario");
    }
    // ---- dummy code ends

    [SerializeField] ShowHideOffscreen _showHideOffscreen;
    public UnityAction Show => _showHideOffscreen.Show;
    public UnityAction Hide => _showHideOffscreen.Hide;
  }
}
