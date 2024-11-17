using System.Drawing;

record Boarder(Point Point, List<bool> Plots)
{
  public virtual bool Equals(Boarder? other)
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
