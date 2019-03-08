using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czar.AbpDemo.JobSchedule;
using Czar.AbpDemo.Pages;
using Czar.AbpDemo.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Czar.AbpDemo.Web.Pages.JobSchedule
{
    public class EditModalModel : AbpDemoPageModelBase
    {
        /// <summary>
        /// [HiddenInput] 和 [BindProperty] 是标准的 ASP.NET Core MVC 特性.
        /// 这里启用 SupportsGet 从Http请求的查询字符串中获取Id的值.
        /// </summary>
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public CreateUpdateJobInfoDto JobInfo { get; set; }

        private readonly IJobInfoAppService _jobInfoAppService;
        private readonly ScheduleCenter _scheduleCenter;
        public EditModalModel(IJobInfoAppService jobInfoAppService, ScheduleCenter scheduleCenter)
        {
            _jobInfoAppService = jobInfoAppService;
            _scheduleCenter = scheduleCenter;
        }
        /// <summary>
        /// 将 BookAppService.GetAsync 方法返回的 BookDto 
        /// 映射成 CreateUpdateBookDto 并赋值给Book属性.
        /// </summary>
        /// <returns></returns>
        public async Task OnGetAsync()
        {
            var jobInfoDto = await _jobInfoAppService.GetAsync(Id);
            JobInfo = ObjectMapper.Map<JobInfoDto, CreateUpdateJobInfoDto>(jobInfoDto);
        }

        /// <summary>
        /// 直接使用 BookAppService.UpdateAsync 来更新实体
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            var jobInfoDto = await _jobInfoAppService.GetAsync(Id);
            ScheduleResult result = new ScheduleResult();

            if (jobInfoDto.JobStatus != JobInfo.JobStatus)
            {
                if (jobInfoDto.JobStatus == JobStatu.Deleted)
                {
                    //如果之前的状态是已删除的话，先创建任务再进行操作
                    await _scheduleCenter.AddJobAsync(JobInfo.JobName,
                        JobInfo.JobGroup,
                        JobInfo.JobNamespace + "." + JobInfo.JobClassName,
                        JobInfo.JobAssemblyName,
                        JobInfo.CronExpress,
                        JobInfo.StarTime,
                        JobInfo.EndTime);
                }
                if (JobInfo.JobStatus == JobStatu.Deleted)
                {
                    result = await _scheduleCenter.DeleteJobAsync(JobInfo.JobName, JobInfo.JobGroup);
                    if (result.Code == 0)
                    {
                        await _jobInfoAppService.DeleteAsync(Id);
                    }
                    return NoContent();
                }
                else if (JobInfo.JobStatus == JobStatu.Running)
                {
                    result = await _scheduleCenter.ResumeJobAsync(JobInfo.JobName, JobInfo.JobGroup);
                }
                else
                {
                    result = await _scheduleCenter.StopJobAsync(JobInfo.JobName, JobInfo.JobGroup);
                }

            }
            if (result.Code == 0)
            {
                await _jobInfoAppService.UpdateAsync(Id, JobInfo);
            }
            return NoContent();
        }
    }
}