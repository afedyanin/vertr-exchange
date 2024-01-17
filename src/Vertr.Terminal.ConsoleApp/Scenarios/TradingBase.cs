using Refit;
using Vertr.Terminal.ApiClient;
using Vertr.Terminal.ConsoleApp.Views;

namespace Vertr.Terminal.ConsoleApp.Scenarios;

public abstract class TradingBase
{
    protected ITerminalApiClient ApiClient { get; }

    protected ApiCommands Commands { get; }


    protected TradingBase(string hostUrl)
    {
        ApiClient = RestService.For<ITerminalApiClient>(hostUrl);
        Commands = new ApiCommands(ApiClient);
    }

    public async Task Execute()
    {
        await InitExchange();
        await StartTrading();
        await DumpResults();
    }

    protected abstract Task StartTrading();

    protected virtual async Task InitExchange()
    {
        await Commands.Reset();

        await Commands.AddSymbols(StaticContext.Symbols.All);

        await Commands.AddUsers([
            StaticContext.UserAccounts.BobAccount,
            StaticContext.UserAccounts.AliceAccount,
        ]);
    }

    protected virtual async Task DumpResults()
    {
        var report = await Commands.GetSingleUserReport(StaticContext.Users.Bob);
        SingleUserReportView.Render(report);

        report = await Commands.GetSingleUserReport(StaticContext.Users.Alice);
        SingleUserReportView.Render(report);

        var trades = await ApiClient.GetTrades();
        TradesView.Render(trades);

        /*
        var ob = await api.GetOrderBook(Symbols.MSFT.Id);
        OrderBookView.Render(ob, "Random walk");
        var orders = await api.GetOrders();
        OrdersView.Render(orders);
         */
    }
}
