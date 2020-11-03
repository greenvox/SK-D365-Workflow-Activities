using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SKWorkflowActivities
{
    public class GetAccountByFetch : CodeActivity
    {
        [RequiredArgument]
        [Input("Filter Type")]
        [Default("and")]
        public InArgument<string> FilterType { get; set; }

        [RequiredArgument]
        [Input("Filters")]
        [Default("<condition attribute='address1_postalcode' operator='eq' value='76021'><condition attribute='address1_city' operator='eq' value='Bedford'>")]
        public InArgument<string> Filters { get; set; }

        [Output("Account")]
        [ReferenceTarget("account")]
        public OutArgument<EntityReference> AccountEntity { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            //var tracingService = executionContext.GetExtension<ITracingService>();

            var filterType = FilterType.Get(executionContext);
            var filters = Filters.Get(executionContext);

            var entity = CrmUtility.GetEntityByUsingFetch(service, Account.EntityLogicalName, filterType, filters);

            if (entity != null)
            {
                var record = entity.ToEntityReference();

                AccountEntity.Set(executionContext, record);
            }
            else
            {
                // TODO: Is this type of exception actually necessary?
                throw new InvalidPluginExecutionException($"A matching record was not found for the specified fetchXml.");
            }
        }
    }
}
