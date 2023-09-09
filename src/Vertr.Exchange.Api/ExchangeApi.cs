using System.Runtime.CompilerServices;
using Vertr.Exchange.Api.Awaiting;
using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Api.Factories;
using Vertr.Exchange.Infrastructure;

[assembly: InternalsVisibleTo("Vertr.Exchange.Api.Tests")]

namespace Vertr.Exchange.Api;

internal sealed class ExchangeApi
{
    private readonly IRequestAwaitingService _requestAwaitingService;
    private readonly IExchangeCoreService _exchangeCoreService;

    public ExchangeApi(
        IRequestAwaitingService requestAwaitingService,
        IExchangeCoreService exchangeCoreService)
    {
        _requestAwaitingService = requestAwaitingService;
        _exchangeCoreService = exchangeCoreService;
    }

    public long Execute(ApiCommand command)
    {
        var orderId = 123L;
        ExecuteInternal(command, orderId);
        return orderId;
    }

    public async Task<ApiCommandResult> ExecuteAsync(ApiCommand command, CancellationToken token = default)
    {
        var orderId = 123L;
        var awaitngTask = _requestAwaitingService.Register(orderId, token);

        ExecuteInternal(command, orderId);

        var awaitingResponse = await awaitngTask;
        var apiResult = ApiCommandResultFactory.CreateResult(awaitingResponse.OrderCommand);
        return apiResult;
    }

    private void ExecuteInternal(ApiCommand command, long orderId)
    {
        var orderCommand = OrderCommandFactory.Create(command);
        orderCommand.OrderId = orderId;
        _exchangeCoreService.Send(orderCommand);
    }
}
