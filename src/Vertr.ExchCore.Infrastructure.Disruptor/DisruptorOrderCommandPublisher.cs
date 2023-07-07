using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertr.ExchCore.Domain.Abstractions;
using Vertr.ExchCore.Domain.ValueObjects;
using Vertr.ExchCore.Infrastructure.Disruptor.Abstractions;

namespace Vertr.ExchCore.Infrastructure.Disruptor
{
    public class DisruptorOrderCommandPublisher : IOrderCommandPublisher
    {
        private readonly IDisruptorService<OrderCommand> _disruptor;

        public DisruptorOrderCommandPublisher(
            IDisruptorService<OrderCommand> disruptor)
        {
            _disruptor = disruptor;
        }

        public void Publish(OrderCommand command)
        {
            _disruptor.Publish(command);
        }
    }
}
