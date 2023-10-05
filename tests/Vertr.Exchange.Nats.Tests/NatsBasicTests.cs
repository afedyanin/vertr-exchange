using NATS.Client.Core;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.Common.Messages;

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

    [Test]
    public async Task CanReceiveApiCommandResult()
    {
        var opts = NatsOpts.Default with { Url = "127.0.0.1:4222" };
        Console.WriteLine($"Connecting to {opts.Url}...");

        await using var natsConn = new NatsConnection(opts);

        await using var sub = await natsConn.SubscribeAsync<ApiCommandResult>("commands.>");

        Console.WriteLine("[SUB] waiting for messages...");

        var task = Task.Run(async () =>
        {
            await foreach (var msg in sub.Msgs.ReadAllAsync())
            {
                var commandResult = msg.Data;
                Console.WriteLine($"[SUB] received {msg.Subject}: {commandResult}");
            }

            Console.WriteLine($"[SUB] unsubscribed");
        });

        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine($"[PUB] publishing api command result...");

            var cmd = new ApiCommandResult
            {
                OrderId = i,
                ResultCode = CommandResultCode.ACCEPTED,
                Seq = i * 100,
                Timestamp = DateTime.Now,
                Uid = 77,
            };

            await natsConn.PublishAsync($"commands.{i}", cmd);
            await Task.Delay(1_000);
        }

        await sub.UnsubscribeAsync();
        await task;

        Console.WriteLine("Bye!");
    }
}

public record Order(int OrderId);


