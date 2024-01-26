using System.Threading.Channels;
using Microsoft.AspNetCore.SignalR;
using Vertr.Exchange.Adapters.SignalR.Extensions;
using Vertr.Exchange.Adapters.SignalR.MessageHandlers;
using Vertr.Exchange.Application.Commands.Api;
using Vertr.Exchange.Application.Generators;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Requests;

namespace Vertr.Exchange.Adapters.SignalR.Hubs;

public class ExchangeApiHub(
    IExchangeCommandsApi api,
    IOrderIdGenerator orderIdGenerator,
    ITimestampGenerator timestampGenerator,
    IObservableMessageHandler messageHandler) : Hub, IExchangeApiHub
{
    private readonly IExchangeCommandsApi _api = api;
    private readonly IOrderIdGenerator _orderIdGenerator = orderIdGenerator;
    private readonly ITimestampGenerator _timestampGenerator = timestampGenerator;
    private readonly IObservableMessageHandler _messageHandler = messageHandler;

    private const int _maxBufferSize = 10;

    public ChannelReader<ApiCommandResult> ApiCommandResults()
        => _messageHandler.ApiCommandResultStream().AsChannelReader(_maxBufferSize);

    public ChannelReader<OrderBook> OrderBooks()
        => _messageHandler.OrderBookStream().AsChannelReader(_maxBufferSize);

    public ChannelReader<ReduceEvent> ReduceEvents()
        => _messageHandler.ReduceEventStream().AsChannelReader(_maxBufferSize);

    public ChannelReader<RejectEvent> RejectEvents()
        => _messageHandler.RejectEventStream().AsChannelReader(_maxBufferSize);

    public ChannelReader<TradeEvent> TradeEvents()
        => _messageHandler.TradeEventStream().AsChannelReader(_maxBufferSize);

    public long Nop()
    {
        var cmd = new Application.Commands.NopCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime);

        _api.Send(cmd);
        return cmd.OrderId;
    }

    public long GetNextOrderId() => _orderIdGenerator.NextId;

    public long GetOrderBook(OrderBookRequest request)
    {
        var cmd = new Application.Commands.OrderBookRequest(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.Symbol,
            request.Size);

        _api.Send(cmd);
        return cmd.OrderId;
    }

    public long AddSymbols(AddSymbolsRequest request)
    {
        var cmd = new Application.Commands.AddSymbolsCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.Symbols.ToDomain());

        _api.Send(cmd);
        return cmd.OrderId;
    }

    public long AddUser(UserRequest request)
    {
        var cmd = new Application.Commands.AddUserCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.UserId);

        _api.Send(cmd);
        return cmd.OrderId;
    }

    public long AddAccounts(AddAccountsRequest request)
    {
        var cmd = new Application.Commands.AddAccountsCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.UserAccounts.ToDomain());

        _api.Send(cmd);
        return cmd.OrderId;
    }

    public long PlaceOrder(PlaceOrderRequest request, long? orderId = null)
    {
        var cmd = new Application.Commands.PlaceOrderCommand(
            orderId ?? _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.Price,
            request.Size,
            (Domain.Common.Enums.OrderAction)request.Action,
            (Domain.Common.Enums.OrderType)request.OrderType,
            request.UserId,
            request.Symbol);

        _api.Send(cmd);
        return cmd.OrderId;
    }

    public long AdjustBalance(AdjustBalanceRequest request)
    {
        var cmd = new Application.Commands.AdjustBalanceCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.UserId,
            request.Currency,
            request.Amount);

        _api.Send(cmd);
        return cmd.OrderId;
    }

    public long CancelOrder(CancelOrderRequest request)
    {
        var cmd = new Application.Commands.CancelOrderCommand(
            request.OrderId,
            _timestampGenerator.CurrentTime,
            request.UserId,
            request.Symbol);

        _api.Send(cmd);
        return cmd.OrderId;
    }

    public long MoveOrder(MoveOrderRequest request)
    {
        var cmd = new Application.Commands.MoveOrderCommand(
            request.OrderId,
            _timestampGenerator.CurrentTime,
            request.UserId,
            request.NewPrice,
            request.Symbol);

        _api.Send(cmd);
        return cmd.OrderId;
    }

    public long ReduceOrder(ReduceOrderRequest request)
    {
        var cmd = new Application.Commands.ReduceOrderCommand(
            request.OrderId,
            _timestampGenerator.CurrentTime,
            request.UserId,
            request.Symbol,
            request.ReduceSize);

        _api.Send(cmd);
        return cmd.OrderId;
    }

    public long Reset()
    {
        var cmd = new Application.Commands.ResetCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime);

        _api.Send(cmd);
        return cmd.OrderId;
    }

    public long ResumeUser(UserRequest request)
    {
        var cmd = new Application.Commands.ResumeUserCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.UserId);

        _api.Send(cmd);
        return cmd.OrderId;
    }

    public long SuspendUser(UserRequest request)
    {
        var cmd = new Application.Commands.SuspendUserCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.UserId);

        _api.Send(cmd);
        return cmd.OrderId;
    }

    public long GetSingleUserReport(UserRequest request)
    {
        var cmd = new Application.Commands.Queries.SingleUserReport(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.UserId);

        _api.Send(cmd);
        return cmd.OrderId;
    }
}
