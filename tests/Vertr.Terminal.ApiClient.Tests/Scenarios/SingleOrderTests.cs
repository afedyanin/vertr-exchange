using Refit;
using Vertr.Exchange.Contracts.Requests;
using Vertr.Exchange.Shared.Reports;

namespace Vertr.Terminal.ApiClient.Tests.Scenarios;

[TestFixture(Category = "System")]
public class SingleOrderTests
{
    private ApiCommands _api;

    [SetUp]
    public void SetUp()
    {
        var apiClient = RestService.For<ITerminalApiClient>("http://localhost:5010");
        _api = new ApiCommands(apiClient);
    }

    [Test]
    public async Task InitialProfileHasValidState()
    {
        await _api.Reset();
        await _api.AddSymbols(StaticContext.Symbols.All);
        await _api.AddUsers([StaticContext.UserAccounts.BobAccount]);

        var request = new UserRequest
        {
            UserId = StaticContext.Users.Bob.Id,
        };

        var profile = await _api.GetSingleUserReport(request);
    }

    public void ValidateInitialProfile(SingleUserReportResult reportResult)
    {

    }
}
