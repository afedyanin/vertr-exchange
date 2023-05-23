using Vertr.Infrastructure.Common.Messaging;

namespace Vertr.Infrastructure.Common.Tests
{
    public class TopicTests
    {
        [Test]
        public async Task CanWorkWithTopic()
        {
            var topic = new ChannelBasedTopic<int>("some");

            var cts = new CancellationTokenSource();

            var producer = Task.Run(async () =>
            {
                for (int i = 0; i <= 10; i++)
                {
                    await topic.Produce(i);
                }
            });

            var consumer = Task.Run(async () =>
            {
                var item = -1;

                while (item != 10)
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
}
