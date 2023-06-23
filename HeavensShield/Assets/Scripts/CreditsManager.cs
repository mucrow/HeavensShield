using UnityEngine;

namespace Mtd {
  public class CreditsManager: MonoBehaviour {
    public async void OnUIReady() {
      await Globals.UI.WhiteOutOverlay.Hide();
    }
  }
}
