namespace Vertr.OrderMatching.Api.Disruptor
{
    public interface IDisruptorService : IDisposable
    {
        void PublishPing(string ping);
    }
}
