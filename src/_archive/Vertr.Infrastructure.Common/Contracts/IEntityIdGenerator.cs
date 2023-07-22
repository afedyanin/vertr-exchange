namespace Vertr.Infrastructure.Common.Contracts
{
    public interface IEntityIdGenerator<T> where T : struct
    {
        T GetNextId();
    }
}
