using Refit;
using Vertr.Terminal.ApiClient;
using Vertr.Terminal.ApiClient.Contracts;
using static Vertr.Terminal.ConsoleApp.StaticContext;

using Vertr.Terminal.ConsoleApp.Views;

namespace Vertr.Terminal.ConsoleApp.Scenarios;

public abstract class TradingBase
{
    protected ITerminalApiClient ApiClient { get; }

    protected ApiCommands Commands { get; }

    protected Symbol Symbol { get; }

    protected TradingBase(string terminalHhostUrl, Symbol symbol)
    {
        ApiClient = RestService.For<ITerminalApiClient>(terminalHhostUrl);
        Commands = new ApiCommands(ApiClient);
        Symbol = symbol;
    }

    public async Task Execute()
    {
        await InitExchange();
        await StartTrading();
    }

    protected abstract Task StartTrading();

    protected virtual async Task InitExchange()
    {
        await Commands.Reset();

        await Commands.AddSymbols(Symbols.All);

        await Commands.AddUsers([
            UserAccounts.BobAccount,
            UserAccounts.AliceAccount,
        ]);
    }

    protected virtual async Task DumpUserResults(User user)
    {
        var report = await Commands.GetSingleUserReport(user);
        SingleUserReportView.Render(report);
    }

    protected virtual async Task DumpTrades()
    {
        var trades = await ApiClient.GetTrades();
        TradesView.Render(trades);
    }

    protected virtual async Task DumpOrderBook()
    {
        var ob = await ApiClient.GetOrderBook(Symbols.MSFT.Id);
        OrderBookView.Render(ob, $"Order Book");
    }

    protected virtual async Task DumpOrders()
    {
        var orders = await ApiClient.GetOrders();
        var filtered = orders.Where(o => o.Symbol == Symbol.Id).ToArray();

        OrdersView.Render(filtered);
    }

    protected virtual async Task DumpPosition(User user)
    {
        var portfolios = await ApiClient.GetPortfolios();
        var portfolio = portfolios.Where(p => p.Uid == user.Id).First();
        var position = portfolio.Positions.Where(p => p.Symbol == Symbols.MSFT.Id).First();
        PositionView.Render(position);
    }

    protected async Task DumpResults()
    {
        await DumpUserResults(Users.Bob);
        await DumpPosition(Users.Bob);
        await DumpUserResults(Users.Alice);
        await DumpPosition(Users.Alice);
        await DumpOrderBook();
        await DumpTrades();
        // await DumpOrders();
    }

    protected virtual Task PlaceBid(User user, decimal price, long size)
    {
        var res = Commands.PlaceOrder(user, Symbol, price, size);
        return res;
    }

    protected virtual Task PlaceAsk(User user, decimal price, long size)
        => PlaceBid(user, price, size * (-1));
}
