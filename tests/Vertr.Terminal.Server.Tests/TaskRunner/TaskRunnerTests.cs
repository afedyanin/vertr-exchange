namespace Vertr.Terminal.Server.Tests.TaskRunner;

[TestFixture(Category = "Unit")]
public class TaskRunnerTests
{

    [Test]
    public async Task CanRunTask()
    {
        var progress = new Progress<int>();

        progress.ProgressChanged += (sender, args) =>
        {
            Console.WriteLine($"Progress: {args}");
        };

        var t1 = Task.Run(async () =>
        {
            await DoWork(progress);
        });

        while (!t1.IsCompleted)
        {
            Console.WriteLine("Waiting...");
            await Task.Delay(100);
        }

        Console.WriteLine("Completed.");
    }

    private async Task DoWork(IProgress<int> progress, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Task started.");

        for (int i = 0; i < 100; i++)
        {
            await Task.Delay(5, cancellationToken);
            progress?.Report(i);
        }

        Console.WriteLine("Task finished.");
    }
}
