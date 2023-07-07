namespace Vertr.ExchCore.Domain.Enums;

public enum OrderCommandType : byte
{
    NONE = 0,
    PLACE_ORDER = 1,
    CANCEL_ORDER = 2,
    MOVE_ORDER = 3,
    REDUCE_ORDER = 4,
    ORDER_BOOK_REQUEST = 6,
}
