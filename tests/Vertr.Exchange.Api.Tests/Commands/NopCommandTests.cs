using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Api.Tests.Stubs;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Tests.Commands;

[TestFixture(Category = "Unit")]
public class NopCommandTests
{
    [Test]
    public void CanSendNopCommand()
    {
        using var api = ExcnageApiStub.GetNoEnginesApi();

        var cmd = new NopCommand(1L, DateTime.UtcNow);

        api.Send(cmd);

        Assert.Pass();
    }

    [Test]
    public async Task CanSendAsyncNopCommand()
    {
        using var api = ExcnageApiStub.GetNoEnginesApi();

        var cmd = new NopCommand(1L, DateTime.UtcNow);

        var res = await api.SendAsync(cmd);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
    }
}
