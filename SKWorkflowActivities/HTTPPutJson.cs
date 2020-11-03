using System;
using System.Activities;
using System.Net;
using System.Text;
using Microsoft.Xrm.Sdk.Workflow;

namespace SKWorkflowActivities
{
    /// <summary>
    /// Making a PUT request to a remote web service and sending the data in JSON format.
    /// </summary>
    public class HTTPPutJson : CodeActivity
    {
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
            using (var client = new ExtendedWebClient())
            {
                var response = client.UploadString(url, "PUT", json ?? string.Empty);
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