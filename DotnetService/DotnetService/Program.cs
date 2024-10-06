using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DotnetService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("JobExecutionContext", "AgendadorDeTarefas")
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss}][{Level:u3}][{JobExecutionContext}] {Message:lj}{NewLine}")
                .CreateLogger();

            return Host
                .CreateDefaultBuilder(args)
                //.UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Service>();
                })
                .UseConsoleLifetime() //não usar quando for Serviço do Windows ;)
                .UseSerilog(Log.Logger);
        }
    }
}
