using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;

namespace SKWorkflowActivities
{
    public class FileUpload : CodeActivity
    {
        [Input("documentbody")]
        public InArgument<string> documentbody { get; set; }

        [Input("filename")]
        public InArgument<string> filename { get; set; }

        [Input("subject")]
        [Default("Related Upload")]
        public InArgument<string> subject { get; set; }

        [Input("notetext")]
        [Default("File Uploaded Externally.")]
        public InArgument<string> notetext { get; set; }

        [Input("objecttypecode")]
        [Default("3")]
        public InArgument<int> objecttypecode { get; set; }
        [Input("mimetype")]

        [Default(@"text/plain")]
        public InArgument<string> mimetype { get; set; }
        [Input("isdocument")]

        [Default("true")]
        public InArgument<bool> isdocument { get; set; }

        [Input("recordid")]
        public InArgument<string> recordid { get; set; }

        [Input("entityname")]
        public InArgument<string> entityname { get; set; }

        [Output("AnnotationId")]
        public OutArgument<string> annotationid { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            //tracingService = executionContext.GetExtension<ITracingService>();

            var documentBody = documentbody.Get(executionContext);
            var subject1 = subject.Get(executionContext);
            var fileName = filename.Get(executionContext);
            var noteText = notetext.Get(executionContext);
            var isDocument = isdocument.Get(executionContext);
            var mimeType = mimetype.Get(executionContext);
            var entityName = entityname.Get(executionContext);
            var recordId = recordid.Get(executionContext);
            var objectTypeCode = objecttypecode.Get(executionContext);

            var annotation = new Annotation
            {
                ObjectId = new EntityReference(entityName, Guid.Parse(recordId)),
                ["objecttypecode"] = objectTypeCode,
                Subject = subject1,
                DocumentBody = documentBody,
                MimeType = mimeType,
                NoteText = noteText,
                IsDocument = isDocument,
                FileName = fileName
            };

            var response = service.Create(annotation);

            annotationid.Set(executionContext, response.ToString());

        }
    }
}