using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  public class EnemyController: MonoBehaviour {
    [SerializeField] float _speed = 3f;

    [SerializeField] Path _path;

    int _pathIndex = 0;

    void Update() {
      MoveTowardNextWaypoint();
    }

    void MoveTowardNextWaypoint() {
      if (_pathIndex >= _path.WaypointCount()) {
        return;
      }

      Vector3 currentPos = transform.position;
      Vector3 nextWaypoint = _path.GetWaypoint(_pathIndex);
      if (currentPos == nextWaypoint) {
        _pathIndex += 1;
        MoveTowardNextWaypoint();
      }

      float dt = Time.deltaTime;
      transform.position = Vector3.MoveTowards(currentPos, nextWaypoint, _speed * dt);
    }
  }
}
