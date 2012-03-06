using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace SharePoint.DI.Windsor.Samples.EventReceiver5
{
    /// <summary>
    /// List Workflow Events
    /// </summary>
    public class EventReceiver5 : SPWorkflowEventReceiver
    {
       /// <summary>
       /// A workflow is starting.
       /// </summary>
       public override void WorkflowStarting(SPWorkflowEventProperties properties)
       {
           base.WorkflowStarting(properties);
       }


    }
}
