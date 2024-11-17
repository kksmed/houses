using System.Diagnostics;
using System.Drawing;

var cache = new Dictionary<Boarder, (int Best, Complex solution)>();
var sw = Stopwatch.StartNew();
var (mostHouses, bestSolution) = Solve(new(0,0), new());

sw.Stop();
PrintComplex(bestSolution);

Console.WriteLine();
Console.WriteLine($"With: {mostHouses} houses");

Console.WriteLine();
Console.WriteLine($"In: {sw.Elapsed}");
Console.WriteLine();
Console.WriteLine("<Press any key to exit>");
Console.ReadKey();
return;

(int Houses, Complex Complex) Solve(Point point, Complex complex)
{
  var boarder = new Boarder(point, FindBoarder(point, complex).ToList());
  if (cache.TryGetValue(boarder, out var cached))
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
    cache[boarder] = (houses, solutionToCache);
  }
}

IEnumerable<bool> FindBoarder(Point point, Complex complex)
{
  var boarderY = point.X > 0 ? point.Y : point.Y - 1;
  if (boarderY < 0) yield break;
  for (var boarderX = 0; boarderX < Complex.Size; boarderX++)
  {
    yield return complex.Plots[boarderX, boarderY];
    if (boarderX == point.X - 1)
    {
      boarderY--;
      if (boarderY < 0) yield break;
      yield return complex.Plots[boarderX, boarderY];
    }
  }
}

IEnumerable<Point> FindNeighbours(Point point)
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

int ParkSurplus(Point point, Complex complex)
{
  var neighbourHouses = 0;
  var neighbourParks = 0;

  foreach (var neighbour in FindNeighbours(point))
  {
    if (complex.Plots[neighbour.X, neighbour.Y])
      neighbourHouses++;
    else
      neighbourParks++;
  }
  return neighbourParks - neighbourHouses;
}

bool HouseAllowed(Point point, Complex complex) =>
  ParkSurplus(point, complex) > 0
  && FindNeighbours(point)
    .Where(complex.HasHouse)
    .All(n => ParkSurplus(n, complex) >= 2);

void PrintComplex(Complex complex)
{
  for (var y = 0; y < Complex.Size; y++)
  {
    for (var x = 0; x < Complex.Size; x++)
    {
      Console.Write(complex.Plots[x, y] ? "X" : "O");
    }
    Console.WriteLine();
  }
}