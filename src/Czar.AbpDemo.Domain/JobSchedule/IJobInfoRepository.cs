/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：扩展默认仓储方法                                                    
*│　作    者：yilezhu                                             
*│　版    本：1.0                                                 
*│　创建时间：2019/3/1 16:50:20                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： Czar.AbpDemo.JobSchedule                                   
*│　类    名： IJobInfoRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Czar.AbpDemo.JobSchedule
{
    public interface IJobInfoRepository : IRepository<JobInfo, int>
    {
        /// <summary>
        /// 应用程序停止时暂停所有任务
        /// </summary>
        /// <returns></returns>
        Task<bool> SystemStoppedAsync();

        /// <summary>
        /// 应用程序启动时启动所有任务
        /// </summary>
        /// <returns></returns>
        Task<bool> ResumeSystemStoppedAsync();

        /// <summary>
        /// 根据状态获取所有的任务列表
        /// </summary>
        /// <param name="jobStatu"></param>
        /// <returns></returns>
        Task<List<JobInfo>> GetListByJobStatuAsync(JobStatu jobStatu);


    }
}
