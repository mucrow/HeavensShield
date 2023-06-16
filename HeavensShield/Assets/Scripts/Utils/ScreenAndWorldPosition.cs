using UnityEngine;

namespace Mtd.Utils {
  public struct ScreenAndWorldPoint {
    public Vector2 Screen;
    public Vector3 World;

    public static ScreenAndWorldPoint Zero() {
      return new ScreenAndWorldPoint() {
        Screen = Vector2.zero,
        World = Vector3.zero
      };
    }

    public static ScreenAndWorldPoint FromScreenPoint(Camera camera, Vector2 screen) {
      var ret = ScreenAndWorldPoint.Zero();
      ret.UpdateFromScreenPoint(camera, screen);
      return ret;
    }

    public static ScreenAndWorldPoint FromWorldPoint(Camera camera, Vector3 world) {
      var ret = ScreenAndWorldPoint.Zero();
      ret.UpdateFromWorldPoint(camera, world);
      return ret;
    }

    public void UpdateFromScreenPoint(Camera camera, Vector2 screen) {
      Screen = screen;
      World = camera.ScreenToWorldPoint(screen);
      World.z = 0;
    }

    public void UpdateFromWorldPoint(Camera camera, Vector3 world) {
      World = world;
      Screen = camera.WorldToScreenPoint(world);
    }
  }
}
