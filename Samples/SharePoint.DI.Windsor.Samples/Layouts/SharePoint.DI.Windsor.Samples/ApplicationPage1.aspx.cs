using System;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SharePoint.DI.Windsor.Samples.Layouts.SharePoint.DI.Windsor.Samples
{
    public partial class ApplicationPage1 : LayoutsPageBase
    {
        [Inject]
        public ILogger Logger { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Text = Logger == null ? "The logger property was not injected" : "The logger property was succesfully injected";
        }
    }
}
