using Vertr.Exchange.Contracts;

namespace Vertr.Exchange.Server.Extensions;

internal static class SymbolSpecificationExtensions
{
    public static Domain.Common.SymbolSpecification[] ToDomain(this IEnumerable<SymbolSpecification> specs)
        => specs.Select(spec => spec.ToDomain()).ToArray();

    private static Domain.Common.SymbolSpecification ToDomain(this SymbolSpecification spec)
    {
        return new Domain.Common.SymbolSpecification
        {
            SymbolId = spec.SymbolId,
            Type = spec.Type,
            Currency = spec.Currency,
        };
    }
}
