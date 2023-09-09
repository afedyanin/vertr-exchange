using Vertr.Exchange.Api.Commands;

namespace Vertr.Exchange.Api;

internal sealed class ExchangeApi
{
    public Task<ApiCommandResult> Execute(ApiCommand command)
    {
        var result = new ApiCommandResult();
        return Task.FromResult(result);
    }
}
