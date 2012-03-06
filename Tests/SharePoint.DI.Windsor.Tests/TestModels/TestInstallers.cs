using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.Practices.SharePoint.Common.Logging;
using SharePoint.DI.Windsor.Tests.Stubs;

namespace SharePoint.DI.Windsor.Tests.TestModels
{
    public class TestInstaller1:IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ILogger>().ImplementedBy<SharePointLogger>());
        }
    }
    public class TestInstaller2 : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IConfigManager>().ImplementedBy<ConfigManager>());
        }
    }
    public class TestInstaller3 : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IInterface>().ImplementedBy<DerivedObject>());
        }
    }
}
