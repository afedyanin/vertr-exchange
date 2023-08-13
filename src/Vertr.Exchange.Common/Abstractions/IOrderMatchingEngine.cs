namespace Vertr.Exchange.Common.Abstractions;

public interface IOrderMatchingEngine
{
    void ProcessOrder(long seq, OrderCommand cmd);
}
