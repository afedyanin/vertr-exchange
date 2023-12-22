using Refit;
using Vertr.Exchange.Contracts.Requests;
using Vertr.Exchange.Shared.Enums;
using Vertr.Terminal.ApiClient.Extensions;

namespace Vertr.Terminal.ApiClient.Tests;

/*
C:\Users\Anatoly\Documents\GitHub\vertr\src\Vertr.Exchange.Server\bin\Debug\net8.0
D:\workspace\vertr\src\Vertr.Exchange.Server\bin\Debug\net8.0
.\Vertr.Exchange.Server.exe


C:\Users\Anatoly\Documents\GitHub\vertr\src\Vertr.Terminal.Server\bin\Debug\net8.0
D:\workspace\vertr\src\Vertr.Terminal.Server\bin\Debug\net8.0
.\Vertr.Terminal.Server.exe
 */

[TestFixture(Category = "System")]
public class AdminTests
{
    private ITerminalApiClient _api;

    [SetUp]
    public void SetUp()
    {
        _api = RestService.For<ITerminalApiClient>("http://localhost:5010");
    }

    [TearDown]
    public async Task CleanUp()
    {
        await _api.Reset();
    }

    [Test]
    public async Task CanReset()
    {
        var res = await _api.Reset();

        Assert.That(res, Is.Not.Null);
        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
    }

    [Test]
    public async Task CanAddSymbols()
    {
        var req = new AddSymbolsRequest()
        {
            Symbols = StaticContext.Symbols.All.Select(s => s.GetSpecification()).ToArray(),
        };

        var res = await _api.AddSymbols(req);

        Assert.That(res, Is.Not.Null);
        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
    }

    [Test]
    public async Task CanAddAccounts()
    {
        var req = new AddAccountsRequest()
        {
            UserAccounts =
            [
                StaticContext.UserAccounts.AliceAccount.ToDto(),
                StaticContext.UserAccounts.BobAccount.ToDto()
            ],
        };

        var res = await _api.AddAccounts(req);

        Assert.That(res, Is.Not.Null);
        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
    }
}
