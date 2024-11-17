var (best, solution) = Solve(0, 0, new());

PrintComplex(solution);

Console.WriteLine();
Console.WriteLine($"With: {best} houses");

Console.WriteLine();
Console.WriteLine("<Press any key to exit>");
Console.ReadKey();
return;

(int Houses, Complex Complex) Solve(int x, int y, Complex complex)
{
  var next = FindNext();
  if (!HouseAllowed(x, y, complex))
    return next == null ? (CountHouses(complex), complex) : Solve(next.Value.X, next.Value.Y, complex);

  var complexWithHouse = complex.Copy();
  complexWithHouse.Plots[x, y] = true;
  if (next == null)
    return (CountHouses(complex), complexWithHouse);

  var solutionWith = Solve(next.Value.X, next.Value.Y, complexWithHouse);
  var solutionWithout = Solve(next.Value.X, next.Value.Y, complex);
  return solutionWithout.Houses > solutionWith.Houses
    ? solutionWithout
    : solutionWith;

  (int X, int Y)? FindNext() =>
    (x, y) switch
      {
        (Complex.Size - 1, Complex.Size - 1) => null,
        (Complex.Size - 1, _) => (0, y + 1),
        _ => (x + 1, y)
      };
}

int CountHouses(Complex complex){
  var count = 0;
  for(var x = 0; x < Complex.Size; x++)
  {
    for(var y = 0; y < Complex.Size; y++)
    {
      if (complex.Plots[x, y])
        count++;
    }
  }
  return count;
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

record Complex
{
  public const int Size = 8;
  public bool[,] Plots { get; }

  public Complex() : this(new bool[Size, Size])
  { }

  Complex(bool[,] initialPlots)
  {
    Plots = initialPlots;
  }

  public Complex Copy() => new((bool[,])Plots.Clone());
}
