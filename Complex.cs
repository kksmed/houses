using System.Drawing;

record Complex
{
  public const int Size = 8;
  bool[,] Plots { get; }

  public Complex() : this(new bool[Size, Size])
  { }

  Complex(bool[,] initialPlots)
  {
    Plots = initialPlots;
  }

  public bool HasHouse(Point point) => Plots[point.X, point.Y];
  public void SetHouse(Point point) => Plots[point.X, point.Y] = true;

  public Complex Copy() => new((bool[,])Plots.Clone());
}
