using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mtd.UI {
  public class ConfirmCancelButtons: MonoBehaviour {
    [SerializeField] Button _confirmButton;
    [SerializeField] TMP_Text _confirmButtonText;
    [SerializeField] ShowHideOffscreen _showHideOffscreen;

    public void Show() {
      _showHideOffscreen.Show();
    }

    public void Hide() {
      _showHideOffscreen.Hide();
    }

    public void ConfigureConfirmButton(bool interactable, string text="Confirm") {
      _confirmButtonText.text = text;
      _confirmButton.interactable = interactable;
    }
  }
}
