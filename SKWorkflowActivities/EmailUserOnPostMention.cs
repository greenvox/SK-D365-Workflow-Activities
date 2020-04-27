using System;
using Microsoft.Xrm.Sdk;
using System.Activities;
using System.Linq;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;

namespace SKWorkflowActivities
{
    public class EmailUserOnPostMention : CodeActivity
    {
        #region Exposed Workflow properties

        //[Input("Email Template")]
        //[RequiredArgument]
        //[ReferenceTarget("template")]
        //public InArgument<EntityReference> EmailTemplate
        //{
        //    get;
        //    set;
        //}

        [Input("URL Prefix")]
        [RequiredArgument]
        public InArgument<string> UrlPrefix
        {
            get;
            set;
        }

        #endregion Exposed Workflow properties

        protected override void Execute(CodeActivityContext executionContext)
        {
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var crmService = serviceFactory.CreateOrganizationService(context.UserId);
            var tracingService = executionContext.GetExtension<ITracingService>();

            try
            {
                var urlPrefix = UrlPrefix.Get(executionContext);

                var primaryEntityName = context.PrimaryEntityName;
                var entity = context.PreEntityImages["PreBusinessEntity"];

                var regardingObjectId = entity.GetEntityReference("regardingobjectid");
                var text = entity.GetString("text");
                var mentions = Utility.SplitStringOnBrackets(text);

                tracingService.Trace($"{primaryEntityName} - {regardingObjectId.Id} - {regardingObjectId.LogicalName}");

                var regardingObject = crmService.Retrieve(regardingObjectId.LogicalName, regardingObjectId.Id, new ColumnSet(true));

                var fullName = regardingObject.GetString("fullname");
             
                foreach (var mention in mentions)
                {
                    tracingService.Trace($"Mention: {mention}");
                }

                var htmlTemplate = $"Click the following link to view the {regardingObjectId.LogicalName} record.<a href='{urlPrefix}{regardingObjectId.LogicalName}&id={regardingObjectId.Id}'>{fullName}</a>";

                tracingService.Trace($"Mentions: {mentions.Count}");
                tracingService.Trace($"URL: {htmlTemplate}");

                foreach (var user in mentions.Select(CrmUtility.CreateEntityReferenceFromString))
                {
                    var fromActivityParty = new Entity("activityparty");
                    var toActivityParty = new Entity("activityparty");

                    fromActivityParty["partyid"] = CrmUtility.GetCrmAdminUser(crmService);
                    toActivityParty["partyid"] = user;

                    var email = new Entity("email")
                    {
                        ["from"] = new[] { fromActivityParty },
                        ["to"] = new[] { toActivityParty },
                        ["regardingobjectid"] = regardingObjectId,
                        ["subject"] = $"You have been mentioned in a post on the {regardingObjectId.LogicalName}: {fullName}",
                        ["description"] = htmlTemplate,
                        ["directioncode"] = true
                    };

                    var emailId = crmService.Create(email);

                    var sendEmailRequest = new SendEmailRequest
                    {
                        EmailId = emailId,
                        TrackingToken = string.Empty,
                        IssueSend = true
                    };

                    var sendEmailResponse = (SendEmailResponse)crmService.Execute(sendEmailRequest);
                }


                //var request = new InstantiateTemplateRequest
                //{
                //    TemplateId = emailTemplate.Id,
                //    ObjectId = referenceEntity.Id,
                //    ObjectType = referenceEntity.LogicalName
                //};

                //var response = (InstantiateTemplateResponse)crmService.Execute(request);



            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(
                    $"An error occurred in the {GetType()} plug-in. {Utility.HandleExceptions(ex)}");
            }
        }
    }
}
;