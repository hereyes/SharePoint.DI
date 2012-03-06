using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls.WebParts;
using Castle.Windsor;

namespace SharePoint.DI.Windsor
{
    public class DIWebPart:WebPart
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
