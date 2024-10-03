using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

namespace DotnetServicePython
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
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
                .ConfigureServices((context, services) =>
                {
                    services.AddHostedService<Service>();
                })
                .UseSerilog(Log.Logger);
        }
    }
}
