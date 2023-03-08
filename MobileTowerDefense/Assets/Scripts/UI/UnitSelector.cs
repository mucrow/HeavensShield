using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Mtd.UI {
  public class UnitSelector: MonoBehaviour {
    [SerializeField] ShowHideOffscreen _showHideOffscreen;
    [SerializeField] ShowHideOffscreen _confirmCancelButtons;

    public bool IsHidden => _showHideOffscreen.IsHidden;

    public void Open(Vector3 placementPosition) {
      Globals.UI.SelectionCircle.Show();
      Globals.UI.SelectionCircle.SetWorldPosition(placementPosition);
      Globals.UI.SelectionCircle.StopPreviewingRange();
      _showHideOffscreen.Show();
    }

    public void PickUnit() {
      Globals.UI.SelectionCircle.PreviewRange(3.5f);
      _confirmCancelButtons.Show();
    }

    public void UnpickUnit() {
      Globals.UI.SelectionCircle.StopPreviewingRange();
      _confirmCancelButtons.Hide();
    }

    public void Close() {
      _confirmCancelButtons.Hide();
      Globals.UI.SelectionCircle.Hide();
      _showHideOffscreen.Hide();
    }
  }
}
