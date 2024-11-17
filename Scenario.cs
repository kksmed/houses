using System.Drawing;

record Scenario(Point Point, List<bool> Plots)
{
  public virtual bool Equals(Scenario? other)
  {
    if (other is null) return false;
    if (ReferenceEquals(this, other)) return true;
    return Point == other.Point && Plots.SequenceEqual(other.Plots);
  }

  public override int GetHashCode()
  {
    // Don't bother to include Plots in the hash code
    return Point.GetHashCode();
  }
}
