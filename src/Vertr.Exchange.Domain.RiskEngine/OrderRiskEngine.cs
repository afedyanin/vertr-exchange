using Vertr.Exchange.Domain.Common.Enums;
using System.Runtime.CompilerServices;
using Vertr.Exchange.Domain.RiskEngine.Orders;
using Vertr.Exchange.Domain.RiskEngine.Symbols;
using Vertr.Exchange.Domain.RiskEngine.Binary;
using Microsoft.Extensions.Logging;
using Vertr.Exchange.Domain.Common.Abstractions;
using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Domain.Accounts.UserCommands;
using Vertr.Exchange.Domain.Common.Binary.Commands;
using Vertr.Exchange.Domain.Common.Binary.Reports;

[assembly: InternalsVisibleTo("Vertr.Exchange.Domain.RiskEngine.Tests")]

namespace Vertr.Exchange.Domain.RiskEngine;
internal sealed class OrderRiskEngine(
    IUserProfileProvider userProfiles,
    ISymbolSpecificationProvider symbolSpecificationProvider,
    ILogger<OrderRiskEngine> logger) : IOrderRiskEngine
{
    private readonly ILogger<OrderRiskEngine> _logger = logger;

    public IUserProfileProvider UserProfiles { get; } = userProfiles;

    public ISymbolSpecificationProvider SymbolSpecificationProvider { get; } = symbolSpecificationProvider;

    public void PreProcessCommand(long seq, OrderCommand cmd)
    {
        if (HasErrorResult(cmd))
        {
            _logger.LogInformation("Skip command={CommandType} OrderId={OrderId}. InvalidState={ResultCode}",
                cmd.Command,
                cmd.OrderId,
                cmd.ResultCode);

            return;
        }

        switch (cmd.Command)
        {
            case OrderCommandType.PLACE_ORDER:
                _logger.LogDebug("Pre-processing PLACE_ORDER OrderId={OrderId} Uid={Uid}", cmd.OrderId, cmd.Uid);
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
                _logger.LogDebug("Processing BinaryCommand={CommandType} OrderId={OrderId}", cmd.BinaryCommandType, cmd.OrderId);
                cmd.ResultCode = AcceptBinaryCommand(cmd);
                return;

            case OrderCommandType.BINARY_DATA_QUERY:
                _logger.LogDebug("Processing BinaryQuery={CommandType} OrderId={OrderId}", cmd.BinaryCommandType, cmd.OrderId);
                cmd.ResultCode = AcceptBinaryQuery(cmd);
                return;

            case OrderCommandType.RESET:
                _logger.LogWarning("Processing RESET command OrderId={OrderId}", cmd.OrderId);
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
                _logger.LogDebug("Skip processing Command={CommandType} OrderId={OrderId}", cmd.Command, cmd.OrderId);
                break;
        }
    }

    public void PostProcessCommand(long seq, OrderCommand cmd)
    {
        if (HasErrorResult(cmd))
        {
            _logger.LogInformation("Skip command={CommandType} OrderId={OrderId}. InvalidState={ResultCode}",
                cmd.Command,
                cmd.OrderId,
                cmd.ResultCode);

            return;
        }

        _logger.LogDebug("Post processing Command={CommandType} OrderId={OrderId}", cmd.Command, cmd.OrderId);
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
            _logger.LogDebug("Adding accounts into UserProfilesProvider. OrderId={OrderId}", cmd.OrderId);
            return batchAddAccountsCommand.HandleCommand(UserProfiles);
        }

        return cmd.ResultCode;
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
            _logger.LogDebug("Populate SingleUserReportQuery result. OrderId={OrderId}", cmd.OrderId);
            return singleUserReport.HandleQuery(cmd, UserProfiles);
        }

        return cmd.ResultCode;
    }

    private bool HasErrorResult(OrderCommand cmd)
    {
        return cmd.ResultCode is
            not CommandResultCode.SUCCESS and
            not CommandResultCode.NEW;
    }
}
