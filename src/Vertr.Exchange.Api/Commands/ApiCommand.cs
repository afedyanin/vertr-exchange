namespace Vertr.Exchange.Api.Commands;
public abstract record class ApiCommand
{
    public DateTime TimeStamp { get; set; }
}
