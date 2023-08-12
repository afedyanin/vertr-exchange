namespace Vertr.Exchange.MatchingEngine.Helpers;

// https://stackoverflow.com/questions/931891/reverse-sorted-dictionary-in-net
internal sealed class LongDescendingComparer : IComparer<long>
{
    public static LongDescendingComparer Instance => new LongDescendingComparer();

    public int Compare(long x, long y) => -x.CompareTo(y);
}
