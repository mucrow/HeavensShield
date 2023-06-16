using UnityEngine;

namespace Mtd.Utils {
  public enum Direction {
    Left,
    Right,
    Bottom,
    Top,
  }

  public static class DirectionExtensions {
    public static Vector2 ToVector2(this Direction direction) {
      if (direction == Direction.Left) {
        return Vector2.left;
      }
      if (direction == Direction.Right) {
        return Vector2.right;
      }
      if (direction == Direction.Bottom) {
        return Vector2.down;
      }
      return Vector2.up;
    }
  }
}
