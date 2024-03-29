using Vertr.Exchange.Application.Commands;
using Vertr.Exchange.Domain.Common.Enums;

namespace Vertr.Exchange.Application.Tests.Commands;

[TestFixture(Category = "Unit")]
public class ResumeUserCommandTests : ApiTestBase
{
    [Test]
    public async Task CanResumeUser()
    {
        var add = new AddUserCommand(1L, DateTime.UtcNow, 100L);
        var res = await SendAsync(add);

        var susp = new SuspendUserCommand(2L, DateTime.UtcNow, add.Uid);
        res = await SendAsync(susp);

        var resume = new ResumeUserCommand(3L, DateTime.UtcNow, add.Uid);
        res = await SendAsync(resume);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
    }

    [Test]
    public async Task CannotResumeActiveUser()
    {
        var add = new AddUserCommand(1L, DateTime.UtcNow, 100L);
        var res = await SendAsync(add);

        var resume = new ResumeUserCommand(3L, DateTime.UtcNow, add.Uid);
        res = await SendAsync(resume);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.USER_MGMT_USER_NOT_SUSPENDED));
    }

    [Test]
    public async Task CannotResumeNotExistingUser()
    {
        var resume = new ResumeUserCommand(3L, DateTime.UtcNow, 200L);
        var res = await SendAsync(resume);

        Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.USER_MGMT_USER_NOT_FOUND));
    }
}
