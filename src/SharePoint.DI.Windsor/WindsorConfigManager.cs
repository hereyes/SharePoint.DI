using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.Core.Internal;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace SharePoint.DI.Windsor
{
    public class WindsorConfigManager
    {
        private IConfigManager _configManager;

        public WindsorConfigManager(IConfigManager configManager)
        {
            _configManager = configManager;
        }

        public void AddInstallerAssembly(params Assembly[] assemblies)
        {
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPFarm);
            AddInstallerAssembly(bag, assemblies);
        }

        public void AddInstallerAssembly(SPWeb web, params Assembly[] assemblies)
        {
            _configManager.SetWeb(web);
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPWeb);
            AddInstallerAssembly(bag, assemblies);
        }

        public void AddInstallerAssembly(SPSite site, params Assembly[] assemblies)
        {
            _configManager.SetWeb(site.RootWeb);
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPSite);
            AddInstallerAssembly(bag, assemblies);
        }

        public void AddInstallerAssembly(SPWebApplication webApp, params Assembly[] assemblies)
        {
            if (webApp.Sites.Count > 0)
            {
                _configManager.SetWeb(webApp.Sites[0].RootWeb);
            }
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPWebApplication);
            AddInstallerAssembly(bag, assemblies);
        }

        private void AddInstallerAssembly(IPropertyBag bag, params Assembly[] assemblies)
        {
            var components = _configManager.GetFromPropertyBag<string[]>(Constants.WindsorInstallerAssemblies, bag);
            if (components == null)
            {
                _configManager.SetInPropertyBag(Constants.WindsorInstallerAssemblies, GetAssemblyNames(assemblies), bag);
                return;
            }
            else
            {
                List<Assembly> componentsList = GetAssembliesFromNames(components).ToList();
                foreach(Assembly registration in assemblies)
                {
                    if (!componentsList.Contains(registration))
                    {
                        componentsList.Add(registration);
                    }
                }
                _configManager.SetInPropertyBag(Constants.WindsorInstallerAssemblies, GetAssemblyNames(componentsList.ToArray()), bag);
            }
        }



        public void AddInstaller(params IWindsorInstaller[] installers)
        {
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPFarm);
            AddInstaller(bag, installers);
        }

        private void AddInstaller(IPropertyBag bag, params IWindsorInstaller[] installers)
        {
            var components = _configManager.GetFromPropertyBag<string[]>(Constants.WindsorInstallers, bag);
            if (components == null)
            {
                _configManager.SetInPropertyBag(Constants.WindsorInstallers, GetTypeNames(installers), bag);
                return;
            }
            else
            {
                List<IWindsorInstaller> componentsList = GetInstallersFromTypeNames(components).ToList();
                foreach (IWindsorInstaller registration in installers)
                {
                    if (!componentsList.Contains(registration))
                    {
                        componentsList.Add(registration);
                    }
                }
                _configManager.SetInPropertyBag(Constants.WindsorInstallers, GetTypeNames(componentsList.ToArray()), bag);
            }
        }

        private void RemoveInstallerAssembly(IPropertyBag bag, params Assembly[] assemblies)
        {
            var currentRegistrations = _configManager.GetFromPropertyBag<string[]>(Constants.WindsorInstallerAssemblies, bag);

            if (currentRegistrations == null || !currentRegistrations.Any()) return;

            List<Assembly> registrationList = GetAssembliesFromNames(currentRegistrations).ToList();
            var matchingRegistrations = registrationList.Where(assemblies.Contains);
            foreach (Assembly matchingRegistration in matchingRegistrations)
            {
                registrationList.Remove(matchingRegistration);
                break;
            }

            _configManager.SetInPropertyBag(Constants.WindsorInstallerAssemblies, GetAssemblyNames(registrationList.ToArray()), bag);
        }

        private void RemoveInstaller(IPropertyBag bag, params IWindsorInstaller[] installers)
        {
            var currentInstallers = _configManager.GetFromPropertyBag<string[]>(Constants.WindsorInstallers, bag);

            if (currentInstallers == null || !currentInstallers.Any()) return;

            List<IWindsorInstaller> installerList = GetInstallersFromTypeNames(currentInstallers).ToList();
            var matchingInstallers = installerList.Where(x=>GetTypes(installers).Contains(x.GetType()));
            foreach (IWindsorInstaller matchingInstaller in matchingInstallers)
            {
                installerList.Remove(matchingInstaller);
                break;
            }

            _configManager.SetInPropertyBag(Constants.WindsorInstallers, GetTypeNames(installerList.ToArray()), bag);
        }

        public void RemoveInstaller(params IWindsorInstaller[] installers)
        {
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPFarm);
            RemoveInstaller(bag, installers);
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

        public void RemoveInstaller(SPWeb web, params IWindsorInstaller[] Installers)
        {
            _configManager.SetWeb(web);
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPWeb);
            RemoveInstaller(bag, Installers);
        }

        public void RemoveInstaller(SPSite site, params IWindsorInstaller[] Installers)
        {
            _configManager.SetWeb(site.RootWeb);
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPSite);
            RemoveInstaller(bag, Installers);
            foreach (SPWeb web in site.AllWebs)
            {
                RemoveInstaller(web, Installers);
            }
        }

        public void RemoveInstaller(SPWebApplication webApp, params IWindsorInstaller[] Installers)
        {
            if (webApp.Sites.Count > 0)
            {
                _configManager.SetWeb(webApp.Sites[0].RootWeb);
                IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPWebApplication);
                RemoveInstaller(bag, Installers);
                foreach (SPSite site in webApp.Sites)
                {
                    RemoveInstaller(site, Installers);
                }
            }
        }

        public void RemoveInstallerAssembly(params Assembly[] assemblies)
        {
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPFarm);
            RemoveInstallerAssembly(bag, assemblies);
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

        public void RemoveInstallerAssembly(SPWeb web, params Assembly[] assemblies)
        {
            _configManager.SetWeb(web);
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPWeb);
            RemoveInstallerAssembly(bag, assemblies);
        }

        public void RemoveInstallerAssembly(SPSite site, params Assembly[] assemblies)
        {
            _configManager.SetWeb(site.RootWeb);
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPSite);
            RemoveInstallerAssembly(bag, assemblies);
            foreach (SPWeb web in site.AllWebs)
            {
                RemoveInstallerAssembly(web, assemblies);
            }
        }

        public void RemoveInstallerAssembly(SPWebApplication webApp, params Assembly[] assemblies)
        {
            if (webApp.Sites.Count > 0)
            {
                _configManager.SetWeb(webApp.Sites[0].RootWeb);
                IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPWebApplication);
                RemoveInstallerAssembly(bag, assemblies);
                foreach (SPSite site in webApp.Sites)
                {
                    RemoveInstallerAssembly(site, assemblies);
                }
            }
        }

        public Assembly[] GetInstallerAssemblies()
        {
            List<Assembly> registrations = new List<Assembly>();
            IPropertyBag bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPFarm);
            string[] serverRegistrations = _configManager.GetFromPropertyBag<string[]>(Constants.WindsorInstallerAssemblies, bag);
            if(serverRegistrations!=null)
                registrations.AddRange(GetAssembliesFromNames(serverRegistrations));
            bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPWebApplication);
            serverRegistrations = _configManager.GetFromPropertyBag<string[]>(Constants.WindsorInstallerAssemblies, bag);
            if (serverRegistrations != null)
                registrations.AddRange(GetAssembliesFromNames(serverRegistrations));
            bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPSite);
            serverRegistrations = _configManager.GetFromPropertyBag<string[]>(Constants.WindsorInstallerAssemblies, bag);
            if (serverRegistrations != null)
                registrations.AddRange(GetAssembliesFromNames(serverRegistrations));
            bag = _configManager.GetPropertyBag(ConfigLevel.CurrentSPWeb);
            serverRegistrations = _configManager.GetFromPropertyBag<string[]>(Constants.WindsorInstallerAssemblies, bag);
            if (serverRegistrations != null)
                registrations.AddRange(GetAssembliesFromNames(serverRegistrations));

            return registrations.ToArray();
        }

        public string[] GetAssemblyNames(Assembly[] assemblies)
        {
            List<string> assemblyNames = new List<string>();
            foreach (Assembly assembly in assemblies)
            {
                assemblyNames.Add(assembly.FullName);
            }
            return assemblyNames.ToArray();
        }

        public string[] GetTypeNames(IWindsorInstaller[] installers)
        {
            List<string> installerNames = new List<string>();
            foreach (IWindsorInstaller installer in installers)
            {
                installerNames.Add(installer.GetType().AssemblyQualifiedName);
            }
            return installerNames.ToArray();
        }

        public Assembly[] GetAssembliesFromNames(string[] assemblyNames)
        {
            List<Assembly> assemblies = new List<Assembly>();
            foreach (string assemblyName in assemblyNames)
            {
                var assembly = ReflectionUtil.GetAssemblyNamed(assemblyName);
                assemblies.Add(assembly);
            }
            return assemblies.ToArray();
        }

        public IWindsorInstaller[] GetInstallersFromTypeNames(string[] typeNames)
        {
            List<IWindsorInstaller> installers = new List<IWindsorInstaller>();
            foreach (string typeName in typeNames)
            {
                var installer = Type.GetType(typeName).CreateInstance<IWindsorInstaller>();
                installers.Add(installer);
            }
            return installers.ToArray();
        }

        public ICollection<Type> GetTypes(object[] objects)
        {
            List<Type> types = new List<Type>();
            foreach (object obj in objects)
            {
                var type = obj.GetType();
                types.Add(type);
            }
            return types;
        }
    }
}
