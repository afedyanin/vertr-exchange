using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disruptor;
using Disruptor.Dsl;
using Vertr.ExchCore.Domain.ValueObjects;

namespace Vertr.ExchCore.Infrastructure.Disruptor.Helpers
{
    internal static class DisruptorInitializer
    {
        public static void AttachEventHandlers<T>(
            Disruptor<T> disruptor,
            IEnumerable<IEventHandler<T>[]> eventHandlers) where T : class
        {
            if (eventHandlers == null || !eventHandlers.Any())
            {
                return;
            }

            EventHandlerGroup<T>? group = null;

            foreach (var handlers in eventHandlers)
            {
                if (handlers == null || !handlers.Any())
                {
                    continue;
                }

                if (group is null)
                {
                    group = disruptor.HandleEventsWith(handlers);
                }
                else
                {
                    group = group.Then(handlers);
                }
            }
        }
    }
}
