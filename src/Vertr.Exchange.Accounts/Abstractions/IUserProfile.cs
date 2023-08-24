using Vertr.Exchange.Accounts.Enums;
using Vertr.Exchange.Common;
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

    void UpdatePosition(SymbolSpecification spec, OrderAction action, long tradeSize, decimal tradePrice);
}
