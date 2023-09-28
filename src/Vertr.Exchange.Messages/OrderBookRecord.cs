namespace Vertr.Exchange.Messages;

public record class OrderBookRecord(decimal Price, long Volume, int Orders);
