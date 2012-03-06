using Microsoft.SharePoint.Client.Services;
using System.ServiceModel.Activation;

namespace SharePoint.DI.Windsor.Samples
{
    [BasicHttpBindingServiceMetadataExchangeEndpoint]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class WCFService1 : IWCFService1
    {
        // To test this service, use the Visual Studio WCF Test client
        // set the endpoint to http://<Your server name>/_vti_bin/SharePoint.DI.Windsor.Samples/WCFService1.svc/mex
        public string HelloWorld()
        {
            return "Hello World from WCF and SharePoint 2010";
        }
    }
}
