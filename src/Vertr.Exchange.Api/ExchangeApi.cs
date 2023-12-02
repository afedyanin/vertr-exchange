using System.Runtime.CompilerServices;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Core;

[assembly: InternalsVisibleTo("Vertr.Exchange.Api.Tests")]

namespace Vertr.Exchange.Api;

internal sealed class ExchangeApi : IExchangeApi
{
    private readonly IExchangeCoreService _exchangeCoreService;

    public ExchangeApi(IExchangeCoreService exchangeCoreService)
    {
        _exchangeCoreService = exchangeCoreService;
    }

    public void Send(IApiCommand command)
    {
        _exchangeCoreService.Send(command);
    }

    public void Dispose()
    {
        _exchangeCoreService?.Dispose();
    }
}
