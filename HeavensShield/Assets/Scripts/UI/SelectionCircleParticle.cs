using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mtd.UI {
  public class SelectionCircleParticle: MonoBehaviour {
    public void SetExtension(float length) {
      transform.localPosition = new Vector3(length, 0f, 0f);
    }
  }
}
