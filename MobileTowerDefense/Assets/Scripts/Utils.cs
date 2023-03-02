using System.Collections.Generic;
using UnityEngine;

namespace Mtd {
  public static class Utils {
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

    public static int GetIndexOfClosest(List<float> sortedChoices, float target) {
      float lower = sortedChoices[0];
      if (target <= lower) {
        return 0;
      }

      for (int i = 1; i < sortedChoices.Count; ++i) {
        float upper = sortedChoices[i];
        if (target <= upper) {
          float distanceToLower = target - lower;
          float distanceToUpper = upper - target;
          return distanceToUpper < distanceToLower ? i : i - 1;
        }
        lower = upper;
      }

      return sortedChoices.Count - 1;
    }

    public static float GetClosest(List<float> sortedChoices, float target) {
      int index = GetIndexOfClosest(sortedChoices, target);
      return sortedChoices[index];
    }

    public static float MapRange(float minInput, float maxInput, float minOutput, float maxOutput, float input, float tension = 1f) {
      float inputRange = maxInput - minInput;
      float outputRange = maxOutput - minOutput;

      float normalizedInput = (input - minInput) / inputRange;
      float pointInOutputRange = Mathf.Pow(normalizedInput, tension);

      return pointInOutputRange * outputRange + minOutput;
    }

    public static Vector2 MapRange(float minInput, float maxInput, Vector2 minOutput, Vector2 maxOutput, float input) {
      float x = MapRange(minInput, maxInput, minOutput.x, maxOutput.x, input, 1f);
      float y = MapRange(minInput, maxInput, minOutput.y, maxOutput.y, input, 1f);
      return new Vector2(x, y);
    }
  }
}
