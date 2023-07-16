using System.Data;
using Vertr.ExchCore.Domain.Abstractions;
using Vertr.ExchCore.Domain.Enums;
using Vertr.ExchCore.Domain.ValueObjects;

namespace Vertr.ExchCore.Domain.Entities;

internal class OrderBook : IOrderBook
{
    public CommandResultCode ProcessCommand(OrderCommand orderCommand)
    {
        OrderCommandType commandType = orderCommand.CommandType;

        if (commandType == OrderCommandType.MOVE_ORDER)
        {

            return MoveOrder(orderCommand);

        }
        else if (commandType == OrderCommandType.CANCEL_ORDER)
        {

            return CancelOrder(orderCommand);

        }
        else if (commandType == OrderCommandType.REDUCE_ORDER)
        {

            return ReduceOrder(orderCommand);

        }
        else if (commandType == OrderCommandType.PLACE_ORDER)
        {

            if (orderCommand.ResultCode == CommandResultCode.VALID_FOR_MATCHING_ENGINE)
            {
                NewOrder(orderCommand);
                return CommandResultCode.SUCCESS;
            }
            else
            {
                return orderCommand.ResultCode; // no change
            }

        }
        else if (commandType == OrderCommandType.ORDER_BOOK_REQUEST)
        {
            int size = (int)orderCommand.Size;
            orderCommand.L2MarketData = GetL2MarketDataSnapshot(size >= 0 ? size : int.MaxValue);
            return CommandResultCode.SUCCESS;

        }
        else
        {
            return CommandResultCode.MATCHING_UNSUPPORTED_COMMAND;
        }
    }

    public L2MarketData GetL2MarketDataSnapshot(int size)
    {
        throw new NotImplementedException();
    }

    private void NewOrder(OrderCommand orderCommand)
    {
        throw new NotImplementedException();
    }

    private CommandResultCode CancelOrder(OrderCommand orderCommand)
    {
        throw new NotImplementedException();
    }

    private CommandResultCode ReduceOrder(OrderCommand orderCommand)
    {
        throw new NotImplementedException();
    }

    private CommandResultCode MoveOrder(OrderCommand orderCommand)
    {
        throw new NotImplementedException();
    }
}
