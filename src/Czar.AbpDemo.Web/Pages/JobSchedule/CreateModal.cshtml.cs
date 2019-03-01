using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czar.AbpDemo.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Czar.AbpDemo.JobSchedule;
using Czar.AbpDemo.Schedule;

namespace Czar.AbpDemo.Web.Pages.JobSchedule
{
    /// <summary>
    /// 继承了 AbpDemoPageModelBase 而非默认的 PageModel. 
    /// AbpDemoPageModelBase 继承了 PageModel 并且添加了一些Razor页面模型通用的属性和方法.
    /// </summary>
    public class CreateModalModel : AbpDemoPageModelBase
    {
        /// <summary>
        /// 在 JobInfo 属性上标记的 [BindProperty] 特性绑定了post请求提交上来的数据.
        /// </summary>
        [BindProperty]
        public CreateUpdateJobInfoDto JobInfo { get; set; }

        private readonly IJobInfoAppService _jobInfoAppService;
        private readonly ScheduleCenter _scheduleCenter;

        /// <summary>
        /// 构造函数注入IJobInfoAppService服务
        /// </summary>
        /// <param name="jobInfoAppService"></param>
        /// <param name="scheduleCenter"></param>
        public CreateModalModel(IJobInfoAppService jobInfoAppService, ScheduleCenter scheduleCenter)
        {
            _jobInfoAppService = jobInfoAppService;
            _scheduleCenter = scheduleCenter;
        }



        /// <summary>
        /// 并且在 OnPostAsync 方法中调用了服务的 CreateAsync 方法
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _scheduleCenter.AddJobAsync(JobInfo);
            if (result.Code==0)
            {
                JobInfo.JobStatus = JobStatu.Running;
                await _jobInfoAppService.CreateAsync(JobInfo);
            }
            return NoContent();
        }
    }
}