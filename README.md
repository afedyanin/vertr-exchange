# vertr-exchange

# Simple NET Exchange Engine 

## Inspired by

- [LMAX Disruptor](https://github.com/LMAX-Exchange/disruptor)
- [Disruptor-net](https://github.com/disruptor-net/Disruptor-net)
- [exchange-core](https://github.com/exchange-core/exchange-core)

## Samples

### Simple console app with embedded exchange engine

[Program.cs](samples/Vertr.Exchange.ConsoleApp/Program.cs)

```csharp

    public static async Task Main()
    {
        var sp = ServicePorviderBuilder.BuildServiceProvider();
        var idGen = sp.GetRequiredService<IOrderIdGenerator>();
        var api = sp.GetRequiredService<IExchangeCommandsApi>();

        // Add Symbols
        api.Send(new AddSymbolsCommand(
            orderId: idGen.NextId,
            timestamp: DateTime.UtcNow,
            Symbols.AllSymbolSpecs));

        // Add Users and Accounts
        api.Send(new AddAccountsCommand(
            orderId: idGen.NextId,
            timestamp: DateTime.UtcNow,
            users: UserAccounts.All));

        // Place ASK order
        api.Send(new PlaceOrderCommand(
            orderId: idGen.NextId,
            timestamp: DateTime.UtcNow,
            price: 120.45m,
            size: 10,
            action: OrderAction.ASK,
            orderType: OrderType.GTC,
            uid: Users.Alice.Id,
            symbol: Symbols.MSFT.Id));

        // Place BID order
        api.Send(new PlaceOrderCommand(
            orderId: idGen.NextId,
            timestamp: DateTime.UtcNow,
            price: 123.56m,
            size: 7,
            action: OrderAction.BID,
            orderType: OrderType.GTC,
            uid: Users.Bob.Id,
            symbol: Symbols.MSFT.Id));

        // wait to end processing
        await Task.Delay(2000);
    }


```

### Console app communicating with Exchange host via SignalR

[Program.cs](samples/Vertr.Exchange.Client.SignalR.ConsoleApp/Program.cs)

```csharp

    private static async Task PlaceAsk(IExchangeApiClient api, ILogger<Program> logger)
    {
        var askOrderId = await api.GetNextOrderId();
        var askOrderResult = await api.PlaceOrder(
            new PlaceOrderRequest
            {
                OrderId = askOrderId,
                OrderType = OrderType.GTC,
                Action = OrderAction.ASK,
                UserId = Users.Alice.Id,
                Price = NextRandomPrice(123),
                Size = NextRandomQty(10),
                Symbol = Symbols.MSFT.Id
            });

        logger.LogWarning("ASK order result: {orderResult}", askOrderResult);
    }

    private static async Task PlaceBid(IExchangeApiClient api, ILogger<Program> logger)
    {
        var bidOrderId = await api.GetNextOrderId();
        var bidOrderResult = await api.PlaceOrder(
            new PlaceOrderRequest
            {
                OrderId = bidOrderId,
                OrderType = OrderType.GTC,
                Action = OrderAction.BID,
                UserId = Users.Bob.Id,
                Price = NextRandomPrice(123),
                Size = NextRandomQty(10),
                Symbol = Symbols.MSFT.Id
            });

        logger.LogWarning("BID order result: {orderResult}", bidOrderResult);
    }

```
