using Czar.AbpDemo.Localization.AbpDemo;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Czar.AbpDemo.Permissions
{
    public class AbpDemoPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(AbpDemoPermissions.GroupName);

            //Define your own permissions here. Examaple:
            //myGroup.AddPermission(AbpDemoPermissions.MyPermission1, L("Permission:MyPermission1"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AbpDemoResource>(name);
        }
    }
}
