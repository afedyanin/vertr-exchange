using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertr.ExchCore.Domain.Enums;

namespace Vertr.ExchCore.Domain.ValueObjects
{
    public record class OrderCommandResult
    {
        public OrderCommand? Command { get; set; }

        public CommandResultCode ResultCode { get; set; }

        public long Sequence { get; set; }
    }
}
