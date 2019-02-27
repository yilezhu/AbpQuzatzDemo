using Microsoft.Extensions.Logging;
using Quartz;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czar.AbpDemo.Schedule
{
    public class LogTestJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            using (var logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .WriteTo.File("Logs/" + DateTime.Now.ToString("yyyy-MM-dd") + "logs.txt")
                    .CreateLogger())
            {
                logger.Information(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
           
            await Task.CompletedTask;
        }
    }
}
