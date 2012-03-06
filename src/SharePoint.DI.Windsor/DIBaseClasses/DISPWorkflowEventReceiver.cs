using Castle.Windsor;
using Microsoft.SharePoint.Workflow;

namespace SharePoint.DI.Windsor
{
    public class DISPWorkflowEventReceiver : SPWorkflowEventReceiver
    {
        public DISPWorkflowEventReceiver()
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