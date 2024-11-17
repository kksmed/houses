using System.Drawing;

record Scenario(Point Point, List<bool> EdgePlots)
{
  public virtual bool Equals(Scenario? other)
  {
    if (other is null) return false;
    if (ReferenceEquals(this, other)) return true;
    return Point == other.Point && EdgePlots.SequenceEqual(other.EdgePlots);
  }

  public override int GetHashCode()
  {
    // Don't bother to include Plots in the hash code
    return Point.GetHashCode();
  }
}
