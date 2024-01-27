namespace Vertr.Exchange.Application.Generators;

public interface ITimestampGenerator
{
    DateTime CurrentTime { get; }
}
