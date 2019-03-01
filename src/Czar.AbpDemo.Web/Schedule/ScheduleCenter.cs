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
        public async Task<IScheduler> GetSchedulerAsync()
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
        public async Task<ScheduleResult> AddJobAsync(CreateUpdateJobInfoDto infoDto)
        {
            ScheduleResult result = new ScheduleResult();
            try
            {
                if (infoDto == null)
                {
                    result.Code = -3;
                    result.Message = $"参数{typeof(CreateUpdateJobInfoDto)}不能为空";
                    return result;//出现异常
                }

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
                //var jobType =Type.GetType("Czar.AbpDemo.Schedule.LogTestJob,Czar.AbpDemo.Web");
                var jobType = Type.GetType(infoDto.JobNamespace + "." + infoDto.JobClassName + "," + infoDto.JobAssemblyName);
                if (jobType == null)
                {
                    result.Code = -1;
                    result.Message = "系统找不到对应的任务，请重新设置";
                    return result;//出现异常
                }
                IJobDetail job = JobBuilder.Create(jobType)
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
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                result.Code = -4;
                result.Message = ex.ToString();
                return result;//出现异常
            }
        }

        /// <summary>
        /// 暂停指定任务计划
        /// </summary>
        /// <param name="jobName">任务名</param>
        /// <param name="jobGroup">任务分组</param>
        /// <returns></returns>
        public async Task<ScheduleResult> StopJobAsync(string jobName, string jobGroup)
        {
            ScheduleResult result = new ScheduleResult();
            try
            {
                JobKey jobKey = new JobKey(jobName, jobGroup);
                scheduler = await GetSchedulerAsync();
                if (await scheduler.CheckExists(jobKey))
                {
                    await scheduler.PauseJob(new JobKey(jobName, jobGroup));

                }
                else
                {
                    result.Code = -1;
                    result.Message = "任务不存在";
                }

            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                result.Code = -4;
                result.Message = ex.ToString();

            }
            return result;//出现异常
        }

        /// <summary>
        /// 恢复指定的任务计划,如果是程序奔溃后 或者是进程杀死后的恢复，此方法无效
        /// </summary>
        /// <param name="jobName">任务名称</param>
        /// <param name="jobGroup">任务组</param>
        /// <returns></returns>
        public async Task<ScheduleResult> ResumeJobAsync(string jobName, string jobGroup)
        {
            ScheduleResult result = new ScheduleResult();
            try
            {
                JobKey jobKey = new JobKey(jobName, jobGroup);
                scheduler = await GetSchedulerAsync();
                if (await scheduler.CheckExists(jobKey))
                {
                    //resumejob 恢复
                    await scheduler.PauseJob(jobKey);
                    await scheduler.ResumeJob(jobKey);
                }
                else
                {
                    result.Code = -1;
                    result.Message = "任务不存在";
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                result.Code = -4;
                result.Message = ex.ToString();
            }
            return result;
        }

        /// <summary>
        /// 删除指定的任务
        /// </summary>
        /// <param name="jobName">任务名称</param>
        /// <param name="jobGroup">任务组</param>
        /// <returns></returns>
        public async Task<ScheduleResult> DeleteJobAsync(string jobName, string jobGroup)
        {
            ScheduleResult result = new ScheduleResult();
            try
            {
                JobKey jobKey = new JobKey(jobName, jobGroup);
                scheduler = await GetSchedulerAsync();
                if (await scheduler.CheckExists(jobKey))
                {
                    //先暂停，再移除
                    await scheduler.PauseJob(jobKey);
                    await scheduler.DeleteJob(jobKey);
                }
                else
                {
                    result.Code = -1;
                    result.Message = "任务不存在";
                }

            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                result.Code = -4;
                result.Message = ex.ToString();
            }
            return result;
        }
    }
}
