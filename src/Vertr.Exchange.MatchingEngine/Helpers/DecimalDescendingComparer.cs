namespace Vertr.Exchange.MatchingEngine.Helpers;

internal sealed class DecimalDescendingComparer : IComparer<decimal>
{
    public static DecimalDescendingComparer Instance => new DecimalDescendingComparer();

    public int Compare(decimal x, decimal y) => -x.CompareTo(y);
}
