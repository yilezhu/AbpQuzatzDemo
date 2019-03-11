using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Czar.AbpDemo.Localization.AbpDemo;
using Volo.Abp.UI.Navigation;

namespace Czar.AbpDemo.Menus
{
    public class AbpDemoMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<AbpDemoResource>>();
            context.Menu.Items.Insert(0, new ApplicationMenuItem("Czar.AbpDemo.Home", l["Menu:Home"].Value, "/"));
            context.Menu.AddItem(
                new ApplicationMenuItem("BooksStore", l["Menu:BookStore"].Value)
                    .AddItem(new ApplicationMenuItem("BooksStore.Books", l["Menu:Books"].Value, url: "/Books")));
            context.Menu.AddItem(
               new ApplicationMenuItem("JobSchedule", l["Menu:JobSchedule"].Value)
                   .AddItem(new ApplicationMenuItem("JobSchedule.JobInfo", l["Menu:JobInfo"].Value, url: "/JobSchedule")));
             await Task.CompletedTask;
        }
    }
}
