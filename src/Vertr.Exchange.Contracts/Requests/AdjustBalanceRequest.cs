namespace Vertr.Exchange.Contracts.Requests;

public record AdjustBalanceRequest
{
    public long UserId { get; set; }

    public int Currency { get; set; }

    public decimal Amount { get; set; }
}
