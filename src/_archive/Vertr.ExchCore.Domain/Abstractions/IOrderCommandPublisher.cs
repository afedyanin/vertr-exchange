using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertr.ExchCore.Domain.ValueObjects;

namespace Vertr.ExchCore.Domain.Abstractions;

public interface IOrderCommandPublisher
{
    void Publish(OrderCommand command);
}
