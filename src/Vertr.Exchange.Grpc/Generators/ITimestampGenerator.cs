namespace Vertr.Exchange.Grpc.Generators;

public interface ITimestampGenerator
{
    DateTime CurrentTime { get; }
}
