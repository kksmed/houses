
static class ComplexExtensions
{
  public static int CountHouses(this Complex complex){
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
}
