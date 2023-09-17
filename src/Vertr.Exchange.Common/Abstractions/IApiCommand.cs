namespace Vertr.Exchange.Common.Abstractions;
public interface IApiCommand
{
    void Fill(ref OrderCommand command);

    long OrderId { get; }

    DateTime Timestamp { get; }
}
