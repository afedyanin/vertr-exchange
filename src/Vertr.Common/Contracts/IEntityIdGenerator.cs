namespace Vertr.Common.Contracts
{
    public interface IEntityIdGenerator<T> where T : struct
    {
        T GetNextId();
    }
}
