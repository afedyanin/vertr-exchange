using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Vertr.Exchange.Api.Awaiting;
using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Infrastructure;

[assembly: InternalsVisibleTo("Vertr.Exchange.Api.Tests")]

namespace Vertr.Exchange.Api;

internal sealed class ExchangeApi
{
    private readonly IRequestAwaitingService _requestAwaitingService;
    private readonly IExchangeCoreService _exchangeCoreService;
    private readonly ILogger<ExchangeApi> _logger;

    public ExchangeApi(
        IRequestAwaitingService requestAwaitingService,
        IExchangeCoreService exchangeCoreService,
        ILogger<ExchangeApi> logger)
    {
        _requestAwaitingService = requestAwaitingService;
        _exchangeCoreService = exchangeCoreService;
        _logger = logger;
    }

    public async Task<ApiCommandResult> ExecuteAsync(ApiCommand command, CancellationToken token = default)
    {
        var orderId = 123L;
        var response = await _requestAwaitingService.Register(orderId, token);

        var result = new ApiCommandResult();
        return result;
    }
}
