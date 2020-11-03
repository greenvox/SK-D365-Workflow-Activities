using System;
using System.Activities;
using System.Net;
using System.Text;
using Microsoft.Xrm.Sdk.Workflow;

namespace SKWorkflowActivities
{
    public class PostDocuSignRestAPI : CodeActivity
    {
        /// <summary>
        /// X-DocuSign-Authentication Header contains IntegratorKey, SendOnBehalfOf, Username, and Password.
        /// {"Username":"sarfraz.khan@mcaconnect.com","Password":"encrypytedordecyptedpassword","IntegratorKey":"Get from DocuSign"}
        /// </summary>
        [Input("X-DocuSign-Authentication")]
        [RequiredArgument]
        public InArgument<string> Key { get; set; }

        /// <summary>
        /// Remote web service address.
        /// </summary>
        [Input("Method")]
        [RequiredArgument]
        public InArgument<string> Method { get; set; }

        /// <summary>
        /// Remote web service address.
        /// </summary>
        [Input("Service URL")]
        [RequiredArgument]
        public InArgument<string> Url { get; set; }


        /// <summary>
        /// JSON string.
        /// </summary>
        [Input("JSON")]
        public InArgument<string> Json { get; set; }


        /// <summary>
        /// Web service response content.
        /// </summary>
        [Output("Response")]
        public OutArgument<string> Response { get; set; }

        /// <inheritdoc />
        protected override void Execute(CodeActivityContext context)
        {
            var url = Url.Get(context);
            var json = Json.Get(context);
            var key = Key.Get(context);
            var method = Method.Get(context);

            using (var client = new ExtendedWebClient())
            {
                client.Headers.Add("X-DocuSign-Authentication", key);
                var response = client.UploadString(url, method, json ?? string.Empty);
                Response.Set(context, response);
            }
        }


        /// <inheritdoc />
        protected class ExtendedWebClient : WebClient
        {
            public ExtendedWebClient()
            {
                Encoding = Encoding.UTF8;
                Headers.Add(HttpRequestHeader.ContentType, "application/json");
            }

            protected override WebRequest GetWebRequest(Uri address)
            {
                var w = base.GetWebRequest(address);
                w.Timeout = int.MaxValue;
                return w;
            }
        }
    }
}