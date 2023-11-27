using Vertr.Exchange.Client.ConsoleApp.StaticData;
using Vertr.Exchange.Protos;
using static Vertr.Exchange.Protos.Exchange;

namespace Vertr.Exchange.Client.ConsoleApp.Extensions;
internal static class ExchangeClientExtensions
{
    public static async Task<CommandResult> RegisterSymbols(this ExchangeClient client)
    {
        var req = new AddSymbolsRequest();
        req.Symbols.Add(Symbols.All.Select(s => s.GetSpecification()));
        var reply = await client.AddSymbolsAsync(req);
        return reply;
    }
}
