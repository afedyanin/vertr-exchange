using Vertr.Exchange.Shared.Enums;

namespace Vertr.Terminal.ApiClient.Tests;

[TestFixture(Category = "System", Explicit = true)]
public class MultiTradeTests : TerminalApiTestBase
{
    [SetUp]
    public async Task Setup()
    {
        await Init();
    }

    [Test]
    public async Task BidAskOpenMatch()
    {
        var bobOpenOrderId = await PlaceBobBid(3m, 2);
        var aliceOrderId = await PlaceAliceAsk(3m, 2);

        var bobProfile = await ApiCommands.GetSingleUserReport(BobAccount.User);
        var bobPosition = bobProfile!.Positions[Msft.Id];

        var aliceProfile = await ApiCommands.GetSingleUserReport(AliceAccount.User);
        var alicePosition = aliceProfile!.Positions[Msft.Id];

        Assert.Multiple(() =>
        {
            Assert.That(bobPosition.Direction, Is.EqualTo(PositionDirection.DIR_LONG));
            Assert.That(bobPosition.RealizedPnL, Is.EqualTo(-6));
            Assert.That(bobPosition.OpenVolume, Is.EqualTo(2));
            Assert.That(alicePosition.Direction, Is.EqualTo(PositionDirection.DIR_SHORT));
            Assert.That(alicePosition.RealizedPnL, Is.EqualTo(-6));
            Assert.That(alicePosition.OpenVolume, Is.EqualTo(2));
        });

        // Console.WriteLine($"Bob={bobPosition}");
        // Console.WriteLine($"Alice={alicePosition}");
    }

    [Test]
    public async Task BidAskCloseMatch()
    {
        var bobOrderId = await PlaceBobBid(3m, 2);
        var aliceOrderId = await PlaceAliceAsk(3m, 2);
        bobOrderId = await PlaceBobAsk(3m, 2);
        aliceOrderId = await PlaceAliceBid(3m, 2);

        var bobProfile = await ApiCommands.GetSingleUserReport(BobAccount.User);
        var bobPosition = bobProfile!.Positions[Msft.Id];

        var aliceProfile = await ApiCommands.GetSingleUserReport(AliceAccount.User);
        var alicePosition = aliceProfile!.Positions[Msft.Id];

        Assert.Multiple(() =>
        {
            Assert.That(bobPosition.Direction, Is.EqualTo(PositionDirection.EMPTY));
            Assert.That(bobPosition.RealizedPnL, Is.EqualTo(0));
            Assert.That(bobPosition.OpenVolume, Is.EqualTo(0));
            Assert.That(alicePosition.Direction, Is.EqualTo(PositionDirection.EMPTY));
            Assert.That(alicePosition.RealizedPnL, Is.EqualTo(0));
            Assert.That(alicePosition.OpenVolume, Is.EqualTo(0));
        });

        // Console.WriteLine($"Bob={bobPosition}");
        // Console.WriteLine($"Alice={alicePosition}");
    }

    [Test]
    public async Task BidProfit()
    {
        var bobOrderId = await PlaceBobBid(3m, 2);
        var aliceOrderId = await PlaceAliceAsk(3m, 2);
        bobOrderId = await PlaceBobAsk(5m, 2);
        aliceOrderId = await PlaceAliceBid(5m, 2);

        var bobProfile = await ApiCommands.GetSingleUserReport(BobAccount.User);
        var bobPosition = bobProfile!.Positions[Msft.Id];

        var aliceProfile = await ApiCommands.GetSingleUserReport(AliceAccount.User);
        var alicePosition = aliceProfile!.Positions[Msft.Id];

        Assert.Multiple(() =>
        {
            Assert.That(bobPosition.Direction, Is.EqualTo(PositionDirection.EMPTY));
            Assert.That(bobPosition.RealizedPnL, Is.EqualTo(4));
            Assert.That(bobPosition.OpenVolume, Is.EqualTo(0));
            Assert.That(alicePosition.Direction, Is.EqualTo(PositionDirection.EMPTY));
            Assert.That(alicePosition.RealizedPnL, Is.EqualTo(-4));
            Assert.That(alicePosition.OpenVolume, Is.EqualTo(0));
        });

        // Console.WriteLine($"Bob={bobPosition}");
        // Console.WriteLine($"Alice={alicePosition}");
    }

    [Test]
    public async Task AskProfit()
    {
        var bobOrderId = await PlaceBobBid(3m, 2);
        var aliceOrderId = await PlaceAliceAsk(3m, 2);
        bobOrderId = await PlaceBobAsk(1m, 2);
        aliceOrderId = await PlaceAliceBid(1m, 2);

        var bobProfile = await ApiCommands.GetSingleUserReport(BobAccount.User);
        var bobPosition = bobProfile!.Positions[Msft.Id];

        var aliceProfile = await ApiCommands.GetSingleUserReport(AliceAccount.User);
        var alicePosition = aliceProfile!.Positions[Msft.Id];

        Assert.Multiple(() =>
        {
            Assert.That(bobPosition.Direction, Is.EqualTo(PositionDirection.EMPTY));
            Assert.That(bobPosition.RealizedPnL, Is.EqualTo(-4));
            Assert.That(bobPosition.OpenVolume, Is.EqualTo(0));
            Assert.That(alicePosition.Direction, Is.EqualTo(PositionDirection.EMPTY));
            Assert.That(alicePosition.RealizedPnL, Is.EqualTo(4));
            Assert.That(alicePosition.OpenVolume, Is.EqualTo(0));
        });

        // Console.WriteLine($"Bob={bobPosition}");
        // Console.WriteLine($"Alice={alicePosition}");
    }

    [Test]
    public async Task BidProfitDiffQty()
    {
        var bobOrderId = await PlaceBobBid(3m, 2);
        var aliceOrderId = await PlaceAliceAsk(3m, 2);
        bobOrderId = await PlaceBobAsk(5m, 4);
        aliceOrderId = await PlaceAliceBid(5m, 4);

        var bobProfile = await ApiCommands.GetSingleUserReport(BobAccount.User);
        var bobPosition = bobProfile!.Positions[Msft.Id];

        var aliceProfile = await ApiCommands.GetSingleUserReport(AliceAccount.User);
        var alicePosition = aliceProfile!.Positions[Msft.Id];

        Assert.Multiple(() =>
        {
            Assert.That(bobPosition.Direction, Is.EqualTo(PositionDirection.DIR_SHORT));
            Assert.That(bobPosition.RealizedPnL, Is.EqualTo(4 - 10));
            Assert.That(bobPosition.OpenVolume, Is.EqualTo(2));
            Assert.That(alicePosition.Direction, Is.EqualTo(PositionDirection.DIR_LONG));
            Assert.That(alicePosition.RealizedPnL, Is.EqualTo(-4 - 10));
            Assert.That(alicePosition.OpenVolume, Is.EqualTo(2));
        });

        // Console.WriteLine($"Bob={bobPosition}");
        // Console.WriteLine($"Alice={alicePosition}");
    }

    [Test]
    public async Task AskProfitDiffQty()
    {
        var bobOrderId = await PlaceBobBid(3m, 2);
        var aliceOrderId = await PlaceAliceAsk(3m, 2);
        bobOrderId = await PlaceBobAsk(1m, 4);
        aliceOrderId = await PlaceAliceBid(1m, 4);

        var bobProfile = await ApiCommands.GetSingleUserReport(BobAccount.User);
        var bobPosition = bobProfile!.Positions[Msft.Id];

        var aliceProfile = await ApiCommands.GetSingleUserReport(AliceAccount.User);
        var alicePosition = aliceProfile!.Positions[Msft.Id];

        Assert.Multiple(() =>
        {
            Assert.That(bobPosition.Direction, Is.EqualTo(PositionDirection.DIR_SHORT));
            Assert.That(bobPosition.RealizedPnL, Is.EqualTo(-4 - 2));
            Assert.That(bobPosition.OpenVolume, Is.EqualTo(2));
            Assert.That(alicePosition.Direction, Is.EqualTo(PositionDirection.DIR_LONG));
            Assert.That(alicePosition.RealizedPnL, Is.EqualTo(4 - 2));
            Assert.That(alicePosition.OpenVolume, Is.EqualTo(2));
        });

        // Console.WriteLine($"Bob={bobPosition}");
        // Console.WriteLine($"Alice={alicePosition}");
    }

}
