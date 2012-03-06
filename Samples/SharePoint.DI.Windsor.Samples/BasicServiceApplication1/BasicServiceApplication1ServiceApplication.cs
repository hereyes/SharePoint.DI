using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace SharePoint.DI.Windsor.Samples
{
    [Guid("70a855fa-4d0f-4787-9d54-d64ef8fb3ea4")]
    class BasicServiceApplication1ServiceApplication
        : SPServiceApplication
    {
        public override string TypeName
        {
            get { return "BasicServiceApplication1 Application"; }
        }


        public BasicServiceApplication1ServiceApplication()
        {
        }

        public BasicServiceApplication1ServiceApplication(string name, BasicServiceApplication1Service service)
            : base(name, service)
        {
        }

        public static BasicServiceApplication1ServiceApplication Create(
            string name, BasicServiceApplication1Service service)
        {
            BasicServiceApplication1ServiceApplication serviceApplication = new BasicServiceApplication1ServiceApplication(name, service);
            serviceApplication.Update();
            return serviceApplication;
        }

        public string SampleMethod()
        {
            return "Hello Service Application World";
        }
    }
}
