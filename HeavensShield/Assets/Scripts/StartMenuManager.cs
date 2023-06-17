using UnityEngine;

namespace Mtd {
  public class StartMenuManager: MonoBehaviour {
    public void OnUIReady() {
      Globals.UI.StartMenuVersionText.text = "v" + Application.version;
    }
  }
}
