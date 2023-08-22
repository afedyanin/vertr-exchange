using System.Diagnostics;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Accounts;

internal class SymbolPosition
{
    public long Uid { get; }

    public int Symbol { get; }

    // Валюта прописана в спеке к символу. Ее оттуда нужно брать
    // Ожидается, что все суммы будут приходить в этой валюте?
    // Нужно чекать валюту в OrderCommand?
    public int Currency { get; }

    public PositionDirection Direction { get; private set; }

    public decimal OpenVolume { get; private set; }

    public decimal OpenPriceSum { get; private set; }

    public decimal Profit { get; private set; }

    public decimal PendingSellSize { get; private set; }

    public decimal PendingBuySize { get; private set; }

    public bool IsEmpty()
        => Direction == PositionDirection.EMPTY
            && PendingSellSize == 0L
            && PendingBuySize == 0L;

    // Конструктор работает по спеке к символу
    public SymbolPosition(long uid, int symbol, int currency)
    {
        Uid = uid;
        Symbol = symbol;
        Direction = PositionDirection.EMPTY;
        Currency = currency;
    }

    public void PendingHold(OrderAction orderAction, long size)
    {
        if (orderAction == OrderAction.ASK)
        {
            PendingSellSize += size;
        }
        else
        {
            PendingBuySize += size;
        }
    }

    public void PendingRelease(OrderAction orderAction, long size)
    {
        if (orderAction == OrderAction.ASK)
        {
            PendingSellSize -= size;
            Debug.Assert(PendingSellSize >= decimal.Zero);
        }
        else
        {
            PendingBuySize -= size;
            Debug.Assert(PendingBuySize >= decimal.Zero);
        }
    }

}
