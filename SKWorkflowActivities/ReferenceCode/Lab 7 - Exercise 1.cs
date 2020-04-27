using System;
using Microsoft.Xrm.Sdk;
using System.Activities;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Workflow;

namespace Dynamics_CRM_Workflow_Activity.ReferenceCode
{
    public class Lab_7___Exercise_1 : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            var tracingService = executionContext.GetExtension<ITracingService>();
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var crmService = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                var userWithLowestOwnershipCount = WorkflowUtility.GetLowestUserId(crmService, context.PrimaryEntityName);

                var request = new AssignRequest
                {
                    Assignee = new EntityReference(SystemUser.EntityLogicalName, userWithLowestOwnershipCount),
                    Target = new EntityReference(context.PrimaryEntityName, context.PrimaryEntityId)
                };

                crmService.Execute(request);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(
                 string.Format("An error occurred in the {0} plug-in. {1}", GetType(), Utility.HandleExceptions(ex)));
            }
        }
    }
}
;