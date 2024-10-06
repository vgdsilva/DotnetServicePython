using DotnetServiceTasks.Scheduler.Jobs;
using Quartz;
using Quartz.Impl;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetServiceTasks.Scheduler
{
    public sealed class Scheduler
    {
        public static ILogger Logger;
        private static readonly Scheduler instance = new Scheduler();

        static Scheduler() { }

        private Scheduler() { }

        public static Scheduler Instance
        {
            get { return instance; }
        }

        private IScheduler _Scheduler { get; set; }
        public IScheduler SchedulerSingleton
        {
            get
            {
                if (_Scheduler == null)
                    _Scheduler = new StdSchedulerFactory().GetScheduler().Result;
                return _Scheduler;
            }
        }

        public bool EstaIniciado => _Scheduler.IsStarted;
        public bool EstaParado => _Scheduler.IsShutdown;


        async Task ScheduleTasks()
        {
            try
            {
                IJobDetail job = JobBuilder.Create()
                    .OfType(typeof(GeneratePDFJob))   // Define o tipo do Job
                    .WithIdentity("GeneratePDFJob")   // Identidade única do Job
                    .WithDescription("")              // Descrição opcional do Job
                    .Build();                         // Constroi o Job

                ITrigger trigger = TriggerBuilder.Create()
                    .ForJob(job)                      // Associa o Trigger ao Job
                    .StartNow()                       // Inicia o Job imediatamente
                    .WithSimpleSchedule(x => x        // Define uma agenda simples
                        .WithIntervalInMinutes(1)     // Intervalo de 1 minuto entre as execuções
                        .RepeatForever())             // Repete indefinidamente
                    .Build();                         // Constroi o Trigger

                await SchedulerSingleton.ScheduleJob(job, trigger);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
            }
        }


        public async Task Start()
        {
            await SchedulerSingleton.Start();

            await ScheduleTasks();
        }

        public async Task Stop()
        {
            if (!EstaParado)
            {
                await SchedulerSingleton.Shutdown();
            }
        }
    }
}
