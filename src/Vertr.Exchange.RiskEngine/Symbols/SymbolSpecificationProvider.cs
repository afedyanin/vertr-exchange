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
        // TODO: Validate for duplicates

        foreach (var spec in symbols)
        {
            AddSymbol(spec);
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

    private CommandResultCode AddSymbol(SymbolSpecification symbolSpecification)
    {
        if (GetSymbol(symbolSpecification.SymbolId) != null)
        {
            return CommandResultCode.SYMBOL_MGMT_SYMBOL_ALREADY_EXISTS;
        }
        else
        {
            RegisterSymbol(symbolSpecification.SymbolId, symbolSpecification);
            return CommandResultCode.SUCCESS;
        }
    }
    private void RegisterSymbol(int symbol, SymbolSpecification spec)
    {
        if (!_symbolSpecs.ContainsKey(symbol))
        {
            _symbolSpecs[symbol] = spec;
            return;
        }

        _symbolSpecs.Add(symbol, spec);
    }
}
