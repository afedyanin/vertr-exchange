using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertr.ExchCore.Domain.Commands;
using Vertr.ExchCore.Domain.Enums;

namespace Vertr.ExchCore.Domain.ValueObjects
{
    public record class OrderCommandResult
    {
        public ApiCommand ApiCommand { get; }

        public CommandResultCode ResultCode { get; }

        public long Sequence { get; }

        public OrderCommandResult(
            ApiCommand apiCommand,
            CommandResultCode resultCode,
            long sequence)
        {
            ApiCommand = apiCommand;
            ResultCode = resultCode;
            Sequence = sequence;
        }
    }
}
