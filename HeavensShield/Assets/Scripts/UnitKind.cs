using UnityEngine;

namespace Mtd {
  [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/UnitKind", order = 1)]
  public class UnitKind: ScriptableObject {
    [SerializeField] string _name;
    public string Name => _name;

    [SerializeField] int _placementCost;
    public int PlacementCost => _placementCost;

    [SerializeField] GameObject _prefab;
    public GameObject Prefab => _prefab;

    [SerializeField] GameObject _hologramPrefab;
    public GameObject HologramPrefab => _hologramPrefab;
  }
}
