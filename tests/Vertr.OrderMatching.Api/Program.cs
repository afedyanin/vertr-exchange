
using Vertr.OrderMatching.Api.Disruptor;
using Vertr.OrderMatching.Api.Disruptor.EventHandlers;

namespace Vertr.OrderMatching.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<IEventHandlerOne, PingEventHandlerOne>();
            builder.Services.AddSingleton<IEventHandlerTwo, PingEventHandlerTwo>();
            builder.Services.AddSingleton<IDisruptorService, DisruptorService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
