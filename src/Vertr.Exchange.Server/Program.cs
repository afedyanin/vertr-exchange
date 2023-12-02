using Vertr.Exchange.Api;
using Vertr.Exchange.Core;
using Vertr.Exchange.RiskEngine;
using Vertr.Exchange.Accounts;
using Vertr.Exchange.MatchingEngine;
using Vertr.Exchange.Server.MessageHandlers;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Server.Hubs;
using Microsoft.AspNetCore.Http.Connections;

namespace Vertr.Exchange.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

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

        builder.Services.AddSingleton<ObservableMessageHandler>();
        builder.Services.AddSingleton<IObservableMessageHandler>(
            x => x.GetRequiredService<ObservableMessageHandler>());
        builder.Services.AddSingleton<IMessageHandler>(
            x => x.GetRequiredService<ObservableMessageHandler>());

        var app = builder.Build();

        app.UseFileServer();

        app.UseRouting();

        app.MapHub<ExchangeApiHub>("/exchange",
            options =>
            {
                options.Transports = HttpTransportType.WebSockets;
            });

        app.Run();
    }
}
