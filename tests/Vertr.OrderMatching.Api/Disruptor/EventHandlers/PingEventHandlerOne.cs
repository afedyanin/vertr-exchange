using Disruptor;
using Vertr.OrderMatching.Api.Disruptor.Events;

namespace Vertr.OrderMatching.Api.Disruptor.EventHandlers
{
    public class PingEventHandlerOne : IEventHandlerOne
    {
        public void OnEvent(PingEvent data, long sequence, bool endOfBatch)
        {
            Console.WriteLine($"Ping event handler ONE: seq#{sequence} data={data}");
            data.Message += "::ONE processed::";
        }
    }
}
