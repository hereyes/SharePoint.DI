using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace SharePoint.DI.Windsor.Samples.EventReceiver3
{
    /// <summary>
    /// List Email Events
    /// </summary>
    public class EventReceiver3 : SPEmailEventReceiver
    {
       /// <summary>
       /// The list received an e-mail message.
       /// </summary>
       public override void EmailReceived(SPList list, SPEmailMessage emailMessage, String receiverData)
       {
           base.EmailReceived(list, emailMessage, receiverData);
       }


    }
}
