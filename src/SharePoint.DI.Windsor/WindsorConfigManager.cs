using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using SharePoint.DI.Common;

namespace SharePoint.DI.Windsor
{
    public class WindsorConfigManager : DIConfigManagerBase<IWindsorInstaller>
    {
        public WindsorConfigManager(IConfigManager configManager):base(configManager, Constants.WindsorInstallerAssemblies, Constants.WindsorInstallers)
        {
        }
    }
}
