using UnityEngine;
using UnityEngine.Tilemaps;

namespace Mtd {
  public class Fog: MonoBehaviour {
    [SerializeField] Tilemap _tilemap;

    [SerializeField] SpriteRenderer _westEdge;
    [SerializeField] SpriteRenderer _eastEdge;
    [SerializeField] SpriteRenderer _northEdge;
    [SerializeField] SpriteRenderer _southEdge;

    void Start() {
      _tilemap.CompressBounds();
      var mapBounds = _tilemap.localBounds;

      _westEdge.transform.position = new Vector3(mapBounds.min.x - _westEdge.transform.localScale.x / 2f, mapBounds.center.y, 0f);
      _eastEdge.transform.position = new Vector3(mapBounds.max.x + _eastEdge.transform.localScale.x / 2f, mapBounds.center.y, 0f);
      _southEdge.transform.position = new Vector3(mapBounds.center.x, mapBounds.min.y - _southEdge.transform.localScale.y / 2f, 0f);
      _northEdge.transform.position = new Vector3(mapBounds.center.x, mapBounds.max.y + _northEdge.transform.localScale.y / 2f, 0f);

      _westEdge.enabled = true;
      _eastEdge.enabled = true;
      _southEdge.enabled = true;
      _northEdge.enabled = true;
    }
  }
}
