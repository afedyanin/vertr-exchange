using Vertr.Exchange.Application.Commands;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Application.Tests.Commands;

[TestFixture(Category = "Unit")]
public class AddUserCommandTests : ApiTestBase
{
    [Test]
    public async Task CanAddUser()
    {
        var cmd = new AddUserCommand(OrderIdGenerator.NextId, DateTime.UtcNow, 100L);
        var res = await SendAsync(cmd);

        Assert.Multiple(() =>
        {
            Assert.That(res.OrderId, Is.EqualTo(cmd.OrderId));
            Assert.That(res.Timestamp, Is.EqualTo(cmd.Timestamp));
            Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
        });
    }

    [Test]
    public async Task CannotAddUserTwice()
    {
        var cmd1 = new AddUserCommand(OrderIdGenerator.NextId, DateTime.UtcNow, 100L);
        var cmd2 = new AddUserCommand(OrderIdGenerator.NextId, DateTime.UtcNow, 100L);

        var res1 = await SendAsync(cmd1);
        var res2 = await SendAsync(cmd2);

        Assert.Multiple(() =>
        {
            Assert.That(res1.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
            Assert.That(res2.ResultCode, Is.EqualTo(CommandResultCode.USER_MGMT_USER_ALREADY_EXISTS));
        });
    }

    [Test]
    public async Task CanExecInParallel()
    {
        var t1 = Task.Run(async () =>
        {
            var cmd = new AddUserCommand(OrderIdGenerator.NextId, DateTime.UtcNow, 100L);
            var res = await SendAsync(cmd);

            Console.WriteLine($"T1 Result. OrderId={res.OrderId}, Code={res.ResultCode}");
        });

        var t2 = Task.Run(async () =>
        {
            var cmd = new AddUserCommand(OrderIdGenerator.NextId, DateTime.UtcNow, 100L);
            var res = await SendAsync(cmd);

            Console.WriteLine($"T2 Result. OrderId={res.OrderId}, Code={res.ResultCode}");
        });

        var t3 = Task.Run(async () =>
        {
            var cmd1 = new AddUserCommand(OrderIdGenerator.NextId, DateTime.UtcNow, 100L);
            var res1 = await SendAsync(cmd1);

            var cmd2 = new AddUserCommand(OrderIdGenerator.NextId, DateTime.UtcNow, 100L);
            var res2 = await SendAsync(cmd1);

            Console.WriteLine($"T31 Result. OrderId={res1.OrderId}, Code={res1.ResultCode}");
            Console.WriteLine($"T32 Result. OrderId={res2.OrderId}, Code={res2.ResultCode}");
        });

        await Task.WhenAll(t1, t2, t3);

        Console.WriteLine("Completed");
        Assert.Pass();
    }
}
