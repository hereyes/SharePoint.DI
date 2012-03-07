using System.Reflection;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace SharePoint.DI.Common
{
    public interface IDIConfigManager<T>
    {
        void AddInstallerAssembly(params Assembly[] assemblies);
        void AddInstallerAssembly(SPWeb web, params Assembly[] assemblies);
        void AddInstallerAssembly(SPSite site, params Assembly[] assemblies);
        void AddInstallerAssembly(SPWebApplication webApp, params Assembly[] assemblies);
        void AddInstaller(params T[] installers);
        void AddInstaller(SPWeb web, params T[] installer);
        void AddInstaller(SPSite site, params T[] installer);
        void AddInstaller(SPWebApplication webApp, params T[] installer);
        void RemoveInstallerAssembly(params Assembly[] assemblies);
        void RemoveOnlyFromSPFarm(Assembly[] assemblies);
        void RemoveInstallerAssembly(SPWeb web, params Assembly[] assemblies);
        void RemoveInstallerAssembly(SPSite site, params Assembly[] assemblies);
        void RemoveOnlyFromSPSite(SPSite site, Assembly[] assemblies);
        void RemoveInstallerAssembly(SPWebApplication webApp, params Assembly[] assemblies);
        void RemoveOnlyFromSPWebApplication(SPWebApplication webApp, Assembly[] assemblies);
        void RemoveInstaller(params T[] installers);
        void RemoveOnlyFromSPFarm(T[] installers);
        void RemoveInstaller(SPWeb web, params T[] Installers);
        void RemoveInstaller(SPSite site, params T[] Installers);
        void RemoveOnlyFromSPSite(SPSite site, T[] Installers);
        void RemoveInstaller(SPWebApplication webApp, params T[] Installers);
        void RemoveOnlyFromSPWebApplication(SPWebApplication webApp, T[] Installers);
        Assembly[] GetInstallerAssemblies();
        Assembly[] GetInstallerAssemblies(SPWeb web);
        Assembly[] GetInstallerAssemblies(SPSite site);
        Assembly[] GetInstallerAssemblies(SPWebApplication webApp);
        Assembly[] GetInstallerAssembliesOnlyFromSPFarm();
        T[] GetInstallers();
        T[] GetInstallers(SPWeb web);
        T[] GetInstallers(SPSite site);
        T[] GetInstallers(SPWebApplication webApp);
        T[] GetInstallersOnlyFromSPFarm();
    }
}