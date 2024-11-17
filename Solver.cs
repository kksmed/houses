using System.Drawing;

class Solver
{
  readonly Dictionary<Scenario, (int Best, Complex solution)> cache = new();

  /// <summary>
  /// Finds a solution with the maximum number of houses.
  /// </summary>
  public (int Houses, Complex Complex) Solve() => Solve(new(0, 0), new());

  /// <summary>
  /// Finds a solution with the maximum number of houses.
  /// Starting at <paramref name="point"/> and using existing houses from <paramref name="complex"/>.
  /// </summary>
  (int Houses, Complex Complex) Solve(Point point, Complex complex)
  {
    var scenario = new Scenario(point, FindEdge(point, complex).ToList());
    if (cache.TryGetValue(scenario, out var cached))
      return cached;

    var next = FindNext();
    if (!HouseAllowed(point, complex))
    {
      var (houses, solution) = next.HasValue ? Solve(next.Value, complex) : (complex.CountHouses(), complex);
      CacheSolution(houses, solution);
      return (houses, solution);
    }

    var complexWithHouse = complex.Copy();
    complexWithHouse.SetHouse(point);
    if (!next.HasValue)
    {
      var houses = complexWithHouse.CountHouses();
      CacheSolution(houses, complexWithHouse);
      return (houses, complexWithHouse);
    }

    var solutionWith = Solve(next.Value, complexWithHouse);
    var solutionWithout = Solve(next.Value, complex);

    if (solutionWithout.Houses > solutionWith.Houses)
    {
      CacheSolution(solutionWithout.Houses, solutionWithout.Complex);
      return solutionWithout;
    }

    CacheSolution(solutionWith.Houses, solutionWith.Complex);
    return solutionWith;

    Point? FindNext() => point switch
      {
          { X: Complex.Size - 1, Y: Complex.Size - 1 } => null,
          { X: Complex.Size - 1 } => new(0, point.Y + 1),
        _ => point with { X = point.X + 1 }
      };

    void CacheSolution(int houses, Complex solutionToCache)
    {
      cache[scenario] = (houses, solutionToCache);
    }
  }

  /// <summary>
  /// Lists the housing status on plots edged up to the unsolved plots.
  /// </summary>
  static IEnumerable<bool> FindEdge(Point point, Complex complex)
  {
    var y = point.X > 0 ? point.Y : point.Y - 1;
    if (y < 0) yield break;
    for (var x = 0; x < Complex.Size; x++)
    {
      yield return complex.HasHouse(new(x, y));

      if (x != point.X - 1) continue;

      y--;
      if (y < 0) yield break;
      yield return complex.HasHouse(new(x, y));
    }
  }

  /// <summary>
  /// Determines how many parks vs houses in the surrounding neighbourhood of <paramref name="point"/>.
  /// </summary>
  /// <remarks>If <paramref name="complex"/> is valid the return value will minimum be 0.</remarks>
  static int ParkSurplus(Point point, Complex complex)
  {
    var neighbourHouses = 0;
    var neighbourParks = 0;

    foreach (var neighbour in point.FindNeighbours())
    {
      if (complex.HasHouse(neighbour))
        neighbourHouses++;
      else
        neighbourParks++;
    }
    return neighbourParks - neighbourHouses;
  }

  /// <summary>
  /// Determines if a house can be placed at <paramref name="point"/> according to the rule:
  /// "A house must only be built on a plot if at least half of the adjacent plots - within the complex - are designated for use as parks."
  /// </summary>
  static bool HouseAllowed(Point point, Complex complex) =>
    ParkSurplus(point, complex) > 0
    && point.FindNeighbours()
      .Where(complex.HasHouse)
      .All(n => ParkSurplus(n, complex) >= 2);
}
