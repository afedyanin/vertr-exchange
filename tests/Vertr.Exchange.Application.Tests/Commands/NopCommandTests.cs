using Vertr.Exchange.Application.Commands;
using Vertr.Exchange.Domain.Common.Enums;

namespace Vertr.Exchange.Application.Tests.Commands;

[TestFixture(Category = "Unit")]
public class NopCommandTests : ApiTestBase
{

    [Test]
    public void CanSendNopCommand()
    {
        var cmd = new NopCommand(1L, DateTime.UtcNow);

        Api.Send(cmd);

        Assert.Pass();
    }

    [Test]
    public async Task CanSendAsyncNopCommand()
    {
        var cmd = new NopCommand(1L, DateTime.UtcNow);

        var res = await SendAsync(cmd);

        Assert.Multiple(() =>
        {
            Assert.That(res.OrderId, Is.EqualTo(cmd.OrderId));
            Assert.That(res.Timestamp, Is.EqualTo(cmd.Timestamp));
            Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
        });
    }
}
