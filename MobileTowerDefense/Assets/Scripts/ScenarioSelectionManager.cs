using System;
using UnityEngine;

namespace Mtd {
  public class ScenarioSelectionManager: MonoBehaviour {
    void Start() {
      Globals.UI.ScenarioSelection.RefreshUI();
      Globals.UI.ScenarioSelection.ShowInstant();
    }

    void OnDestroy() {
      Globals.UI.ScenarioSelection.HideInstant();
    }
  }
}
