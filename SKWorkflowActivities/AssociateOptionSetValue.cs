using System;
using System.Linq;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SKWorkflowActivities
{
    public class AssociateOptionSetValue : CodeActivity
    {
        [RequiredArgument]
        [Input("OptionSet Label")]
        public InArgument<string> OptionSetLabel { get; set; }

        [RequiredArgument]
        [Input("Attribute Name")]
        public InArgument<string> AttributeName { get; set; }

        [RequiredArgument]
        [Input("Record Id or Url")]
        public InArgument<string> RecordId { get; set; }

        [RequiredArgument]
        [Input("Entity Name")]
        public InArgument<string> EntityName { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            //var tracingService = executionContext.GetExtension<ITracingService>();

            try
            {
                var optionSetLabel = OptionSetLabel.Get(executionContext);
                var attributeName = AttributeName.Get(executionContext);
                var recordId = RecordId.Get(executionContext);
                var entityName = EntityName.Get(executionContext);

                if (recordId.StartsWith("http"))
                {
                    string parsedId = CrmUtility.GetRecordID(recordId);
                    recordId = parsedId;
                }

                var entity = service.Retrieve(entityName, Guid.Parse(recordId), new ColumnSet(attributeName));

                var query = new QueryByAttribute("stringmap")
                {
                    ColumnSet = new ColumnSet("attributevalue")
                };

                query.AddAttributeValue("value", optionSetLabel);
                query.AddAttributeValue("attributename", attributeName);

                var result = service.RetrieveMultiple(query).Entities.FirstOrDefault();

                if (result == null)
                {
                    return;
                }

                entity[attributeName] = result.GetOptionSetValue("attributevalue");

                service.Update(entity);
            }
            catch (Exception)
            {
                // TODO: Why absorbing exception?
            }
        }
    }
}
