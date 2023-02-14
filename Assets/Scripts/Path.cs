using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  public class Path: MonoBehaviour {
    [SerializeField] GameObject[] _waypoints;

    public int WaypointCount() {
      return _waypoints.Length;
    }

    public Vector3 GetWaypoint(int index) {
      return _waypoints[index].transform.position;
    }
  }
}
