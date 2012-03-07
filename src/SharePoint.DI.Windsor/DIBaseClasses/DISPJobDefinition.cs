using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Microsoft.SharePoint.Administration;

namespace SharePoint.DI.Windsor
{
    public class DISPJobDefinition : SPJobDefinition
    {

        /// <summary>
        /// Reserved for internal use.
        /// </summary>
        protected DISPJobDefinition()
        {
            InjectProperties();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.SharePoint.Administration.SPJobDefinition"/> class and provides parameters for specifying key objects.
        /// </summary>
        /// <param name="name">Specifies the name of the job definition.</param><param name="service">Specifies an <see cref="T:Microsoft.SharePoint.Administration.SPService"/> object instance.</param><param name="server">Specifies an <see cref="T:Microsoft.SharePoint.Administration.SPServer"/> object instance.</param><param name="lockType">Specifies an enumeration value from the <see cref="T:Microsoft.SharePoint.Administration.SPJobLockType"/> enum.</param>
        protected DISPJobDefinition(string name, SPService service, SPServer server, SPJobLockType lockType)
            : base(name, service, server, lockType)
        {
            InjectProperties();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.SharePoint.Administration.SPJobDefinition"/> class and provides parameters for specifying key objects.
        /// </summary>
        /// <param name="name">Specifies the name of the job definition.</param><param name="webApplication">Specifies an <see cref="T:Microsoft.SharePoint.Administration.SPWebApplication"/> object instance.</param><param name="server">Specifies an <see cref="T:Microsoft.SharePoint.Administration.SPServer"/> object instance.</param><param name="lockType">Specifies an enumeration value from the <see cref="T:Microsoft.SharePoint.Administration.SPJobLockType"/> enum.</param>
        protected DISPJobDefinition(string name, SPWebApplication webApplication, SPServer server,
                                    SPJobLockType lockType)
            : base(name, webApplication, server, lockType)
        {
            InjectProperties();
        }

        private void InjectProperties()
        {
            WindsorContainer container = new WindsorContainer();
            if (container.LoadInstallers() > 0)
            {
                container.InjectProperties(this);
            }
        }
    }
}
