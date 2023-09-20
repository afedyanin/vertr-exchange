using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Tests.Commands;

[TestFixture(Category = "Unit")]
public class SuspendUserCommandTests : CommandTestBase
{
    [Test]
    public async Task CanSuspendUser()
    {
        var add = new AddUserCommand(1L, DateTime.UtcNow, 100L);
        var res = await Api.SendAsync(add);

        var susp = new SuspendUserCommand(2L, DateTime.UtcNow, add.Uid);
        res = await Api.SendAsync(susp);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
    }

    [Test]
    public async Task CannotSuspendAlreadySuspenedUser()
    {
        var add = new AddUserCommand(1L, DateTime.UtcNow, 100L);
        var res = await Api.SendAsync(add);

        var susp1 = new SuspendUserCommand(2L, DateTime.UtcNow, add.Uid);
        var res1 = await Api.SendAsync(susp1);

        var susp2 = new SuspendUserCommand(3L, DateTime.UtcNow, add.Uid);
        var res2 = await Api.SendAsync(susp2);

        Assert.That(res2.ResultCode, Is.EqualTo(CommandResultCode.USER_MGMT_USER_ALREADY_SUSPENDED));
    }

    [Test]
    public async Task CannotSuspendNotExistingUser()
    {
        var add = new AddUserCommand(1L, DateTime.UtcNow, 100L);
        var res = await Api.SendAsync(add);

        var susp = new SuspendUserCommand(2L, DateTime.UtcNow, 200L);
        res = await Api.SendAsync(susp);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.USER_MGMT_USER_NOT_FOUND));
    }

    [Test]
    public Task CannotSuspendUserHasPositions()
    {
        // TODO: Implement this
        // Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.USER_MGMT_USER_NOT_SUSPENDABLE_HAS_POSITIONS));
        return Task.CompletedTask;
    }

    [Test]
    public Task CannotSuspendUserNonEmptyAccounts()
    {
        // TODO: Implement this
        // Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.USER_MGMT_USER_NOT_SUSPENDABLE_NON_EMPTY_ACCOUNTS));
        return Task.CompletedTask;
    }
}
