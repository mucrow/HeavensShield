using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mtd {
  public class GameManager: MonoBehaviour {
    public void LoadScene(string sceneName) {
      SceneManager.LoadScene(sceneName);
    }
  }
}
