using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Activities;
using Castle.Windsor;

namespace SharePoint.DI.Windsor
{
    public class DIStateMachineWorkflowActivity : StateMachineWorkflowActivity
    {
        protected override void InitializeProperties()
        {
            base.InitializeProperties();
            InjectProperties();
        }

        private void InjectProperties()
        {
            WindsorContainer container = new WindsorContainer();
            if (container.LoadInstallers() > 0)
            {
                container.InjectProperties(this);
            }

            foreach (var activity in Activities)
            {
                container.InjectProperties(activity);
            }
        }
    }
}
