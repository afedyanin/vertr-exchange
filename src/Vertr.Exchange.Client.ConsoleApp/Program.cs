using Grpc.Net.Client;
using Vertr.Exchange.Client.ConsoleApp.Extensions;
using static Vertr.Exchange.Protos.Exchange;

namespace Vertr.Exchange.Client.ConsoleApp;

public class Program
{
    public static async Task Main()
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:5000");
        var client = new ExchangeClient(channel);
        var reply = await client.RegisterSymbols();
        // Ошибки не доходят - кейс с дубликатом ID
        // Попробовать зарегать повторно
        // Выключение сервера зависает
        // Куда сейчас пишет серверный логер? перенаправить в файл+консоль
        // Убрать NATS и заменть inMemoryDB - сделать отдельный grpc API к БД + grpc стриминг к ней
        Console.WriteLine($"code={reply.CommandResultCode} orderId={reply.OrderId}");
    }

    /*
    private static async Task SubscribeToNats()
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
    */
}
