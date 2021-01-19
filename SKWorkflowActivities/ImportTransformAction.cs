using System;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk.Messages;

namespace SKWorkflowActivities
{
    public class ImportTransformAction : CodeActivity
    {
        [RequiredArgument]
        [Input("ImportId")]
        public InArgument<string> ImportId { get; set; }

        [Output("Response")]
        public OutArgument<string> ResponseName { get; set; }
        protected override void Execute(CodeActivityContext executionContext)
        {

            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);

            var importId = ImportId.Get(executionContext);

            TransformImportRequest transRequest = new TransformImportRequest();
            transRequest.ImportId = Guid.Parse(importId);
            TransformImportResponse transResponse = (TransformImportResponse)service.Execute(transRequest);
            ResponseName.Set(executionContext, transResponse.ResponseName);

        }
    }
}
