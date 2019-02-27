using Czar.AbpDemo.Localization.AbpDemo;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Czar.AbpDemo.Pages
{
    public abstract class AbpDemoPageModelBase : AbpPageModel
    {
        protected AbpDemoPageModelBase()
        {
            LocalizationResourceType = typeof(AbpDemoResource);
        }
    }
}