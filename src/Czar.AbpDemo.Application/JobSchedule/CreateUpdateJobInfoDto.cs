/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：创建和更新任务的时候被使用,用来从页面获取任务信息                                                    
*│　作    者：yilezhu                                             
*│　版    本：1.0                                                 
*│　创建时间：2019/2/26 14:24:33                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： Czar.AbpDemo.JobSchedule                                   
*│　类    名： CreateUpdateJobInfoDto                                      
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Czar.AbpDemo.JobSchedule
{
    [AutoMapTo(typeof(JobInfo))]
    [AutoMapFrom(typeof(JobInfoDto))]
    public class CreateUpdateJobInfoDto
    {
        [Required, StringLength(128)]
        public string JobGroup { get; set; }
        [Required, StringLength(64)]
        public string JobDescription { get; set; }
        [Required, StringLength(64)]
        public string JobName { get; set; }
        [Required, StringLength(256)]
        public string JobAssemblyName { get; set; }
        [Required, StringLength(256)]
        public string JobNamespace { get; set; }
        [Required, StringLength(128)]
        public string JobClassName { get; set; }
        [Required]
        public JobStatu JobStatus { get; set; } = JobStatu.Stopped;
        [Required, StringLength(64)]
        public string CronExpress { get; set; }

        public DateTime StarTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime NextTime { get; set; }
    }
}
