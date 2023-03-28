using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mtd.UI {
  public class ConfirmCancelButtons: MonoBehaviour {
    [SerializeField] Button _confirmButton;
    [SerializeField] TMP_Text _confirmButtonText;
    [SerializeField] ShowHideOffscreen _showHideOffscreen;

    public Func<Task> Show => _showHideOffscreen.Show;
    public Func<Task> Hide => _showHideOffscreen.Hide;

    public void ConfigureConfirmButton(bool interactable, string text="Confirm") {
      _confirmButtonText.text = text;
      _confirmButton.interactable = interactable;
    }
  }
}
