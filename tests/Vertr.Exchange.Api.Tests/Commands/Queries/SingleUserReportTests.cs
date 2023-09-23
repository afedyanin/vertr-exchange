using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Api.Commands.Queries;
using Vertr.Exchange.Api.Tests.Stubs;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Tests.Commands.Queries;


[TestFixture(Category = "Unit")]
public class SingleUserReportTests : ApiTestBase
{
    [Test]
    public async Task CanGetUserReport()
    {
        var cmd = new AddAccountsCommand(1L, DateTime.UtcNow, AccountsStub.UserAccounts);
        var res = await Api.SendAsync(cmd);
        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        var rep = new SingleUserReport(2L, DateTime.UtcNow, AccountsStub.FirstUserUid);
        res = await Api.SendAsync(rep);
        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        var report = rep.GetResult(res);
        Assert.That(report, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(report.Uid, Is.EqualTo(AccountsStub.FirstUserUid));
            Assert.That(report.UserStatus, Is.EqualTo(UserStatus.ACTIVE));
            Assert.That(report.ExecutionStatus, Is.EqualTo(QueryExecutionStatus.OK));

            foreach (var acct in report.Accounts)
            {
                Assert.That(AccountsStub.FirstUserAccounts.ContainsKey(acct.Key));
                Assert.That(AccountsStub.FirstUserAccounts[acct.Key], Is.EqualTo(acct.Value));
            }

            // TODO: Check orders
            // TODO: Check positions
        });
    }

    [Test]
    public async Task CanGetEmptyReportForNotExistingUser()
    {
        var rep = new SingleUserReport(2L, DateTime.UtcNow, AccountsStub.FirstUserUid);
        var res = await Api.SendAsync(rep);
        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        var report = rep.GetResult(res);
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
    }
}
