using Vertr.Infrastructure.Common.Contracts;

namespace Vertr.Infrastructure.Common.Implementation
{
    public sealed class RealTimeService : ITimeService
    {
        public DateTime GetCurrentUtcTime()
        {
            return DateTime.UtcNow;
        }
    }
}
