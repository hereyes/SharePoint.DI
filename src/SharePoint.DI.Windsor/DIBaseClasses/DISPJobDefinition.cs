using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Microsoft.SharePoint.Administration;

namespace SharePoint.DI.Windsor
{
    public class DISPJobDefinition:SPJobDefinition
    {
        public override void Execute(Guid targetInstanceId)
        {
            base.Execute(targetInstanceId);

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
