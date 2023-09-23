using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Api.Tests.Stubs;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Tests.Commands;

[TestFixture(Category = "Unit")]
public class AddAccountsCommandTests : CommandTestBase
{
    [Test]
    public async Task CanAddUserAccounts()
    {
        var cmd = new AddAccountsCommand(1L, DateTime.UtcNow, AccountsStub.UserAccounts);
        var res = await Api.SendAsync(cmd);
        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
    }

    [Test]
    public void CanAddAccountWithAnotherCurrency()
    {
        // TODO: Implement this
        // Need to get Account info first
        Assert.Pass();
    }

    [Test]
    public void CanAddAccountForExistingUser()
    {
        // TODO: Implement this
        // Need to get Account info first
        Assert.Pass();
    }
}