using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Common;
using Vertr.Exchange.Infrastructure.Extensions;

namespace Vertr.Exchange.Api.Factories;
internal class OrderCommandFactory : IOrderCommandFactory
{
    public OrderCommand Create(IApiCommand cmd)
    {
        return cmd switch
        {
            AddUserCommand addUserCommand => Create(addUserCommand),
            AdjustUserBalanceCommand adjustUserBalanceCommand => Create(adjustUserBalanceCommand),
            CancelOrderCommand cancelOrderCommand => Create(cancelOrderCommand),
            MoveOrderCommand moveOrderCommand => Create(moveOrderCommand),
            NopCommand nopCommand => Create(nopCommand),
            OrderBookRequest orderBookRequest => Create(orderBookRequest),
            PlaceOrderCommand placeOrderCommand => Create(placeOrderCommand),
            ReduceOrderCommand reduceOrderCommand => Create(reduceOrderCommand),
            ResetCommand resetCommand => Create(resetCommand),
            ResumeUserCommand resumeUserCommand => Create(resumeUserCommand),
            SuspendUserCommand suspendUserCommand => Create(suspendUserCommand),
            _ => throw new InvalidOperationException($"Unknown API command: {cmd.GetType()}"),
        };
    }

    private OrderCommand Create(AddUserCommand cmd)
    {
        return OrderCommandExtensions.EmptyCommand;
    }

    private OrderCommand Create(AdjustUserBalanceCommand cmd)
    {
        return OrderCommandExtensions.EmptyCommand;
    }

    private OrderCommand Create(CancelOrderCommand cmd)
    {
        return OrderCommandExtensions.EmptyCommand;
    }

    private OrderCommand Create(MoveOrderCommand cmd)
    {
        return OrderCommandExtensions.EmptyCommand;
    }

    private OrderCommand Create(NopCommand cmd)
    {
        return OrderCommandExtensions.EmptyCommand;
    }

    private OrderCommand Create(OrderBookRequest cmd)
    {
        return OrderCommandExtensions.EmptyCommand;
    }

    private OrderCommand Create(PlaceOrderCommand cmd)
    {
        return OrderCommandExtensions.EmptyCommand;
    }

    private OrderCommand Create(ReduceOrderCommand cmd)
    {
        return OrderCommandExtensions.EmptyCommand;
    }
    private OrderCommand Create(ResetCommand cmd)
    {
        return OrderCommandExtensions.EmptyCommand;
    }
    private OrderCommand Create(ResumeUserCommand cmd)
    {
        return OrderCommandExtensions.EmptyCommand;
    }

    private OrderCommand Create(SuspendUserCommand cmd)
    {
        return OrderCommandExtensions.EmptyCommand;
    }
}
