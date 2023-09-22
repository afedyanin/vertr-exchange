namespace Vertr.Exchange.Api.Tests.Stubs;
public static class AccountsStub
{
    public static IDictionary<long, IDictionary<int, decimal>> UserAccounts
        => new Dictionary<long, IDictionary<int, decimal>>
        {
            { 100L, Accounts1 },
            { 200L, Accounts2 },
        };

    private static IDictionary<int, decimal> Accounts1
        => new Dictionary<int, decimal>
            {
                { SymbolSpecificationStub.Currencies[0], 150M },
                { SymbolSpecificationStub.Currencies[3], 12M },
            };

    private static IDictionary<int, decimal> Accounts2
        => new Dictionary<int, decimal>
            {
                { SymbolSpecificationStub.Currencies[1], 60.45M },
                { SymbolSpecificationStub.Currencies[2], 23_000.456M },
            };
}
