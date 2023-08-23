using Vertr.Exchange.Accounts.Enums;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Accounts.Abstractions;

public interface IUserProfile
{
    long Uid { get; }

    UserStatus Status { get; }

    decimal AddToValue(int currency, decimal toBeAdded);

    CommandResultCode Suspend();

    CommandResultCode Resume();

    IPosition? GetPosition(int symbol);

    void UpdatePosition(int symbol, OrderAction action, long tradeSize, decimal tradePrice, int currency);
}
