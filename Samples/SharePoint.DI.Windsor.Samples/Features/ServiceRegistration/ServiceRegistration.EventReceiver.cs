using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace SharePoint.DI.Windsor.Samples.Features.ServiceRegistration
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("cfc1b209-4760-45d6-8dea-77347873cb93")]
    public class ServiceRegistrationEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            WindsorConfigManager config = new WindsorConfigManager(new ConfigManager());
            config.AddInstallerAssembly(Assembly.GetExecutingAssembly());
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            WindsorConfigManager config = new WindsorConfigManager(new ConfigManager());
            config.RemoveInstallerAssembly(Assembly.GetExecutingAssembly());
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
