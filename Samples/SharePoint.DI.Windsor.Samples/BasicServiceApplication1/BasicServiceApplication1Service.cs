using System;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace SharePoint.DI.Windsor.Samples
{
    [Guid("c69c8df6-845f-42bf-a407-ea6da59a9b0d")]
    class BasicServiceApplication1Service
        : SPService, IServiceAdministration
    {
        public BasicServiceApplication1Service()
        {
        }

        public BasicServiceApplication1Service(SPFarm farm)
            : base(String.Empty, farm)
        {
        }

        public SPServiceApplication CreateApplication(string name,
            Type serviceApplicationType,
            SPServiceProvisioningContext provisioningContext)
        {
            ValidateApplicationType(serviceApplicationType);
            return BasicServiceApplication1ServiceApplication.Create(name, this);
        }

        public SPServiceApplicationProxy CreateProxy(string name,
            SPServiceApplication serviceApplication,
            SPServiceProvisioningContext provisioningContext)
        {
            ValidateApplicationType(serviceApplication.GetType());
            BasicServiceApplication1ServiceProxy serviceProxy = Farm.ServiceProxies.GetValue<BasicServiceApplication1ServiceProxy>();
            return new BasicServiceApplication1ServiceApplicationProxy(name, serviceProxy, serviceApplication.Id);
        }

        public SPPersistedTypeDescription GetApplicationTypeDescription(
            Type serviceApplicationType)
        {
            ValidateApplicationType(serviceApplicationType);
            return new SPPersistedTypeDescription("BasicServiceApplication1 Application", "");
        }

        public override SPAdministrationLink GetCreateApplicationLink(
            Type serviceApplicationType)
        {
            ValidateApplicationType(serviceApplicationType);
            return new SPAdministrationLink("/_admin/SharePoint.DI.Windsor.Samples/BasicServiceApplication1/NewApplication.aspx");
        }

        public override SPCreateApplicationOptions GetCreateApplicationOptions(
            Type serviceApplicationType)
        {
            ValidateApplicationType(serviceApplicationType);
            return SPCreateApplicationOptions.None;
        }

        public Type[] GetApplicationTypes()
        {
            return new Type[] { typeof(BasicServiceApplication1ServiceApplication) };
        }

        void ValidateApplicationType(Type serviceApplicationType)
        {
            if (serviceApplicationType != typeof(BasicServiceApplication1ServiceApplication))
            {
                throw new NotSupportedException();
            }
        }
    }
}
