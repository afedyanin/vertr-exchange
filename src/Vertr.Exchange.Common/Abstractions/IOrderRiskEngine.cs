namespace Vertr.Exchange.Common.Abstractions;
public interface IOrderRiskEngine
{
    bool PreProcessCommand(long seq, OrderCommand cmd);

    bool PostProcessCommand(long seq, OrderCommand cmd);
}
