using System.Text;
using System.Text.Json;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Binary.Commands;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Api.Commands;


public class AddAccountsCommand(
    long orderId,
    DateTime timestamp,
    IDictionary<long, IDictionary<int, decimal>> users) : ApiCommandBase(orderId, timestamp)
{
    private readonly IDictionary<long, IDictionary<int, decimal>> _users = users ?? new Dictionary<long, IDictionary<int, decimal>>();
    public override OrderCommandType CommandType => OrderCommandType.BINARY_DATA_COMMAND;

    public override void Fill(ref OrderCommand command)
    {
        var cmd = new BatchAddAccountsCommand()
        {
            Users = _users,
        };

        base.Fill(ref command);
        command.BinaryCommandType = BinaryDataType.COMMAND_ADD_ACCOUNTS;
        command.BinaryData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(cmd));
    }
}
