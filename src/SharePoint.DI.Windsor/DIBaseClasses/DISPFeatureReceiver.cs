using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Microsoft.SharePoint;

namespace SharePoint.DI.Windsor
{
    public class DISPFeatureReceiver : SPFeatureReceiver
    {
        public DISPFeatureReceiver()
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
