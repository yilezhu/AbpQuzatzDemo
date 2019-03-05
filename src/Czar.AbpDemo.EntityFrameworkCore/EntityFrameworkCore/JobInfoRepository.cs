/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：自定义仓储实现                                                    
*│　作    者：yilezhu                                             
*│　版    本：1.0                                                 
*│　创建时间：2019/3/1 17:12:39                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： Czar.AbpDemo.EntityFrameworkCore                                   
*│　类    名： JobInfoRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using Czar.AbpDemo.JobSchedule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace Czar.AbpDemo.EntityFrameworkCore
{
    public class JobInfoRepository : EfCoreRepository<AbpDemoDbContext, JobInfo, int>, IJobInfoRepository
    {
        public JobInfoRepository(IDbContextProvider<AbpDemoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<JobInfo>> GetListByJobStatuAsync(JobStatu jobStatu)
        {
            return await DbContext.JobInfos.Where(p=>p.JobStatus== jobStatu).ToListAsync();
        }

        public async Task<bool> ResumeSystemStoppedAsync()
        {
            return await DbContext.JobInfos.Where(p => p.JobStatus == JobStatu.SystemStopped).UpdateAsync(x => new JobInfo { JobStatus = JobStatu.Running }) > 0;

        }

        public async Task<bool> SystemStoppedAsync()
        {
            return await DbContext.JobInfos.Where(p=>p.JobStatus==JobStatu.Running).UpdateAsync(x=>new JobInfo { JobStatus=JobStatu.SystemStopped})>0 ;
        }
    }
}
