namespace Vertr.Exchange.Domain.Common.Abstractions;
public interface IApiCommand
{
    void Fill(ref OrderCommand command);

    long OrderId { get; }

    DateTime Timestamp { get; }
}
