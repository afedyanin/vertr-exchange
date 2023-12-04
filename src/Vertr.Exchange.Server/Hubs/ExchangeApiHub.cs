using System.Threading.Channels;
using Microsoft.AspNetCore.SignalR;
using Vertr.Exchange.Api;
using Vertr.Exchange.Api.Generators;
using Vertr.Exchange.Protos;
using Vertr.Exchange.Server.Extensions;
using Vertr.Exchange.Server.MessageHandlers;

namespace Vertr.Exchange.Server.Hubs;

public class ExchangeApiHub : Hub
{
    private readonly IExchangeApi _api;
    private readonly IOrderIdGenerator _orderIdGenerator;
    private readonly ITimestampGenerator _timestampGenerator;
    private readonly ILogger<ExchangeApiHub> _logger;
    private readonly IObservableMessageHandler _messageHandler;

    private const int _maxBufferSize = 10;

    public ExchangeApiHub(
        IExchangeApi api,
        IOrderIdGenerator orderIdGenerator,
        ITimestampGenerator timestampGenerator,
        ILogger<ExchangeApiHub> logger,
        IObservableMessageHandler messageHandler)
    {
        _api = api;
        _orderIdGenerator = orderIdGenerator;
        _timestampGenerator = timestampGenerator;
        _logger = logger;
        _messageHandler = messageHandler;
    }

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

    public void Nop()
    {
        var cmd = new Api.Commands.NopCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime);

        _api.Send(cmd);
    }

    public void GetOrderBook(OrderBookRequest request)
    {
        var cmd = new Api.Commands.OrderBookRequest(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.Symbol,
            request.Size);

        _api.Send(cmd);
    }

    public void AddSymbols(AddSymbolsRequest request)
    {
        _logger.LogDebug("AddSymbols command received.");

        var cmd = new Api.Commands.AddSymbolsCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.Symbols.ToDomain());

        _logger.LogDebug("AddSymbols command completed.");
        _api.Send(cmd);
    }

    public void AddUser(UserRequest request)
    {
        var cmd = new Api.Commands.AddUserCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.UserId);

        _api.Send(cmd);
    }

    public void AddAccounts(AddAccountsRequest request)
    {
        var cmd = new Api.Commands.AddAccountsCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.UserAccounts.ToDomain());

        _api.Send(cmd);
    }

    public void PlaceOrder(PlaceOrderRequest request)
    {
        var cmd = new Api.Commands.PlaceOrderCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.Price,
            request.Size,
            request.Action.ToDomain(),
            request.OrderType.ToDomain(),
            request.UserId,
            request.Symbol);

        _api.Send(cmd);
    }

    public void AdjustBalance(AdjustBalanceRequest request)
    {
        var cmd = new Api.Commands.AdjustBalanceCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.UserId,
            request.Currency,
            request.Amount);

        _api.Send(cmd);
    }

    public void CancelOrder(CancelOrderRequest request)
    {
        var cmd = new Api.Commands.CancelOrderCommand(
            request.OrderId,
            _timestampGenerator.CurrentTime,
            request.UserId,
            request.Symbol);

        _api.Send(cmd);
    }

    public void MoveOrder(MoveOrderRequest request)
    {
        var cmd = new Api.Commands.MoveOrderCommand(
            request.OrderId,
            _timestampGenerator.CurrentTime,
            request.UserId,
            request.NewPrice,
            request.Symbol);

        _api.Send(cmd);
    }

    public void ReduceOrder(ReduceOrderRequest request)
    {
        var cmd = new Api.Commands.ReduceOrderCommand(
            request.OrderId,
            _timestampGenerator.CurrentTime,
            request.UserId,
            request.Symbol,
            request.ReduceSize);

        _api.Send(cmd);
    }

    public void Reset()
    {
        var cmd = new Api.Commands.ResetCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime);

        _api.Send(cmd);
    }

    public void ResumeUser(UserRequest request)
    {
        var cmd = new Api.Commands.ResumeUserCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.UserId);

        _api.Send(cmd);
    }

    public void SuspendUser(UserRequest request)
    {
        var cmd = new Api.Commands.SuspendUserCommand(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.UserId);

        _api.Send(cmd);
    }

    public void GetSingleUserReport(UserRequest request)
    {
        var cmd = new Api.Commands.Queries.SingleUserReport(
            _orderIdGenerator.NextId,
            _timestampGenerator.CurrentTime,
            request.UserId);

        _api.Send(cmd);
    }
}
