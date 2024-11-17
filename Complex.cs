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
