using Grpc.Net.Client;

namespace Vertr.Exchange.Grpc.Tests;

[TestFixture(Category = "Integration")]
public class ExchangeApiServiceTests
{
    [Test]
    public async Task CanCallNopCommand()
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:5294");

        var client = new ExchangeApi.ExchangeApiClient(channel);
        var result = await client.NopAsync(new ApiCommandNoParams());

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.CommandResultCode, Is.EqualTo(CommandResultCode.Success));
            Assert.That(result.OrderId, Is.GreaterThan(0L));
            Assert.That(result.Timestamp.ToDateTime(), Is.GreaterThan(DateTime.MinValue));
        });
    }
}
