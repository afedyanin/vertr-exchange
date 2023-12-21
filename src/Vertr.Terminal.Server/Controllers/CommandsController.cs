using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vertr.Exchange.Contracts.Requests;
using Vertr.Terminal.Application.Commands.Orders;
using Vertr.Terminal.Domain.Abstractions;

namespace Vertr.Terminal.Server.Controllers;

[Route("commands")]
[ApiController]
public class CommandsController(
    IExchangeApiClient exchangeApiClient,
    IMediator mediator)
    : ControllerBase
{
    private readonly IExchangeApiClient _exchangeApiClient = exchangeApiClient;
    private readonly IMediator _mediator = mediator;

    [HttpPost("nop")]
    public async Task<IActionResult> Nop()
    {
        var res = await _exchangeApiClient.Nop();
        return Ok(res);
    }

    [HttpPost("place-order")]
    public async Task<IActionResult> PlaceOrder(PlaceOrderRequest request)
    {
        var req = new PlaceRequest
        {
            PlaceOrderRequest = request,
        };

        var res = await _mediator.Send(req);

        return Ok(res);
    }

    [HttpPost("cancel-order")]
    public async Task<IActionResult> CancelOrder(CancelOrderRequest request)
    {
        var req = new CancelRequest
        {
            CancelOrderRequest = request,
        };

        var res = await _mediator.Send(req);

        return Ok(res);
    }

    [HttpPost("move-order")]
    public async Task<IActionResult> MoveOrder(MoveOrderRequest request)
    {
        var req = new MoveRequest
        {
            MoveOrderRequest = request,
        };

        var res = await _mediator.Send(req);

        return Ok(res);
    }

    [HttpPost("reduce-order")]
    public async Task<IActionResult> ReduceOrder(ReduceOrderRequest request)
    {
        var req = new ReduceRequest
        {
            ReduceOrderRequest = request,
        };

        var res = await _mediator.Send(req);

        return Ok(res);
    }
}
