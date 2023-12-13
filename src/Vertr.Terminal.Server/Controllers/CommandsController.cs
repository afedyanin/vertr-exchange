using Microsoft.AspNetCore.Mvc;
using Vertr.Exchange.Contracts.Requests;
using Vertr.Terminal.Server.Awaiting;
using Vertr.Terminal.Server.OrderManagement;
using Vertr.Terminal.Server.Providers;

namespace Vertr.Terminal.Server.Controllers;

[Route("commands")]
[ApiController]
public class CommandsController(
    HubConnectionProvider connectionProvider,
    ICommandAwaitingService commandAwaitingService,
    IOrderEventHandler orderEventHandler)
    : ExchangeControllerBase(
        connectionProvider,
        commandAwaitingService)
{
    private readonly IOrderEventHandler _orderEventHandler = orderEventHandler;

    [HttpPost("nop")]
    public async Task<IActionResult> Nop()
    {
        var res = await InvokeHubMethod("Nop");
        return Ok(res);
    }

    [HttpPost("place-order")]
    public async Task<IActionResult> PlaceOrder(PlaceOrderRequest request)
    {
        var res = await InvokeHubMethod("PlaceOrder", request);
        await _orderEventHandler.HandlePlaceOrderRequest(request, res);

        return Ok(res);
    }

    [HttpPost("cancel-order")]
    public async Task<IActionResult> CancelOrder(CancelOrderRequest request)
    {
        var res = await InvokeHubMethod("CancelOrder", request);
        await _orderEventHandler.HandleCancelOrderRequest(request, res);

        return Ok(res);
    }

    [HttpPost("move-order")]
    public async Task<IActionResult> MoveOrder(MoveOrderRequest request)
    {
        var res = await InvokeHubMethod("MoveOrder", request);
        await _orderEventHandler.HandleMoveOrderRequest(request, res);

        return Ok(res);
    }

    [HttpPost("reduce-order")]
    public async Task<IActionResult> ReduceOrder(ReduceOrderRequest request)
    {
        var res = await InvokeHubMethod("ReduceOrder", request);
        await _orderEventHandler.HandleReduceOrderRequest(request, res);

        return Ok(res);
    }
}
