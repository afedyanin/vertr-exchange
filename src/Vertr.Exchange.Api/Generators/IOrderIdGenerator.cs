namespace Vertr.Exchange.Api.Generators;

public interface IOrderIdGenerator
{
    long NextId { get; }
}
