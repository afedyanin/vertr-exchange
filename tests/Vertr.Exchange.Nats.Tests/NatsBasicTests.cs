using NATS.Client.Core;

namespace Vertr.Exchange.Nats.Tests;

[TestFixture(Category = "Integration")]
public class NatsBasicTests
{
    [Test]
    // https://natsbyexample.com/examples/messaging/pub-sub/dotnet2
    public async Task CanReceivePublishedMessages()
    {
        var opts = NatsOpts.Default with { Url = "127.0.0.1:4222" };
        Console.WriteLine($"Connecting to {opts.Url}...");

        await using var natsConn = new NatsConnection(opts);

        await using var sub = await natsConn.SubscribeAsync<Order>("orders.>");

        Console.WriteLine("[SUB] waiting for messages...");

        var task = Task.Run(async () =>
        {
            await foreach (var msg in sub.Msgs.ReadAllAsync())
            {
                var order = msg.Data;
                Console.WriteLine($"[SUB] received {msg.Subject}: {order}");
            }


            Console.WriteLine($"[SUB] unsubscribed");
        });

        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine($"[PUB] publishing order {i}...");
            await natsConn.PublishAsync($"orders.new.{i}", new Order(OrderId: i));
            await Task.Delay(1_000);
        }

        await sub.UnsubscribeAsync();
        await task;

        Console.WriteLine("Bye!");
    }
}

public record Order(int OrderId);
