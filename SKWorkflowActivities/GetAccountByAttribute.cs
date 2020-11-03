using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SKWorkflowActivities
{
    public class GetAccountByAttribute : CodeActivity
    {
        [RequiredArgument]
        [Input("Attribute Name")]
        public InArgument<string> FieldName { get; set; }

        [RequiredArgument]
        [Input("Attribute Value")]
        public InArgument<string> FieldValue { get; set; }

        [Output("Account")]
        [ReferenceTarget("account")]
        public OutArgument<EntityReference> AccountEntity { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            //var tracingService = executionContext.GetExtension<ITracingService>();

            var fieldValue = FieldValue.Get(executionContext);
            var fieldName = FieldName.Get(executionContext);

            var entity = CrmUtility.GetEntityByAttribute(service, Account.EntityLogicalName, fieldName, fieldValue, new ColumnSet());

            if (entity != null)
            {
                var record = entity.ToEntityReference();

                AccountEntity.Set(executionContext, record);
            }
            else
            {
                // TODO: Is this type of exception actually necessary?
                throw new InvalidPluginExecutionException($"A matching record was not found for {fieldName}: {fieldValue}");
            }
        }
    }
}
