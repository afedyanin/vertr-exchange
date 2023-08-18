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
internal sealed class OrderRiskEngine : IOrderRiskEngine
{
    private readonly RiskEngineConfiguration _config;
    private readonly IUserProfileService _userProfileService;
    private readonly ISymbolSpecificationProvider _symbolSpecificationProvider;

    // symbol
    private readonly IDictionary<int, LastPriceCacheRecord> _lastPriceCache;

    // currency
    private readonly IDictionary<int, decimal> _fees;

    // currency
    private readonly IDictionary<int, decimal> _adjustments;

    // currency
    private readonly IDictionary<int, decimal> _suspends;

    public bool IsMarginTradingEnabled => _config.MarginTradingEnabled;

    public bool IgnoreRiskProcessing => _config.IgnoreRiskProcessing;

    public OrderRiskEngine(
        IOptions<RiskEngineConfiguration> configuration,
        IUserProfileService userProfileService,
        ISymbolSpecificationProvider symbolSpecificationProvider)
    {
        _config = configuration.Value;
        _userProfileService = userProfileService;
        _symbolSpecificationProvider = symbolSpecificationProvider;
        _lastPriceCache = new Dictionary<int, LastPriceCacheRecord>();
        _adjustments = new Dictionary<int, decimal>();
        _fees = new Dictionary<int, decimal>();
        _suspends = new Dictionary<int, decimal>();
    }

    public bool PreProcessCommand(long seq, OrderCommand cmd)
    {
        switch (cmd.Command)
        {
            case OrderCommandType.PLACE_ORDER:
                var command = new PreProcessOrderCommand(
                    _userProfileService,
                    _symbolSpecificationProvider,
                    this,
                    cmd);

                cmd.ResultCode = command.Execute();
                return false;

            case OrderCommandType.ADD_USER:
            case OrderCommandType.SUSPEND_USER:
            case OrderCommandType.RESUME_USER:
            case OrderCommandType.BALANCE_ADJUSTMENT:
                var userCommand = UserCommandFactory.CreateUserCommand(_userProfileService, this, cmd);
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
            this,
            cmd);

        return command.Execute();
    }

    private void Reset()
    {
        _userProfileService.Reset();
        _symbolSpecificationProvider.Reset();
        _lastPriceCache.Clear();
        _fees.Clear();
        _adjustments.Clear();
        _suspends.Clear();
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
                _userProfileService.BalanceAdjustment(
                    uid,
                    currency,
                    balance,
                    1_000_000_000 + currency);

                AdjustBalance(currency, balance, BalanceAdjustmentType.ADJUSTMENT);
            }
        }
    }

    public void AdjustBalance(int currency, decimal amount, BalanceAdjustmentType adjustmentType)
    {
        switch (adjustmentType)
        {
            case BalanceAdjustmentType.ADJUSTMENT:
                AddToValue(_adjustments, currency, -amount);
                break;

            case BalanceAdjustmentType.SUSPEND:
                AddToValue(_suspends, currency, -amount);
                break;
            default:
                break;
        }
    }

    public decimal AddFeeValue(int currency, decimal toBeAdded)
        => AddToValue(_fees, currency, toBeAdded);

    public void AddLastPriceCache(int symbol, decimal askPrice, decimal bidPrice)
    {
        if (_lastPriceCache.ContainsKey(symbol))
        {
            _lastPriceCache[symbol] = new LastPriceCacheRecord(askPrice, bidPrice);
        }
        else
        {
            _lastPriceCache.Add(symbol, new LastPriceCacheRecord(askPrice, bidPrice));
        }
    }

    public LastPriceCacheRecord? GetLastPriceCache(int symbol)
    {
        _lastPriceCache.TryGetValue(symbol, out var priceCache);
        return priceCache;
    }

    private static decimal AddToValue(IDictionary<int, decimal> dict, int key, decimal toBeAdded)
    {
        if (!dict.ContainsKey(key))
        {
            dict.Add(key, toBeAdded);
            return toBeAdded;
        }
        else
        {
            dict[key] += toBeAdded;
            return dict[key];
        }
    }
}
