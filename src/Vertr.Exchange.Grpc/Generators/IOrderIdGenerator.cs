namespace Vertr.Exchange.Grpc.Generators;

public interface IOrderIdGenerator
{
    long NextId { get; }
}
