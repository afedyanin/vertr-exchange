using Vertr.Common.Contracts;

namespace Vertr.Common.Implementation
{
    public sealed class RealTimeService : ITimeService
    {
        public DateTime GetCurrentUtcTime()
        {
            return DateTime.UtcNow;
        }
    }
}
