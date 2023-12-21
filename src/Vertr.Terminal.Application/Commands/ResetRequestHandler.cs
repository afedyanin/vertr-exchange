using MediatR;
using Microsoft.Extensions.Logging;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.Shared.Enums;
using Vertr.Terminal.Domain.Abstractions;

namespace Vertr.Terminal.Application.Commands;

internal sealed class ResetRequestHandler(
    IExchangeApiClient exchangeApiClient,
    IOrderBookSnapshotsRepository orderBookRepository,
    ITradeEventsRepository tradeEventsRepository,
    IOrderRepository orderRepository,
    ILogger<ResetRequestHandler> logger) : IRequestHandler<ResetRequest, ApiCommandResult>
{
    private readonly IOrderBookSnapshotsRepository _orderBookRepository = orderBookRepository;
    private readonly ITradeEventsRepository _tradeEventsRepository = tradeEventsRepository;
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IExchangeApiClient _exchangeApiClient = exchangeApiClient;
    private readonly ILogger<ResetRequestHandler> _logger = logger;

    public async Task<ApiCommandResult> Handle(ResetRequest request, CancellationToken cancellationToken)
    {
        var res = await _exchangeApiClient.Reset();

        if (res.ResultCode == CommandResultCode.SUCCESS)
        {
            await _orderBookRepository.Reset();
            await _tradeEventsRepository.Reset();
            await _orderRepository.Reset();

            _logger.LogDebug("Reset command completed.");
        }
        else
        {
            _logger.LogError("Reset command failed. ResultCode={ResultCode}", res.ResultCode);
        }

        return res;
    }
}
