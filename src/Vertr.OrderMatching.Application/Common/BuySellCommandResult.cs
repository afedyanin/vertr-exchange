namespace Vertr.OrderMatching.Application.Common
{
    public record class BuySellCommandResult
    {
        public bool HasErrors => Errors.Any();

        public Guid OrderId { get; }

        public string[] Errors { get; }

        public BuySellCommandResult(Guid orderId)
        {
            OrderId = orderId;
            Errors = Array.Empty<string>();
        }

        public BuySellCommandResult(string[] errors)
        {
            OrderId = Guid.Empty;
            Errors = errors;
        }
    }
}
