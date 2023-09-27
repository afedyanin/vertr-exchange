using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Vertr.Exchange.Api;
using Vertr.Exchange.Api.Commands;

namespace Vertr.Exchange.Grpc.Services;

public class ExchangeApiService : Exchange.ExchangeBase
{
    private readonly IExchangeApi _api;

    private readonly ILogger<ExchangeApiService> _logger;

    public ExchangeApiService(
        IExchangeApi api,
        ILogger<ExchangeApiService> logger)
    {
        _api = api;
        _logger = logger;
    }

    public override async Task<CommandResult> Nop(CommandNoParams request, ServerCallContext context)
    {
        _logger.LogDebug("NOP command received.");

        var cmd = new NopCommand(100L, DateTime.UtcNow);

        var result = await _api.SendAsync(cmd);

        var res = new CommandResult()
        {
            CommandResultCode = ResultCode.Success,
            OrderId = 1000L,
            Timestamp = DateTime.UtcNow.ToTimestamp(),
        };

        return res;
    }

    public override Task<CommandResult> GetOrderBook(OrderBookRequest request, ServerCallContext context)
    {
        _logger.LogDebug("GetOrderBook command received.");

        var res = new CommandResult()
        {
            CommandResultCode = ResultCode.Success,
            OrderId = 1000L,
            Timestamp = DateTime.UtcNow.ToTimestamp(),
            MarketData = CreateMarketData(),
        };

        return Task.FromResult(res);
    }

    private static Level2MarketData CreateMarketData()
    {
        var res = new Level2MarketData
        {
            AskSize = 1,
            BidSize = 1,
            Timestamp = DateTime.UtcNow.ToTimestamp(),
            ReferenceSeq = 1,
        };

        res.AskVolumes.AddRange(new long[] { 1, 2, 3, 4 });
        res.BidVolumes.AddRange(new long[] { 1, 2, 3, 4 });
        res.AskOrders.AddRange(new long[] { 10, 20, 30, 40 });
        res.BidOrders.AddRange(new long[] { 11, 21, 31, 41 });
        res.AskPrices.AddRange(new DecimalValue[] { 1.34m, 2.45m, 3.67m, 4.85m });
        res.BidPrices.AddRange(new DecimalValue[] { 1.34m, 2.45m, 3.67m, 4.85m });

        return res;
    }
}
