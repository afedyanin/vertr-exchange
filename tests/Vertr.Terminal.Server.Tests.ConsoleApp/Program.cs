using Vertr.Exchange.Contracts;
using Vertr.Exchange.Tests.Stubs;
using Vertr.Terminal.Server.Awaiting;

namespace Vertr.Terminal.Server.Tests.ConsoleApp;

internal sealed class Program
{
    public static async Task Main()
    {
        await Test001();
    }

    public static async Task Test003()
    {
        var logger = LoggerStub.CreateConsoleLogger<CommandAwaitingService>();
        var service = new CommandAwaitingService(logger);

        Console.WriteLine("T1 starting...");

        var t1 = Task.Run(async () =>
        {
            var apiResult = new ApiCommandResult
            {
                OrderId = 100L,
            };

            var resp = new CommandResponse(apiResult);
            service.Complete(resp);
            await Task.Delay(10);
        });

        Console.WriteLine("T2 starting...");
        var t2 = Task.Run(async () =>
        {
            await Task.Delay(10);
            var response = await service.Register(100L);
        });

        Console.WriteLine("Awaiting T1 & T2");
        await Task.WhenAll(t1, t2);

        Console.WriteLine("Completed.");
    }

    public static async Task Test002()
    {
        var logger = LoggerStub.CreateConsoleLogger<CommandAwaitingService>();
        var service = new CommandAwaitingService(logger);

        var apiResult = new ApiCommandResult
        {
            OrderId = 100L,
        };

        var resp = new CommandResponse(apiResult);
        service.Complete(resp);

        await Task.Delay(200);
        var response = await service.Register(100L);

        Console.WriteLine($"Completed. OrderId={response.CommandResult.OrderId}");
    }

    public static async Task Test001()
    {
        var logger = LoggerStub.CreateConsoleLogger<CommandAwaitingService>();
        var service = new CommandAwaitingService(logger);

        Console.WriteLine("T1 starting...");
        var t1 = Task.Run(async () =>
        {
            await RegisterWait(service);
        });

        Console.WriteLine("T2 starting...");
        var t2 = Task.Run(async () =>
        {
            await SetCompleted(service);
        });

        Console.WriteLine("Awaiting T1 & T2");
        await Task.WhenAll(t1, t2);

        Console.WriteLine("Completed.");
    }

    private static async Task RegisterWait(CommandAwaitingService service)
    {
        for (int i = 0; i < 100; i++)
        {
            Console.WriteLine($"Register #{i}");
            var response = await service.Register(i);
            await Task.Delay(Random.Shared.Next(0, 10));
            Console.WriteLine($"Register completed. #{i}  OrderId={response.CommandResult.OrderId}");
        }
    }

    private static async Task SetCompleted(CommandAwaitingService service)
    {
        for (int i = 0; i < 100; i++)
        {
            var apiResult = new ApiCommandResult
            {
                OrderId = i,
            };

            var resp = new CommandResponse(apiResult);
            service.Complete(resp);
            await Task.Delay(Random.Shared.Next(0, 10));
            Console.WriteLine($"Set result completed. #{i}");
        }
    }
}
