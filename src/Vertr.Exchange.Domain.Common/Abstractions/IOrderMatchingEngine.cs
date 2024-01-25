namespace Vertr.Exchange.Domain.Common.Abstractions;

public interface IOrderMatchingEngine
{
    void ProcessOrder(long seq, OrderCommand cmd);
}
