using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SKWorkflowActivities
{
    public class GetLeadByAttribute : CodeActivity
    {
        [RequiredArgument]
        [Input("Attribute Value")]
        public InArgument<string> FieldValue { get; set; }

        [RequiredArgument] 
        [Input("Attribute Name")] 
        public InArgument<string> FieldName { get; set; }

        [Output("Lead")] 
        [ReferenceTarget("lead")] 
        public OutArgument<EntityReference> LeadEntity { get; set; }

        [Output("Status")] 
        public OutArgument<string> Status { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            //var tracingService = executionContext.GetExtension<ITracingService>();

            var fieldValue = FieldValue.Get(executionContext);
            var fieldName = FieldName.Get(executionContext);

            var entity = CrmUtility.GetEntityByAttribute(service, Lead.EntityLogicalName, fieldName, fieldValue, new ColumnSet());

            if (entity != null)
            {
                LeadEntity.Set(executionContext, entity.ToEntityReference());
            }
            else
            {
                Status.Set(executionContext, "NO RECORD");
            }
        }
    }
}
