using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SKWorkflowActivities
{
    public class GetNoteByAttribute : CodeActivity
    {
        [RequiredArgument] 
        [Input("Attribute Value")] 
        public InArgument<string> FieldValue { get; set; }

        [RequiredArgument]
        [Input("Attribute Name")]
        public InArgument<string> FieldName { get; set; }

        [Input("Additional Fetch Conditions")]
        public InArgument<string> FetchFilters { get; set; }

        [Input("Order By")]
        public InArgument<string> OrderBy { get; set; }

        [Input("Order By Descending")]
        public InArgument<string> OrderDescending { get; set; }



        [Output("Note")]
        [ReferenceTarget("annotation")]
        public OutArgument<EntityReference> ReferenceEntity { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            //var tracingService = executionContext.GetExtension<ITracingService>();

            var fieldValue = FieldValue.Get(executionContext);
            var fieldName = FieldName.Get(executionContext);
            var filter = "and";
            var fetchFilters = FetchFilters.Get(executionContext);
            var orderBy = OrderBy.Get(executionContext);
            var descendingOrder = OrderDescending.Get(executionContext);
            var order = string.Empty;
            if (orderBy != null)
            {
                order = $@"<order attribute='{orderBy}' descending='{descendingOrder}' />";
            }
            var entity = CrmUtility.GetEntityByUsingFetch(service,Annotation.EntityLogicalName,filter,fetchFilters,order);

            if (entity != null)
            {
                var record = entity.ToEntityReference();

                ReferenceEntity.Set(executionContext, record);
            }
            else
            {
                // TODO: Is this type of exception actually necessary?
                throw new InvalidPluginExecutionException($"A matching record was not found for {fieldName}: {fieldValue}");
            }
        }
    }
}
