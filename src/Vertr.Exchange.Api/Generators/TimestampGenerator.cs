namespace Vertr.Exchange.Api.Generators;

internal sealed class TimestampGenerator : ITimestampGenerator
{
    public DateTime CurrentTime => DateTime.UtcNow;
}