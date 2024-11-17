
static class ComplexExtensions
{
  public static int CountHouses(this Complex complex)
  {
    var count = 0;
    for (var x = 0; x < Complex.Size; x++)
    {
      for (var y = 0; y < Complex.Size; y++)
      {
        if (complex.HasHouse(new(x, y)))
          count++;
      }
    }

    return count;
  }

  public static void Print(this Complex complex)
  {
    for (var y = 0; y < Complex.Size; y++)
    {
      for (var x = 0; x < Complex.Size; x++)
      {
        Console.Write(complex.HasHouse(new(x, y)) ? "X" : "O");
      }
      Console.WriteLine();
    }
  }
}
