using Grpc.Core;
using Vertr.Exchange.Api;
using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Grpc.Extensions;

namespace Vertr.Exchange.Grpc.Services;

public class ExchangeApiService : Exchange.ExchangeBase
{
    private readonly IExchangeApi _api;

    public ExchangeApiService(IExchangeApi api)
    {
        _api = api;
    }

    public override async Task<CommandResult> Nop(CommandNoParams request, ServerCallContext context)
    {
        var cmd = new NopCommand(
            100L,
            DateTime.UtcNow);

        var apiResult = await _api.SendAsync(cmd);
        return apiResult.ToGrpc();
    }

    public override async Task<CommandResult> GetOrderBook(OrderBookRequest request, ServerCallContext context)
    {
        var cmd = new Api.Commands.OrderBookRequest(
            100L,
            DateTime.UtcNow,
            request.Symbol,
            request.Size);

        var apiResult = await _api.SendAsync(cmd);
        return apiResult.ToGrpc();
    }
}
