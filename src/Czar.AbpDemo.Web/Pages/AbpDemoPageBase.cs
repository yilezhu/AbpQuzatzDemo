using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Czar.AbpDemo.Localization.AbpDemo;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Czar.AbpDemo.Pages
{
    public abstract class AbpDemoPageBase : AbpPage
    {
        [RazorInject]
        public IHtmlLocalizer<AbpDemoResource> L { get; set; }
    }
}
