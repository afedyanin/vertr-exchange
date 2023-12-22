using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Requests;
using Vertr.Exchange.Shared.Enums;
using Vertr.Terminal.ApiClient.Contracts;
using Vertr.Terminal.ApiClient.Extensions;

namespace Vertr.Terminal.ApiClient;
public sealed class ApiCommands(ITerminalApiClient client)
{
    private readonly ITerminalApiClient _client = client;
    public async Task Reset()
    {
        var res = await _client.Reset();
        EnsureSuccess(res);
    }

    public async Task AddSymbols(Symbol[] symbols)
    {
        var req = new AddSymbolsRequest()
        {
            Symbols = symbols.Select(s => s.GetSpecification()).ToArray(),
        };

        var res = await _client.AddSymbols(req);
        EnsureSuccess(res);
    }
    public async Task AddUsers(Contracts.UserAccount[] userAccounts)
    {
        var req = new AddAccountsRequest()
        {
            UserAccounts = userAccounts.Select(ua => ua.ToDto()).ToArray(),
        };

        var res = await _client.AddAccounts(req);
        EnsureSuccess(res);
    }

    public async Task Nop()
    {
        var res = await _client.Nop();
        EnsureSuccess(res);
    }

    private static void EnsureSuccess(ApiCommandResult? result)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (result.ResultCode != CommandResultCode.SUCCESS)
        {
            throw new InvalidOperationException($"Invalid result code: {result.ResultCode}");
        }
    }
}
