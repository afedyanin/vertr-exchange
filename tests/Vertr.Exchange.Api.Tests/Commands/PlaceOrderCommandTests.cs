using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Tests.Commands;

[TestFixture(Category = "Unit")]
public class PlaceOrderCommandTests : CommandTestBase
{
    [Test]
    public async Task CanPlaceIocOrder()
    {
        var uid = 100L;
        var symbol = 2;

        await AddUser(uid);
        await AddSymbol(symbol);

        var cmd = new PlaceOrderCommand(
            123L,
            DateTime.UtcNow,
            120.34m,
            37,
            OrderAction.BID,
            OrderType.IOC,
            uid,
            symbol);

        var res = await Api.SendAsync(cmd);
        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
    }
}

