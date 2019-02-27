using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace Czar.AbpDemo.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class AbpDemoDbContext : AbpDbContext<AbpDemoDbContext>
    {
        public AbpDemoDbContext(DbContextOptions<AbpDemoDbContext> options)
            : base(options)
        {

        }

        public DbSet<BookStore.Book> Books { get; set; }
        public DbSet<JobSchedule.JobInfo> JobInfos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureIdentity();
            modelBuilder.ConfigurePermissionManagement();
            modelBuilder.ConfigureSettingManagement();
            modelBuilder.ConfigureBackgroundJobs();
            modelBuilder.ConfigureAuditLogging();
        }
    }
}
