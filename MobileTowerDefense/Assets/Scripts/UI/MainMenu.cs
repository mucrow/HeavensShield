using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Mtd.UI {
  public class MainMenu: MonoBehaviour {
    [SerializeField] Button _scenarioSelectionButton;
    public UnityEvent OnClickScenarioSelection => _scenarioSelectionButton.onClick;

    [SerializeField] Button _backButton;
    public UnityEvent OnClickBack => _backButton.onClick;

    [SerializeField] ShowHideOffscreen _showHideOffscreen;
    public UnityAction Show => _showHideOffscreen.Show;
    public UnityAction Hide => _showHideOffscreen.Hide;
  }
}
