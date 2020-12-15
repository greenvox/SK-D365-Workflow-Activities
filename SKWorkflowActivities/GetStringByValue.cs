using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;

namespace SKWorkflowActivities
{
    public class GetStringByValue : CodeActivity
    {
        [RequiredArgument] 
        [Input("Search Value")] 
        public InArgument<string> FieldValue { get; set; }

        [RequiredArgument]
        [Input("Search Field")]
        public InArgument<string> FieldName { get; set; }

        [RequiredArgument] 
        [Input("Search Entity")] 
        public InArgument<string> FieldEntity { get; set; }

        [RequiredArgument]
        [Input("Result Field")] 
        public InArgument<string> ResultField { get; set; }

        [RequiredArgument] 
        [Input("Filter Condition")]
        [Default("and")]
        [ArgumentDescription(@"and OR or")]
        public InArgument<string> Filter { get; set; }

        [Input("Additional Fetch Conditions")] 
        [ArgumentDescription(@"<condition attribute='createdon' operator='last-x-hours' value='1' />")]
        public InArgument<string> FetchFilters { get; set; }

        [Input("Order By")]
        [Default("createdon")]
        public InArgument<string> OrderBy { get; set; }
        
        [Input("Sort Order Descending")]
        public InArgument<bool> DescendingOrder { get; set; }


        [Output("Response")]
        public OutArgument<string> Response { get; set; }

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
            var entityName = FieldEntity.Get(executionContext);
            var resultField = ResultField.Get(executionContext);
            var fetchFilters = FetchFilters.Get(executionContext);
            var orderBy = OrderBy.Get(executionContext);
            var descendingOrder = DescendingOrder.Get(executionContext);
            var order = string.Empty;
            if (orderBy != null)
            {
                order = $@"<order attribute='{orderBy}' descending='{descendingOrder}' />";
            }

            var filter = Filter.Get(executionContext);
            var status = "Failure";

            var value = CrmUtility.GetStringByValueUsingFetch(service, entityName, resultField, filter, fieldName, fieldValue, fetchFilters, orderBy);

            if (!string.IsNullOrEmpty(value))
            {
                Response.Set(executionContext, value);

                status = "Success";
            }

            Status.Set(executionContext, status);
        }
    }
}
