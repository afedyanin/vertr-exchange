using System.Text;
using System.Text.Json;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Binary.Commands;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Commands;


public class AddAccountsCommand : ApiCommandBase
{
    private readonly IDictionary<int, IDictionary<int, decimal>> _users;
    public override OrderCommandType CommandType => OrderCommandType.BINARY_DATA_COMMAND;

    public AddAccountsCommand(
        long orderId,
        DateTime timestamp,
        IDictionary<int, IDictionary<int, decimal>> users) : base(orderId, timestamp)
    {
        _users = users ?? new Dictionary<int, IDictionary<int, decimal>>();
    }

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
