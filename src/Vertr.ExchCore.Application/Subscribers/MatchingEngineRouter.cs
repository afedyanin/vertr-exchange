using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Vertr.ExchCore.Application.Subscribers.Enums;
using Vertr.ExchCore.Domain.Abstractions;
using Vertr.ExchCore.Domain.Enums;
using Vertr.ExchCore.Domain.ValueObjects;

namespace Vertr.ExchCore.Application.Subscribers;

internal class MatchingEngineRouter : IOrderCommandSubscriber
{
    private readonly ILogger<MatchingEngineRouter> _logger;

    // Symbol to orderbook map
    private readonly IDictionary<int, IOrderBook> _orderBooks;

    // sharding by symbolId
    private int _shardId;
    private long _shardMask;

    // perf gfg
    private bool _cfgSendL2ForEveryCmd;
    private int _cfgL2RefreshDepth;

    public MatchingEngineRouter(
        int shardId,
        long numShards,
        ILogger<MatchingEngineRouter> logger)
    {
        _logger = logger;

        _shardId = shardId;
        _shardMask = numShards - 1;
        /*
        if (Long.bitCount(numShards) != 1)
        {
            throw new IllegalArgumentException("Invalid number of shards " + numShards + " - must be power of 2");
        }
        */

        _cfgL2RefreshDepth = 100;
        _cfgSendL2ForEveryCmd = true;
        /*
        final PerformanceConfiguration perfCfg = exchangeCfg.getPerformanceCfg();
        this.cfgSendL2ForEveryCmd = perfCfg.isSendL2ForEveryCmd();
        this.cfgL2RefreshDepth = perfCfg.getL2RefreshDepth();
         */

        // TODO: Need API to add symbol
        _orderBooks = new Dictionary<int, IOrderBook>();
    }
    public int Priority => (int)GroupPriority.Processing;

    public void HandleEvent(OrderCommand data, long sequence, bool endOfBatch)
    {
        OrderCommandType command = data.CommandType;

        if (command == OrderCommandType.MOVE_ORDER
                || command == OrderCommandType.CANCEL_ORDER
                || command == OrderCommandType.PLACE_ORDER
                || command == OrderCommandType.REDUCE_ORDER
                || command == OrderCommandType.ORDER_BOOK_REQUEST)
        {
            // process specific symbol group only
            if (SymbolForThisHandler(data.Symbol))
            {
                ProcessMatchingCommand(data);
            }
        }
        else if (command == OrderCommandType.BINARY_DATA_QUERY || command == OrderCommandType.BINARY_DATA_COMMAND)
        {
            // TODO: Implement this
            _logger.LogInformation("Order command type {CommandType} is not supported. Skiping", command);
        }
        else if (command == OrderCommandType.RESET)
        {
            // TODO: Implement this

            // process all symbols groups, only processor 0 writes result
            _orderBooks.Clear();
            // binaryCommandsProcessor.reset();
            if (_shardId == 0)
            {
                data.ResultCode = CommandResultCode.SUCCESS;
            }
        }
        else if (command == OrderCommandType.NOP)
        {
            // TODO: Implement this
            if (_shardId == 0)
            {
                data.ResultCode = CommandResultCode.SUCCESS;
            }

        }
        else if (command == OrderCommandType.PERSIST_STATE_MATCHING)
        {
            // TODO: Implement this
            _logger.LogInformation("Order command type {CommandType} is not supported. Skiping", command);
        }
    }

    private bool SymbolForThisHandler(long symbol)
    {
        return (_shardMask == 0) || ((symbol & _shardMask) == _shardId);
    }

    private void ProcessMatchingCommand(OrderCommand cmd)
    {
        // TODO: Fix it
        var orderBook = _orderBooks[cmd.Symbol];

        if (orderBook == null)
        {
            cmd.ResultCode = CommandResultCode.MATCHING_INVALID_ORDER_BOOK_ID;
        }
        else
        {
            cmd.ResultCode = orderBook.ProcessCommand(cmd);

            if ((_cfgSendL2ForEveryCmd || true) //(cmd.ServiceFlags & 1) != 0)
                    && cmd.CommandType != OrderCommandType.ORDER_BOOK_REQUEST
                    && cmd.ResultCode == CommandResultCode.SUCCESS)
            {
                cmd.L2MarketData = orderBook.GetL2MarketDataSnapshot(_cfgL2RefreshDepth);
            }
        }
    }
}
