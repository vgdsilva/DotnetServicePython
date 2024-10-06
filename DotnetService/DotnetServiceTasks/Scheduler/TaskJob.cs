using Quartz;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetServiceTasks.Scheduler
{
    public abstract class TaskJob : IJob
    {
        public static Stopwatch cronometro;

        public TaskJob()
        {
            cronometro = cronometro ?? new Stopwatch();
        }

        ~TaskJob() { }

        protected void BeforeExecute(IJobExecutionContext context)
        {
            cronometro.Reset();
            Log.Logger.Information($"Início");
            cronometro.Start();
        }

        protected void AfterExecute(IJobExecutionContext context)
        {
            
        }

        protected async Task Success(IJobExecutionContext context)
        {
            cronometro?.Stop();
            Log.Logger.Information($"SUCCESS! (em {cronometro.ElapsedMilliseconds}ms)");
        }

        protected async Task Error(IJobExecutionContext context, Exception ex)
        {
            cronometro?.Stop();
            Log.Logger.Error($"ERRO! {ex.Message} (em {cronometro.ElapsedMilliseconds}ms)");
        }

        public abstract Task MetodoDaTarefa(IJobExecutionContext context);

        public async Task Execute(IJobExecutionContext context)
        {
            using (Serilog.Context.LogContext.PushProperty("JobExecutionContext", context.JobDetail.Key.Name))
            {
                BeforeExecute(context);
                try
                {
                    await MetodoDaTarefa(context);

                    await Success(context);
                }
                catch (Exception ex)
                {
                    await Error(context, ex);
                }
                finally
                {
                    AfterExecute(context);
                }
            }
        }
    }
}
