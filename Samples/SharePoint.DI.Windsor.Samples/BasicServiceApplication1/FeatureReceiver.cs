using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace SharePoint.DI.Windsor.Samples
{
    public class BasicServiceApplication1FeatureReceiver
        : SPFeatureReceiver
    {
        public override void FeatureActivated(
            SPFeatureReceiverProperties properties)
        {
            SPFarm farm = SPFarm.Local;
            BasicServiceApplication1Service service = farm.Services.GetValue<BasicServiceApplication1Service>();
            if (service == null)
            {
                service = new BasicServiceApplication1Service(farm);
                service.Update();
            }
            BasicServiceApplication1ServiceProxy proxy = farm.ServiceProxies.GetValue<BasicServiceApplication1ServiceProxy>();
            if (proxy == null)
            {
                proxy = new BasicServiceApplication1ServiceProxy(farm);
                proxy.Update();
            }
        }

        public override void FeatureDeactivating(
            SPFeatureReceiverProperties properties)
        {
            SPFarm farm = SPFarm.Local;
            BasicServiceApplication1ServiceProxy proxy = farm.ServiceProxies.GetValue<BasicServiceApplication1ServiceProxy>();
            if (proxy != null)
            {
                proxy.Delete();
            }
            BasicServiceApplication1Service service = farm.Services.GetValue<BasicServiceApplication1Service>();
            if (service != null)
            {
                service.Delete();
            }
        }
    }
}
