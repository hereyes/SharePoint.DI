using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
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
    public class SPWindsorHttpModule : IHttpModule
    {
        private HttpApplication httpApplication;
        private WindsorContainer container;

        public void Init(HttpApplication context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this.httpApplication = context;
            this.httpApplication.PreRequestHandlerExecute += this.HttpApplicationOnPreRequestHandlerExecute;
        }

        private void HttpApplicationOnPreRequestHandlerExecute(object sender, EventArgs eventArgs)
        {
            var page = this.httpApplication.Context.CurrentHandler as Page;
            if (page == null)
            {
                return;
            }


            container = new WindsorContainer();
            if (container.LoadInstallers() > 0)
            {
                container.InjectProperties(page);
                page.PreLoad += (o, args) => InjectUserControls(page, true);
            }
        }

        private void InjectUserControls(Control parent, bool skipDataBoundControls)
        {
            if (parent == null)
            {
                return;
            }

            if (!skipDataBoundControls)
            {
                var dataBoundControl = parent as DataBoundControl;
                if (dataBoundControl != null)
                {
                    dataBoundControl.DataBound += DataBoundControlOnDataBound;
                }
            }

            foreach (Control control in parent.Controls)
            {
                if (control is UserControl || control is WebPart)
                {
                    container.InjectProperties(control);
                    InjectUserControls(control, true);
                }
            }
        }

        private void DataBoundControlOnDataBound(object sender, EventArgs eventArgs)
        {
            var dataBoundControl = sender as DataBoundControl;
            if (dataBoundControl != null)
            {
                dataBoundControl.DataBound -= DataBoundControlOnDataBound;
                InjectUserControls(dataBoundControl, true);
            }
        }

        public void Dispose()
        {
        }
    }
}
