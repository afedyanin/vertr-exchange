using Vertr.Exchange.Application.Commands;
using Vertr.Exchange.Application.Tests.Stubs;
using Vertr.Exchange.Domain.Common.Enums;

namespace Vertr.Exchange.Application.Tests.Commands;

[TestFixture(Category = "Unit")]
public class AdjustBalanceCommandTests : ApiTestBase
{
    [Test]
    public async Task CanAdjustBalance()
    {
        var cmd = new AddUserCommand(1L, DateTime.UtcNow, 100L);
        var res = await SendAsync(cmd);

        var adj = new AdjustBalanceCommand(2L, DateTime.UtcNow, 100L, 10, 45.34M);
        res = await SendAsync(adj);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
    }

    [Test]
    public async Task AdjustBalanceForNotExistingUserReturnsError()
    {
        var adj = new AdjustBalanceCommand(2L, DateTime.UtcNow, 100L, 10, 45.34M);
        var res = await SendAsync(adj);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.USER_MGMT_USER_NOT_FOUND));
    }

    [Test]
    public async Task CanAdjustBalanceAfterAddingAccounts()
    {
        var cmd = new AddAccountsCommand(1L, DateTime.UtcNow, AccountsStub.UserAccounts);
        var res = await SendAsync(cmd);
        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        var currency = AccountsStub.FirstUserAccounts.Keys.First();
        var adj = new AdjustBalanceCommand(2L, DateTime.UtcNow, AccountsStub.FirstUserUid, currency, 45.34M);
        res = await SendAsync(adj);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        // TODO: Chcek adjusted amount
    }
}
