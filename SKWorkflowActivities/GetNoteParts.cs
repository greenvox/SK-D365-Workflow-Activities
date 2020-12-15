using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace SKWorkflowActivities
{
    public class GetNoteParts : CodeActivity
    {
        [RequiredArgument]
        [Input("Note")]
        [ReferenceTarget("annotation")]
        public InArgument<EntityReference> Note { get; set; }

        [Output("File Name")]
        public OutArgument<string> FileName { get; set; }

        [Output("File Body")]
        public OutArgument<string> FileBody { get; set; }

        [Output("File Size")]
        public OutArgument<int> FileSize { get; set; }

        [Output("Mime Type")]
        public OutArgument<string> MimeType { get; set; }

        [Output("Message")]
        public OutArgument<string> Message { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            //var tracingService = executionContext.GetExtension<ITracingService>();

            var note = Note.Get(executionContext);

            var document = new Entity();
            document = service.Retrieve(note.LogicalName, note.Id, new ColumnSet(true));

            if (document == null)
            {
                Message.Set(executionContext, "File not found");
                return;
            }
            if ((bool)document["isdocument"] == false)
            {
                Message.Set(executionContext, "Note is not a file");
                return;
            }

            var documentBody = document["documentbody"];
            var fileSize = document["filesize"];
            var fileName = document["filename"];
            var mimeType = document["mimetype"];

            FileBody.Set(executionContext, documentBody);
            FileName.Set(executionContext, fileName);
            FileSize.Set(executionContext, fileSize);
            MimeType.Set(executionContext, mimeType);
            Message.Set(executionContext, "File found");


        }
    }
}
