using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mtd.Utils {
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

    /**
     * Maps a range of numbers to another range of numbers with a given "tension".
     *
     * The `input` value is raised to the power of `tension` and then mapped to the output range.
     *
     * For example, given the following arguments:
     *   minInput = 10.0f
     *   maxInput = 20.0f
     *   minOutput = 2.0f
     *   maxOutput = 3.0f
     *   tension = 2.0f
     *
     * For those values, an `input` of 15.0f is the mean of the input range, and the tension is 2,
     * so we calculate 0.5^2 = 0.25 and use 0.25 as the index in the output range, giving a return
     * value of 2.25f (the value 25% of the way through the output range).
     */
    public static float MapRange(float minInput, float maxInput, float minOutput, float maxOutput, float input, float tension) {
      float inputRange = maxInput - minInput;
      float outputRange = maxOutput - minOutput;

      float normalizedInput = (input - minInput) / inputRange;
      float pointInOutputRange = Mathf.Pow(normalizedInput, tension);

      return pointInOutputRange * outputRange + minOutput;
    }

    /**
     * Maps a range of numbers to another range of numbers linearly.
     *
     * For example, given the following arguments:
     *   minInput = 10.0f
     *   maxInput = 20.0f
     *   minOutput = 2.0f
     *   maxOutput = 3.0f
     *
     * These values for the `input` argument give the following return values:
     *   10 => 2.0f
     *   15 => 2.5f
     *   20 => 3.0f
     */
    public static float MapRange(float minInput, float maxInput, float minOutput, float maxOutput, float input) {
      return MapRange(minInput, maxInput, minOutput, maxOutput, 1f);
    }

    public static Vector2 MapRange(float minInput, float maxInput, Vector2 minOutput, Vector2 maxOutput, float input) {
      float x = MapRange(minInput, maxInput, minOutput.x, maxOutput.x, input, 1f);
      float y = MapRange(minInput, maxInput, minOutput.y, maxOutput.y, input, 1f);
      return new Vector2(x, y);
    }

    /**
     * Adds items to the given list only if they are not already present.
     *
     * Be warned this method may be a bit slow.
     */
    public static void AddNewToList<T>(List<T> list, params T[] items) {
      list.AddRange(items.Except(list));
    }

    public static Vector3 SnapPointToBottomLeftOfTile(Vector3 point) {
      var x = Mathf.Floor(point.x);
      var y = Mathf.Floor(point.y);
      return new Vector3(x, y, point.z);
    }

    public static Vector3 SnapPointToTileCenter(Vector3 point) {
      var pointSnappedToBottomLeft = SnapPointToBottomLeftOfTile(point);
      var halfTile = new Vector3(0.5f, 0.5f, 0f);
      return pointSnappedToBottomLeft + halfTile;
    }

    public static void DebugDrawBounds(Bounds bounds, Color color) {
      var minX = bounds.min.x;
      var maxX = bounds.max.y;
      var minY = bounds.min.x;
      var maxY = bounds.max.y;

      if (minX == maxX) {
        minX -= 0.05f;
        maxX += 0.05f;
      }

      if (minY == maxY) {
        minY -= 0.05f;
        maxY += 0.05f;
      }

      var bottomLeft = new Vector3(minX, minY, 0);
      var bottomRight = new Vector3(maxX, minY, 0);
      var topLeft = new Vector3(minX, maxY, 0);
      var topRight = new Vector3(maxX, maxY, 0);

      Debug.DrawLine(bottomLeft, bottomRight, color);
      Debug.DrawLine(topLeft, topRight, color);
      Debug.DrawLine(bottomLeft, topLeft, color);
      Debug.DrawLine(bottomRight, topRight, color);
    }

    public static void DebugDrawBounds(Bounds bounds) {
      DebugDrawBounds(bounds, Color.white);
    }

    public static string FormatNumberWithCommas(float n) {
      return n.ToString("N0");
    }

    public static Bounds GetWorldBoundsInCameraView(Camera cam) {
      var position = cam.transform.position;
      position.z = 0f;

      var height = cam.orthographicSize * 2f;
      var width = height * cam.aspect;
      var size = new Vector3(width, height, 1f);

      return new Bounds(position, size);
    }
  }
}
