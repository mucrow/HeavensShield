using UnityEngine;

namespace Mtd {
  [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ScenarioOrder", order = 1)]
  public class ScenarioOrder: ScriptableObject {
    [SerializeField] ScenarioOrderChapter[] _chapters;
    public ScenarioOrderChapter[] Chapters => _chapters;
  }
}
