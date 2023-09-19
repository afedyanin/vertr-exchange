using Microsoft.Extensions.DependencyInjection;
using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Api.Tests.Stubs;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Tests.Commands;

[TestFixture(Category = "Unit")]
public class AddUserCommandTests
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
    public async Task CanAddUser()
    {
        var cmd = new AddUserCommand(1L, DateTime.UtcNow, 100L);

        var res = await _api.SendAsync(cmd);

        Assert.Multiple(() =>
        {
            Assert.That(res.OrderId, Is.EqualTo(cmd.OrderId));
            Assert.That(res.Timestamp, Is.EqualTo(cmd.Timestamp));
            Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
        });
    }

    [Test]
    public async Task CannotAddUserTwice()
    {
        var cmd1 = new AddUserCommand(1L, DateTime.UtcNow, 100L);
        var cmd2 = new AddUserCommand(2L, DateTime.UtcNow, 100L);

        var res1 = await _api.SendAsync(cmd1);
        var res2 = await _api.SendAsync(cmd2);

        Assert.Multiple(() =>
        {
            Assert.That(res1.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
            Assert.That(res2.ResultCode, Is.EqualTo(CommandResultCode.USER_MGMT_USER_ALREADY_EXISTS));
        });
    }
}
