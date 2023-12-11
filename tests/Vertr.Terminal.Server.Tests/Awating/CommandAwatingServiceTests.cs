using Vertr.Exchange.Contracts;
using Vertr.Exchange.Tests.Stubs;
using Vertr.Terminal.Server.Awaiting;

namespace Vertr.Terminal.Server.Tests.Awating;

[TestFixture(Category = "Unit")]
public class CommandAwatingServiceTests
{
    [Test]
    public async Task CanAwaitSimpleTask()
    {
        var logger = LoggerStub.CreateConsoleLogger<CommandAwaitingService>();
        var service = new CommandAwaitingService(logger);

        var commandId = 100L;
        CommandResponse? response = null;

        Console.WriteLine("T1 starting...");
        var t1 = Task.Run(async () =>
        {
            Console.WriteLine("Register starting...");
            response = await service.Register(commandId);
            Console.WriteLine("Register completed.");
        });

        Console.WriteLine("T2 starting...");
        var t2 = Task.Run(() =>
        {
            Console.WriteLine("Set result starting...");
            var apiResult = new ApiCommandResult
            {
                OrderId = commandId,
            };

            var resp = new CommandResponse(apiResult);
            service.Complete(resp);
            Console.WriteLine("Set result completed.");
        });

        Console.WriteLine("Awaiting T1 & T2");
        await Task.WhenAll(t1, t2);

        Assert.That(response, Is.Not.Null);
        Assert.That(response.CommandResult.OrderId, Is.EqualTo(commandId));
        Console.WriteLine("Completed.");
    }


}
