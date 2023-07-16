using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disruptor;
using Disruptor.Dsl;
using Vertr.ExchCore.Domain.Abstractions;
using Vertr.ExchCore.Domain.ValueObjects;

namespace Vertr.ExchCore.Infrastructure.Disruptor.Extensions
{
    internal static class DisruptorExtensions
    {
        public static void AttachEventHandlers(this
            Disruptor<OrderCommand> disruptor,
            IEnumerable<IOrderCommandSubscriber> subscribers)
        {
            Debug.Assert(subscribers != null);
            Debug.Assert(subscribers.Any());

            EventHandlerGroup<OrderCommand>? eventHandlerGroup = null;

            var subscriberGroups = subscribers.GroupBy(s => s.Priority);

            foreach (var subscriberGroup in subscriberGroups.OrderBy(g => g.Key))
            {
                var currentGroup = subscriberGroup?.ToArray();

                if (currentGroup is null)
                {
                    continue;
                }

                var wrappedHandlers = WrapHandlers(currentGroup);

                if (eventHandlerGroup is null)
                {
                    eventHandlerGroup = disruptor.HandleEventsWith(wrappedHandlers);
                }
                else
                {
                    eventHandlerGroup = eventHandlerGroup.Then(wrappedHandlers);
                }
            }
        }

        private static IEventHandler<OrderCommand>[] WrapHandlers(IOrderCommandSubscriber[] handlers)
            => handlers.Select(h => new DisruptorOrderCommandEventHanlder(h)).ToArray();
    }
}
