using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetServicePython
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


    }
}
