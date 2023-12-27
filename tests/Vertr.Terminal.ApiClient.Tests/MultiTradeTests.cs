namespace Vertr.Terminal.ApiClient.Tests;

[TestFixture(Category = "System")]
public class MultiTradeTests : TerminalApiTestBase
{
    [SetUp]
    public async Task Setup()
    {
        await Init();
    }

    [TestCase(124, 24, 120, 26)]
    [TestCase(124, 28, 120, 26)]
    [TestCase(124, 26, 120, 26)]
    public async Task UserProfilesHaveValidStateAfterTrades(
        decimal bidPrice,
        long bidQty,
        decimal askPrice,
        long askQty)
    {
        // TODO: Implement this

        var minQty = Math.Min(bidQty, askQty);

        var bobOrderId = await PlaceBobBid(bidPrice, bidQty);
        var aliceOrderId = await PlaceAliceAsk(askPrice, askQty);

        var bobProfile = await ApiCommands.GetSingleUserReport(BobAccount.User);
        ValidateProfileAmounts(bobProfile, BobInitialAmount, decimal.Zero, minQty);

        var aliceProfile = await ApiCommands.GetSingleUserReport(AliceAccount.User);
        ValidateProfileAmounts(aliceProfile, AliceInitialAmount, decimal.Zero, minQty);
    }
}
