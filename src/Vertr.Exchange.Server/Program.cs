using Vertr.Exchange.Application;
using Vertr.Exchange.Domain.RiskEngine;
using Vertr.Exchange.Domain.Accounts;
using Vertr.Exchange.Domain.MatchingEngine;
using Microsoft.AspNetCore.Http.Connections;
using Vertr.Exchange.Adapters.SignalR;
using Vertr.Exchange.Adapters.SignalR.Hubs;

namespace Vertr.Exchange.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSignalR(hubOptions =>
        {
            hubOptions.EnableDetailedErrors = true;
            // https://stackoverflow.com/questions/76371471/signalr-timeoutexception-every-30-seconds
            hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(15);
        }).AddMessagePackProtocol();

        builder.Services.AddExchangeCommandsApi();
        builder.Services.AddExchangeSignalrAdapter();
        builder.Services.AddAccounts();
        builder.Services.AddRiskEngine();
        builder.Services.AddMatchingEngine();


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
