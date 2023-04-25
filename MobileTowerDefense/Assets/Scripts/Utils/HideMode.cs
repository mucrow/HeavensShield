using UnityEngine;

namespace Mtd.Utils {
  public enum HideMode {
    Fade = 4,
    SlideLeft = 0,
    SlideRight = 1,
    SlideDown = 2,
    SlideUp = 3,
  }

  public static class HideModeExtensions {
    public static bool IsDirectional(this HideMode hideMode) {
      return hideMode != HideMode.Fade;
    }

    public static Vector2 ToVector2(this HideMode hideMode) {
      if (hideMode == HideMode.SlideLeft) {
        return Vector2.left;
      }
      if (hideMode == HideMode.SlideRight) {
        return Vector2.right;
      }
      if (hideMode == HideMode.SlideDown) {
        return Vector2.down;
      }
      if (hideMode == HideMode.SlideUp) {
        return Vector2.up;
      }
      return Vector2.zero;
    }
  }
}
