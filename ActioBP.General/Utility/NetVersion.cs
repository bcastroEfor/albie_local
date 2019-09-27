using System;
using System.Reflection;

namespace ActioBP.General.Utility
{
    public class NetVersion
    {
        public static string GetNetCoreVersion()
        {
            try
            {
                var assembly = typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly;
                var assemblyPath = assembly.CodeBase.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                int netCoreAppIndex = Array.IndexOf(assemblyPath, "Microsoft.NETCore.App");
                if (netCoreAppIndex > 0 && netCoreAppIndex < assemblyPath.Length - 2)
                    return ".NET Core v" + assemblyPath[netCoreAppIndex + 1];
            }
            catch (Exception)
            {

            }
            return "Unknown version";
        }
    }
}
