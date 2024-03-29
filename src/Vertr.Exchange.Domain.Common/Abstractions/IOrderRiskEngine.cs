namespace Vertr.Exchange.Domain.Common.Abstractions;
public interface IOrderRiskEngine
{
    void PreProcessCommand(long seq, OrderCommand cmd);

    void PostProcessCommand(long seq, OrderCommand cmd);
}
