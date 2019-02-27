/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：yilezhu                                             
*│　版    本：1.0                                                 
*│　创建时间：2019/2/26 9:06:15                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： Czar.AbpDemo.JobSchedule                                   
*│　类    名： JobInfoDto                                      
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AutoMapper;

namespace Czar.AbpDemo.JobSchedule
{
    [AutoMapFrom(typeof(JobInfo))]
    public class JobInfoDto: AuditedEntityDto<int>
    {

        public string JobGroup { get; set; }

        public string JobDescription { get; set; }

        public string JobName { get; set; }

        public JobStatu JobStatus { get; set; } 

        public string CronExpress { get; set; }

        public DateTime StarTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime NextTime { get; set; }
    }
}
