using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  public class EnemyPath: MonoBehaviour {
    Transform[] _waypoints;

    public int WaypointCount => _waypoints.Length;
    public Vector3 this[int index] => _waypoints[index].position;

    void Awake() {
      int numWaypoints = transform.childCount;
      _waypoints = new Transform[numWaypoints];
      for (int i = 0; i < transform.childCount; ++i) {
        _waypoints[i] = transform.GetChild(i);
      }
    }
  }
}
