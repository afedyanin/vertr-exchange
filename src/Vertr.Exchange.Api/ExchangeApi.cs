using System.Runtime.CompilerServices;
using Vertr.Exchange.Api.Awaiting;
using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Api.Generators;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Infrastructure;

[assembly: InternalsVisibleTo("Vertr.Exchange.Api.Tests")]

namespace Vertr.Exchange.Api;

internal sealed class ExchangeApi : IExchangeApi
{
    private readonly IRequestAwaitingService _requestAwaitingService;
    private readonly IExchangeCoreService _exchangeCoreService;
    private readonly ITimestampGenerator _timestampGenerator;

    public ExchangeApi(
        IRequestAwaitingService requestAwaitingService,
        IExchangeCoreService exchangeCoreService,
        ITimestampGenerator timestampGenerator)
    {
        _requestAwaitingService = requestAwaitingService;
        _exchangeCoreService = exchangeCoreService;
        _timestampGenerator = timestampGenerator;
    }

    public void Send(IApiCommand command)
    {
        _exchangeCoreService.Send(command);
    }

    public async Task<IApiCommandResult> SendAsync(IApiCommand command, CancellationToken token = default)
    {
        var awaitngTask = _requestAwaitingService.Register(command.OrderId, token);

        _exchangeCoreService.Send(command);

        var awaitingResponse = await awaitngTask;
        var apiResult = ApiCommandResult.Create(awaitingResponse.OrderCommand, _timestampGenerator.CurrentTime);

        return apiResult;
    }
    public void Dispose()
    {
        _exchangeCoreService?.Dispose();
    }
}
