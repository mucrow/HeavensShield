namespace Mtd.Utils {
  public struct SafeAreaPadding {
    public int Left;
    public int Right;
    public int Bottom;
    public int Top;

    public override string ToString() {
      return Left + "px left, " + Right + "px right, " + Bottom + "px bottom, " + Top + "px top";
    }
  }
}
