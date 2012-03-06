using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Microsoft.SharePoint.WebControls;

namespace SharePoint.DI.Windsor
{
    public class DILayoutsPageBase:LayoutsPageBase
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InjectProperties();
        }

        private void InjectProperties()
        {
            WindsorContainer container = new WindsorContainer();
            if (container.LoadInstallers() > 0)
            {
                container.InjectProperties(this);
            }

            ContainerExtensions.InjectUserControls(this, container);
        }
    }
}
