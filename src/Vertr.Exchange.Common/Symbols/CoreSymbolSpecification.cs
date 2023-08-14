using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Common.Symbols;

public class CoreSymbolSpecification
{
    public int SymbolId { get; set; }
    public SymbolType Type { get; set; }

    // currency pair specification
    public int BaseCurrency { get; }  // base currency
    public int QuoteCurrency { get; } // quote/counter currency (OR futures contract currency)
    public long BaseScaleK { get; }   // base currency amount multiplier (lot size in base currency units)
    public long QuoteScaleK { get; }  // quote currency amount multiplier (step size in quote currency units)

    // fees per lot in quote? currency units
    public long TakerFee { get; } // TODO check invariant: taker fee is not less than maker fee
    public long MakerFee { get; }

    // margin settings (for type=FUTURES_CONTRACT only)
    public long MarginBuy { get; }   // buy margin (quote currency)
    public long MarginSell { get; }  // sell margin (quote currency)

}
