namespace Vertr.Exchange.Api.Commands;
public record class ResumeUserCommand : ApiCommand
{
    public long Uid { get; set; }
}
