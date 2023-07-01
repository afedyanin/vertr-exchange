using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disruptor.Dsl;

namespace Vertr.OrderMatching.ConsoleApp.DisroptorSample;

internal class SampleMain
{
    public static void Run()
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
