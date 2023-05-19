using Vertr.Common.Contracts;

namespace Vertr.Common.Implementation
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
