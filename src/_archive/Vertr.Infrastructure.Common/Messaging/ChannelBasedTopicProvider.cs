using System.Collections.Concurrent;
using Vertr.Infrastructure.Common.Contracts;

namespace Vertr.Infrastructure.Common.Messaging
{
    public class ChannelBasedTopicProvider<T> : ITopicProvider<T>
    {
        private readonly ConcurrentDictionary<string, ITopic<T>> _topics = new();

        public ITopic<T> GetOrAdd(string name)
        {
            var topic = new ChannelBasedTopic<T>(name);
            return _topics.GetOrAdd(name, topic);
        }

        public ITopic<T>? Get(string name)
        {
            var found = _topics.TryGetValue(name, out var topic);
            return found ? topic : null;
        }

        public bool Remove(string name)
        {
            return _topics.TryRemove(name, out var _);
        }
    }
}
