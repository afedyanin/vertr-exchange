using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vertr.Exchange.Contracts.Requests;
using Vertr.Terminal.Application.Commands;
using Vertr.Terminal.Domain.Abstractions;

namespace Vertr.Terminal.Server.Controllers;

[Route("admin")]
[ApiController]
public class AdminController(
    IExchangeApiClient exchangeApiClient,
    IMediator mediator) : ControllerBase
{
    private readonly IExchangeApiClient _exchangeApiClient = exchangeApiClient;
    private readonly IMediator _mediator = mediator;

    [HttpPost("add-symbols")]
    public async Task<IActionResult> AddSymbols(AddSymbolsRequest request)
    {
        var res = await _exchangeApiClient.AddSymbols(request);
        return Ok(res);
    }

    [HttpPost("add-accounts")]
    public async Task<IActionResult> AddAccounts(AddAccountsRequest request)
    {
        var res = await _exchangeApiClient.AddAccounts(request);
        return Ok(res);
    }

    [HttpPost("reset")]
    public async Task<IActionResult> Reset()
    {
        var res = await _mediator.Send(new ResetRequest());
        return Ok(res);
    }
}

