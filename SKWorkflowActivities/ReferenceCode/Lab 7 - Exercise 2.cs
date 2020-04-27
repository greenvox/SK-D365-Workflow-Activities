using System;
using Microsoft.Xrm.Sdk;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;

namespace Dynamics_CRM_Workflow_Activity.ReferenceCode
{
    public class Lab_7___Exercise_2 : CodeActivity
    {
        #region Exposed Workflow properties

        [Input("Email")]
        [RequiredArgument]
        public InArgument<string> Email
        {
            get;
            set;
        }

        [Input("Company Email Domain")]
        [RequiredArgument]
        public InArgument<string> CompanyEmailDomain
        {
            get;
            set;
        }

        [Output("Valid Email Address")]
        [RequiredArgument]
        public OutArgument<bool> Valid
        {
            get;
            set;
        }

        #endregion Exposed Workflow properties

        protected override void Execute(CodeActivityContext executionContext)
        {
            var tracingService = executionContext.GetExtension<ITracingService>();
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var crmService = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                var emailAddress = Email.Get(executionContext);
                var companyEmailDomain = CompanyEmailDomain.Get(executionContext);
                var isValid = !emailAddress.Contains(companyEmailDomain);

                Valid.Set(executionContext, isValid);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(
                 string.Format("An error occurred in the {0} plug-in. {1}", GetType(), Utility.HandleExceptions(ex)));
            }
        }
    }
}
