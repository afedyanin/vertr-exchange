using Microsoft.Extensions.DependencyInjection;
using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Api.Tests.Stubs;

namespace Vertr.Exchange.Api.Tests.Commands;

[TestFixture(Category = "Unit")]
public class NopCommandTests
{
    private IServiceProvider _serviceProvider;
    private IExchangeApi _api;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _serviceProvider = ServiceProviderStub.BuildServiceProvider();
    }

    [SetUp]
    public void Setup()
    {
        _api = _serviceProvider.GetRequiredService<IExchangeApi>();
    }

    [TearDown]
    public void TearDown()
    {
        _api?.Dispose();
    }

    [Test]
    public void CanSendNopCommand()
    {
        var cmd = new NopCommand(1L, DateTime.UtcNow);

        _api.Send(cmd);

        Assert.Pass();
    }

    [Test]
    public async Task CanSendAsyncNopCommand()
    {
        var cmd = new NopCommand(1L, DateTime.UtcNow);

        var res = await _api.SendAsync(cmd);

        Assert.Multiple(() =>
        {
            Assert.That(res.OrderId, Is.EqualTo(cmd.OrderId));
            Assert.That(res.Timestamp, Is.EqualTo(cmd.Timestamp));
        });
    }
}
