namespace Vertr.Exchange.RiskEngine.Abstractions;
internal interface IFeeCalculationService
{
    decimal AddFeeValue(int currency, decimal toBeAdded);

    void Reset();
}
