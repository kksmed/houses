var complex = new Complex();

for(var x = 0; x < Complex.Size; x++)
{
  for(var y = 0; y < Complex.Size; y++)
  {
    if (HouseAllowed(x, y))
      complex.Plots[x, y] = true;
  }
}

PrintComplex();

Console.WriteLine();
Console.WriteLine("<Press any key to exit>");
Console.ReadKey();
return;

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

int ParkSurplus(int x, int y)
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

bool HouseAllowed(int x, int y) =>
  ParkSurplus(x, y) > 0
  && FindNeighbours(x, y)
    .Where(n => complex.Plots[n.X, n.Y])
    .All(n => ParkSurplus(n.X, n.Y) >= 2);

void PrintComplex()
{
  for (var x = 0; x < Complex.Size; x++)
  {
    for (var y = 0; y < Complex.Size; y++)
    {
      Console.Write(complex.Plots[x, y] ? "X" : "O");
    }
    Console.WriteLine();
  }
}

record Complex
{
  public const int Size = 8;
  public bool[,] Plots { get; } = new bool[Size, Size];
}
