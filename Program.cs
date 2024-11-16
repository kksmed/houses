var complex = new Complex();

for(var x = 0; x < Complex.Size; x++)
{
  for(var y = 0; y < Complex.Size; y++)
  {
    if (HouseAllowed(x, y))
      complex.Plots[x, y] = true;
  }
}

PrintComplex(complex);
Console.ReadKey();
return;

bool HouseAllowed(int x, int y)
{
  var neighbourHouses = 0;
  var neighbourParks = 0;

  if (x - 1 >= 0) TestRow(x - 1);
  TestRow(x);
  if (x +1 < Complex.Size) TestRow(x + 1);

  return neighbourHouses <= neighbourParks;

  void TestRow(int xToTest)
  {
    if (y - 1 >= 0) TestNeighbour(xToTest, y - 1);
    if (xToTest != x) TestNeighbour(xToTest, y);
    if (y + 1 < Complex.Size) TestNeighbour(xToTest, y + 1);
  }

  void TestNeighbour(int xToTest, int yToTest)
  {
    if (complex.Plots[xToTest, yToTest])
      neighbourHouses++;
    else
      neighbourParks++;
  }
}

void PrintComplex(Complex complex1)
{
  for (var x = 0; x < Complex.Size; x++)
  {
    for (var y = 0; y < Complex.Size; y++)
    {
      Console.Write(complex1.Plots[x, y] ? "X" : "O");
    }
    Console.WriteLine();
  }
}

record Complex
{
  public const int Size = 8;
  public bool[,] Plots { get; } = new bool[Size, Size];
}
