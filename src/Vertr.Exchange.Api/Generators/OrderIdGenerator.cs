namespace Vertr.Exchange.Application.Generators;

internal sealed class OrderIdGenerator : IOrderIdGenerator
{
    private long _currentOrderId;

    public long NextId => Interlocked.Increment(ref _currentOrderId);
}
