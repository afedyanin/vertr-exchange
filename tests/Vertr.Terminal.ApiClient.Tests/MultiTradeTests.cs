namespace Vertr.Terminal.ApiClient.Tests;

[TestFixture(Category = "System", Explicit = true)]
public class MultiTradeTests : TerminalApiTestBase
{
    [SetUp]
    public async Task Setup()
    {
        await Init();
    }

    [TestCase(124, 24, 120, 26)]
    public async Task UserProfilesHaveValidStateAfterTrades(
        decimal bidPrice,
        long bidQty,
        decimal askPrice,
        long askQty)
    {
        // TODO: Implement this

        var minQty = Math.Min(bidQty, askQty);

        var bobOpenOrderId = await PlaceBobBid(bidPrice, bidQty);
        var aliceOrderId = await PlaceAliceAsk(askPrice, askQty);

        var bobProfile = await ApiCommands.GetSingleUserReport(BobAccount.User);
        var bobPosition = bobProfile!.Positions[Msft.Id];

        var aliceProfile = await ApiCommands.GetSingleUserReport(AliceAccount.User);
        var alicePosition = aliceProfile!.Positions[Msft.Id];
        Console.WriteLine($"Before Bob={bobPosition}");
        Console.WriteLine($"Before Alice={alicePosition}");

        var bobCloseOrderId = await PlaceOrder(BobAccount, 130, -28); // Close position
        var aliceCloseOrderId = await PlaceOrder(AliceAccount, 132, 28); // Close position

        bobProfile = await ApiCommands.GetSingleUserReport(BobAccount.User);
        bobPosition = bobProfile!.Positions[Msft.Id];

        aliceProfile = await ApiCommands.GetSingleUserReport(AliceAccount.User);
        alicePosition = aliceProfile!.Positions[Msft.Id];
        Console.WriteLine($"After Bob={bobPosition}");
        Console.WriteLine($"After Alice={alicePosition}");

        // ValidateProfileAmounts(bobProfile, BobInitialAmount, decimal.Zero, decimal.Zero);
        // ValidateProfilePosition(bobProfile, decimal.Zero, decimal.Zero);

        // var aliceProfile = await ApiCommands.GetSingleUserReport(AliceAccount.User);
        // ValidateProfileAmounts(aliceProfile, AliceInitialAmount, decimal.Zero, decimal.Zero);
    }

    /*
        private void ValidateProfilePosition(
            SingleUserReportResult? report,
            decimal expectedPnl,
            decimal expectedOpenVolume)
        {
            Assert.That(report, Is.Not.Null);

            var position = report.Positions[Msft.Id];

            Console.WriteLine($"Position={position}");

            Assert.Multiple(() =>
            {
                Assert.That(position.RealizedPnL, Is.EqualTo(expectedPnl));
                Assert.That(position.OpenVolume, Is.EqualTo(expectedOpenVolume));
            });
        }
    */
}
