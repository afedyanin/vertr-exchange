using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Vertr.Terminal.Server.Awaiting;
using Vertr.Terminal.Server.BackgroundServices;
using Vertr.Terminal.Server.OrderManagement;
using Vertr.Terminal.Server.Providers;
using Vertr.Terminal.Server.Repositories;

[assembly: InternalsVisibleTo("Vertr.Terminal.Server.Tests")]
[assembly: InternalsVisibleTo("Vertr.Terminal.Server.Tests.ConsoleApp")]

namespace Vertr.Terminal.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<HubConnectionProvider>();
        builder.Services.AddSingleton<IOrderBookSnapshotsRepository, OrderBookSnapshotsRepository>();
        builder.Services.AddSingleton<ITradeEventsRepository, TradeEventsRepository>();
        builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
        builder.Services.AddTransient<IOrderEventHandler, OrderEventHandler>();
        builder.Services.AddHostedService<CommandResultService>();
        builder.Services.AddHostedService<OrderBooksService>();
        builder.Services.AddHostedService<TradeEventsService>();
        builder.Services.AddHostedService<ReduceEventsService>();
        builder.Services.AddHostedService<RejectEventsService>();
        builder.Services.AddSingleton<ICommandAwaitingService, CommandAwaitingService>();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
