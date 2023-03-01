using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Mtd {
  [RequireComponent(typeof(Tilemap))]
  public class GridLines: MonoBehaviour {
    [SerializeField] Tilemap _tilemap;
    [SerializeField] TileBase _gridTile;

    void Awake() {
      // it would be nice to just have enough gridlines to cover the viewport but i don't think
      // it's really worth engineering a solution for that right now.
      var tilePos = new Vector3Int(0, 0, 0);
      for (tilePos.y = -50; tilePos.y <= 50; tilePos.y += 1) {
        for (tilePos.x = -50; tilePos.x <= 50; tilePos.x += 1) {
          _tilemap.SetTile(tilePos, _gridTile);
        }
      }
    }
  }
}
