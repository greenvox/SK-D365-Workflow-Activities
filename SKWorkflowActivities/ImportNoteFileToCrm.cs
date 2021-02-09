using System;
using System.Linq;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;
using System.Text;
using System.IO;

namespace SKWorkflowActivities
{

    public class ImportNoteFileToCrm : CodeActivity
    {
        [RequiredArgument]
        [Input("Import Map Id")]
        public InArgument<string> ImportMapId { get; set; }
        
        [RequiredArgument]
        [Input("AnnotationId")]
        public InArgument<string> Note { get; set; }

        [RequiredArgument]
        [Input("Import Name")]
        [Default("My Data Import")]
        public InArgument<string> ImportName { get; set; }

        [RequiredArgument]
        [Input("Dedupe")]
        public InArgument<bool> Dedupe { get; set; }

        [Output("Response")]
        public OutArgument<string> Response { get; set; }

        [Output("ImportId")]
        public OutArgument<string> ImportId { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);


            // Get the Guid of the DataMap by using the datamap name
            string mapString = ImportMapId.Get(executionContext);
            Guid importMapId = Guid.Parse(mapString);
            var noteId = Note.Get(executionContext);
            var noteRec = service.Retrieve("annotation", Guid.Parse(noteId), new ColumnSet(true));
            string importName = ImportName.Get(executionContext);

            // Create Import
            var import = new Entity("import");

            // IsImport is obsolete; use ModeCode to declare Create or Update.
            import["modecode"] = new OptionSetValue((int)ImportModeCode.Create);
            import["name"] = importName;

            var importId = service.Create(import);

            // Get the Guid of the DataMap by using the datamap name
            var importMap = service.Retrieve("importmap", importMapId, new ColumnSet(true));

            // Get the Mapping Set
            var query = new QueryExpression("importentitymapping");
            query.ColumnSet.AllColumns = true;
            query.Criteria.AddCondition("importmapid", ConditionOperator.Equal, importMap.Id);
            Entity importEntityMap = service.RetrieveMultiple(query).Entities.FirstOrDefault();


            //Create a copy of the mapping set.
            var newMap = new Entity("importentitymapping");
            newMap["processcode"] = importEntityMap["processcode"];
            newMap["sourceentityname"] = Path.GetFileNameWithoutExtension((string)noteRec["filename"]);
            newMap["targetentityname"] = importEntityMap["targetentityname"];
            newMap["importmapid"] = importEntityMap["importmapid"];
            newMap["dedupe"] = importEntityMap["dedupe"];

            var newImportEntityMap = service.Create(newMap);

            // Create the ImportFile class
            var importFile = new Entity("importfile");
            
            // Get note body
            byte[] data = Convert.FromBase64String((string)noteRec["documentbody"]);
            string decodedString = Encoding.UTF8.GetString(data);

            // Get the current user to set as record owner.
            var systemUserRequest = new WhoAmIRequest();
            var systemUserResponse =
                (WhoAmIResponse)service.Execute(systemUserRequest);

            // Add the xml/csv document content
            importFile["content"] = decodedString; //decodedString;
            importFile["name"] = noteRec["filename"];
            importFile["filetypecode"] = new OptionSetValue(0); //0 = CSV, 1 = XML, 3 = XLS
            importFile["isfirstrowheader"] = true;
            importFile["source"] = noteRec["filename"];
            importFile["sourceentityname"] = Path.GetFileNameWithoutExtension((string)noteRec["filename"]);

            // Schema name of the Target entity
            importFile["targetentityname"] = importEntityMap["targetentityname"];
            importFile["importmapid"] = new EntityReference("importmap", importMapId);
            importFile["importid"] = new EntityReference("import", importId);
            importFile["size"] = (importFile["content"].ToString()).Length.ToString();
            importFile["fielddelimitercode"] = new OptionSetValue((int)ImportFileFieldDelimiterCode.Comma);
            importFile["datadelimitercode"] = new OptionSetValue((int)ImportFileDataDelimiterCode.DoubleQuote);
            importFile["processcode"] = new OptionSetValue((int)ImportFileProcessCode.Process);
            importFile["usesystemmap"] = false;
            importFile["enableduplicatedetection"] = Dedupe.Get(executionContext);
            // Set the owner ID.				
            importFile["recordsownerid"] =
                new EntityReference(SystemUser.EntityLogicalName, systemUserResponse.UserId);

            var importFileId = service.Create(importFile);

            ParseImportRequest parseRequest = new ParseImportRequest();
            parseRequest.ImportId = importId;
            service.Execute(parseRequest);
            TransformImportRequest transRequest = new TransformImportRequest();
            transRequest.ImportId = importId;
            TransformImportResponse transResponse = (TransformImportResponse)service.Execute(transRequest);
            ImportRecordsImportRequest request = new ImportRecordsImportRequest();
            request.ImportId = importId;
            ImportRecordsImportResponse response = (ImportRecordsImportResponse)service.Execute(request);

            Response.Set(executionContext, response.ResponseName);
            ImportId.Set(executionContext, importId.ToString());

        }
    }
}


