using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.SharePoint;

namespace SharePoint.DI.Windsor
{
    /// <summary>
    /// Adds an extension method for the Windsor IOC container so that injects properties to an existing object
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Populates the properties decorated with the Inject attributed in an existing object using the Windsor container
        /// </summary>
        /// <param name="container"></param>
        /// <param name="instance">the object</param>
        /// <returns>returns the same object with the selected properties injected</returns>
        public static void InjectProperties(this WindsorContainer container, object instance)
        {
            InjectDependencies(container, instance);
        }

        public static void InjectDependencies(WindsorContainer container, object instance)
        {
            foreach (var property in instance.GetType().GetProperties())
            {
                object[] attributes = property.GetCustomAttributes(typeof (InjectAttribute), false);
                if (attributes.Length > 0)
                {
                    object value = container.Resolve(property.PropertyType);
                    property.GetSetMethod().Invoke(instance, new[] {value});
                }
            }
        }

        public static int LoadInstallers(this WindsorContainer container)
        {
            WindsorConfigManager config = new WindsorConfigManager(new ConfigManager());
            Assembly[] installerAssemblies = default(Assembly[]);
            IWindsorInstaller[] installers = default(IWindsorInstaller[]);
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                installerAssemblies = config.GetInstallerAssemblies();
                //installers
            });
            if (installerAssemblies.Length > 0)
            {
                foreach (Assembly assembly in installerAssemblies)
                {
                    container.Install(Castle.Windsor.Installer.FromAssembly.Instance(assembly));
                }
            }

            return installerAssemblies.Length;
        }


        public static void InjectUserControls(Control parent, WindsorContainer container)
        {
            if (parent == null)
            {
                return;
            }

            foreach (Control control in parent.Controls)
            {
                if (control is UserControl || control is WebPart)
                {
                    container.InjectProperties(control);
                    InjectUserControls(control, container);
                }
            }
        }
    }
}