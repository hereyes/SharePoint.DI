using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace SharePoint.DI.Windsor.Samples
{
    [Guid("b6e891bf-cc72-4817-8dc6-00b0e851217e")]
    class BasicServiceApplication1ServiceApplicationProxy
        : SPServiceApplicationProxy
    {
        [Persisted]
        Guid _serviceApplicationID;

        public override string TypeName
        {
            get { return "BasicServiceApplication1 Application Proxy"; }
        }

        public BasicServiceApplication1ServiceApplication ServiceApplication
        {
            get { return (BasicServiceApplication1ServiceApplication)Farm.GetObject(_serviceApplicationID); }
        }

        public BasicServiceApplication1ServiceApplicationProxy()
        {
        }

        public BasicServiceApplication1ServiceApplicationProxy(string name, BasicServiceApplication1ServiceProxy proxy, Guid serviceApplicationID)
            : base(name, proxy)
        {
            _serviceApplicationID = serviceApplicationID;
        }

        public static BasicServiceApplication1ServiceApplicationProxy Create(string name, BasicServiceApplication1ServiceProxy proxy, Guid serviceApplicationID)
        {
            BasicServiceApplication1ServiceApplicationProxy applicationProxy = new BasicServiceApplication1ServiceApplicationProxy(
                name, proxy, serviceApplicationID);
            applicationProxy.Update();
            return applicationProxy;
        }

        public string SampleMethod()
        {
            return ServiceApplication.SampleMethod();
        }
    }
}