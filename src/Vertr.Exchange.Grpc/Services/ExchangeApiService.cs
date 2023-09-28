using Grpc.Core;
using Vertr.Exchange.Api;
using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Grpc.Extensions;
using Vertr.Exchange.Grpc.Generators;

namespace Vertr.Exchange.Grpc.Services;

public class ExchangeApiService : Exchange.ExchangeBase
{
    private readonly IExchangeApi _api;
    private readonly IOrderIdGenerator _orderIdGenerator;
    private readonly ITimestampGenerator _timestampGenerator;

    public ExchangeApiService(
        IExchangeApi api,
        IOrderIdGenerator orderIdGenerator,
        ITimestampGenerator timestampGenerator)
    {
        _api = api;
        _orderIdGenerator = orderIdGenerator;
        _timestampGenerator = timestampGenerator;
    }

    public override async Task<CommandResult> Nop(CommandNoParams request, ServerCallContext context)
    {
        var cmd = new NopCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime);

        var apiResult = await _api.SendAsync(cmd);
        return apiResult.ToGrpc();
    }

    public override async Task<CommandResult> GetOrderBook(OrderBookRequest request, ServerCallContext context)
    {
        var cmd = new Api.Commands.OrderBookRequest(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.Symbol,
            request.Size);

        var apiResult = await _api.SendAsync(cmd);
        return apiResult.ToGrpc();
    }

    public override async Task<CommandResult> AddSymbols(AddSymbolsRequest request, ServerCallContext context)
    {
        var cmd = new AddSymbolsCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.Symbols.ToDomain());

        var apiResult = await _api.SendAsync(cmd);
        return apiResult.ToGrpc();
    }

    public override Task<CommandResult> AddUser(UserRequest request, ServerCallContext context)
    {
        return base.AddUser(request, context);
    }

    public override Task<CommandResult> AddAccounts(AddAccountsRequest request, ServerCallContext context)
    {
        return base.AddAccounts(request, context);
    }

    public override Task<CommandResult> AdjustBalance(AdjustBalanceRequest request, ServerCallContext context)
    {
        return base.AdjustBalance(request, context);
    }

    public override Task<CommandResult> CancelOrder(CancelOrderRequest request, ServerCallContext context)
    {
        return base.CancelOrder(request, context);
    }

    public override Task<CommandResult> MoveOrder(MoveOrderRequest request, ServerCallContext context)
    {
        return base.MoveOrder(request, context);
    }

    public override Task<CommandResult> PlaceOrder(PlaceOrderRequest request, ServerCallContext context)
    {
        return base.PlaceOrder(request, context);
    }

    public override Task<CommandResult> ReduceOrder(ReduceOrderRequest request, ServerCallContext context)
    {
        return base.ReduceOrder(request, context);
    }

    public override Task<CommandResult> Reset(CommandNoParams request, ServerCallContext context)
    {
        return base.Reset(request, context);
    }

    public override Task<CommandResult> ResumeUser(UserRequest request, ServerCallContext context)
    {
        return base.ResumeUser(request, context);
    }

    public override Task<CommandResult> SuspendUser(UserRequest request, ServerCallContext context)
    {
        return base.SuspendUser(request, context);
    }
}
