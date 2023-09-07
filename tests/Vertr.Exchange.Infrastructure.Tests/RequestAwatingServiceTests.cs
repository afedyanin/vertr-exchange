namespace Vertr.Exchange.Infrastructure.Tests;

[TestFixture(Category = "Unit")]
public class RequestAwatingServiceTests
{
    [Test]
    public async Task CanRegisterRequest()
    {
        var svc = new RequestAwatingService();
        var orderId = 17L;
        var cts = new CancellationTokenSource(100);
        var res = await svc.Register(orderId, cts.Token);

        Assert.Pass();
    }
}
