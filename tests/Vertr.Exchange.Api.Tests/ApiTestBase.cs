using Microsoft.Extensions.DependencyInjection;
using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Api.Commands.Queries;
using Vertr.Exchange.Api.Tests.Stubs;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Binary.Reports;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Tests;
public abstract class ApiTestBase
{
    protected IServiceProvider ServiceProvider { get; private set; }

    protected IExchangeApi Api { get; private set; }

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        ServiceProvider = ServiceProviderStub.BuildServiceProvider();
    }

    [SetUp]
    public void Setup()
    {
        Api = ServiceProvider.GetRequiredService<IExchangeApi>();
    }

    [TearDown]
    public void TearDown()
    {
        Api?.Dispose();
    }

    protected async Task AddUser(long userId)
    {
        var cmd = new AddUserCommand(1000L, DateTime.UtcNow, userId);

        var res = await Api.SendAsync(cmd);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
    }

    protected async Task AddSymbol(
        int symbol,
        int currency = 100,
        SymbolType symbolType = SymbolType.CURRENCY_EXCHANGE_PAIR)
    {
        var symSpec = new SymbolSpecification
        {
            Currency = currency,
            Type = symbolType,
            SymbolId = symbol
        };

        var cmd = new AddSymbolsCommand(1010L, DateTime.UtcNow, new SymbolSpecification[] { symSpec });

        var res = await Api.SendAsync(cmd);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
    }

    protected async Task<SingleUserReportResult?> GetUserReport(long uid)
    {
        var rep = new SingleUserReport(1020L, DateTime.UtcNow, uid);
        var res = await Api.SendAsync(rep);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        var report = rep.GetResult(res);

        return report;
    }

    protected async Task<IApiCommandResult> PlaceGTCOrder(
        OrderAction orderAction,
        long uid,
        int symbol,
        decimal price,
        long size,
        long orderId = 1030L)
    {
        var cmd = new PlaceOrderCommand(
            orderId,
            DateTime.UtcNow,
            price,
            size,
            orderAction,
            OrderType.GTC,
            uid,
            symbol);

        var res = await Api.SendAsync(cmd);
        return res;
    }
}
