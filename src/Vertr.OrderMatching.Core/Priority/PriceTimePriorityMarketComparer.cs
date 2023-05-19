namespace Vertr.OrderMatching.Core.Priority
{
    internal class PriceTimePriorityMarketComparer : IComparer<PriceTimePriority>
    {
        public int Compare(PriceTimePriority x, PriceTimePriority y)
        {
            if (x.Time < y.Time)
            {
                return -1;
            }

            if (x.Time > y.Time)
            {
                return 1;
            }

            return 0;
        }
    }
}
