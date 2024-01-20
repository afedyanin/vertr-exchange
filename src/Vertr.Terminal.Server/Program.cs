using System.Text.Json.Serialization;

using Vertr.Terminal.Domain;
using Vertr.Terminal.ExchangeClient;
using Vertr.Terminal.DataAccess.InMemory;
using Vertr.Terminal.Application.Commands;

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

        builder.Services.AddExchangeApi(builder.Configuration);
        builder.Services.AddDataAccess();
        builder.Services.AddDomainServices();
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ResetRequest>());

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
