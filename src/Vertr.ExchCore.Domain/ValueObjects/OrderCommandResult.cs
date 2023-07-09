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
        public OrderCommand? Command { get; }

        public CommandResultCode ResultCode { get; }

        public long Sequence { get; }

        public OrderCommandResult(
            OrderCommand command,
            CommandResultCode resultCode,
            long sequence)
        {
            Command = command;
            ResultCode = resultCode;
            Sequence = sequence;
        }
    }
}
