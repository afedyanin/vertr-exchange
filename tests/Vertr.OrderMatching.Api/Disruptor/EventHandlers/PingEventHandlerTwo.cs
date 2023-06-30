using Disruptor;
using Vertr.OrderMatching.Api.Disruptor.Events;

namespace Vertr.OrderMatching.Api.Disruptor.EventHandlers
{
    public class PingEventHandlerTwo : IEventHandlerTwo
    {
        public void OnEvent(PingEvent data, long sequence, bool endOfBatch)
        {
            Console.WriteLine($"Ping event handler TWO: seq#{sequence} data={data}");
        }
    }
}
