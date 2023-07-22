using System.Threading.Channels;
using Vertr.Infrastructure.Common.Contracts;

namespace Vertr.Infrastructure.Common.Messaging
{
    public class ChannelBasedTopic<T> : ITopic<T>
    {
        private readonly Channel<T> _channel;

        public string Name { get; }

        public ChannelBasedTopic(string name)
        {
            Name = name;
            _channel = Channel.CreateUnbounded<T>();
        }

        public async ValueTask Produce(T item, CancellationToken token = default)
        {
            await _channel.Writer.WriteAsync(item, token);
        }

        public async ValueTask<T> Consume(CancellationToken token = default)
        {
            return await _channel.Reader.ReadAsync(token);
        }
    }
}
