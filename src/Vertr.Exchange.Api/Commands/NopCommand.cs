using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.Api.Commands;
public record class NopCommand : IApiCommand
{
    public void Fill(ref OrderCommand command)
    {
        throw new NotImplementedException();
    }
}
