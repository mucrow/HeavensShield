using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Mtd.UI {
  public class ConfirmCancelButtons: MonoBehaviour {
    [SerializeField] Button _confirmButton;
    [SerializeField] TMP_Text _confirmButtonText;
    [SerializeField] ShowHideUIElement _showHide;

    public Func<Task> Show => _showHide.Show;
    public Func<Task> Hide => _showHide.Hide;
    public UnityAction ShowInstant => _showHide.ShowInstant;
    public UnityAction HideInstant => _showHide.HideInstant;

    public void ConfigureConfirmButton(bool interactable, string text="Confirm") {
      _confirmButtonText.text = text;
      _confirmButton.interactable = interactable;
    }
  }
}
