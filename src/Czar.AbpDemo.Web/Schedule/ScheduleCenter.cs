using Czar.AbpDemo.JobSchedule;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Czar.AbpDemo.Schedule
{
    /// <summary>
    /// 任务调度中心
    /// </summary>
    public class ScheduleCenter
    {
        private readonly ILogger _logger;
        public ScheduleCenter(ILogger<ScheduleCenter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 任务计划
        /// </summary>
        public IScheduler scheduler = null;
        public  async Task<IScheduler> GetSchedulerAsync()
        {
            if (scheduler != null)
            {
                return scheduler;
            }
            else
            {
                // 从Factory中获取Scheduler实例
                NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" },
                    //以下配置需要数据库表配合使用，表结构sql地址：https://github.com/quartznet/quartznet/tree/master/database/tables
                    //{ "quartz.jobStore.type","Quartz.Impl.AdoJobStore.JobStoreTX, Quartz"},
                    //{ "quartz.jobStore.driverDelegateType","Quartz.Impl.AdoJobStore.StdAdoDelegate, Quartz"},
                    //{ "quartz.jobStore.tablePrefix","QRTZ_"},
                    //{ "quartz.jobStore.dataSource","myDS"},
                    //{ "quartz.dataSource.myDS.connectionString",AppSettingHelper.MysqlConnection},//连接字符串
                    //{ "quartz.dataSource.myDS.provider","MySql"},
                    //{ "quartz.jobStore.usePropert ies","true"}

                };
                StdSchedulerFactory factory = new StdSchedulerFactory(props);
                return await factory.GetScheduler();

            }
        }
        
        /// <summary>
        /// 添加调度任务
        /// </summary>
        /// <param name="jobName">任务名称</param>
        /// <param name="jobGroup">任务分组</param>
        /// <returns></returns>
        public async Task<bool> AddJobAsync(CreateUpdateJobInfoDto infoDto)
        {
            try
            {
                if (infoDto!=null)
                {
                    if (infoDto.StarTime == null)
                    {
                        infoDto.StarTime = DateTime.Now;
                    }
                    DateTimeOffset starRunTime = DateBuilder.NextGivenSecondDate(infoDto.StarTime, 1);
                    if (infoDto.EndTime == null)
                    {
                        infoDto.EndTime = DateTime.MaxValue.AddDays(-1);
                    }
                    DateTimeOffset endRunTime = DateBuilder.NextGivenSecondDate(infoDto.EndTime, 1);
                    scheduler = await GetSchedulerAsync();
                    JobKey jobKey = new JobKey(infoDto.JobName, infoDto.JobGroup);
                    if (await scheduler.CheckExists(jobKey))
                    {
                        await scheduler.PauseJob(jobKey);
                        await scheduler.DeleteJob(jobKey);
                    }
                    IJobDetail job = JobBuilder.Create<LogTestJob>()
                      .WithIdentity(jobKey)
                      .Build();
                    ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                                                 .StartAt(starRunTime)
                                                 .EndAt(endRunTime)
                                                 .WithIdentity(infoDto.JobName, infoDto.JobGroup)
                                                 .WithCronSchedule(infoDto.CronExpress)
                                                 .Build();
                    await scheduler.ScheduleJob(job, trigger);
                    await scheduler.Start();
                    return true;
                }

                return false;//JobInfo为空
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                return false;//出现异常
            }
        }

        /// <summary>
        /// 暂停指定任务计划
        /// </summary>
        /// <param name="jobName">任务名</param>
        /// <param name="jobGroup">任务分组</param>
        /// <returns></returns>
        public async Task<bool> StopJobAsync(string jobName, string jobGroup)
        {
            try
            {
                JobKey jobKey = new JobKey(jobName, jobGroup);
                scheduler = await GetSchedulerAsync();
                if (await scheduler.CheckExists(jobKey))
                {
                    await scheduler.PauseJob(new JobKey(jobName, jobGroup));
                    return true;
                }
                else
                {
                    return false;//任务不存在
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                return false;//出现异常
            }
        }

        /// <summary>
        /// 恢复指定的任务计划,如果是程序奔溃后 或者是进程杀死后的恢复，此方法无效
        /// </summary>
        /// <param name="jobName">任务名称</param>
        /// <param name="jobGroup">任务组</param>
        /// <returns></returns>
        public async Task<bool> ResumeJobAsync(string jobName, string jobGroup)
        {
            try
            {
                JobKey jobKey = new JobKey(jobName, jobGroup);
                scheduler = await GetSchedulerAsync();
                if (await scheduler.CheckExists(jobKey))
                {
                    //resumejob 恢复
                    await scheduler.ResumeJob(new JobKey(jobName, jobGroup));
                    return true;
                }
                else
                {
                    return false;//不存在任务
                }
              
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                return false;//出现异常
            }
        }

        /// <summary>
        /// 恢复指定的任务计划,如果是程序奔溃后 或者是进程杀死后的恢复，此方法无效
        /// </summary>
        /// <param name="jobName">任务名称</param>
        /// <param name="jobGroup">任务组</param>
        /// <returns></returns>
        public async Task<bool> DeleteJobAsync(string jobName, string jobGroup)
        {
            try
            {
                JobKey jobKey = new JobKey(jobName, jobGroup);
                scheduler = await GetSchedulerAsync();
                if (await scheduler.CheckExists(jobKey))
                {
                    //DeleteJob 恢复
                    await scheduler.DeleteJob(jobKey);
                    return true;
                }
                else
                {
                    return false;//不存在任务
                }

            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                return false;//出现异常
            }
        }
    }
}
