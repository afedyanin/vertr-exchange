namespace Vertr.Common.Contracts
{
    public interface IEntity<T> where T : struct
    {
        T Id { get; }
    }
}
