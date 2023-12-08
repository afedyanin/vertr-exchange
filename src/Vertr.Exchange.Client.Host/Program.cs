using System.Text.Json.Serialization;
using Vertr.Exchange.Client.Host.Awaiting;
using Vertr.Exchange.Client.Host.BackgroundServices;
using Vertr.Exchange.Client.Host.Providers;

namespace Vertr.Exchange.Client.Host;

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
        builder.Services.AddHostedService<CommandResultService>();
        builder.Services.AddSingleton<ICommandAwaitingService, CommandAwaitingService>();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
