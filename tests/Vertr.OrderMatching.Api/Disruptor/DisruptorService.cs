using Disruptor.Dsl;
using Vertr.OrderMatching.Api.Disruptor.EventHandlers;
using Vertr.OrderMatching.Api.Disruptor.Events;

namespace Vertr.OrderMatching.Api.Disruptor
{
    public class DisruptorService : IDisruptorService
    {
        private readonly Disruptor<PingEvent> _disruptor;
        private readonly IEventHandlerOne _eventHandlerOne;
        private readonly IEventHandlerTwo _eventHandlerTwo;

        public DisruptorService(
            IEventHandlerOne one,
            IEventHandlerTwo two)
        {
            _eventHandlerOne = one;
            _eventHandlerTwo = two;

            _disruptor = new Disruptor<PingEvent>(() => new PingEvent(), ringBufferSize: 1024);
            _disruptor.HandleEventsWith(_eventHandlerOne).Then(_eventHandlerTwo);
            _disruptor.Start();
        }

        public void Dispose()
        {
            _disruptor.Shutdown();
        }

        public void PublishPing(string ping)
        {
            using var scope = _disruptor.PublishEvent();

            var data = scope.Event();
            data.Id = 42;
            data.Message = ping;
        }
    }
}
