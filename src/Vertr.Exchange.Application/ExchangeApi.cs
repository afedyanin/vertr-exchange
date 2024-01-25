using System.Runtime.CompilerServices;
using Vertr.Exchange.Application.Core;
using Vertr.Exchange.Domain.Common.Abstractions;

[assembly: InternalsVisibleTo("Vertr.Exchange.Application.Tests")]

namespace Vertr.Exchange.Application;

internal sealed class ExchangeApi(IExchangeCoreService exchangeCoreService) : IExchangeApi
{
    private readonly IExchangeCoreService _exchangeCoreService = exchangeCoreService;

    public Guid Id { get; } = Guid.NewGuid();

    public void Send(IApiCommand command)
    {
        _exchangeCoreService.Send(command);
    }

    public void Dispose()
    {
        _exchangeCoreService?.Dispose();
    }
}
