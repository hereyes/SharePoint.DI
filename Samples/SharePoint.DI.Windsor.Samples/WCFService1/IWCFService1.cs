using System.ServiceModel;

namespace SharePoint.DI.Windsor.Samples
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWCFService1" in both code and config file together.
    [ServiceContract]
    public interface IWCFService1
    {
        [OperationContract]
        string HelloWorld();
    }
}

