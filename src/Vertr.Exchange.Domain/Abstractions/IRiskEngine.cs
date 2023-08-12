namespace Vertr.Exchange.Domain.Abstractions;
internal interface IRiskEngine
{
    bool PreProcessCommand(long seq, OrderCommand cmd);

    bool HandlerRiskRelease(long seq, OrderCommand cmd);
}
