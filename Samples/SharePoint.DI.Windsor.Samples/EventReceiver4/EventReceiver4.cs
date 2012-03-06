using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace SharePoint.DI.Windsor.Samples.EventReceiver4
{
    /// <summary>
    /// Web Events
    /// </summary>
    public class EventReceiver4 : SPWebEventReceiver
    {
       /// <summary>
       /// A site collection is being deleted.
       /// </summary>
       public override void SiteDeleting(SPWebEventProperties properties)
       {
           base.SiteDeleting(properties);
       }

       /// <summary>
       /// A site is being provisioned.
       /// </summary>
       public override void WebAdding(SPWebEventProperties properties)
       {
           base.WebAdding(properties);
       }

       /// <summary>
       /// A site collection was deleted.
       /// </summary>
       public override void SiteDeleted(SPWebEventProperties properties)
       {
           base.SiteDeleted(properties);
       }

       /// <summary>
       /// A site was provisioned.
       /// </summary>
       public override void WebProvisioned(SPWebEventProperties properties)
       {
           base.WebProvisioned(properties);
       }


    }
}
