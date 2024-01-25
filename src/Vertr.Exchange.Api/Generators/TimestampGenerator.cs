namespace Vertr.Exchange.Application.Generators;

internal sealed class TimestampGenerator : ITimestampGenerator
{
    public DateTime CurrentTime => DateTime.UtcNow;
}
