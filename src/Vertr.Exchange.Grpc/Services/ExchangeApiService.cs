using Grpc.Core;
using Vertr.Exchange.Api;
using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Api.Commands.Queries;
using Vertr.Exchange.Api.Generators;
using Vertr.Exchange.Grpc.Extensions;

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

    public override async Task<CommandResult> AddUser(UserRequest request, ServerCallContext context)
    {
        var cmd = new AddUserCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.UserId);

        var apiResult = await _api.SendAsync(cmd);
        return apiResult.ToGrpc();
    }

    public override async Task<CommandResult> AddAccounts(AddAccountsRequest request, ServerCallContext context)
    {
        var cmd = new AddAccountsCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.UserAccounts.ToDomain());

        var apiResult = await _api.SendAsync(cmd);
        return apiResult.ToGrpc();
    }
    public override async Task<CommandResult> PlaceOrder(PlaceOrderRequest request, ServerCallContext context)
    {
        var cmd = new PlaceOrderCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.Price,
            request.Size,
            request.Action.ToDomain(),
            request.OrderType.ToDomain(),
            request.UserId,
            request.Symbol);

        var apiResult = await _api.SendAsync(cmd);
        return apiResult.ToGrpc();
    }

    public override async Task<CommandResult> AdjustBalance(AdjustBalanceRequest request, ServerCallContext context)
    {
        var cmd = new AdjustBalanceCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.UserId,
            request.Currency,
            request.Amount);

        var apiResult = await _api.SendAsync(cmd);
        return apiResult.ToGrpc();
    }

    public override async Task<CommandResult> CancelOrder(CancelOrderRequest request, ServerCallContext context)
    {
        var cmd = new CancelOrderCommand(
            request.OrderId,
            _timestampGenerator.CurrentTime,
            request.UserId,
            request.Symbol);

        var apiResult = await _api.SendAsync(cmd);
        return apiResult.ToGrpc();
    }

    public override async Task<CommandResult> MoveOrder(MoveOrderRequest request, ServerCallContext context)
    {
        var cmd = new MoveOrderCommand(
            request.OrderId,
            _timestampGenerator.CurrentTime,
            request.UserId,
            request.NewPrice,
            request.Symbol);

        var apiResult = await _api.SendAsync(cmd);
        return apiResult.ToGrpc();
    }

    public override async Task<CommandResult> ReduceOrder(ReduceOrderRequest request, ServerCallContext context)
    {
        var cmd = new ReduceOrderCommand(
            request.OrderId,
            _timestampGenerator.CurrentTime,
            request.UserId,
            request.Symbol,
            request.ReduceSize);

        var apiResult = await _api.SendAsync(cmd);
        return apiResult.ToGrpc();
    }

    public override async Task<CommandResult> Reset(CommandNoParams request, ServerCallContext context)
    {
        var cmd = new ResetCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime);

        var apiResult = await _api.SendAsync(cmd);
        return apiResult.ToGrpc();
    }

    public override async Task<CommandResult> ResumeUser(UserRequest request, ServerCallContext context)
    {
        var cmd = new ResumeUserCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.UserId);

        var apiResult = await _api.SendAsync(cmd);
        return apiResult.ToGrpc();
    }

    public override async Task<CommandResult> SuspendUser(UserRequest request, ServerCallContext context)
    {
        var cmd = new SuspendUserCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.UserId);

        var apiResult = await _api.SendAsync(cmd);
        return apiResult.ToGrpc();
    }

    public override async Task<CommandResult> GetSingleUserReport(UserRequest request, ServerCallContext context)
    {
        var cmd = new SingleUserReport(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.UserId);

        var apiResult = await _api.SendAsync(cmd);
        return apiResult.ToGrpc();
    }
}
