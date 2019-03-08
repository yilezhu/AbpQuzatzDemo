
using Czar.AbpDemo.JobSchedule;
using Czar.AbpDemo.Result;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Czar.AbpDemo
{
    /// <summary>
    /// 任务调度中心
    /// </summary>
    public class ScheduleCenter
    {
        private readonly ILogger<ScheduleCenter> _logger;

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

                NameValueCollection parms = new NameValueCollection
                {
                    ////scheduler名字
                    ["quartz.scheduler.instanceName"] = "TestScheduler",
                    //序列化类型
                    ["quartz.serializer.type"] = "binary",//json,切换为数据库存储的时候需要设置json
                    //自动生成scheduler实例ID，主要为了保证集群中的实例具有唯一标识
                    //["quartz.scheduler.instanceId"] = "AUTO",
                    ////是否配置集群
                    //["quartz.jobStore.clustered"] = "true",
                    ////线程池个数
                    //["quartz.threadPool.threadCount"] = "20",
                    ////类型为JobStoreXT,事务
                    //["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz",
                    ////以下配置需要数据库表配合使用，表结构sql地址：https://github.com/quartznet/quartznet/tree/master/database/tables
                    ////JobDataMap中的数据都是字符串
                    ////["quartz.jobStore.useProperties"] = "true",
                    ////数据源名称
                    //["quartz.jobStore.dataSource"] = "myDS",
                    ////数据表名前缀
                    //["quartz.jobStore.tablePrefix"] = "QRTZ_",
                    ////使用Sqlserver的Ado操作代理类
                    //["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz",
                    ////数据源连接字符串
                    //["quartz.dataSource.myDS.connectionString"] = "Server=[yourserver];Database=quartzDb;Uid=sa;Pwd=[yourpass]",
                    ////数据源的数据库
                    //["quartz.dataSource.myDS.provider"] = "SqlServer"
                };
                // 从Factory中获取Scheduler实例
                StdSchedulerFactory factory = new StdSchedulerFactory(parms);
                return await factory.GetScheduler();

            }
        }

        /// <summary>
        /// 添加调度任务
        /// </summary>
        /// <param name="JobName">任务名称</param>
        /// <param name="JobGroup">任务分组</param>
        /// <param name="JobNamespaceAndClassName">任务完全限定名</param>
        /// <param name="JobAssemblyName">任务程序集名称</param>
        /// <param name="CronExpress">Cron表达式</param>
        /// <param name="StarTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public async Task<ScheduleResult> AddJobAsync(String JobName, String JobGroup, String JobNamespaceAndClassName, String JobAssemblyName, string CronExpress, DateTime StarTime, DateTime EndTime)
        {
            ScheduleResult result = new ScheduleResult();
            try
            {
                if (string.IsNullOrEmpty(JobName) || string.IsNullOrEmpty(JobGroup) || string.IsNullOrEmpty(JobNamespaceAndClassName) || string.IsNullOrEmpty(JobAssemblyName) || string.IsNullOrEmpty(CronExpress))
                {
                    result.Code = -3;
                    result.Message = $"参数不能为空";
                    return result;//出现异常
                }
                if (StarTime == null)
                {
                    StarTime = DateTime.Now;
                }
                DateTimeOffset starRunTime = DateBuilder.NextGivenSecondDate(StarTime, 1);
                if (EndTime == null)
                {
                    EndTime = DateTime.MaxValue.AddDays(-1);
                }
                DateTimeOffset endRunTime = DateBuilder.NextGivenSecondDate(EndTime, 1);
                scheduler = await GetSchedulerAsync();
                JobKey jobKey = new JobKey(JobName, JobGroup);
                if (await scheduler.CheckExists(jobKey))
                {
                    await scheduler.PauseJob(jobKey);
                    await scheduler.DeleteJob(jobKey);
                }
                //var jobType =Type.GetType("Czar.AbpDemo.Schedule.LogTestJob,Czar.AbpDemo.Web");
                var jobType = Type.GetType(JobNamespaceAndClassName + "," + JobAssemblyName);
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
                                             .UsingJobData("ServerName", scheduler.SchedulerName)
                                             .WithIdentity(JobName, JobGroup)
                                             .WithCronSchedule(CronExpress)
                                             .Build();
                await scheduler.ScheduleJob(job, trigger);
                if (!scheduler.IsStarted)
                {
                    await scheduler.Start();
                }
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
