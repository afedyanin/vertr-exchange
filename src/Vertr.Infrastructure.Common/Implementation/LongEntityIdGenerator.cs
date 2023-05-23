using Vertr.Infrastructure.Common.Contracts;

namespace Vertr.Infrastructure.Common.Implementation
{
    public sealed class LongEntityIdGenerator : IEntityIdGenerator<long>
    {
        private long _next = 0L;

        public long GetNextId()
        {
            return Interlocked.Increment(ref _next);
        }
    }
}
