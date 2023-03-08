using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Mtd.UI {
  public class UnitSelector: MonoBehaviour {
    [SerializeField] ShowHideOffscreen _showHideOffscreen;

    public bool IsHidden => _showHideOffscreen.IsHidden;

    public void StartUnitSelection(Vector3 placementPosition) {
      Globals.UI.SelectionCircle.Show();
      Globals.UI.SelectionCircle.SetWorldPosition(placementPosition);
      Globals.UI.SelectionCircle.StopPreviewingRange();
      _showHideOffscreen.Show();
    }

    public void CancelUnitSelection() {
      Globals.UI.SelectionCircle.Hide();
      _showHideOffscreen.Hide();
    }
  }
}
