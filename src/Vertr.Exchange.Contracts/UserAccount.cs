namespace Vertr.Exchange.Contracts;

public record UserAccount
{
    public long UserId { get; set; }

    public Dictionary<int, decimal> Balances { get; set; } = [];
}
