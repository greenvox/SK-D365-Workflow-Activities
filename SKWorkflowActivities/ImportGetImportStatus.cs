using System;
using System.Linq;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace SKWorkflowActivities
{

    public class ImportGetImportStatus : CodeActivity
    {
        [RequiredArgument]
        [Input("Import Id")]
        public InArgument<string> ImportId { get; set; }

        [Output("Import Status")]
        public OutArgument<string> Response { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            var importId = ImportId.Get(executionContext);
            var import = service.Retrieve("import", Guid.Parse(importId), new ColumnSet(true));
            var statusCode = "";
            var statusCodeValue = ((Microsoft.Xrm.Sdk.OptionSetValue)(import["statuscode"])).Value;
            switch (statusCodeValue)
            {
                case 0:
                    statusCode = "Submitted";
                    break;
                case 1:
                    statusCode = "Parsing";
                    break;
                case 2:
                    statusCode = "Transforming";
                    break;
                case 3:
                    statusCode = "Importing";
                    break;
                case 4:
                    statusCode = "Completed";
                    break;
                case 5:
                    statusCode = "Failed";
                    break;
                default:
                    statusCode = "";
                    break;
            };

            Response.Set(executionContext, statusCode);

        }
    }
}


