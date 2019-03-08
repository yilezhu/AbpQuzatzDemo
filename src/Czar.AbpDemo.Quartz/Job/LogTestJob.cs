
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
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            string serverName = dataMap.GetString("ServerName");
            if (string.IsNullOrEmpty(serverName))
            {
                serverName = "kong";
            }
            Log.Information(serverName);
            await Task.CompletedTask;
        }

        public async Task Test()
        {
            Thread.Sleep(10000);
            await Task.CompletedTask;

        }
    }
}
