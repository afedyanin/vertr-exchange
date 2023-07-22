using System.Collections.Concurrent;
using Vertr.OrderMatching.Domain.Contracts;

namespace Vertr.OrderMatching.Domain.Repositories
{
    public class EntityInMemoryRepository<T> where T : IEntity<Guid>
    {
        private readonly ConcurrentDictionary<Guid, T> _dictionary = new();

        public bool Delete(Guid entityId)
        {
            return _dictionary.TryRemove(entityId, out _);
        }

        public T[] GetAll()
        {
            return _dictionary.Values.ToArray();
        }

        public T? GetById(Guid entityId)
        {
            var found = _dictionary.TryGetValue(entityId, out var entity);
            return found ? entity : default;
        }

        public bool Insert(T entity)
        {
            return _dictionary.TryAdd(entity.Id, entity);
        }
    }
}
