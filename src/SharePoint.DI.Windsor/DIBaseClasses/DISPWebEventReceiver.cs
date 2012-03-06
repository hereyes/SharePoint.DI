using Castle.Windsor;
using Microsoft.SharePoint;

namespace SharePoint.DI.Windsor
{
    public class DISPWebEventReceiver : SPWebEventReceiver
    {
        public DISPWebEventReceiver()
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