using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Mtd {
  [RequireComponent(typeof(Tilemap))]
  public class DebugDrawTilemapBounds: MonoBehaviour {
    Tilemap _tilemap;

    void Start() {
      _tilemap = GetComponent<Tilemap>();
      _tilemap.CompressBounds();
    }

    void Update() {
      Utils.Utils.DebugDrawBounds(_tilemap.localBounds, Color.red);
    }
  }
}
