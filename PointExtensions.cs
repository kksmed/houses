using System.Drawing;

static class PointExtensions
{
  public static IEnumerable<Point> FindNeighbours(this Point point)
  {
    // X - 1
    if (point.X - 1 >= 0)
      foreach (var neighbour in TestRow(point.X - 1))
        yield return neighbour;

    // X
    foreach (var neighbour in TestRow(point.X)) yield return neighbour;

    // X + 1
    if (point.X + 1 < Complex.Size)
      foreach (var neighbour in TestRow(point.X + 1))
        yield return neighbour;
    yield break;

    IEnumerable<Point> TestRow(int xToTest)
    {
      if (point.Y - 1 >= 0) yield return new(xToTest, point.Y - 1);
      if (xToTest != point.X) yield return point with { X = xToTest };
      if (point.Y + 1 < Complex.Size) yield return new(xToTest, point.Y + 1);
    }
  }
}
