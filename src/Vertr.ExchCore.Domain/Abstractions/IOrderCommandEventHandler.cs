using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertr.ExchCore.Domain.ValueObjects;

namespace Vertr.ExchCore.Domain.Abstractions;

public interface IOrderCommandEventHandler
{
    void HandleEvent(OrderCommand data, long sequence, bool endOfBatch);
}
