using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Microsoft.Practices.SharePoint.Common.Logging;

namespace SharePoint.DI.Windsor.Samples
{
    [Serializable]
    public class SPWindsorInstaller:IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ILogger>().ImplementedBy<SharePointLogger>());
        }
    }
}
