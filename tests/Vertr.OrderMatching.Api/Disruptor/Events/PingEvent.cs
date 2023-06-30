namespace Vertr.OrderMatching.Api.Disruptor.Events
{
    public record class PingEvent
    {
        public int Id { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
