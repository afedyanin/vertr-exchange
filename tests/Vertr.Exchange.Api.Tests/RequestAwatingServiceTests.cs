using Vertr.Exchange.Api.Awaiting;
using Vertr.Exchange.Common;
using Vertr.Exchange.Tests.Stubs;

namespace Vertr.Exchange.Api.Tests;

[TestFixture(Category = "Unit")]
public class RequestAwatingServiceTests
{
    [Test]
    public async Task CanRegisterRequest()
    {
        var logger = LoggerStub.CreateConsoleLogger<RequestAwatingService>();
        var svc = new RequestAwatingService(logger);
        var orderId = 17L;
        var cts = new CancellationTokenSource(200);

        var t1 = Task.Run(async () =>
        {
            await Task.Delay(10);

            var cmd = new OrderCommand
            {
                OrderId = orderId,
            };

            var resp = new AwaitingResponse(cmd);
            svc.Complete(resp);
        });

        var res = await svc.Register(orderId, cts.Token);

        Assert.That(res.OrderCommand.OrderId, Is.EqualTo(orderId));
    }
}
