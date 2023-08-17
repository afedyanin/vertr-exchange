using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.Common;
using Vertr.Exchange.RiskEngine.Abstractions;
using Vertr.Exchange.Common.Binary;
using Vertr.Exchange.Common.Abstractions;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using Vertr.Exchange.RiskEngine.Commands.Users;
using Vertr.Exchange.RiskEngine.Commands.Orders;

[assembly: InternalsVisibleTo("Vertr.Exchange.RiskEngine.Tests")]

namespace Vertr.Exchange.RiskEngine;
public class OrderRiskEngine : IOrderRiskEngine
{
    private readonly RiskEngineConfiguration _config;
    private readonly IUserProfileService _userProfileService;
    private readonly ISymbolSpecificationProvider _symbolSpecificationProvider;

    // symbol
    private readonly IDictionary<int, LastPriceCacheRecord> _lastPriceCache;

    public OrderRiskEngine(
        IOptions<RiskEngineConfiguration> configuration,
        IUserProfileService userProfileService,
        ISymbolSpecificationProvider symbolSpecificationProvider)
    {
        _config = configuration.Value;
        _userProfileService = userProfileService;
        _symbolSpecificationProvider = symbolSpecificationProvider;
        _lastPriceCache = new Dictionary<int, LastPriceCacheRecord>();
    }

    public bool PreProcessCommand(long seq, OrderCommand cmd)
    {
        switch (cmd.Command)
        {
            case OrderCommandType.PLACE_ORDER:
                var command = new PreProcessOrderCommand(
                    _userProfileService,
                    _symbolSpecificationProvider,
                    _lastPriceCache,
                    cmd,
                    _config.IgnoreRiskProcessing,
                    _config.MarginTradingEnabled);

                cmd.ResultCode = command.Execute();
                return false;

            case OrderCommandType.ADD_USER:
            case OrderCommandType.SUSPEND_USER:
            case OrderCommandType.RESUME_USER:
            case OrderCommandType.BALANCE_ADJUSTMENT:
                var userCommand = UserCommandFactory.CreateUserCommand(_userProfileService, cmd);
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
        var command = new PostProcessOrderCommand(
            _userProfileService,
            _symbolSpecificationProvider,
            _lastPriceCache,
            cmd,
            _config.MarginTradingEnabled);

        return command.Execute();
    }

    private void Reset()
    {
        _userProfileService.Reset();
        _symbolSpecificationProvider.Reset();
        _lastPriceCache.Clear();

        // fees.clear();
        // adjustments.clear();
        // suspends.clear();
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
            AddSymbols(batchAddSymbolsCommand);
        }
        else if (binCmd is BatchAddAccountsCommand batchAddAccountsCommand)
        {
            AddAccounts(batchAddAccountsCommand);
        }
    }

    private void AddSymbols(BatchAddSymbolsCommand batchAddSymbolsCommand)
    {
        var symbols = batchAddSymbolsCommand.Symbols;

        foreach (var spec in symbols)
        {
            if (spec.Type == SymbolType.CURRENCY_EXCHANGE_PAIR || _config.MarginTradingEnabled)
            {
                _symbolSpecificationProvider.AddSymbol(spec);
            }
            else
            {
                // log.warn("Margin symbols are not allowed: {}", spec);
            }
        }
    }

    private void AddAccounts(BatchAddAccountsCommand batchAddAccountsCommand)
    {
        var users = batchAddAccountsCommand.Users;

        foreach (var (uid, acounts) in users)
        {
            if (!_userProfileService.AddEmptyUserProfile(uid))
            {
                // log.debug("User already exist: {}", uid);
                continue;
            }
            foreach (var (currency, balance) in acounts)
            {
                // BalanceAdjustmentType.ADJUSTMENT
                _userProfileService.BalanceAdjustment(
                    uid,
                    currency,
                    balance,
                    1_000_000_000 + currency);
            }
        }
    }
}
