using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Api.Tests.Stubs;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Api.Tests.Commands;

public class ResetCommandTests : ApiTestBase
{
    [Test]
    public async Task CanSendResetCommand()
    {
        var cmd = new AddAccountsCommand(1L, DateTime.UtcNow, AccountsStub.UserAccounts);
        var res = await SendAsync(cmd);
        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        var reset = new ResetCommand(2L, DateTime.UtcNow);
        res = await SendAsync(reset);

        var report = await GetUserReport(AccountsStub.FirstUserUid);
        Assert.That(report, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(report.Uid, Is.EqualTo(AccountsStub.FirstUserUid));
            Assert.That(report.UserStatus, Is.EqualTo(UserStatus.SUSPENDED));
            Assert.That(report.ExecutionStatus, Is.EqualTo(QueryExecutionStatus.USER_NOT_FOUND));
            Assert.That(report.Accounts, Is.Empty);
            Assert.That(report.Positions, Is.Empty);
            Assert.That(report.Orders, Is.Empty);
        });

        Assert.Pass();
    }
}
