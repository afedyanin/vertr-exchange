using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Vertr.Exchange.Grpc.Services;

public class ExchangeApiService : ExchangeApi.ExchangeApiBase
{
    private readonly ILogger<ExchangeApiService> _logger;

    public ExchangeApiService(ILogger<ExchangeApiService> logger)
    {
        _logger = logger;
    }

    public override Task<ApiCommandResult> Nop(ApiCommandNoParams request, ServerCallContext context)
    {
        _logger.LogDebug("NOP command received.");

        var res = new ApiCommandResult()
        {
            CommandResultCode = CommandResultCode.Success,
            OrderId = 1000L,
            Timestamp = DateTime.UtcNow.ToTimestamp(),
        };

        return Task.FromResult(res);
    }

    public override Task<ApiCommandResult> GetOrderBook(OrderBookRequest request, ServerCallContext context)
    {
        _logger.LogDebug("GetOrderBook command received.");

        var res = new ApiCommandResult()
        {
            CommandResultCode = CommandResultCode.Success,
            OrderId = 1000L,
            Timestamp = DateTime.UtcNow.ToTimestamp(),
            MarketData = CreateMarketData(),
        };

        return Task.FromResult(res);
    }

    private static L2MarketData CreateMarketData()
    {
        var res = new L2MarketData
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
