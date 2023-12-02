using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Vertr.Exchange.Client.ConsoleApp.StaticData;
using Vertr.Exchange.Protos;

namespace Vertr.Exchange.Client.ConsoleApp;

public class Program
{
    public static async Task Main()
    {
        await ConnectToHub();
    }

    private static async Task ConnectToHub()
    {
        var connection = new HubConnectionBuilder()
            .WithUrl(
            url: "http://localhost:5000/exchange",
            transports: HttpTransportType.WebSockets,
            options =>
            {
                options.SkipNegotiation = true;
            })
            .ConfigureLogging(logging =>
            {
                logging.AddConsole();
            })
            .AddMessagePackProtocol()
            .Build();

        await connection.StartAsync();

        Console.WriteLine("Starting connection. Press Ctrl-C to close.");
        var cts = new CancellationTokenSource();

        Console.CancelKeyPress += (sender, a) =>
        {
            a.Cancel = true;
            cts.Cancel();
        };

        connection.Closed += e =>
        {
            Console.WriteLine("Connection closed with error: {0}", e);

            cts.Cancel();
            return Task.CompletedTask;
        };

        var t1 = ListenToApiCommandReultStream(connection, cts);
        var t2 = SetupSymbols(connection);
        await Task.WhenAll(t1, t2);
    }

    private static Task ListenToApiCommandReultStream(HubConnection connection, CancellationTokenSource cts)
    {
        return Task.Run(async () =>
        {
            Console.WriteLine($"Start listening API Command result stream...");
            var channel = await connection.StreamAsChannelAsync<ApiCommandResult>("ApiCommandResults", CancellationToken.None);
            while (await channel.WaitToReadAsync() && !cts.IsCancellationRequested)
            {
                while (channel.TryRead(out var apiCommandResult))
                {
                    Console.WriteLine($"API Commad result received. OrderId={apiCommandResult.OrderId} ResultCode={apiCommandResult.ResultCode}");
                }
            }
        });
    }
    private static Task SetupSymbols(HubConnection connection)
    {
        return Task.Run(async () =>
        {
            Console.WriteLine($"Setup Symbols...");
            var req = CreateAddSymbolsRequest();
            await connection.InvokeCoreAsync("AddSymbols", new object[] { req });
            Console.WriteLine($"Symbol setup completed.");
        });
    }

    private static AddSymbolsRequest CreateAddSymbolsRequest()
    {
        var req = new AddSymbolsRequest();
        req.Symbols.Add(Symbols.All.Select(s => s.GetSpecification()));
        return req;
    }
}
