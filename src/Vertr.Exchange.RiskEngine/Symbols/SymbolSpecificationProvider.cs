using Vertr.Exchange.Common.Symbols;
using Vertr.Exchange.RiskEngine.Abstractions;

namespace Vertr.Exchange.RiskEngine.Symbols;
internal sealed class SymbolSpecificationProvider : ISymbolSpecificationProvider
{
    private readonly IDictionary<int, CoreSymbolSpecification> _symbolSpecs;

    public SymbolSpecificationProvider()
    {
        _symbolSpecs = new Dictionary<int, CoreSymbolSpecification>();
    }

    public bool AddSymbol(CoreSymbolSpecification symbolSpecification)
    {
        if (GetSymbolSpecification(symbolSpecification.SymbolId) != null)
        {
            return false; // CommandResultCode.SYMBOL_MGMT_SYMBOL_ALREADY_EXISTS;
        }
        else
        {
            RegisterSymbol(symbolSpecification.SymbolId, symbolSpecification);
            return true;
        }
    }

    public void AddSymbols(CoreSymbolSpecification[] symbols)
    {
        foreach (var spec in symbols)
        {
            AddSymbol(spec);
        }
    }

    public CoreSymbolSpecification? GetSymbolSpecification(int symbol)
    {
        _symbolSpecs.TryGetValue(symbol, out var specification);
        return specification;
    }

    public void RegisterSymbol(int symbol, CoreSymbolSpecification spec)
    {
        if (!_symbolSpecs.ContainsKey(symbol))
        {
            _symbolSpecs[symbol] = spec;
            return;
        }

        _symbolSpecs.Add(symbol, spec);
    }

    public void Reset()
    {
        _symbolSpecs.Clear();
    }
}
