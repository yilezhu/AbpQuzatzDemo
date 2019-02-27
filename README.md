# 用abp vNext快速开发Quartz.NET定时任务管理界面

今天这篇文章我将通过实例代码带着大家一步一步通过abp vNext这个asp.net core的快速开发框架来进行Quartz.net定时任务调度的管理界面的开发。大伙最好跟着一起敲一下代码，当然源码我会上传到github上，有兴趣的小伙伴可以在文章底部查看源码链接。

> 作者：依乐祝
> 原文链接：

## 写在前面
有几天没更新博客了，一方面因为比较忙，另一方面是因为最近在准备组织我们霸都合肥的.NET技术社区首次非正式的线下聚会，忙着联系人啊，这里欢迎有兴趣的小伙伴加我wx:jkingzhu进行详细的了解，当然也欢迎同行加我微信，然后我拉你进入我们合肥.NET技术社区微信群跟大伙进行交流。
## 概念
开始之前还有必要跟大伙说一下abp vNext以及Quartz.net是什么，防止有小白。如果对这两个概念非常熟悉的话可以直接阅读下一节。项目最终实现的效果如下图所示：

![1551252591337](https://img2018.cnblogs.com/blog/1377250/201902/1377250-20190227153105469-2085620000.png)

### abp vNext是什么
说起abp vNext就要从另一个概念开始说起了，那就是大名鼎鼎的ABP了。
ABP 官方的介绍是：ASP.NET Boilerplate 是一个用最佳实践和流行技术开发现代 WEB 应用程序的新起点，它旨在成为一个通用的 WEB 应用程序基础框架和项目模板。基于 DDD 的经典分层架构思想，实现了众多 DDD 的概念（但没有实现所有 DDD 的概念）。
而ABPVNext的出现是为了抛弃掉.net framework 版本下的包袱，重新启动的 abp 框架，目的是为了放弃对传统技术的支持，让 asp.net core 能够自身做到更加的模块化，目前这块的内容还不够成熟。原因是缺少组件信息和内容。
如果你想用于生产环境建议你可以使用ABP，如果你敢于尝试，勇于创新的话可以直接使用abp vNext进行开发的。
abp vNext官网：https://abp.io/
github：https://github.com/abpframework/abp
文档：https://abp.io/documents

### Quartz.NET是什么
Quartz.NET是一个强大、开源、轻量的作业调度框架，你能够用它来为执行一个作业而创建简单的或复杂的作业调度。它有很多特征，如：数据库支持，集群，插件，支持cron-like表达式等等。目前已经正式支持了.NET Core 和async/await。
说白了就是你可以使用Quartz.NET可以很方便的开发定时任务诸如平时的工作中，定时轮询数据库同步，定时邮件通知，定时处理数据等。 

## 实例演练
这一节我们通过实例进行操作，相信跟着做的你也能够把代码跑起来。
### ABP vNext代码
既然我们此次演练的项目是使用的abp vNext这个asp.net core的快速开发框架来完成的，所以首先在项目开始之前，你需要到ABP vNext的官网上去下载项目代码。英文站打开慢的话，可以访问中文子域名进行访问：https://cn.abp.io/Templates 。下面给出具体步骤：
1. 打开https://cn.abp.io/Templates 然后如图填写对应的项目名称，这里我用的`Czar.AbpDemo` 项目类型选择ASP.NET Core MVC应用程序，因为这个是带有UI界面的web项目，数据库提供程序选择EFCore这个大家都比较熟悉，然后点击创建就可以了。

   ![1551248124416](https://img2018.cnblogs.com/blog/1377250/201902/1377250-20190227153105038-1241081410.png)

2. 下载后，解压到一个文件夹下面，然后用vs打开解决方案，看到如下图所示的项目结构

   ![1551248279486](https://img2018.cnblogs.com/blog/1377250/201902/1377250-20190227153104653-339685045.png)

3. 这里简单介绍下，每个项目的作用，具体的就不过多介绍了，在下面的实战代码中慢慢体会吧

   - `.Domain` 为领域层.
   - `.Application` 为应用层.
   - `.Web` 为是表示层.
   - `.EntityFrameworkCore` 是EF Core集成.

   解决方案还包含配置好的的单元&集成测试项目, 以便与于**EF Core** 和 **SQLite** 数据库配合使用.

4. 查看`.Web`项目下`appsettings.json`文件中的 **连接字符串**并进行相应的修改，怎么改不要问我: 

   ```c#
   {
     "ConnectionStrings": {
       "Default": "Server=localhost;Database=CzarAbpDemo;Trusted_Connection=True;MultipleActiveResultSets=true"
     }
   }
   ```

5. 右键单击`.Web`项目并将其**设为启动项目** 

   ![1551248539374](https://img2018.cnblogs.com/blog/1377250/201902/1377250-20190227153104379-1393732643.png)

6. 打开**包管理器控制台(Package Manager Console)**, 选择`.EntityFrameworkCore`项目作为**默认项目**并运行`Update-Database`命令: 

   ![1551248591831](https://img2018.cnblogs.com/blog/1377250/201902/1377250-20190227153104048-1352695861.png)

7. 现在可以运行应用程序,它将会打开**home**页面: 

   ![1551248689114](https://img2018.cnblogs.com/blog/1377250/201902/1377250-20190227153103736-1565529982.png)

8. 点击“Login” 输入用户名`admin`, 密码`1q2w3E*`, 登录应用程序.

   启动模板包括 **身份管理(identity management)** 模块. 登录后将提供身份管理菜单,你可以在其中管理**角色**,**用户**及其**权限**. 这个不过多讲解了，自己去动手操作一番吧

### 集成Quartz.NET管理功能
这部分我们将实现Quartz.NET定时任务的管理功能,为了进行Quartz.NET定时任务的管理，我们还需要定义一个表来进行Quartz.NET定时任务的信息的承载，并完成这个表的增删改查功能，这样我们在对这个表的数据进行操作的同时来进行Quartz.NET定时任务的操作即可实现我们的需求。话不多说，开始吧。这部分我们再分成两个小节：JobInfo的增删改查功能的实现，Quartz.NET调度任务功能的增删改查的实现。

#### JobInfo的增删改查功能的实现
这个部分你将体会到我为什么使用abp vNext框架来进行开发了，就是因为快~~~~
1. 创建领域实体对象JobInfo，这个在领域层代码如下：

   ![1551249480050](https://img2018.cnblogs.com/blog/1377250/201902/1377250-20190227153102925-1252334692.png)

2. 将我们的JobInfo实体添加到DBContext中，这样应该在EF层

   ![1551249406105](https://img2018.cnblogs.com/blog/1377250/201902/1377250-20190227153102537-1481190709.png)

3. 添加新的Migration并更新到数据库中，这个应该算EFCore的基础了吧，两个步骤，一个“Add-Migration” 然后“Update-Database”更新到数据库即可

   ```
   Add-Migration "Add_JobInfo_Entity"
   Update-Database
   ```

4. 应用层创建页面显示实体`BookDto` 用来在 **基础设施层** 和 **应用层** **传递数据**

   ![1551249983515](https://img2018.cnblogs.com/blog/1377250/201902/1377250-20190227153102177-1008677781.png)

5. 同样的你还需要在应用层创建一个用来传递增改的Dto对象
   ![1551250041669](https://img2018.cnblogs.com/blog/1377250/201902/1377250-20190227153101758-668504528.png)

6. 万事俱备，只欠服务了，接下来我们创建一下`JobInfo`的服务接口以及服务接口的实现了，这里有个约定，就是所有的服务`AppService`结尾，就跟控制器都以`Controller`结尾的概念差不多。

   ![1551250166378](https://img2018.cnblogs.com/blog/1377250/201902/1377250-20190227153101421-871205481.png)

   服务实现：

   ![1551250189323](https://img2018.cnblogs.com/blog/1377250/201902/1377250-20190227153101084-1780150343.png)

   注释还算清真，相信你应该能看懂。

   

7. 这里abp vNext框架就会自动为我们实现增删改查的API Controllers接口的实现（可以通过swagger进行查看），还会**自动** 为所有的API接口创建了JavaScript **代理**.因此,你可以像调用 **JavaScript function**一样调用任何接口. 

   如下图所示

   ![1551250400532](https://img2018.cnblogs.com/blog/1377250/201902/1377250-20190227153100688-1101321149.png)
   是不是，感觉什么都还没做，所有接口都已经实现的感觉。

8. 新增一个菜单任务调度的菜单，如下代码所示：

   ![1551250546971](https://img2018.cnblogs.com/blog/1377250/201902/1377250-20190227153100347-619461625.png)

9. 对应的，我们需要在`Pages/JobSchedule` 这个路径下面创建对应的Index.cshtml页面，以及新增，编辑的页面。由于内容太多，这里就不贴代码了，只给大家贴下图：

   Index.cshtml

   ![1551250659492](https://img2018.cnblogs.com/blog/1377250/201902/1377250-20190227153059979-916322902.png)

   CreateModal.cshtml代码如下：

   ![1551250688733](https://img2018.cnblogs.com/blog/1377250/201902/1377250-20190227153059664-1648174999.png)

10. 然后我们运行起来查看下：

    ![1551250773502](https://img2018.cnblogs.com/blog/1377250/201902/1377250-20190227153059295-1939663022.png)

11. 点击，右上角的新增，会弹出新增界面，点击每一行的操作，会弹出删除（删除，这里只做了一个假功能），编辑的两个选项。

12. 到此，`JobInfo`的增删改查就做好了，是不是很简单，这就是abp vNext赋予我们的高效之处。

#### Quartz.NET调度任务功能的增删改的实现
在使用Quartz.NET之前，你需要通过Nuget进行下安装，然后才能进行调用。这里我不会给你详细讲解Quartz.NET的使用，因为这将占用大量的篇幅，并偏离本文的主旨
1. 安装Quartz.NET的Nuget包：

   ![1551251014507](https://img2018.cnblogs.com/blog/1377250/201902/1377250-20190227153058436-91712048.png)

2. 新建一个`ScheduleCenter` 的任务调度中心，代码如下所示：

   ```c#
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
           public  async Task<IScheduler> GetSchedulerAsync()
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
           public async Task<bool> AddJobAsync(CreateUpdateJobInfoDto infoDto)
           {
               try
               {
                   if (infoDto!=null)
                   {
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
                       IJobDetail job = JobBuilder.Create<LogTestJob>()
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
                       return true;
                   }
   
                   return false;//JobInfo为空
               }
               catch (Exception ex)
               {
                   _logger.LogException(ex);
                   return false;//出现异常
               }
           }
   
           /// <summary>
           /// 暂停指定任务计划
           /// </summary>
           /// <param name="jobName">任务名</param>
           /// <param name="jobGroup">任务分组</param>
           /// <returns></returns>
           public async Task<bool> StopJobAsync(string jobName, string jobGroup)
           {
               try
               {
                   JobKey jobKey = new JobKey(jobName, jobGroup);
                   scheduler = await GetSchedulerAsync();
                   if (await scheduler.CheckExists(jobKey))
                   {
                       await scheduler.PauseJob(new JobKey(jobName, jobGroup));
                       return true;
                   }
                   else
                   {
                       return false;//任务不存在
                   }
               }
               catch (Exception ex)
               {
                   _logger.LogException(ex);
                   return false;//出现异常
               }
           }
   
           /// <summary>
           /// 恢复指定的任务计划,如果是程序奔溃后 或者是进程杀死后的恢复，此方法无效
           /// </summary>
           /// <param name="jobName">任务名称</param>
           /// <param name="jobGroup">任务组</param>
           /// <returns></returns>
           public async Task<bool> ResumeJobAsync(string jobName, string jobGroup)
           {
               try
               {
                   JobKey jobKey = new JobKey(jobName, jobGroup);
                   scheduler = await GetSchedulerAsync();
                   if (await scheduler.CheckExists(jobKey))
                   {
                       //resumejob 恢复
                       await scheduler.ResumeJob(new JobKey(jobName, jobGroup));
                       return true;
                   }
                   else
                   {
                       return false;//不存在任务
                   }
                 
               }
               catch (Exception ex)
               {
                   _logger.LogException(ex);
                   return false;//出现异常
               }
           }
   
           /// <summary>
           /// 恢复指定的任务计划,如果是程序奔溃后 或者是进程杀死后的恢复，此方法无效
           /// </summary>
           /// <param name="jobName">任务名称</param>
           /// <param name="jobGroup">任务组</param>
           /// <returns></returns>
           public async Task<bool> DeleteJobAsync(string jobName, string jobGroup)
           {
               try
               {
                   JobKey jobKey = new JobKey(jobName, jobGroup);
                   scheduler = await GetSchedulerAsync();
                   if (await scheduler.CheckExists(jobKey))
                   {
                       //DeleteJob 恢复
                       await scheduler.DeleteJob(jobKey);
                       return true;
                   }
                   else
                   {
                       return false;//不存在任务
                   }
   
               }
               catch (Exception ex)
               {
                   _logger.LogException(ex);
                   return false;//出现异常
               }
           }
       }
   ```

3. 新建一个`LogTestJob` 的计划任务，代码如下所示，需要继承`IJob`接口：

   ![1551251169229](https://img2018.cnblogs.com/blog/1377250/201902/1377250-20190227153057526-1170632537.png)

4. 至此Quartz.NET调度任务功能完成

#### 集成
这里我们按照之前的思路对`JobInfo`跟Quartz.NET任务进行集成
1. 新增时，启动任务：

   ![1551251315532](https://img2018.cnblogs.com/blog/1377250/201902/1377250-20190227153057135-191179577.png)

2. 编辑时，更新任务

   ![1551251351318](https://img2018.cnblogs.com/blog/1377250/201902/1377250-20190227153056730-1539076977.png)

3. 这里细心的网友，可能注意到任务的删除是在编辑里面进行实现的。而列表页面的删除功能并没有实现真正意义的功能的删除。

## 功能演示
上面我们演示的任务是一个每5秒写入当前时间的一个任务，并实现了对这个任务的新增，删除，编辑的功能，这里大伙可以自行实现进行测试，也可以下载我的代码进行尝试。效果图如下所示：

![1551251560062](https://img2018.cnblogs.com/blog/1377250/201902/1377250-20190227153055488-921100311.png)

## 功能扩展
目前只能对既定义好任务进行调度，后期可以根据任务的名称，如我们实例中的测试任务`LogTestJob` 的名字找到这个任务，然后动态的进行处理。这样就可以在界面实现对多个任务进行调度了！当然还有其他的扩展，本文只是作为引子。

## 源码地址
GitHub：https://github.com/yilezhu/AbpQuzatzDemo

## 总结
本文只是简单的利用abp vNext框架进行Quartz.NET任务调度进行UI的管理，实现的功能也比较简单，大家完全可以在此基础上进行扩展完善，最后感谢大伙的阅读。

