namespace Vertr.Exchange.Api.Commands;
public record class AddUserCommand : ApiCommand
{
    public long Uid { get; set; }
}
