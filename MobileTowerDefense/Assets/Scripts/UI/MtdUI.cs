using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mtd.UI {
  public class MtdUI: MonoBehaviour {
    [SerializeField] EventSystem _eventSystem;
    [SerializeField] GraphicRaycaster _uiGraphicRaycaster;

    [SerializeField] UnitSelector _unitSelector;
    public UnitSelector UnitSelector => _unitSelector;

    [SerializeField] SelectionCircle _selectionCircle;
    public SelectionCircle SelectionCircle => _selectionCircle;

    /** Check if the UI currently covers the given screen point. */
    public bool DoesUICoverScreenPoint(Vector2 point) {
      var pointerEventData = new PointerEventData(_eventSystem) {
        position = point
      };
      List<RaycastResult> results = new List<RaycastResult>();
      _uiGraphicRaycaster.Raycast(pointerEventData, results);
      return results.Count > 0;
    }
  }
}
