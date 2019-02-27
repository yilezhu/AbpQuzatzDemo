using System;

namespace Czar.AbpDemo.Permissions
{
    public static class AbpDemoPermissions
    {
        public const string GroupName = "AbpDemo";

        //Add your own permission names. Example:
        //public const string MyPermission1 = GroupName + ".MyPermission1";

        public static string[] GetAll()
        {
            //Return an array of all permissions
            return Array.Empty<string>();
        }
    }
}