using System;
using System.Collections.Generic;
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
            IEnumerable<IOrderCommandEventHandler[]> eventHandlers)
        {
            if (eventHandlers == null || !eventHandlers.Any())
            {
                return;
            }

            EventHandlerGroup<OrderCommand>? group = null;

            foreach (var handlers in eventHandlers)
            {
                if (handlers == null || !handlers.Any())
                {
                    continue;
                }

                var wrappedHandlers = WrapHandlers(handlers);

                if (group is null)
                {
                    group = disruptor.HandleEventsWith(wrappedHandlers);
                }
                else
                {
                    group = group.Then(wrappedHandlers);
                }
            }
        }

        private static IEventHandler<OrderCommand>[] WrapHandlers(IOrderCommandEventHandler[] handlers)
        {
            return handlers.Select(h => new DisruptorOrderCommandEventHanlder(h)).ToArray();
        }
    }
}
