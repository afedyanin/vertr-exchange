namespace Vertr.Exchange.Application.Generators;

public interface IOrderIdGenerator
{
    long NextId { get; }
}
