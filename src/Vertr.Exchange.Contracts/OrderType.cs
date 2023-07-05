namespace Vertr.Exchange.Contracts;

public enum OrderType : byte
{
    NONE = 0,
    GTC = 10,
    IOC = 20,
    IOC_BUDGET = 30,
    FOK = 40,
    FOK_BUDGET = 50
}
