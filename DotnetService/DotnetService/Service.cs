using DotnetServiceTasks.Scheduler;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetService
{
    public class Service : BackgroundService
    {
        private readonly ILogger<Service> _log;
        private readonly IHostApplicationLifetime _applicationLifetime;
        public Service(ILogger<Service> log, IHostApplicationLifetime applicationLifetime)
        {
            _log = log;
            _applicationLifetime = applicationLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) { }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await base.StartAsync(cancellationToken);

                await Scheduler.Instance.Start();

                //await Agendador.Agendador.Instance.Iniciar();
            }
            catch (Exception ex) when (/*ex is Npgsql.PostgresException ||*/ ex is System.Net.Sockets.SocketException)
            {
                _log.LogCritical("Não foi possível acessar o banco de dados com os dados informados no arquivo Start.ini.");
                _log.LogCritical($"Motivo: {ex.Message}");
                _log.LogCritical("Revise as informações de acesso ao banco de dados no arquivo Start.ini e tente novamente.");

                //Environment.Exit(1);
                _applicationLifetime.StopApplication();
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            //await Agendador.Agendador.Instance.Parar();

            await Scheduler.Instance.Stop();

            await base.StopAsync(cancellationToken);
        }
    }
}
