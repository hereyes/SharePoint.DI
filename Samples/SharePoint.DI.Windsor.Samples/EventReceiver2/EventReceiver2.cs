using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace SharePoint.DI.Windsor.Samples.EventReceiver2
{
    /// <summary>
    /// List Events
    /// </summary>
    public class EventReceiver2 : SPListEventReceiver
    {
       /// <summary>
       /// A field was added.
       /// </summary>
       public override void FieldAdded(SPListEventProperties properties)
       {
           base.FieldAdded(properties);
       }


    }
}
