using UnityEngine;

namespace Mtd {
  [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ScenarioOrderChapter", order = 1)]
  public class ScenarioOrderChapter: ScriptableObject {
    [SerializeField] ScenarioInfo[] _scenarios;
    public ScenarioInfo[] Scenarios => _scenarios;
  }
}
