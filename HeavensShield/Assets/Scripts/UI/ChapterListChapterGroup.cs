using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Mtd.UI {
  public class ChapterListChapterGroup: MonoBehaviour {
    [SerializeField] TMP_Text _text;

    public void SetHeadingText(string headingText) {
      gameObject.name = headingText;
      _text.text = headingText;
    }
  }
}
