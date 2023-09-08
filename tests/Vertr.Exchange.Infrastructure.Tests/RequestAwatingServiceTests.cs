using Vertr.Exchange.Common;

namespace Vertr.Exchange.Infrastructure.Tests;

[TestFixture(Category = "Unit")]
public class RequestAwatingServiceTests
{
    [Test]
    public async Task CanRegisterRequest()
    {
        var svc = new RequestAwatingService();
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
