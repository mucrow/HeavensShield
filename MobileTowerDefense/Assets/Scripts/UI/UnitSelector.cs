using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mtd.UI {
  public class UnitSelector: MonoBehaviour {
    [SerializeField] ConfirmCancelButtons _confirmCancelButtons;
    [SerializeField] SelectionCircle _selectionCircle;
    [SerializeField] ShowHideOffscreen _unitChoiceGroup;
    [SerializeField] Transform _unitsGroup;

    public bool IsHidden => _unitChoiceGroup.IsHidden;

    GameObject _hologram;
    UnitKind _pickedUnit;

    void Awake() {
      if (!_unitsGroup) {
        Debug.LogWarning("_unitsGroup is null - created units will be added as top-level game objects in the hierarchy", this);
      }
    }

    void Start() {
      Globals.Player.MoneyChange.AddListener(UpdateConfirmButton);
    }

    public void Open(Vector3 placementPosition) {
      _selectionCircle.Show();
      _selectionCircle.SetWorldPosition(placementPosition);
      _selectionCircle.StopPreviewingRange();
      _unitChoiceGroup.Show();
    }

    public void PickUnit(UnitKind unitKind) {
      var selectedPosition = _selectionCircle.transform.position;
      if (_hologram) {
        Destroy(_hologram);
      }
      _hologram = Instantiate(unitKind.HologramPrefab, selectedPosition, Quaternion.identity);
      _pickedUnit = unitKind;
      var unitRange = unitKind.Prefab.GetComponent<Unit>().Range;
      _selectionCircle.PreviewRange(unitRange);

      UpdateConfirmButton(Globals.Player.Money);
      _confirmCancelButtons.Show();
    }

    public void UnpickUnit() {
      Destroy(_hologram);
      _selectionCircle.StopPreviewingRange();
      _confirmCancelButtons.Hide();
      _pickedUnit = null;
    }

    public void PlaceUnit() {
      Globals.Player.AddMoney(-1 * _pickedUnit.PlacementCost);
      var selectedPosition = _selectionCircle.transform.position;
      Instantiate(_pickedUnit.Prefab, selectedPosition, Quaternion.identity, _unitsGroup);
      Close();
    }

    public void Close() {
      _confirmCancelButtons.Hide();
      _selectionCircle.Hide();
      _unitChoiceGroup.Hide();
      if (_hologram) {
        Destroy(_hologram);
      }
      _pickedUnit = null;
    }

    void UpdateConfirmButton(int playerMoney) {
      if (!_pickedUnit) {
        _confirmCancelButtons.ConfigureConfirmButton(false, "No Unit Selected");
      }
      else if (playerMoney < _pickedUnit.PlacementCost) {
        _confirmCancelButtons.ConfigureConfirmButton(false, "Not Enough Money");
      }
      else {
        _confirmCancelButtons.ConfigureConfirmButton(true);
      }
    }
  }
}
