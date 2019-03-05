/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：yilezhu                                             
*│　版    本：1.0                                                 
*│　创建时间：2019/2/25 17:26:48                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： Czar.AbpDemo.JobSchedule                                   
*│　类    名： JobStatus                                      
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Czar.AbpDemo.JobSchedule
{
    public enum JobStatu: byte
    {
        [Description("执行中")]
        Running,
        [Description("已完成")]
        Completed,
        [Description("已停止")]
        Stopped,
        [Description("系统停止")]
        SystemStopped,
        [Description("已删除")]
        Deleted,
    }
}
