using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace SKWorkflowActivities
{ 
    public class GetOpportunityProductByAttribute : CodeActivity
    {
        [Input("Attribute Value")]
        public InArgument<string> FieldValue { get; set; }

        [Input("Attribute Name")]
        public InArgument<string> FieldName { get; set; }

        [Input("Opportunity")]
        [ReferenceTarget("opportunity")]
        public InArgument<EntityReference> OpportunityEntity { get; set; }

        [RequiredArgument]
        [Input("Search By Opportunity?")]
        public InArgument<bool> SearchBy { get; set; }

        [Output("Opportunity Product")]
        [ReferenceTarget("opportunityproduct")]
        public OutArgument<EntityReference> OpportunityProductEntity { get; set; }

        [Output("Status")]
        public OutArgument<string> Status { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            //var tracingService = executionContext.GetExtension<ITracingService>();

            var searchBy = SearchBy.Get(executionContext);
            var fieldName = string.Empty;
            var fieldValue = string.Empty;

            if (searchBy == false)
            {
                fieldValue = FieldValue.Get(executionContext);
                fieldName = FieldName.Get(executionContext);
            }
            else if (searchBy == true) {
                var opportunity = OpportunityEntity.Get(executionContext);
                fieldName = opportunity.LogicalName;
                fieldValue = Convert.ToString(opportunity.Id);
            }
            var entity = CrmUtility.GetEntityByAttribute(service, OpportunityProduct.EntityLogicalName, fieldName, fieldValue, new ColumnSet());

            if (entity != null)
            {
                OpportunityProductEntity.Set(executionContext, entity.ToEntityReference());
            }
            else
            {
                Status.Set(executionContext, "NO RECORD");
            }
        }
    }
}
