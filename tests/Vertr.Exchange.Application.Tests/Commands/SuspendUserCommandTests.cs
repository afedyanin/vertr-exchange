using Vertr.Exchange.Application.Commands;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Application.Tests.Commands;

[TestFixture(Category = "Unit")]
public class SuspendUserCommandTests : ApiTestBase
{
    [Test]
    public async Task CanSuspendUser()
    {
        var add = new AddUserCommand(1L, DateTime.UtcNow, 100L);
        var res = await SendAsync(add);

        var susp = new SuspendUserCommand(2L, DateTime.UtcNow, add.Uid);
        res = await SendAsync(susp);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
    }

    [Test]
    public async Task CannotSuspendAlreadySuspenedUser()
    {
        var add = new AddUserCommand(1L, DateTime.UtcNow, 100L);
        var res = await SendAsync(add);

        var susp1 = new SuspendUserCommand(2L, DateTime.UtcNow, add.Uid);
        var res1 = await SendAsync(susp1);

        var susp2 = new SuspendUserCommand(3L, DateTime.UtcNow, add.Uid);
        var res2 = await SendAsync(susp2);

        Assert.That(res2.ResultCode, Is.EqualTo(CommandResultCode.USER_MGMT_USER_ALREADY_SUSPENDED));
    }

    [Test]
    public async Task CannotSuspendNotExistingUser()
    {
        var add = new AddUserCommand(1L, DateTime.UtcNow, 100L);
        var res = await SendAsync(add);

        var susp = new SuspendUserCommand(2L, DateTime.UtcNow, 200L);
        res = await SendAsync(susp);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.USER_MGMT_USER_NOT_FOUND));
    }

    [Test]
    public async Task CannotSuspendUserHasPositions()
    {
        var makerUid = 100L;
        var takerUid = 102L;
        var symbol = 2;

        await AddUser(makerUid);
        await AddUser(takerUid);
        await AddSymbol(symbol);
        await PlaceGTCOrder(OrderAction.BID, makerUid, symbol, 23.45m, 34);
        await PlaceGTCOrder(OrderAction.ASK, takerUid, symbol, 23.10m, 30);

        var susp = new SuspendUserCommand(23L, DateTime.UtcNow, makerUid);
        var res = await SendAsync(susp);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.USER_MGMT_USER_NOT_SUSPENDABLE_HAS_POSITIONS));
    }

    [Test]
    public async Task CannotSuspendUserNonEmptyAccounts()
    {
        var uid = 100L;
        await AddUser(uid);

        var adj = new AdjustBalanceCommand(21L, DateTime.UtcNow, uid, 10, 45.34M);
        var res = await SendAsync(adj);

        var susp = new SuspendUserCommand(23L, DateTime.UtcNow, uid);
        res = await SendAsync(susp);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.USER_MGMT_USER_NOT_SUSPENDABLE_NON_EMPTY_ACCOUNTS));
    }
}
