using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Api.Tests.Stubs;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Tests.Commands;

[TestFixture(Category = "Unit")]
public class AddSymbolsCommandTests : CommandTestBase
{
    [Test]
    public async Task CanAddSymbols()
    {
        var cmd = new AddSymbolsCommand(1L, DateTime.UtcNow, SymbolSpecificationStub.GetSymbols);
        var res = await Api.SendAsync(cmd);
        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
    }

    [Test]
    public async Task CannotAddSymbolWithTheSameId()
    {
        // TODO: Implement this
        var cmd = new AddSymbolsCommand(1L, DateTime.UtcNow, SymbolSpecificationStub.GetSymbols);
        var res = await Api.SendAsync(cmd);
        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
    }
}
