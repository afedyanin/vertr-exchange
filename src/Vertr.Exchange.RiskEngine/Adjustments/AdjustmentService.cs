using Vertr.Exchange.RiskEngine.Abstractions;
using Vertr.Exchange.RiskEngine.Extensions;

namespace Vertr.Exchange.RiskEngine.Adjustments;
internal class AdjustmentService : IAdjustmentsService
{
    // currency
    private readonly IDictionary<int, decimal> _adjustments;

    // currency
    private readonly IDictionary<int, decimal> _suspends;

    public AdjustmentService()
    {
        _adjustments = new Dictionary<int, decimal>();
        _suspends = new Dictionary<int, decimal>();
    }

    public void AddAdjustment(int currency, decimal amount, BalanceAdjustmentType adjustmentType)
    {
        switch (adjustmentType)
        {
            case BalanceAdjustmentType.ADJUSTMENT:
                _adjustments.AddToValue(currency, -amount);
                break;

            case BalanceAdjustmentType.SUSPEND:
                _suspends.AddToValue(currency, -amount);
                break;
            default:
                break;
        }
    }

    public void Reset()
    {
        _adjustments.Clear();
        _suspends.Clear();
    }
}
