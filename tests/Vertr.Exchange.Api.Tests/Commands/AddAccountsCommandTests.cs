using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Api.Tests.Stubs;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Api.Tests.Commands;

[TestFixture(Category = "Unit")]
public class AddAccountsCommandTests : ApiTestBase
{
    [Test]
    public async Task CanAddUserAccounts()
    {
        var cmd = new AddAccountsCommand(1002L, DateTime.UtcNow, AccountsStub.UserAccounts);
        var res = await SendAsync(cmd);
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
