using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.WebControls;

namespace SharePoint.DI.Windsor.Samples
{
    public partial class NewBasicServiceApplication1ApplicationPage : Page
    {
        protected InputFormTextBox nameField;
        protected InputFormCheckBox defaultProxyField;

        protected override void OnInit(EventArgs e)
        {
            ((DialogMaster)this.Page.Master).OkButton.Click += OkButton_Click;
            base.OnInit(e);
        }

        void OkButton_Click(object sender, EventArgs e)
        {
            using (SPLongOperation operation = new SPLongOperation(this))
            {
                operation.LeadingHTML = "Creating new BasicServiceApplication1 Application";
                operation.Begin();
                try
                {
                    SPFarm farm = SPFarm.Local;
                    BasicServiceApplication1Service service = farm.Services.GetValue<BasicServiceApplication1Service>();
                    BasicServiceApplication1ServiceProxy serviceProxy = farm.ServiceProxies.GetValue<BasicServiceApplication1ServiceProxy>();

                    string title = nameField.Text;
                    BasicServiceApplication1ServiceApplication application = BasicServiceApplication1ServiceApplication.Create(
                        title, service);
                    application.Provision();
                    BasicServiceApplication1ServiceApplicationProxy applicationProxy = BasicServiceApplication1ServiceApplicationProxy.Create(
                        title, serviceProxy, application.Id);
                    applicationProxy.Provision();
                    if (defaultProxyField.Checked)
                    {
                        SPServiceApplicationProxyGroup.Default.Add(applicationProxy);
                    }
                }
                finally
                {
                    operation.EndScript("window.frameElement.commitPopup();");
                }
            }
        }
    }
}
