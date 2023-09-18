using System.Runtime.CompilerServices;
using Vertr.Exchange.Api.Awaiting;
using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Infrastructure;

[assembly: InternalsVisibleTo("Vertr.Exchange.Api.Tests")]

namespace Vertr.Exchange.Api;

internal sealed class ExchangeApi : IExchangeApi
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

    public long Execute(IApiCommand command)
    {
        _exchangeCoreService.Send(command);
        return command.OrderId;
    }

    public async Task<IApiCommandResult> ExecuteAsync(IApiCommand command, CancellationToken token = default)
    {
        var orderId = command.OrderId;
        var awaitngTask = _requestAwaitingService.Register(orderId, token);

        _exchangeCoreService.Send(command);

        var awaitingResponse = await awaitngTask;
        var apiResult = ApiCommandResult.Create(awaitingResponse.OrderCommand);
        return apiResult;
    }
    public void Dispose()
    {
        _exchangeCoreService?.Dispose();
    }
}
