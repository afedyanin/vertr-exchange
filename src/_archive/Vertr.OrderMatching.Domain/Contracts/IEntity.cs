namespace Vertr.OrderMatching.Domain.Contracts
{
    public interface IEntity<T> where T : struct
    {
        T Id { get; }
    }
}
