using UnityEngine;

namespace Mtd {
  [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ScenarioInfo", order = 1)]
  public class ScenarioInfo: ScriptableObject {
    [SerializeField] string _name;
    public string Name => _name;

    [SerializeField] string _path;
    public string Path => _path;
  }
}
