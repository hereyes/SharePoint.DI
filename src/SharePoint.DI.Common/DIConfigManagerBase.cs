using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace SharePoint.DI.Common
{
    /// <summary>
    /// Adds, removes, and gets installer and assemblies containing installer classes from the SharePoint properties bags at different levels
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DIConfigManagerBase<T>:IDIConfigManager<T>
    {
        private readonly IConfigManager _configManager;
        private readonly string _assemblyPropKey;
        private readonly string _installerPropKey;

        public DIConfigManagerBase(IConfigManager configManager, string assemblyPropKey, string installerPropKey
            )
        {
            _configManager = configManager;
            _assemblyPropKey = assemblyPropKey;
            _installerPropKey = installerPropKey;
        }

        #region "Add InstallerAssembly"
        /// <summary>
        /// Adds installer assembly to the Farm level
        /// </summary>
        /// <param name="assemblies">assemblies that contain installer classes to be used by the IOC container when it inject dependencies</param>
        public void AddInstallerAssembly(params Assembly[] assemblies)
        {
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPFarm);
            AddInstallerAssembly(bag, assemblies);
        }

        /// <summary>
        /// Adds assemblies that contain installers/components to the specified SPWeb.  These installers will only be available within the specified SPWeb object
        /// </summary>
        /// <param name="web"></param>
        /// <param name="assemblies"></param>
        public void AddInstallerAssembly(SPWeb web, params Assembly[] assemblies)
        {
            _configManager.SetWeb(web);
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPWeb);
            AddInstallerAssembly(bag, assemblies);
        }

        /// <summary>
        /// Adds assemblies that contain installers/components to the specified SPSite.  These installers will only be available within the specified SPSite object and its child webs
        /// </summary>
        /// <param name="site"></param>
        /// <param name="assemblies"></param>
        public void AddInstallerAssembly(SPSite site, params Assembly[] assemblies)
        {
            _configManager.SetWeb(site.RootWeb);
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPSite);
            AddInstallerAssembly(bag, assemblies);
        }

        /// <summary>
        /// Adds assemblies that contain installers/components to the specified SPWebApplication.  
        /// These installers will only be available within the specified SPWebApplication object and its child sites and webs
        /// </summary>
        /// <param name="webApp"></param>
        /// <param name="assemblies"></param>
        public void AddInstallerAssembly(SPWebApplication webApp, params Assembly[] assemblies)
        {
            if (webApp.Sites.Count > 0)
            {
                _configManager.SetWeb(webApp.Sites[0].RootWeb);
            }
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPWebApplication);
            AddInstallerAssembly(bag, assemblies);
        }


        /// <summary>
        /// Adds assemblies that contain installers/components to the specified Properties bag.
        /// </summary>
        /// <param name="bag"></param>
        /// <param name="assemblies"></param>
        private void AddInstallerAssembly(IPropertyBag bag, params Assembly[] assemblies)
        {
            var components = _configManager.GetFromPropertyBag<string[]>(_assemblyPropKey, bag);
            if (components == null)
            {
                _configManager.SetInPropertyBag(_assemblyPropKey, ReflectionUtil.GetAssemblyNames(assemblies), bag);
                return;
            }
            else
            {
                List<Assembly> componentsList = ReflectionUtil.GetAssembliesFromNames(components).ToList();
                foreach (Assembly registration in assemblies)
                {
                    if (!componentsList.Contains(registration))
                    {
                        componentsList.Add(registration);
                    }
                }
                _configManager.SetInPropertyBag(_assemblyPropKey, ReflectionUtil.GetAssemblyNames(componentsList.ToArray()), bag);
            }
        }
        #endregion

        #region Add Installer
        /// <summary>
        /// Adds installers/components at the SPFarm level.  These installers will be available everywhere in the farm
        /// </summary>
        /// <param name="installers"></param>
        public void AddInstaller(params T[] installers)
        {
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPFarm);
            AddInstaller(bag, installers);
        }
        
        /// <summary>
        /// Adds installers/components to the specified SPWeb.  These installers will only be available within the specified SPWeb object
        /// </summary>
        /// <param name="web"></param>
        /// <param name="assemblies"></param>
        public void AddInstaller(SPWeb web, params T[] installer)
        {
            _configManager.SetWeb(web);
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPWeb);
            AddInstaller(bag, installer);
        }

        /// <summary>
        /// Adds installers/components to the specified SPSite.  These installers will only be available within the specified SPSite object and its child webs
        /// </summary>
        /// <param name="site"></param>
        /// <param name="installer"></param>
        public void AddInstaller(SPSite site, params T[] installer)
        {
            _configManager.SetWeb(site.RootWeb);
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPSite);
            AddInstaller(bag, installer);
        }

        /// <summary>
        /// Adds installers/components to the specified SPWebApplication.  
        /// These installers will only be available within the specified SPSite object and its child sites and webs
        /// </summary>
        /// <param name="webApp"></param>
        /// <param name="installer"></param>
        public void AddInstaller(SPWebApplication webApp, params T[] installer)
        {
            if (webApp.Sites.Count > 0)
            {
                _configManager.SetWeb(webApp.Sites[0].RootWeb);
            }
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPWebApplication);
            AddInstaller(bag, installer);
        }

        /// <summary>
        /// Adds installers/components to the specified properties bag
        /// </summary>
        /// <param name="bag"></param>
        /// <param name="installers"></param>
        private void AddInstaller(IPropertyBag bag, params T[] installers)
        {
            var components = _configManager.GetFromPropertyBag<string[]>(_installerPropKey, bag);
            if (components == null)
            {
                _configManager.SetInPropertyBag(_installerPropKey, ReflectionUtil.GetTypeNames(installers), bag);
                return;
            }
            else
            {
                List<T> componentsList = ReflectionUtil.GetInstallersFromTypeNames<T>(components).ToList();
                foreach (T registration in installers)
                {
                    if (!componentsList.Contains(registration))
                    {
                        componentsList.Add(registration);
                    }
                }
                _configManager.SetInPropertyBag(_installerPropKey, ReflectionUtil.GetTypeNames(componentsList.ToArray()), bag);
            }
        }
        #endregion

        #region Remove Installer Assembly
        /// <summary>
        /// removes assemblies from the whole Farm, including all SPWebs, SPSites, and SPWebApplicaitons
        /// </summary>
        /// <param name="assemblies"></param>
        public void RemoveInstallerAssembly(params Assembly[] assemblies)
        {
            RemoveOnlyFromSPFarm(assemblies);
            foreach (var service in SPFarm.Local.Services)
            {
                if (service is SPWebService)
                {
                    SPWebService webService = (SPWebService)service;
                    foreach (SPWebApplication webApp in webService.WebApplications)
                    {
                        RemoveInstallerAssembly(webApp, assemblies);
                    }
                }
            }
        }

        /// <summary>
        /// Removes assemblies only at the farm level, it will not attempt to remove them from the child SPWebApplicaitons, SPSites, or SPWebs
        /// </summary>
        /// <param name="assemblies"></param>
        public void RemoveOnlyFromSPFarm(Assembly[] assemblies)
        {
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPFarm);
            RemoveInstallerAssembly(bag, assemblies);
        }

        /// <summary>
        /// Removes assemblies only from the specified SPWeb
        /// </summary>
        /// <param name="web"></param>
        /// <param name="assemblies"></param>
        public void RemoveInstallerAssembly(SPWeb web, params Assembly[] assemblies)
        {
            _configManager.SetWeb(web);
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPWeb);
            RemoveInstallerAssembly(bag, assemblies);
        }

        //Removes installer assemblies from the sepecified SPSite and its child webs
        public void RemoveInstallerAssembly(SPSite site, params Assembly[] assemblies)
        {
            RemoveOnlyFromSPSite(site, assemblies);
            foreach (SPWeb web in site.AllWebs)
            {
                RemoveInstallerAssembly(web, assemblies);
            }
        }

        /// <summary>
        /// Removew installer assemblies only from specified SPSite.  It will not attempt to remove them from Child Assemblies
        /// </summary>
        /// <param name="site"></param>
        /// <param name="assemblies"></param>
        public void RemoveOnlyFromSPSite(SPSite site, Assembly[] assemblies)
        {
            _configManager.SetWeb(site.RootWeb);
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPSite);
            RemoveInstallerAssembly(bag, assemblies);
        }

        /// <summary>
        /// Removews installer assemblies from the specified SPWebApplication, and all of its child sites and webs
        /// </summary>
        /// <param name="webApp"></param>
        /// <param name="assemblies"></param>
        public void RemoveInstallerAssembly(SPWebApplication webApp, params Assembly[] assemblies)
        {
            if (webApp.Sites.Count > 0)
            {
                RemoveOnlyFromSPWebApplication(webApp, assemblies);
                foreach (SPSite site in webApp.Sites)
                {
                    RemoveInstallerAssembly(site, assemblies);
                }
            }
        }

        /// <summary>
        /// Removews installer assemblies only from the specified SPWebApplication
        /// It will not attempt to remove them from the child SPSites or SPWebs
        /// </summary>
        /// <param name="webApp"></param>
        /// <param name="assemblies"></param>
        public void RemoveOnlyFromSPWebApplication(SPWebApplication webApp, Assembly[] assemblies)
        {
            if (webApp.Sites.Count > 0)
            {
                _configManager.SetWeb(webApp.Sites[0].RootWeb);
                IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPWebApplication);
                RemoveInstallerAssembly(bag, assemblies);
            }
        }

        /// <summary>
        /// Removes installer assemblies form the specified properties bag
        /// </summary>
        /// <param name="bag"></param>
        /// <param name="assemblies"></param>
        private void RemoveInstallerAssembly(IPropertyBag bag, params Assembly[] assemblies)
        {
            var currentRegistrations = _configManager.GetFromPropertyBag<string[]>(_assemblyPropKey, bag);

            if (currentRegistrations == null || !currentRegistrations.Any()) return;

            List<Assembly> registrationList = ReflectionUtil.GetAssembliesFromNames(currentRegistrations).ToList();
            var matchingRegistrations = registrationList.Where(assemblies.Contains);
            foreach (Assembly matchingRegistration in matchingRegistrations)
            {
                registrationList.Remove(matchingRegistration);
                break;
            }

            _configManager.SetInPropertyBag(_assemblyPropKey, ReflectionUtil.GetAssemblyNames(registrationList.ToArray()), bag);
        }
        #endregion

        #region Remove Installer
        /// <summary>
        /// removes the specified installers from the whole farm, including all web applications, sites and webs
        /// </summary>
        /// <param name="installers"></param>
        public void RemoveInstaller(params T[] installers)
        {
            RemoveOnlyFromSPFarm(installers);
            try
            {
                foreach (var service in SPFarm.Local.Services)
                {
                    if (service is SPWebService)
                    {
                        SPWebService webService = (SPWebService)service;
                        foreach (SPWebApplication webApp in webService.WebApplications)
                        {
                            RemoveInstaller(webApp, installers);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// removes installers only at the farm level, without attempting to remove them from the child web application, sites, or webs
        /// </summary>
        /// <param name="installers"></param>
        public void RemoveOnlyFromSPFarm(T[] installers)
        {
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPFarm);
            RemoveInstaller(bag, installers);
        }

        /// <summary>
        /// Removes installers from the specified web
        /// </summary>
        /// <param name="web"></param>
        /// <param name="Installers"></param>
        public void RemoveInstaller(SPWeb web, params T[] Installers)
        {
            _configManager.SetWeb(web);
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPWeb);
            RemoveInstaller(bag, Installers);
        }

        /// <summary>
        /// Removes installers from the specified site and all of its child webs
        /// </summary>
        /// <param name="site"></param>
        /// <param name="Installers"></param>
        public void RemoveInstaller(SPSite site, params T[] Installers)
        {
            RemoveOnlyFromSPSite(site, Installers);
            foreach (SPWeb web in site.AllWebs)
            {
                RemoveInstaller(web, Installers);
            }
        }

        /// <summary>
        /// Removes installers only from the specified site, without attempting to remove them from its child webs
        /// </summary>
        /// <param name="site"></param>
        /// <param name="Installers"></param>
        public void RemoveOnlyFromSPSite(SPSite site, T[] Installers)
        {
            _configManager.SetWeb(site.RootWeb);
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPSite);
            RemoveInstaller(bag, Installers);
        }

        /// <summary>
        /// removes installers from the specified web applicaiton and its child sites and webs
        /// </summary>
        /// <param name="webApp"></param>
        /// <param name="Installers"></param>
        public void RemoveInstaller(SPWebApplication webApp, params T[] Installers)
        {
            if (webApp.Sites.Count > 0)
            {
                RemoveOnlyFromSPWebApplication(webApp, Installers);
                foreach (SPSite site in webApp.Sites)
                {
                    RemoveInstaller(site, Installers);
                }
            }
        }

        /// <summary>
        /// Removes installer only at the web applicaiton level, without attempting to remove them from its child sites and webs
        /// </summary>
        /// <param name="webApp"></param>
        /// <param name="Installers"></param>
        public void RemoveOnlyFromSPWebApplication(SPWebApplication webApp, T[] Installers)
        {
            if (webApp.Sites.Count > 0)
            {
                _configManager.SetWeb(webApp.Sites[0].RootWeb);
                IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPWebApplication);
                RemoveInstaller(bag, Installers);
            }
        }

        /// <summary>
        /// Removes installers from the specifieed properties bag
        /// </summary>
        /// <param name="bag"></param>
        /// <param name="installers"></param>
        private void RemoveInstaller(IPropertyBag bag, params T[] installers)
        {
            var currentInstallers = _configManager.GetFromPropertyBag<string[]>(_installerPropKey, bag);

            if (currentInstallers == null || !currentInstallers.Any()) return;

            List<T> installerList = ReflectionUtil.GetInstallersFromTypeNames<T>(currentInstallers).ToList();
            var matchingInstallers = installerList.Where(x => ReflectionUtil.GetTypes(installers).Contains(x.GetType()));
            foreach (T matchingInstaller in matchingInstallers)
            {
                installerList.Remove(matchingInstaller);
                break;
            }

            _configManager.SetInPropertyBag(_installerPropKey, ReflectionUtil.GetTypeNames(installerList.ToArray()), bag);
        }
        #endregion

        #region Get Installer Assemblies
        /// <summary>
        /// Gets all installer assemblies at all levels for the current web, site, or web application
        /// </summary>
        /// <returns></returns>
        public Assembly[] GetInstallerAssemblies()
        {
            List<Assembly> registrations = new List<Assembly>();
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPFarm);
            string[] serverRegistrations = _configManager.GetFromPropertyBag<string[]>(_assemblyPropKey, bag);
            if (serverRegistrations != null)
                registrations.AddRange(ReflectionUtil.GetAssembliesFromNames(serverRegistrations));
            bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPWebApplication);
            serverRegistrations = _configManager.GetFromPropertyBag<string[]>(_assemblyPropKey, bag);
            if (serverRegistrations != null)
                registrations.AddRange(ReflectionUtil.GetAssembliesFromNames(serverRegistrations));
            bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPSite);
            serverRegistrations = _configManager.GetFromPropertyBag<string[]>(_assemblyPropKey, bag);
            if (serverRegistrations != null)
                registrations.AddRange(ReflectionUtil.GetAssembliesFromNames(serverRegistrations));
            bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPWeb);
            serverRegistrations = _configManager.GetFromPropertyBag<string[]>(_assemblyPropKey, bag);
            if (serverRegistrations != null)
                registrations.AddRange(ReflectionUtil.GetAssembliesFromNames(serverRegistrations));

            return registrations.ToArray();
        }

        /// <summary>
        /// Gets installer assemblies from the specified web
        /// </summary>
        /// <param name="web"></param>
        /// <returns></returns>
        public Assembly[] GetInstallerAssemblies(SPWeb web)
        {
            _configManager.SetWeb(web);
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPWeb);
            var registrations = GetInstallerAssemblies(bag);

            return registrations.ToArray();
        }

        /// <summary>
        /// Get installer assemblies only from the farm level
        /// </summary>
        /// <returns></returns>
        public Assembly[] GetInstallerAssembliesOnlyFromSPFarm()
        {
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPFarm);
            var registrations = GetInstallerAssemblies(bag);

            return registrations.ToArray();
        }


        /// <summary>
        /// Gets installer assemblies from teh specified site
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public Assembly[] GetInstallerAssemblies(SPSite site)
        {
            _configManager.SetWeb(site.RootWeb);
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPSite);
            var registrations = GetInstallerAssemblies(bag);

            return registrations.ToArray();
        }

        /// <summary>
        /// Gets installer assemblies from the specified web applicaiton
        /// </summary>
        /// <param name="webApp"></param>
        /// <returns></returns>
        public Assembly[] GetInstallerAssemblies(SPWebApplication webApp)
        {
            if (webApp.Sites.Count == 0)
            {
                return new Assembly[]{};
            }
            else
            {
                _configManager.SetWeb(webApp.Sites[0].RootWeb);
            }
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPWebApplication);
            var registrations = GetInstallerAssemblies(bag);

            return registrations.ToArray();
        }

        private List<Assembly> GetInstallerAssemblies(IPropertyBag bag)
        {
            List<Assembly> registrations = new List<Assembly>();
            string[] serverRegistrations = _configManager.GetFromPropertyBag<string[]>(_assemblyPropKey, bag);
            if (serverRegistrations != null)
                registrations.AddRange(ReflectionUtil.GetAssembliesFromNames(serverRegistrations));
            return registrations;
        }
        #endregion

        #region Get Installers
        /// <summary>
        /// gets all installers from all levels that correspond to the current web, site, or web applicaiton
        /// </summary>
        /// <returns></returns>
        public T[] GetInstallers()
        {
            List<T> registrations = new List<T>();
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPFarm);
            string[] serverRegistrations = _configManager.GetFromPropertyBag<string[]>(_assemblyPropKey, bag);
            if (serverRegistrations != null)
                registrations.AddRange(ReflectionUtil.GetInstallersFromTypeNames<T>(serverRegistrations));
            bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPWebApplication);
            serverRegistrations = _configManager.GetFromPropertyBag<string[]>(_assemblyPropKey, bag);
            if (serverRegistrations != null)
                registrations.AddRange(ReflectionUtil.GetInstallersFromTypeNames<T>(serverRegistrations));
            bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPSite);
            serverRegistrations = _configManager.GetFromPropertyBag<string[]>(_assemblyPropKey, bag);
            if (serverRegistrations != null)
                registrations.AddRange(ReflectionUtil.GetInstallersFromTypeNames<T>(serverRegistrations));
            bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPWeb);
            serverRegistrations = _configManager.GetFromPropertyBag<string[]>(_assemblyPropKey, bag);
            if (serverRegistrations != null)
                registrations.AddRange(ReflectionUtil.GetInstallersFromTypeNames<T>(serverRegistrations));

            return registrations.ToArray();
        }

        /// <summary>
        /// Gets installer from the specified web
        /// </summary>
        /// <param name="web"></param>
        /// <returns></returns>
        public T[] GetInstallers(SPWeb web)
        {
            _configManager.SetWeb(web);
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPWebApplication);
            var registrations =GetInstallers(bag);

            return registrations.ToArray();
        }

        /// <summary>
        /// Gets installers registered at the farm level
        /// </summary>
        /// <returns></returns>
        public T[] GetInstallersOnlyFromSPFarm()
        {
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPFarm);
            var registrations = GetInstallers(bag);

            return registrations.ToArray();
        }

        /// <summary>
        /// Gets installers from teh specified site
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public T[] GetInstallers(SPSite site)
        {
            _configManager.SetWeb(site.RootWeb);
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPSite);
            var registrations = GetInstallers(bag);

            return registrations.ToArray();
        }

        /// <summary>
        /// Gets installer from the specified web application
        /// </summary>
        /// <param name="webApp"></param>
        /// <returns></returns>
        public T[] GetInstallers(SPWebApplication webApp)
        {
            if (webApp.Sites.Count == 0)
            {
                return new T[] { };
            }
            else
            {
                _configManager.SetWeb(webApp.Sites[0].RootWeb);
            }
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPWebApplication);
            var registrations = GetInstallers(bag);

            return registrations.ToArray();
        }

        /// <summary>
        /// Gets installers from the specified properties bag
        /// </summary>
        /// <param name="bag"></param>
        /// <returns></returns>
        private List<T> GetInstallers(IPropertyBag bag)
        {
            List<T> registrations = new List<T>();
            string[] serverRegistrations = _configManager.GetFromPropertyBag<string[]>(_assemblyPropKey, bag);
            if (serverRegistrations != null)
                registrations.AddRange(ReflectionUtil.GetInstallersFromTypeNames<T>(serverRegistrations));
            return registrations;
        }
        #endregion
    }
}
