namespace Vertr.Exchange.Api.Factories;
internal class TimeService : ITimeService
{
    public DateTime CurrentTime => DateTime.UtcNow;
}
