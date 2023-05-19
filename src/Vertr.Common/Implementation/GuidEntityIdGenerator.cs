using Vertr.Common.Contracts;

namespace Vertr.Common.Implementation
{
    public sealed class GuidEntityIdGenerator : IEntityIdGenerator<Guid>
    {
        public Guid GetNextId()
        {
            return Guid.NewGuid();
        }
    }
}
