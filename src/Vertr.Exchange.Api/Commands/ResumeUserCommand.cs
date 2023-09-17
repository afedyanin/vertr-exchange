using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.Api.Commands;
public record class ResumeUserCommand : IApiCommand
{
    public long Uid { get; set; }

    public void Fill(ref OrderCommand command)
    {
        throw new NotImplementedException();
    }
}
