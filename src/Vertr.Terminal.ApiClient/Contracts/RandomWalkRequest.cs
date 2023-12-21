namespace Vertr.Terminal.ApiClient.Contracts;

public class RandomWalkRequest
{
    public long UserId { get; set; }
    public int SymbolId { get; set; }
    public decimal BasePrice { get; set; }
    public decimal PriceDelta { get; set; }
    public int OrdersCount { get; set; }
}
