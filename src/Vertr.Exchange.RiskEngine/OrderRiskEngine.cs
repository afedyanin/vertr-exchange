using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.Common;
using Vertr.Exchange.RiskEngine.Abstractions;
using Vertr.Exchange.Common.Binary;
using Vertr.Exchange.Common.Abstractions;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using Vertr.Exchange.RiskEngine.Users.UserCommands;
using Vertr.Exchange.RiskEngine.Orders;

[assembly: InternalsVisibleTo("Vertr.Exchange.RiskEngine.Tests")]

namespace Vertr.Exchange.RiskEngine;
internal sealed class OrderRiskEngine : IOrderRiskEngine, IOrderRiskEngineInternal
{
    private readonly RiskEngineConfiguration _config;

    public bool IsMarginTradingEnabled => _config.MarginTradingEnabled;

    public bool IgnoreRiskProcessing => _config.IgnoreRiskProcessing;

    public IUserProfileService UserProfileService { get; }

    public ISymbolSpecificationProvider SymbolSpecificationProvider { get; }

    public IFeeCalculationService FeeCalculationService { get; }

    public ILastPriceCacheProvider LastPriceCacheProvider { get; }

    public OrderRiskEngine(
        IOptions<RiskEngineConfiguration> configuration,
        IUserProfileService userProfileService,
        ISymbolSpecificationProvider symbolSpecificationProvider,
        IFeeCalculationService feeCalculationService,
        ILastPriceCacheProvider lastPriceCacheProvider)
    {
        _config = configuration.Value;
        UserProfileService = userProfileService;
        SymbolSpecificationProvider = symbolSpecificationProvider;
        FeeCalculationService = feeCalculationService;
        LastPriceCacheProvider = lastPriceCacheProvider;
    }

    public bool PreProcessCommand(long seq, OrderCommand cmd)
    {
        switch (cmd.Command)
        {
            case OrderCommandType.PLACE_ORDER:
                var command = new PreProcessOrderCommand(this, cmd);
                cmd.ResultCode = command.Execute();
                return false;

            case OrderCommandType.ADD_USER:
            case OrderCommandType.SUSPEND_USER:
            case OrderCommandType.RESUME_USER:
            case OrderCommandType.BALANCE_ADJUSTMENT:
                var userCommand = UserCommandFactory.CreateUserCommand(this, cmd);
                cmd.ResultCode = userCommand.Execute();
                return false;

            case OrderCommandType.BINARY_DATA_COMMAND:
            case OrderCommandType.BINARY_DATA_QUERY:
                // ignore return result, because it should be set by MatchingEngineRouter
                var _ = AcceptBinaryCommand(cmd);
                cmd.ResultCode = CommandResultCode.VALID_FOR_MATCHING_ENGINE;
                return false;

            case OrderCommandType.RESET:
                Reset();
                cmd.ResultCode = CommandResultCode.SUCCESS;
                return false;

            case OrderCommandType.MOVE_ORDER:
            case OrderCommandType.CANCEL_ORDER:
            case OrderCommandType.REDUCE_ORDER:
            case OrderCommandType.ORDER_BOOK_REQUEST:
            case OrderCommandType.PERSIST_STATE_MATCHING:
            case OrderCommandType.PERSIST_STATE_RISK:
            case OrderCommandType.GROUPING_CONTROL:
            case OrderCommandType.NOP:
            case OrderCommandType.SHUTDOWN_SIGNAL:
            case OrderCommandType.RESERVED_COMPRESSED:
            default:
                break;
        }
        return false;
    }

    public bool PostProcessCommand(long seq, OrderCommand cmd)
    {
        var command = new PostProcessOrderCommand(this, cmd);

        // TODO: refactor this
        return command.Execute() != CommandResultCode.SUCCESS;
    }

    private void Reset()
    {
        UserProfileService.Reset();
        SymbolSpecificationProvider.Reset();
        FeeCalculationService.Reset();
        LastPriceCacheProvider.Reset();
    }

    private CommandResultCode AcceptBinaryCommand(OrderCommand cmd)
    {
        if (cmd.Command is OrderCommandType.BINARY_DATA_COMMAND)
        {
            var command = BinaryCommandFactory.GetBinaryCommand(cmd.BinaryCommandType, cmd.BinaryData);

            if (command != null)
            {
                HandleBinaryCommand(command);
            }
        }

        return CommandResultCode.SUCCESS;
    }

    private void HandleBinaryCommand(IBinaryCommand binCmd)
    {
        if (binCmd is BatchAddSymbolsCommand batchAddSymbolsCommand)
        {
            SymbolSpecificationProvider.AddSymbols(batchAddSymbolsCommand.Symbols, IsMarginTradingEnabled);
        }
        else if (binCmd is BatchAddAccountsCommand batchAddAccountsCommand)
        {
            UserProfileService.BatchAddAccounts(batchAddAccountsCommand.Users);
        }
    }
}
