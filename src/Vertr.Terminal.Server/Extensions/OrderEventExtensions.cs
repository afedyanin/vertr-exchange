using Vertr.Terminal.ApiClient.Contracts;
using Vertr.Terminal.Domain.OrderManagement;

namespace Vertr.Terminal.Server.Extensions;

internal static class OrderEventExtensions
{
    public static OrderEventDto[] ToDto(this OrderEvent[] orderEvents)
        => orderEvents.Select(ToDto).ToArray();

    public static OrderEventDto ToDto(this OrderEvent orderEvent)
        => new OrderEventDto
        {
            Action = orderEvent.Action,
            CommandResultCode = orderEvent.CommandResultCode,
            EventSource = orderEvent.EventSource,
            OrderCompleted = orderEvent.OrderCompleted,
            OrderId = orderEvent.OrderId,
            Price = orderEvent.Price,
            Seq = orderEvent.Seq,
            TimeStamp = orderEvent.TimeStamp,
            Volume = orderEvent.Volume,
        };
}
