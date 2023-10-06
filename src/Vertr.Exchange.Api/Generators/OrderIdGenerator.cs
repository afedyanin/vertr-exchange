namespace Vertr.Exchange.Api.Generators;

internal sealed class OrderIdGenerator : IOrderIdGenerator
{
    private long _currentOrderId;

    public long NextId => Interlocked.Increment(ref _currentOrderId);
}