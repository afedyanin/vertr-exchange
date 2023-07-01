using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disruptor;

namespace Vertr.OrderMatching.ConsoleApp.DisruptorSample;

public class SampleEventHandler : IEventHandler<SampleEvent>
{
    public void OnEvent(SampleEvent data, long sequence, bool endOfBatch)
    {
        Console.WriteLine($"Event: {data.Id} => {data.Value} ThreadId={Thread.CurrentThread.ManagedThreadId}");
    }
}
