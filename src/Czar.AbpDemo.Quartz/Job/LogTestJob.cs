
using Quartz;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Czar.AbpDemo.Job
{
    public class LogTestJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.Trigger.JobDataMap;
            string serverName = dataMap.GetString("ServerName");
            using (var logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .WriteTo.File("Logs/" + DateTime.Now.ToString("yyyy-MM-dd") + "logs.txt")
                    .CreateLogger())
            {
                if (string.IsNullOrEmpty(serverName)) serverName = "kong";
                logger.Information(DateTime.Now.ToString($"yyyy-MM-dd HH:mm:ss {serverName}"));
                //AsyncHelper.RunSync(Test);
                //logger.Information(DateTime.Now.ToString($"yyyy-MM-dd HH:mm:ss  {serverName}"));
            }

            await Task.CompletedTask;
        }

        public async Task Test()
        {
            Thread.Sleep(10000);
            await Task.CompletedTask;

        }
    }
}
