using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mtd.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Mtd {
  public class ScenarioSelection: MonoBehaviour {
    [SerializeField] Transform _chapterListUIObject;
    [SerializeField] ScenarioOrder _scenarioOrder;

    [SerializeField] GameObject _chapterListChapterGroupPrefab;
    [SerializeField] GameObject _chapterListScenarioPrefab;

    [SerializeField] ShowHideUIElement _showHide;

    public UnityAction ShowInstant => _showHide.ShowInstant;
    public UnityAction HideInstant => _showHide.HideInstant;

    public void RefreshUI() {
      DeleteOldUI();
      BuildUI();
    }

    void DeleteOldUI() {
      foreach (Transform child in _chapterListUIObject.transform) {
        Destroy(child.gameObject);
      }
    }

    void BuildUI() {
      var unlockedScenarioIDs = Globals.GameManager.SaveData.Game.UnlockedScenarioIDs;
      var chapterGameObjects = new Dictionary<int, GameObject>();

      foreach (int scenarioID in unlockedScenarioIDs) {
        var scenarioInfo = _scenarioOrder.GetScenarioByID(scenarioID);
        var chapterID = scenarioInfo.ChapterID;

        if (!chapterGameObjects.TryGetValue(chapterID, out GameObject chapterGameObject)) {
          chapterGameObject = BuildUIForUnlockedChapter(chapterID);
          chapterGameObjects[chapterID] = chapterGameObject;
        }

        BuildUIForUnlockedScenario(chapterGameObject, scenarioInfo);
      }
    }

    GameObject BuildUIForUnlockedChapter(int chapterID) {
      var ret = Instantiate(
        _chapterListChapterGroupPrefab,
        Vector3.zero,
        Quaternion.identity,
        _chapterListUIObject
      );
      var chapter = ret.GetComponent<ChapterListChapterGroup>();
      chapter.SetHeadingText("Chapter " + (chapterID + 1));
      return ret;
    }

    void BuildUIForUnlockedScenario(GameObject chapterGameObject, OrderedScenarioInfo scenarioInfo) {
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
