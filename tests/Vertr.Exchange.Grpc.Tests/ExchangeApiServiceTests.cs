using Grpc.Net.Client;

namespace Vertr.Exchange.Grpc.Tests;

[TestFixture(Category = "Integration")]
public class ExchangeApiServiceTests
{
    [Test]
    public async Task NopCommand()
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:5294");

        var client = new Exchange.ExchangeClient(channel);
        var result = await client.NopAsync(new CommandNoParams());

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.CommandResultCode, Is.EqualTo(ResultCode.Success));
            Assert.That(result.OrderId, Is.GreaterThan(0L));
            Assert.That(result.Timestamp.ToDateTime(), Is.GreaterThan(DateTime.MinValue));
            Assert.That(result.MarketData, Is.Null);
        });
    }

    [Test]
    public async Task GetOrderBookCommand()
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:5294");

        var client = new Exchange.ExchangeClient(channel);
        var result = await client.GetOrderBookAsync(new OrderBookRequest
        {
            Symbol = 1,
            Size = 100
        });

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.CommandResultCode, Is.EqualTo(ResultCode.Success));
            Assert.That(result.MarketData, Is.Not.Null);
            Assert.That(result.Events, Is.Empty);
        });

        var prices = result.MarketData.AskPrices.Select(p => p.ToDecimal());
        var str = string.Join(";", prices);
        Console.WriteLine(str);
    }

    [Test]
    public async Task CanCallNopCommandViaHttps()
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        using var channel = GrpcChannel.ForAddress("https://localhost:7149",
            new GrpcChannelOptions { HttpHandler = handler });

        var client = new Exchange.ExchangeClient(channel);
        var result = await client.NopAsync(new CommandNoParams());

        Assert.That(result, Is.Not.Null);
    }
}
