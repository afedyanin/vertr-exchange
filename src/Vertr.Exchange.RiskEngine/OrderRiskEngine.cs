using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using System.Runtime.CompilerServices;
using Vertr.Exchange.RiskEngine.Orders;
using Vertr.Exchange.Accounts.UserCommands;
using Vertr.Exchange.RiskEngine.Symbols;
using Vertr.Exchange.Common.Binary.Commands;
using Vertr.Exchange.Common.Binary.Reports;
using Vertr.Exchange.RiskEngine.Binary;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("Vertr.Exchange.RiskEngine.Tests")]

namespace Vertr.Exchange.RiskEngine;
internal sealed class OrderRiskEngine : IOrderRiskEngine
{
    private readonly ILogger<OrderRiskEngine> _logger;

    public IUserProfileProvider UserProfiles { get; }

    public ISymbolSpecificationProvider SymbolSpecificationProvider { get; }

    public OrderRiskEngine(
        IUserProfileProvider userProfiles,
        ISymbolSpecificationProvider symbolSpecificationProvider,
        ILogger<OrderRiskEngine> logger)
    {
        UserProfiles = userProfiles;
        SymbolSpecificationProvider = symbolSpecificationProvider;
        _logger = logger;
    }

    public void PreProcessCommand(long seq, OrderCommand cmd)
    {
        switch (cmd.Command)
        {
            case OrderCommandType.PLACE_ORDER:
                var handler = new PreProcessOrderHandler(UserProfiles, SymbolSpecificationProvider);
                cmd.ResultCode = handler.Handle(cmd);
                return;

            case OrderCommandType.ADD_USER:
            case OrderCommandType.SUSPEND_USER:
            case OrderCommandType.RESUME_USER:
            case OrderCommandType.BALANCE_ADJUSTMENT:
                var userCommand = UserCommandFactory.CreateUserCommand(cmd, UserProfiles);
                _logger.LogDebug("Executing UserCommand={CommandType} OrderId={OrderId} Uid={Uid}", cmd.Command, cmd.OrderId, cmd.Uid);
                cmd.ResultCode = userCommand.Execute();
                return;

            case OrderCommandType.BINARY_DATA_COMMAND:
                // ignore return result, because it should be set by MatchingEngineRouter
                _logger.LogDebug("Processing BinaryCommand={CommandType} OrderId={OrderId}", cmd.BinaryCommandType, cmd.OrderId);
                cmd.ResultCode = AcceptBinaryCommand(cmd);
                return;

            case OrderCommandType.BINARY_DATA_QUERY:
                // ignore return result, because it should be set by MatchingEngineRouter
                cmd.ResultCode = AcceptBinaryQuery(cmd);
                return;

            case OrderCommandType.RESET:
                Reset();
                cmd.ResultCode = CommandResultCode.SUCCESS;
                return;

            case OrderCommandType.NOP:
                _logger.LogDebug("Pre processing NOP command. OrderId={OrderId}", cmd.OrderId);
                return;

            case OrderCommandType.MOVE_ORDER:
            case OrderCommandType.CANCEL_ORDER:
            case OrderCommandType.REDUCE_ORDER:
            case OrderCommandType.ORDER_BOOK_REQUEST:
            case OrderCommandType.PERSIST_STATE_MATCHING:
            case OrderCommandType.PERSIST_STATE_RISK:
            case OrderCommandType.GROUPING_CONTROL:
            case OrderCommandType.SHUTDOWN_SIGNAL:
            case OrderCommandType.RESERVED_COMPRESSED:
            default:
                break;
        }
    }

    public void PostProcessCommand(long seq, OrderCommand cmd)
    {
        _logger.LogDebug("Post processing command. OrderId={OrderId}", cmd.OrderId);
        var handler = new PostProcessOrderHandler(UserProfiles, SymbolSpecificationProvider);
        handler.Handle(cmd);
    }

    private void Reset()
    {
        UserProfiles.Reset();
        SymbolSpecificationProvider.Reset();
    }

    private CommandResultCode AcceptBinaryCommand(OrderCommand cmd)
    {
        if (cmd.Command is not OrderCommandType.BINARY_DATA_COMMAND)
        {
            return CommandResultCode.BINARY_COMMAND_FAILED;
        }

        var command = BinaryCommandFactory.GetBinaryCommand(cmd.BinaryCommandType, cmd.BinaryData);

        if (command == null)
        {
            return CommandResultCode.BINARY_COMMAND_FAILED;
        }

        if (command is BatchAddSymbolsCommand batchAddSymbolsCommand)
        {
            _logger.LogDebug("Saving symbols into SymbolSpecificationProvider. OrderId={OrderId}", cmd.OrderId);
            return batchAddSymbolsCommand.HandleCommand(SymbolSpecificationProvider);
        }

        if (command is BatchAddAccountsCommand batchAddAccountsCommand)
        {
            return batchAddAccountsCommand.HandleCommand(UserProfiles);
        }

        return CommandResultCode.SUCCESS;
    }

    internal CommandResultCode AcceptBinaryQuery(OrderCommand cmd)
    {
        if (cmd.Command is not OrderCommandType.BINARY_DATA_QUERY)
        {
            return CommandResultCode.BINARY_COMMAND_FAILED;
        }

        var query = BinaryQueryFactory.GetBinaryQuery(cmd.BinaryCommandType, cmd.BinaryData);

        if (query == null)
        {
            return CommandResultCode.BINARY_COMMAND_FAILED;
        }

        if (query is SingleUserReportQuery singleUserReport)
        {
            return singleUserReport.HandleQuery(cmd, UserProfiles);
        }

        return CommandResultCode.SUCCESS;
    }
}
