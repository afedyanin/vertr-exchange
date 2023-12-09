using System.Text;
using System.Text.Json;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Common.Binary.Reports;
public static class BinaryQueryExtesions
{
    public static OrderCommand ToOrderCommand(this SingleUserReportQuery query)
    {
        var orderCommand = new OrderCommand()
        {
            Command = OrderCommandType.BINARY_DATA_QUERY,
            BinaryCommandType = BinaryDataType.QUERY_SINGLE_USER_REPORT,
            BinaryData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(query)),
        };

        return orderCommand;
    }

    public static byte[] ToBinary(this SingleUserReportResult report)
    {
        if (report == null)
        {
            return [];
        }

        var binaryData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(report));

        return binaryData;
    }
}
