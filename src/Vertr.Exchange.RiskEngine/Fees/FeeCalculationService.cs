using Vertr.Exchange.RiskEngine.Abstractions;
using Vertr.Exchange.RiskEngine.Extensions;

namespace Vertr.Exchange.RiskEngine.Fees;
internal class FeeCalculationService : IFeeCalculationService
{
    // currency
    private readonly IDictionary<int, decimal> _fees;

    public FeeCalculationService()
    {
        _fees = new Dictionary<int, decimal>();
    }

    public decimal AddFeeValue(int currency, decimal toBeAdded)
        => _fees.AddToValue(currency, toBeAdded);

    public void Reset()
    {
        _fees.Clear();
    }
}
