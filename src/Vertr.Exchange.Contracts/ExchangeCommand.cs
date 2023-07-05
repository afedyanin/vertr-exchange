namespace Vertr.Exchange.Contracts;

public class ExchangeCommand
{
    public long Uuid { get; set; }

    public long Timestamp { get; set; }

    public ExchangeCommandType CommandType { get; set; }
}
