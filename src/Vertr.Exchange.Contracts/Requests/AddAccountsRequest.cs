namespace Vertr.Exchange.Contracts.Requests;

public record AddAccountsRequest
{
    public UserAccount[] UserAccounts { get; set; } = [];
}
