using System.Threading.Channels;

namespace Vertr.Entropy.Tests
{
    public class ChannelTests
    {
        [Test]
        public async Task CanWorkWithTopic()
        {
            var topic = new Topic<int>("some");

            var cts = new CancellationTokenSource();

            var producer = Task.Run(async () =>
            {
                for (int i = 0; i<=8; i++)
                {
                    await topic.Produce(i);
                }
            });

            var consumer = Task.Run(async () =>
            {
                var item = -1; 

                while(item !=10)
                {
                    item = await topic.Consume(cts.Token);
                    Console.WriteLine(item);
                }
            });

            await Task.Delay(100);
            cts.Cancel();

            await Task.WhenAll(producer, consumer);

            Assert.Pass();
        }
    }

    public class Topic<T>
    {
        private readonly Channel<T> _channel;

        public string Name { get; }

        public Topic(string name)
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
