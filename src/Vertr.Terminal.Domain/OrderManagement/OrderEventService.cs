using Vertr.Exchange.Contracts;
using Vertr.Terminal.Domain.Abstractions;

namespace Vertr.Terminal.Domain.OrderManagement;
internal sealed class OrderEventService(IOrderRepository orderRepository) : IOrderEventService
{
    private readonly IOrderRepository _orderRepository = orderRepository;

    public Task ProcessReduceEvent(ReduceEvent reduceEvent)
    {
        var evt = OrderEventFactory.Create(reduceEvent);
        return _orderRepository.AddEvent(evt);
    }

    public Task ProcessRejectEvent(RejectEvent rejectEvent)
    {
        var evt = OrderEventFactory.Create(rejectEvent);
        return _orderRepository.AddEvent(evt);
    }

    public async Task ProcessTradeEvent(TradeEvent tradeEvent)
    {
        await HandleTakerTrade(tradeEvent);

        foreach (var makerTrade in tradeEvent.Trades)
        {
            await HandleMakerTrade(tradeEvent, makerTrade);
        }
    }

    private async Task HandleTakerTrade(TradeEvent tradeEvent)
    {
        var takerOrder = await _orderRepository.GetById(tradeEvent.TakerOrderId);
        var evt = OrderEventFactory.Create(tradeEvent, takerOrder);
        await _orderRepository.AddEvent(evt);
    }

    private async Task HandleMakerTrade(TradeEvent tradeEvent, Trade makerTrade)
    {
        var evt = OrderEventFactory.Create(tradeEvent, makerTrade);
        await _orderRepository.AddEvent(evt);
    }
}
