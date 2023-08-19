using Vertr.Exchange.RiskEngine.Adjustments;

namespace Vertr.Exchange.RiskEngine.Abstractions;
internal interface IAdjustmentsService
{
    void AddAdjustment(int currency, decimal amount, BalanceAdjustmentType adjustmentType);

    void Reset();
}
