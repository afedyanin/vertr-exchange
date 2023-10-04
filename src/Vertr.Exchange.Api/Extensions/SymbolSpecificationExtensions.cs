using Vertr.Exchange.Protos;

namespace Vertr.Exchange.Api.Extensions;

public static class SymbolSpecificationExtensions
{
    public static Common.SymbolSpecification[] ToDomain(this IEnumerable<SymbolSpecification> specs)
        => specs.Select(spec => spec.ToDomain()).ToArray();

    private static Common.SymbolSpecification ToDomain(this SymbolSpecification spec)
    {
        return new Common.SymbolSpecification
        {
            SymbolId = spec.SymbolId,
            Type = spec.Type.ToDomain(),
            Currency = spec.Currency,
        };
    }
}
