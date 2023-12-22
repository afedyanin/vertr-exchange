using Vertr.Exchange.Shared.Enums;

namespace Vertr.Terminal.ApiClient.Contracts;

public record Symbol(int Id, Currency Currency, string Code, string Name, SymbolType SymbolType);
