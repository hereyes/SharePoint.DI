using Castle.Windsor;
using Microsoft.SharePoint;

namespace SharePoint.DI.Windsor
{
    public class DISPEmailEventReceiver : SPEmailEventReceiver
    {
        public DISPEmailEventReceiver()
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