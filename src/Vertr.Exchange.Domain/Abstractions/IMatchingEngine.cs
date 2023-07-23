namespace Vertr.Exchange.Domain.Abstractions;

internal interface IMatchingEngine
{
    void ProcessOrder(long seq, OrderCommand cmd);
}
