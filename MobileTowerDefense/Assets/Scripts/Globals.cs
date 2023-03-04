using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  public class Globals: MonoBehaviour {
    static Globals _instance;

    void Awake() {
      if (_instance == null) {
        DontDestroyOnLoad(gameObject);
        _instance = this;
      }
      else {
        Destroy(gameObject);
      }
    }
  }
}
