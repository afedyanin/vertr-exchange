using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertr.ExchCore.Domain.Abstractions;
using Vertr.ExchCore.Domain.Enums;

namespace Vertr.ExchCore.Domain.Entities
{
    public class Order : IOrder
    {
        public long OrderId { get; set; }

        public long Price { get; set; }

        public long Size { get; set; }

        public long Filled { get; set; }

        public long ReserveBidPrice { get; set; }

        public OrderAction Action { get; set; }

        public long Uid { get; set; }

        public long Timestamp { get; set; }
    }
}
