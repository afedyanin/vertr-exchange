using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Domain.Common.Abstractions;

public interface IUserProfile
{
    long Uid { get; }

    UserStatus Status { get; }

    IDictionary<int, decimal> Accounts { get; }

    IDictionary<int, IPosition> Positions { get; }

    decimal AddToValue(int currency, decimal toBeAdded);

    CommandResultCode Suspend();

    CommandResultCode Resume();

    IPosition? GetPosition(int symbol);

    void UpdatePosition(SymbolSpecification spec, OrderAction action, long tradeSize, decimal tradePrice);
}
