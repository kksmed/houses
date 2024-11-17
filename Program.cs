var counter = 0;
var cache = new Dictionary<Boarder, (int Best, Complex solution)>();
var (mostHouses, bestSolution) = Solve(0, 0, new());

PrintComplex(bestSolution);

Console.WriteLine();
Console.WriteLine($"With: {mostHouses} houses");

Console.WriteLine();
Console.WriteLine("<Press any key to exit>");
Console.ReadKey();
return;

(int Houses, Complex Complex) Solve(int x, int y, Complex complex)
{
  var boarder = new Boarder(x, y, FindBoarder(x, y, complex).ToList());
  if (cache.TryGetValue(boarder, out var cached))
    return cached;

  var next = FindNext();
  if (!HouseAllowed(x, y, complex))
  {
    var (houses, solution) =
      next == null ? (complex.CountHouses(), complex) : Solve(next.Value.X, next.Value.Y, complex);
    CacheSolution(houses, solution);
    return (houses, solution);
  }

  var complexWithHouse = complex.Copy();
  complexWithHouse.Plots[x, y] = true;
  if (next == null)
  {
    var houses = complexWithHouse.CountHouses();
    CacheSolution(houses, complexWithHouse);
    return (houses, complexWithHouse);
  }

  var solutionWith = Solve(next.Value.X, next.Value.Y, complexWithHouse);
  var solutionWithout = Solve(next.Value.X, next.Value.Y, complex);

  if (solutionWithout.Houses > solutionWith.Houses)
  {
    CacheSolution(solutionWithout.Houses, solutionWithout.Complex);
    return solutionWithout;
  }

  CacheSolution(solutionWith.Houses, solutionWith.Complex);
  return solutionWith;

  (int X, int Y)? FindNext() =>
    (x, y) switch
      {
        (Complex.Size - 1, Complex.Size - 1) => null,
        (Complex.Size - 1, _) => (0, y + 1),
        _ => (x + 1, y)
      };

  void CacheSolution(int houses, Complex solutionToCache)
  {
    cache[boarder] = (houses, solutionToCache);
  }
}

IEnumerable<bool> FindBoarder(int x, int y, Complex complex)
{
  var boarderY = x > 0 ? y : y - 1;
  if (boarderY < 0) yield break;
  for (var boarderX = 0; boarderX < Complex.Size; boarderX++)
  {
    yield return complex.Plots[boarderX, boarderY];
    if (boarderX == x - 1)
    {
      boarderY--;
      if (boarderY < 0) yield break;
      yield return complex.Plots[boarderX, boarderY];
    }
  }
}

IEnumerable<(int X, int Y)> FindNeighbours(int x, int y)
{
  // X - 1
  if (x - 1 >= 0)
    foreach (var neighbour in TestRow(x - 1))
      yield return neighbour;

  // X
  foreach (var neighbour in TestRow(x)) yield return neighbour;

  // X +1
  if (x + 1 < Complex.Size)
    foreach (var neighbour in TestRow(x + 1))
      yield return neighbour;
  yield break;

  IEnumerable<(int X, int Y)> TestRow(int xToTest)
  {
    if (y - 1 >= 0) yield return (xToTest, y - 1);
    if (xToTest != x) yield return (xToTest, y);
    if (y + 1 < Complex.Size) yield return (xToTest, y + 1);
  }
}

int ParkSurplus(int x, int y, Complex complex)
{
  counter++;
  if (counter % 10_000_000 == 0)
  {
    Console.WriteLine($"Solving ({x}, {y}) - {counter:g2}");
    PrintComplex(complex);
  }
  var neighbourHouses = 0;
  var neighbourParks = 0;

  foreach (var neighbour in FindNeighbours(x, y))
  {
    if (complex.Plots[neighbour.X, neighbour.Y])
      neighbourHouses++;
    else
      neighbourParks++;
  }
  return neighbourParks - neighbourHouses;
}

bool HouseAllowed(int x, int y, Complex complex) =>
  ParkSurplus(x, y, complex) > 0
  && FindNeighbours(x, y)
    .Where(n => complex.Plots[n.X, n.Y])
    .All(n => ParkSurplus(n.X, n.Y, complex) >= 2);

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