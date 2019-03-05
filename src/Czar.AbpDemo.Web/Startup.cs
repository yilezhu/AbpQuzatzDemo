using System;
using System.Linq;
using Czar.AbpDemo.JobSchedule;
using Czar.AbpDemo.Schedule;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Threading;

namespace Czar.AbpDemo
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            services.AddApplication<AbpDemoWebModule>(options =>
            {
                options.UseAutofac();
            });

            return services.BuildServiceProviderFromFactory();
        }

        public void Configure(IApplicationBuilder app
            , ILoggerFactory loggerFactory
            , IApplicationLifetime applicationLifetime
            )
        {
            loggerFactory
                .AddSerilog(new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .WriteTo.File("Logs/"+DateTime.Now.ToString("yyyy-MM-dd")+"Systemlogs.txt")
                    .CreateLogger()
                );

            var jobInfoAppService = app.ApplicationServices.GetRequiredService<IJobInfoAppService>();
            var scheduleCenter = app.ApplicationServices.GetRequiredService<ScheduleCenter>();
            applicationLifetime.ApplicationStarted.Register(async () =>
            {
                var list = await jobInfoAppService.GetListByJobStatuAsync(JobStatu.SystemStopped);
                if (list?.Count() > 0)
                {
                    list.ForEach(async x =>
                    {
                        await scheduleCenter.AddJobAsync(x);
                    });
                    await jobInfoAppService.ResumeSystemStoppedAsync();
                }

            });
            applicationLifetime.ApplicationStopped.Register(async () =>
            {
                var list = await jobInfoAppService.GetListByJobStatuAsync(JobStatu.Running);
                if (list?.Count() > 0)
                {
                    list.ForEach(async x =>
                    {
                        await scheduleCenter.DeleteJobAsync(x.JobName, x.JobGroup);
                    });
                    await jobInfoAppService.SystemStoppedAsync();
                }


            });
            app.InitializeApplication();
        }
    }
}
