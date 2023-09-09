namespace Vertr.Exchange.Api.Commands;
public record class SuspendUserCommand : ApiCommand
{
    public long Uid { get; set; }
}
