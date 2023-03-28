using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mtd.UI {
  public class UnitSelector: MonoBehaviour {
    [SerializeField] ConfirmCancelButtons _confirmCancelButtons;
    [SerializeField] SelectionCircle _selectionCircle;
    [FormerlySerializedAs("_showHideOffscreen")] [SerializeField] ShowHideOffscreen _unitChoiceGroup;
    [SerializeField] Transform _unitsGroup;

    public bool IsHidden => _unitChoiceGroup.IsHidden;

    GameObject _hologram;
    UnitKind _pickedUnit;

    void Awake() {
      if (!_unitsGroup) {
        Debug.LogWarning("_unitsGroup is null - created units will be added as top-level game objects in the hierarchy", this);
      }
      Globals.PlayerAgent.Register.AddListener(OnPlayerAgentRegister);
      Globals.PlayerAgent.Unregister.AddListener(OnPlayerAgentUnregister);
    }

    public void OnPlayerAgentRegister(PlayerAgent playerAgent) {
      playerAgent.MoneyChange.AddListener(UpdateConfirmButton);
    }

    public void OnPlayerAgentUnregister(PlayerAgent playerAgent) {
      playerAgent.MoneyChange.RemoveListener(UpdateConfirmButton);
    }

    public async Task Open(Vector3 placementPosition) {
      await _unitChoiceGroup.Show();
      _selectionCircle.Show();
      _selectionCircle.StopPreviewingRange();
      _selectionCircle.SetWorldPosition(placementPosition);
    }

    public async Task PickUnit(UnitKind unitKind) {
      var selectedPosition = _selectionCircle.transform.position;
      if (_hologram) {
        Destroy(_hologram);
      }
      _hologram = Instantiate(unitKind.HologramPrefab, selectedPosition, Quaternion.identity);
      _pickedUnit = unitKind;
      var unitRange = unitKind.Prefab.GetComponent<Unit>().Range;
      _selectionCircle.PreviewRange(unitRange);

      await Globals.PlayerAgent.WithAsync(async playerAgent => {
        UpdateConfirmButton(playerAgent.Money);
        await _confirmCancelButtons.Show();
      });
    }

    public async void EHPickUnit(UnitKind unitKind) {
      await PickUnit(unitKind);
    }

    public async Task UnpickUnit() {
      Destroy(_hologram);
      _selectionCircle.StopPreviewingRange();
      await _confirmCancelButtons.Hide();
      _pickedUnit = null;
    }

    public async void EHUnpickUnit() {
      await UnpickUnit();
    }

    public async Task PlaceUnit() {
      Globals.PlayerAgent.With(playerAgent => {
        playerAgent.AddMoney(-1 * _pickedUnit.PlacementCost);
      });
      var selectedPosition = _selectionCircle.transform.position;
      Instantiate(_pickedUnit.Prefab, selectedPosition, Quaternion.identity, _unitsGroup);
      await Close();
    }

    public async void EHPlaceUnit() {
      await PlaceUnit();
    }

    public async Task Close() {
      await _confirmCancelButtons.Hide();
      _selectionCircle.Hide();
      await _unitChoiceGroup.Hide();
      if (_hologram) {
        Destroy(_hologram);
      }
      _pickedUnit = null;
    }

    public async void EHClose() {
      await Close();
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
