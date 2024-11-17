record Boarder(int X, int Y, List<bool> Plots)
{
  public virtual bool Equals(Boarder? other)
  {
    if (other is null) return false;
    if (ReferenceEquals(this, other)) return true;
    return X == other.X && Y == other.Y && Plots.SequenceEqual(other.Plots);
  }

  public override int GetHashCode()
  {
    // Don't bother to include Plots in the hash code
    return HashCode.Combine(X, Y);
  }
}
