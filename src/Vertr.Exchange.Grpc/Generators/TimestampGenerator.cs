namespace Vertr.Exchange.Grpc.Generators;

internal sealed class TimestampGenerator : ITimestampGenerator
{
    public DateTime CurrentTime => DateTime.UtcNow;
}
