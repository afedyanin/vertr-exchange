using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertr.ExchCore.Domain.Enums;
using Vertr.ExchCore.Domain.ValueObjects;

namespace Vertr.ExchCore.Domain.Abstractions;

public interface IOrderManagementApi
{
    public void OrderBookRequest(
        int symbolId,
        int depth);

    long PlaceNewOrder(int userCookie,
        long price,
        long reservedBidPrice,
        long size,
        OrderAction action,
        OrderType orderType,
        int symbol,
        long uid);

    void PlaceNewOrder(int serviceFlags,
        long eventsGroup,
        long timestampNs,
        long orderId,
        int userCookie,
        long price,
        long reservedBidPrice,
        long size,
        OrderAction action,
        OrderType orderType,
        int symbol,
        long uid);

    void MoveOrder(
        long price,
        long orderId,
        int symbol,
        long uid);

    void MoveOrder(int serviceFlags,
        long eventsGroup,
        long timestampNs,
        long price,
        long orderId,
        int symbol,
        long uid);

    void CancelOrder(
        long orderId,
        int symbol,
        long uid);

    void CancelOrder(int serviceFlags,
        long eventsGroup,
        long timestampNs,
        long orderId,
        int symbol,
        long uid);

    void ReduceOrder(
        long reduceSize,
        long orderId,
        int symbol,
        long uid);

    void ReduceOrder(int serviceFlags,
        long eventsGroup,
        long timestampNs,
        long reduceSize,
        long orderId,
        int symbol,
        long uid);
}
