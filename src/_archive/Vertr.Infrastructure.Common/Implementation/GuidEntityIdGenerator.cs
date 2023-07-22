using Vertr.Infrastructure.Common.Contracts;

namespace Vertr.Infrastructure.Common.Implementation
{
    public sealed class GuidEntityIdGenerator : IEntityIdGenerator<Guid>
    {
        public Guid GetNextId()
        {
            return Guid.NewGuid();
        }
    }
}
