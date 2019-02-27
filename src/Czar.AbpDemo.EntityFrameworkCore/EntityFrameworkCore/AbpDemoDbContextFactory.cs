using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Czar.AbpDemo.EntityFrameworkCore
{
    public class AbpDemoDbContextFactory : IDesignTimeDbContextFactory<AbpDemoDbContext>
    {
        public AbpDemoDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<AbpDemoDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Default"));

            return new AbpDemoDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Czar.AbpDemo.Web/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
