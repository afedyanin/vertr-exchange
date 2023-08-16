using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.Tests.Stubs;

namespace Vertr.Exchange.MatchingEngine.Tests;

[TestFixture(Category = "Unit")]
public class OrderMatchingEngineTests
{
    [Test]
    public void CanAddSymbols()
    {
        var ome = MatchingEngineStub.Instance;
        var symbolIds = new[] { 1, 2, 3, 4 };
        var cmd = BinaryCommandStub.CreateAddSymbolsCommand(symbolIds);

        var res = ome.AcceptBinaryCommand(cmd);

        Assert.That(res, Is.EqualTo(CommandResultCode.SUCCESS));
    }

    [Test]
    public void CanProcessBidOrder()
    {
        var ome = MatchingEngineStub.Instance;
        var symbolIds = new[] { 1, 2, 3, 4 };
        var cmd = BinaryCommandStub.CreateAddSymbolsCommand(symbolIds);
        ome.AcceptBinaryCommand(cmd);

        var bid = OrderStub.CreateBidOrder(19.23M, 40);

        cmd = OrderCommandStub.GtcOrder(
            bid.Action,
            bid.OrderId,
            bid.Uid,
            bid.Price,
            bid.Size,
            symbolIds[2]);

        ome.ProcessOrder(0L, cmd);

        Assert.Multiple(() =>
        {
            Assert.That(cmd.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
            Assert.That(cmd.MarketData, Is.Not.Null);
        });
    }

    [Test]
    public void CanProcessBidOrderWithInvalidSymbol()
    {
        var ome = MatchingEngineStub.Instance;
        var symbolIds = new[] { 1, 2, 3, 4 };
        var cmd = BinaryCommandStub.CreateAddSymbolsCommand(symbolIds);
        ome.AcceptBinaryCommand(cmd);

        var bid = OrderStub.CreateBidOrder(19.23M, 40);

        cmd = OrderCommandStub.GtcOrder(
            bid.Action,
            bid.OrderId,
            bid.Uid,
            bid.Price,
            bid.Size,
            13);

        ome.ProcessOrder(0L, cmd);

        Assert.That(cmd.ResultCode, Is.EqualTo(CommandResultCode.MATCHING_INVALID_ORDER_BOOK_ID));
    }

    [Test]
    public void ProcessMatchingWithInvalidSymbol()
    {
        var ome = MatchingEngineStub.Instance;
        var symbolIds = new[] { 1, 2, 3, 4 };
        var cmd = BinaryCommandStub.CreateAddSymbolsCommand(symbolIds);
        ome.AcceptBinaryCommand(cmd);

        var bid = OrderStub.CreateBidOrder(19.23M, 40);

        cmd = OrderCommandStub.GtcOrder(
            bid.Action,
            bid.OrderId,
            bid.Uid,
            bid.Price,
            bid.Size,
            13);

        ome.ProcessMatchingCommand(cmd);

        Assert.That(cmd.ResultCode, Is.EqualTo(CommandResultCode.MATCHING_INVALID_ORDER_BOOK_ID));
    }

    [Test]
    public void ProcessMatchingSuccess()
    {
        var ome = MatchingEngineStub.Instance;
        var symbolIds = new[] { 1, 2, 3, 4 };
        var cmd = BinaryCommandStub.CreateAddSymbolsCommand(symbolIds);
        ome.AcceptBinaryCommand(cmd);

        var bid = OrderStub.CreateBidOrder(19.23M, 40);

        cmd = OrderCommandStub.GtcOrder(
            bid.Action,
            bid.OrderId,
            bid.Uid,
            bid.Price,
            bid.Size);

        ome.ProcessMatchingCommand(cmd);

        Assert.That(cmd.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
    }
}
