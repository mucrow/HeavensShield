using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  public class ScenarioSelectionManager: MonoBehaviour {
    void Start() {
      Globals.UI.EnsureReady();
      Globals.UI.ScenarioSelection.Show();
    }

    void OnDestroy() {
      Globals.UI.ScenarioSelection.Hide();
    }
  }
}
