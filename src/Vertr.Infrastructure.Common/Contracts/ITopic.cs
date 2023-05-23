namespace Vertr.Infrastructure.Common.Contracts
{
    public interface ITopic<T>
    {
        string Name { get; }

        ValueTask Produce(T item, CancellationToken token = default);

        ValueTask<T> Consume(CancellationToken token = default);
    }
}
