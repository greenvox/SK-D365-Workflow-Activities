using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using Microsoft.Xrm.Sdk.Query;
using System.Text;

namespace SKWorkflowActivities
{
    public class FixCsvFile : CodeActivity
    {

        [Input("Note")]
        [ReferenceTarget("annotation")]
        public InOutArgument<EntityReference> AnnotationId { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            //tracingService = executionContext.GetExtension<ITracingService>();

            var annotationId = AnnotationId.Get(executionContext);

            var record = service.Retrieve("annotation", annotationId.Id, new ColumnSet("documentbody"));
            var doc = record["documentbody"];

            byte[] data = Convert.FromBase64String((string)record["documentbody"]);
            string terminatorString = Encoding.UTF8.GetString(data);
            string decodedString = BulkImportHelper.RemoveLastComma(terminatorString);

            var newfile = new Entity("annotation",annotationId.Id);
            newfile["documentbody"] = Convert.ChangeType(decodedString, doc.GetType());

            service.Update(newfile);

            AnnotationId.Set(executionContext, annotationId);

        }
    }
}