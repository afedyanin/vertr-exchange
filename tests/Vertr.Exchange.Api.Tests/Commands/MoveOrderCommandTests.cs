using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Tests.Commands;

[TestFixture(Category = "Unit")]
public class MoveOrderCommandTests : ApiTestBase
{
    [Test]
    public async Task CanMoveOrder()
    {
        var uid = 100L;
        await AddUser(uid);

        var symbol = 2;
        await AddSymbol(symbol);

        var res = await PlaceGTCOrder(OrderAction.BID, uid, symbol, 23.45m, 34);
        var orderId = res.OrderId;

        var move = new MoveOrderCommand(orderId, DateTime.UtcNow, uid, 23.56m, symbol);
        res = await Api.SendAsync(move);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));

        // TODO: Check order book
        // TODO: Handle case with matching order
    }
}
