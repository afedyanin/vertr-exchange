using Grpc.Net.Client;

namespace Vertr.Exchange.Grpc.Tests;

[TestFixture(Category = "Integration")]
public class GreeterServiceTests
{
    [Test]
    public async Task CanCallGrpcServiceViaHttp()
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:5294");

        var client = new Greeter.GreeterClient(channel);
        var reply = await client.SayHelloAsync(new HelloRequest { Name = "GreeterClient HTTP" });

        Assert.That(reply, Is.Not.Null);
        Console.WriteLine(reply);
    }

    [Test]
    public async Task CanCallGrpcServiceViaHttps()
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        using var channel = GrpcChannel.ForAddress("https://localhost:7149",
            new GrpcChannelOptions { HttpHandler = handler });

        var client = new Greeter.GreeterClient(channel);
        var reply = await client.SayHelloAsync(new HelloRequest { Name = "GreeterClient HTTPS" });

        Assert.That(reply, Is.Not.Null);
        Console.WriteLine(reply);
    }
}
