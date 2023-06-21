using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ScenarioOrder", order = 1)]
  public class ScenarioOrder: ScriptableObject {
    [SerializeField] ScenarioOrderChapter[] _chapters;
    public ScenarioOrderChapter[] Chapters => _chapters;

    List<OrderedScenarioInfo> _indexedOrdering = null;

    public OrderedScenarioInfo GetScenarioByID(int id) {
      IndexOrdering();
      return _indexedOrdering[id];
    }

    public List<OrderedScenarioInfo> GetScenariosByIDs(List<int> ids) {
      IndexOrdering();
      var ret = new List<OrderedScenarioInfo>(ids.Count);
      foreach (int id in ids) {
        ret.Add(_indexedOrdering[id]);
      }
      return ret;
    }

    public List<OrderedScenarioInfo> GetIndexedOrdering() {
      IndexOrdering();
      return _indexedOrdering;
    }

    public void IndexOrdering() {
      if (_indexedOrdering != null) {
        return;
      }

      // set this to the instance variable at the end so we don't corrupt the state of this object
      var indexedOrdering = new List<OrderedScenarioInfo>();

      int idOfFirstScenarioInChapter = 0;
      for (int indexOfChapter = 0; indexOfChapter < _chapters.Length; ++indexOfChapter) {
        var chapter = _chapters[indexOfChapter];
        bool isLastChapter = indexOfChapter >= (_chapters.Length - 1);
        var scenarios = chapter.Scenarios;
        for (int indexOfScenario = 0; indexOfScenario < scenarios.Length; ++indexOfScenario) {
          var scenario = scenarios[indexOfScenario];
          bool isLastScenarioInChapter = indexOfScenario >= (scenarios.Length - 1);
          bool isFinalScenario = isLastChapter && isLastScenarioInChapter;
          var id = indexOfScenario + idOfFirstScenarioInChapter;
          var unlocks = new List<int>();
          if (!isFinalScenario) {
            unlocks.Add(id + 1);
          }
          var orderedScenarioInfo = new OrderedScenarioInfo() {
            ChapterID = indexOfChapter,
            IndexInChapter = indexOfScenario,
            ID = id,
            Name = scenario.Name,
            Path = scenario.Path,
            Unlocks = unlocks
          };
          indexedOrdering.Add(orderedScenarioInfo);
        }
        idOfFirstScenarioInChapter += scenarios.Length;
      }

      // the ordering was indexed successfully
      _indexedOrdering = indexedOrdering;
    }
  }
}
