using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace Mtd {
  [RequireComponent(typeof(Tilemap))]
  public class GridLines: MonoBehaviour {
    [SerializeField] PlayerAgent _playerAgent;
    [SerializeField] Tilemap _groundTilemap;
    [SerializeField] Tilemap _gridTilemap;
    [SerializeField] TileBase _gridTile;

    void Start() {
      _gridTilemap.transform.position = _groundTilemap.transform.position;
      _gridTilemap.tileAnchor = _groundTilemap.tileAnchor;
      _gridTilemap.origin = _groundTilemap.origin;

      var groundCellBounds = _groundTilemap.cellBounds;
      int groundStartX = groundCellBounds.min.x;
      int groundEndX = groundCellBounds.max.x;
      int groundStartY = groundCellBounds.min.y;
      int groundEndY = groundCellBounds.max.y;

      for (int y = groundStartY; y < groundEndY; ++y) {
        for (int x = groundStartX; x < groundEndX; ++x) {
          var cellPos = new Vector3Int(x, y);
          var worldPos = _groundTilemap.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f, 0f);
          if (_playerAgent.CanPlaceUnit(worldPos)) {
            _gridTilemap.SetTile(cellPos, _gridTile);
          }
        }
      }
    }
  }
}
