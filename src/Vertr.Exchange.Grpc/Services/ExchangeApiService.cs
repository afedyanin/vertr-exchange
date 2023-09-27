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
        _logger.LogDebug("Receiveing NOP command");

        var res = new ApiCommandResult()
        {
            CommandResultCode = CommandResultCode.Success,
            OrderId = 1000L,
            Timestamp = DateTime.UtcNow.ToTimestamp(),
        };

        return Task.FromResult(res);
    }
}
