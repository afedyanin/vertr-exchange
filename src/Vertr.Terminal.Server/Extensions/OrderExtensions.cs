using Vertr.Terminal.ApiClient.Contracts;
using Vertr.Terminal.Domain.OrderManagement;

namespace Vertr.Terminal.Server.Extensions;

internal static class OrderExtensions
{
    public static OrderDto[] ToDto(this Order[] orders)
        => orders.Select(ToDto).ToArray();

    public static OrderDto ToDto(this Order order)
        => new OrderDto
        {
            Action = order.Action,
            OrderEvents = order.OrderEvents.ToDto(),
            OrderId = order.OrderId,
            OrderType = order.OrderType,
            Price = order.Price,
            Size = order.Size,
            Symbol = order.Symbol,
            UserId = order.UserId,
        };
}
