namespace Vertr.Exchange.Api.Factories;
internal class OrderIdFactorty : IOrderIdFactory
{
    private long _id;
    public long NextId => Interlocked.Increment(ref _id);
}
