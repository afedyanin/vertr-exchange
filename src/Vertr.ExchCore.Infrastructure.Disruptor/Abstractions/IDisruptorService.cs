using Vertr.ExchCore.Domain.ValueObjects;

namespace Vertr.ExchCore.Infrastructure.Disruptor.Abstractions
{
    public interface IDisruptorService<T>
    {
        void Publish(T command);
    }
}
