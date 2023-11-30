using Vertr.Exchange.Api;
using Vertr.Exchange.Core;
using Vertr.Exchange.RiskEngine;
using Vertr.Exchange.Accounts;
using Vertr.Exchange.MatchingEngine;
using Vertr.Exchange.Server.MessageHandlers;
using Vertr.Exchange.Server.Services;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Server.Hubs;
using Microsoft.AspNetCore.Http.Connections;

namespace Vertr.Exchange.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // gRPC
        builder.Services.AddGrpc();
        builder.Services.AddGrpcReflection();

        // SignalR
        builder.Services.AddSignalR(hubOptions =>
        {
            hubOptions.EnableDetailedErrors = true;
            hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(1);
        }).AddMessagePackProtocol();

        builder.Services.AddExchangeApi();
        builder.Services.AddExchangeCore();
        builder.Services.AddAccounts();
        builder.Services.AddRiskEngine();
        builder.Services.AddMatchingEngine();

        // builder.Services.AddSingleton<IMessageHandler, LogMessageHandler>();
        builder.Services.AddSingleton<IObservableMessageHandler, ObservableMessageHandler>();

        var app = builder.Build();

        app.UseFileServer();

        app.UseRouting();

        app.MapHub<MarketDataHub>("/market-data",
            options =>
            {
                options.Transports = HttpTransportType.WebSockets;
            });

        // Configure the HTTP request pipeline.
        app.MapGrpcReflectionService();
        app.MapGrpcService<ExchangeApiService>();
        // app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}
