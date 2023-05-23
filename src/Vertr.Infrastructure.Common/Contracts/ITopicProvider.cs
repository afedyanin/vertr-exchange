namespace Vertr.Infrastructure.Common.Contracts
{
    public interface ITopicProvider<T>
    {
        ITopic<T> GetOrAdd(string name);

        ITopic<T>? Get(string name);

        bool Remove(string name);
    }
}
