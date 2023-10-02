using NATS.Client.Core;
using Vertr.Exchange.Messages;

namespace Vertr.Exchange.Client.ConsoleApp;

public class Program
{
    public static async Task Main()
    {
        var opts = NatsOpts.Default with { Url = "127.0.0.1:4222" };
        Console.WriteLine($"Connecting to {opts.Url}...");

        await using var natsConn = new NatsConnection(opts);

        await using var sub = await natsConn.SubscribeAsync<ApiCommandResult>("commands.>");

        Console.WriteLine("[SUB] waiting for messages...");

        await foreach (var msg in sub.Msgs.ReadAllAsync())
        {
            var commandResult = msg.Data;
            Console.WriteLine($"[SUB] received {msg.Subject}: {commandResult}");
        }
    }
}
