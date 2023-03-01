using UnityEngine;

namespace Mtd {
  public class Utils {
    /**
     * Ensure the vector `v` has a magnitude less than or equal to `length`. This is different from
     * `v.normalized` because Clamp can return a vector with a magnitude less than 1.
     */
    public static Vector2 Clamp(Vector2 v, float length) {
      if (v.magnitude <= length) {
        return v;
      }
      return v.normalized * length;
    }
  }
}
