using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SKWorkflowActivities
{
    public class GetOpportunityByAttribute : CodeActivity
    {
        [RequiredArgument]
        [Input("Attribute Value")]
        public InArgument<string> FieldValue { get; set; }

        [RequiredArgument]
        [Input("Attribute Name")]
        public InArgument<string> FieldName { get; set; }

        [Output("Opportunity")]
        [ReferenceTarget("opportunity")]
        [Default(null)]
        public OutArgument<EntityReference> OpportunityEntity { get; set; }

        [Output("Contact")]
        [ReferenceTarget("contact")]
        [Default(null)]
        public OutArgument<EntityReference> ContactEntity { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            //var tracingService = executionContext.GetExtension<ITracingService>();

            var fieldValue = FieldValue.Get(executionContext);
            var fieldName = FieldName.Get(executionContext);
            
            var entity = CrmUtility.GetEntityByAttribute(service, Opportunity.EntityLogicalName, fieldName, fieldValue, 
                new ColumnSet("parentcontactid"));

            var opportunityEntity = entity.ToEntityReference();
            var contactEntity = entity.GetEntityReference("parentcontactid");

            OpportunityEntity.Set(executionContext, opportunityEntity);
            ContactEntity.Set(executionContext, contactEntity);
        }
    }
}
