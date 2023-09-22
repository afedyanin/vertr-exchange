using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.RiskEngine.Symbols;

internal sealed class SymbolSpecificationProvider : ISymbolSpecificationProvider
{
    private readonly IDictionary<int, SymbolSpecification> _symbolSpecs;

    public SymbolSpecificationProvider()
    {
        _symbolSpecs = new Dictionary<int, SymbolSpecification>();
    }

    public CommandResultCode AddSymbols(SymbolSpecification[] symbols)
    {
        if (HasDuplicates(symbols))
        {
            return CommandResultCode.SYMBOL_MGMT_SYMBOL_ALREADY_EXISTS;
        }

        foreach (var spec in symbols)
        {
            _symbolSpecs.Add(spec.SymbolId, spec);
        }

        return CommandResultCode.SUCCESS;
    }

    public SymbolSpecification? GetSymbol(int symbol)
    {
        _symbolSpecs.TryGetValue(symbol, out var specification);
        return specification;
    }

    public void Reset()
    {
        _symbolSpecs.Clear();
    }

    private bool HasDuplicates(SymbolSpecification[] symbols)
    {
        foreach (var symbol in symbols)
        {
            if (_symbolSpecs.ContainsKey(symbol.SymbolId))
            {
                return true;
            }
        }

        return false;
    }
}
