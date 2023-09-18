using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Api.Tests.Stubs;

namespace Vertr.Exchange.Api.Tests.Commands;

[TestFixture(Category = "Unit")]
public class NopCommandTests
{
    [Test]
    public void CanProcessNopCommand()
    {
        using var api = ExcnageApiStub.GetNoEnginesApi();

        var cmd = new NopCommand(1L, DateTime.UtcNow);

        var res = api.Execute(cmd);

        Assert.Pass();
    }
}
