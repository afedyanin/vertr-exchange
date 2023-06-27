using Disruptor;
using Disruptor.Dsl;

namespace Vertr.OrderMatching.ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        var disruptor = new Disruptor<SampleEvent>(() => new SampleEvent(), ringBufferSize: 1024);

        disruptor.HandleEventsWith(new SampleEventHandler());

        Console.WriteLine($"Starting disruptor. Press ESC to exit. ThreadId={Thread.CurrentThread.ManagedThreadId}");

        disruptor.Start();

        do
        {
            var key = Console.ReadKey();

            if (key.Key == ConsoleKey.Escape)
            {
                break;
            }

            using (var scope = disruptor.PublishEvent())
            {
                var data = scope.Event();
                data.Id = 42;
                data.Value = 1.1;
            }

        } while (true);

        disruptor.Shutdown();

        Console.WriteLine($"Completed ThreadId={Thread.CurrentThread.ManagedThreadId}");
    }
}

public class SampleEvent
{
    public int Id { get; set; }
    public double Value { get; set; }
}

public class SampleEventHandler : IEventHandler<SampleEvent>
{
    public void OnEvent(SampleEvent data, long sequence, bool endOfBatch)
    {
        Console.WriteLine($"Event: {data.Id} => {data.Value} ThreadId={Thread.CurrentThread.ManagedThreadId}");
    }
}

