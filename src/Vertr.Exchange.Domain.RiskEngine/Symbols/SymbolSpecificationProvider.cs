using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Domain.Common.Enums;

namespace Vertr.Exchange.Domain.RiskEngine.Symbols;

internal sealed class SymbolSpecificationProvider : ISymbolSpecificationProvider
{
    private readonly Dictionary<int, SymbolSpecification> _symbolSpecs;

    public SymbolSpecificationProvider()
    {
        _symbolSpecs = [];
    }

    public CommandResultCode AddSymbols(SymbolSpecification[] symbols)
    {
        if (HasDuplicates(symbols))
        {
            return CommandResultCode.SYMBOL_MGMT_SYMBOL_ALREADY_EXISTS;
        }

        foreach (var spec in symbols)
        {
            if (!_symbolSpecs.TryAdd(spec.SymbolId, spec))
            {
                _symbolSpecs[spec.SymbolId] = spec;
            }
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
