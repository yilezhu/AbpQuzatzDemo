using Volo.Abp;

namespace Czar.AbpDemo
{
    public abstract class AbpDemoApplicationTestBase : AbpIntegratedTest<AbpDemoApplicationTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
