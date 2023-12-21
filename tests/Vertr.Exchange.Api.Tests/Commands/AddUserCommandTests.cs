using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Api.Tests.Commands;

[TestFixture(Category = "Unit")]
public class AddUserCommandTests : ApiTestBase
{
    [Test]
    public async Task CanAddUser()
    {
        var cmd = new AddUserCommand(1L, DateTime.UtcNow, 100L);

        var res = await Api.SendAsync(cmd);

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

        var res1 = await Api.SendAsync(cmd1);
        var res2 = await Api.SendAsync(cmd2);

        Assert.Multiple(() =>
        {
            Assert.That(res1.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
            Assert.That(res2.ResultCode, Is.EqualTo(CommandResultCode.USER_MGMT_USER_ALREADY_EXISTS));
        });
    }
}
