using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SKWorkflowActivities
{
    public class GetContactByAttribute : CodeActivity
    {
        [RequiredArgument] 
        [Input("Attribute Value")] 
        public InArgument<string> FieldValue { get; set; }

        [RequiredArgument]
        [Input("Attribute Name")]
        public InArgument<string> FieldName { get; set; }

        [Output("Contact")]
        [ReferenceTarget("contact")]
        public OutArgument<EntityReference> ContactEntity { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            //var tracingService = executionContext.GetExtension<ITracingService>();

            var fieldValue = FieldValue.Get(executionContext);
            var fieldName = FieldName.Get(executionContext);

            var entity = CrmUtility.GetEntityByAttribute(service, Contact.EntityLogicalName, fieldName, fieldValue, new ColumnSet());

            if (entity != null)
            {
                var record = entity.ToEntityReference();

                ContactEntity.Set(executionContext, record);
            }
            else
            {
                // TODO: Is this type of exception actually necessary?
                throw new InvalidPluginExecutionException($"A matching record was not found for {fieldName}: {fieldValue}");
            }
        }
    }
}
