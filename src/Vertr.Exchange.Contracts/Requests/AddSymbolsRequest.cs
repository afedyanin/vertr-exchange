namespace Vertr.Exchange.Contracts.Requests;

public record AddSymbolsRequest
{
    public SymbolSpecification[] Symbols { get; set; } = [];
}
