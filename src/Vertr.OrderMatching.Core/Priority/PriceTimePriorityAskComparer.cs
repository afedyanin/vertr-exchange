namespace Vertr.OrderMatching.Core.Priority
{
    internal class PriceTimePriorityAskComparer : IComparer<PriceTimePriority>
    {
        public int Compare(PriceTimePriority x, PriceTimePriority y)
        {
            if (x.Price < y.Price)
            {
                return -1;
            }

            if (x.Price > y.Price)
            {
                return 1;
            }

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
