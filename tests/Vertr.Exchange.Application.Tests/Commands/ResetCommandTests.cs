using Vertr.Exchange.Application.Commands;
using Vertr.Exchange.Application.Tests.Stubs;
using Vertr.Exchange.Domain.Common.Enums;

namespace Vertr.Exchange.Application.Tests.Commands;

public class ResetCommandTests : ApiTestBase
{
    [Test]
    public async Task CanSendResetCommand()
    {
        var cmd = new AddAccountsCommand(OrderIdGenerator.NextId, DateTime.UtcNow, AccountsStub.UserAccounts);
        var res = await SendAsync(cmd);
        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        var reset = new ResetCommand(OrderIdGenerator.NextId, DateTime.UtcNow);
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
