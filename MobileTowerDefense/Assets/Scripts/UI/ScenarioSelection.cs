using System.Collections;
using System.Collections.Generic;
using Mtd.UI;
using UnityEngine;

namespace Mtd {
  public class ScenarioSelection: MonoBehaviour {
    [SerializeField] Transform _chapterListUIObject;
    [SerializeField] ScenarioOrder _scenarioOrder;

    [SerializeField] GameObject _chapterListChapterGroupPrefab;
    [SerializeField] GameObject _chapterListScenarioPrefab;

    void Awake() {
      var chapters = _scenarioOrder.Chapters;
      for (int i = 0; i < chapters.Length; ++i) {
        var chapterInfo = chapters[i];

        var chapterGameObject = Instantiate(
          _chapterListChapterGroupPrefab,
          Vector3.zero,
          Quaternion.identity,
          _chapterListUIObject
        );
        var chapter = chapterGameObject.GetComponent<ChapterListChapterGroup>();
        chapter.SetHeadingText("Chapter " + (i + 1));

        var scenarios = chapterInfo.Scenarios;
        for (int j = 0; j < scenarios.Length; ++j) {
          var scenarioInfo = scenarios[j];
          var scenarioGameObject = Instantiate(
            _chapterListScenarioPrefab,
            Vector3.zero,
            Quaternion.identity,
            chapterGameObject.transform
          );
          var scenario = scenarioGameObject.GetComponent<ChapterListScenario>();
          scenario.InitFromScenarioInfo(scenarioInfo);
        }
      }
    }
  }
}
