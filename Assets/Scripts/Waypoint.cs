using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  public class Waypoint: MonoBehaviour {
    void OnDrawGizmos() {
      Gizmos.DrawIcon(transform.position, "Waypoint.png", false);
    }
  }
}
