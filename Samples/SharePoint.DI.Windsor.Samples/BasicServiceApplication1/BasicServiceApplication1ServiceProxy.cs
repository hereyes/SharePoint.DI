using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace SharePoint.DI.Windsor.Samples
{
    [Guid("162d2cf3-80a9-4d8d-a070-af717f5b45f3")]
    class BasicServiceApplication1ServiceProxy
        : SPServiceProxy
    {
        public BasicServiceApplication1ServiceProxy()
        {
        }

        public BasicServiceApplication1ServiceProxy(SPFarm farm)
            : base(String.Empty, farm)
        {
        }
    }
}
