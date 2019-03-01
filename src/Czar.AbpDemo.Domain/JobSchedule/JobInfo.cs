/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：yilezhu                                             
*│　版    本：1.0                                                 
*│　创建时间：2019/2/25 16:57:54                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： Czar.AbpDemo.TaskSchedule                                   
*│　类    名： TaskInfo                                      
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace Czar.AbpDemo.JobSchedule
{
    [Table("TaskInfo")]
    public class JobInfo : AuditedAggregateRoot<int>
    {
        [Required, StringLength(128), Display(Name = "任务组")]
        public string JobGroup { get; set; }
        [Required, StringLength(64), Display(Name = "任务描述")]
        public string JobDescription { get; set; }
        [Required, StringLength(64), Display(Name = "任务名")]
        public string JobName { get; set; }
        [Required, StringLength(256), Display(Name = "程序集名称")]
        public string JobAssemblyName { get; set; }
        [Required, StringLength(256), Display(Name = "命名空间")]
        public string JobNamespace { get; set; }
        [Required, StringLength(128), Display(Name = "任务类名")]
        public string JobClassName { get; set; }
        [Required, Display(Name = "运行状态")]
        public JobStatu JobStatus { get; set; } = JobStatu.Stopped;
        [Required, StringLength(64), Display(Name = "Cron表达式")]
        public string CronExpress { get; set; }
        [Display(Name = "开始时间")]
        public DateTime StarTime { get; set; }
        [Display(Name = "结束时间")]
        public DateTime EndTime { get; set; }
        [Display(Name = "下次运行时间")]
        public DateTime NextTime { get; set; }
    }
}
