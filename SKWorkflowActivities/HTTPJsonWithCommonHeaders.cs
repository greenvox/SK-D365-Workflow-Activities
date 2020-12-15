using System;
using System.Activities;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.Xrm.Sdk.Workflow;

namespace SKWorkflowActivities
{
    public class HTTPJsonWithCommonHeaders : CodeActivity
    {
        /// <summary>
        /// X-DocuSign-Authentication Header contains IntegratorKey, SendOnBehalfOf, Username, and Password.
        /// {"Username":"sarfraz.khan@mcaconnect.com","Password":"encrypytedordecyptedpassword","IntegratorKey":"Get from DocuSign"}
        /// </summary>
        [Input("Header: Accept")]
        [Default("application/json")]
        public InArgument<string> HeaderAccept { get; set; }

        [Input("Header: Content-Type")]
        [Default("application/json")]
        public InArgument<string> HeaderContentType { get; set; }

        [Input("Header: Authorization")]
        public InArgument<string> HeaderAuthorization { get; set; }

        [Input("Header: Host")]
        public InArgument<string> HeaderHost { get; set; }

        [Input("Header: Origin")]
        public InArgument<string> HeaderOrigin { get; set; }

        [Input("Header: Cache-Control")]
        public InArgument<string> HeaderCacheControl { get; set; }

        [Input("Header: User-Agent")]
        public InArgument<string> HeaderUserAgent { get; set; }

        [Input("Header: Accept-Encoding")]
        public InArgument<string> HeaderAcceptEncoding { get; set; }

        /// <summary>
        /// Remote web service address.
        /// </summary>
        [Input("Method")]
        [RequiredArgument]
        public InArgument<string> Method { get; set; }

        /// <summary>
        /// Remote web service address.
        /// </summary>
        [Input("Service URL (Can have parameters)")]
        [RequiredArgument]
        public InArgument<string> ServiceUrl { get; set; }

        /// <summary>
        /// JSON string.
        /// </summary>
        [Input("Body")]
        public InArgument<string> JsonBody { get; set; }


        /// <summary>
        /// Web service response content.
        /// </summary>
        [Output("Response")]
        public OutArgument<string> Response { get; set; }

        /// <inheritdoc />
        protected override void Execute(CodeActivityContext context)
        {
            var url = ServiceUrl.Get(context);
            var body = JsonBody.Get(context);
            var headerAccept = HeaderAccept.Get(context);
            var headerContentType = HeaderContentType.Get(context);
            var headerAuthorization = HeaderAuthorization.Get(context);
            var headerHost = HeaderHost.Get(context);
            var headerOrigin = HeaderOrigin.Get(context);
            var headerCacheControl = HeaderCacheControl.Get(context);
            var headerUserAgent = HeaderUserAgent.Get(context);
            var headerAcceptEncoding = HeaderAcceptEncoding.Get(context);
            var method = Method.Get(context).ToUpper();

            try
            {
                if (method == "GET")
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    if (headerAccept != null) request.Headers.Add("Accept", headerAccept);
                    if (headerContentType != null) request.Headers.Add("Content-Type", headerContentType);
                    if (headerAuthorization != null) request.Headers.Add("Authorization", headerAuthorization);
                    if (headerHost != null) request.Headers.Add("Host", headerHost);
                    if (headerOrigin != null) request.Headers.Add("Origin", headerOrigin);
                    if (headerCacheControl != null) request.Headers.Add("Cache-Control", headerCacheControl);
                    if (headerUserAgent != null) request.Headers.Add("User-Agent", headerUserAgent);
                    if (headerAcceptEncoding != null) request.Headers.Add("Accept-Encoding", headerAcceptEncoding);
                    request.Method = method;
                    var response = String.Empty;
                    using (HttpWebResponse res = (HttpWebResponse)request.GetResponse())
                    {
                        Stream dataStream = res.GetResponseStream();
                        StreamReader reader = new StreamReader(dataStream);
                        response = reader.ReadToEnd();
                        reader.Close();
                        dataStream.Close();
                    }
                    Response.Set(context, response);
                }

                if (method == "POST" || method == "POST" || method == "PATCH")
                {
                    using (var client = new ExtendedWebClient())
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        if (headerAccept != null) request.Accept = headerAccept;
                        if (headerContentType != null) request.ContentType = headerContentType;
                        if (headerHost != null) request.Host = headerHost;
                        if (headerAuthorization != null) request.Headers.Add("Authorization", headerAuthorization);
                        if (headerOrigin != null) request.Headers.Add("Origin", headerOrigin);
                        if (headerCacheControl != null) request.Headers.Add("Cache-Control", headerCacheControl);
                        if (headerUserAgent != null) request.Headers.Add("User-Agent", headerUserAgent);
                        if (headerAcceptEncoding != null) request.Headers.Add("Accept-Encoding", headerAcceptEncoding);
                        var response = client.UploadString(url, method, body ?? string.Empty);
                        Response.Set(context, response);
                    }
                }
            }
            catch (Exception ex)
            {
                while (ex != null)
                {
                    Response.Set(context, ex.Message);
                    ex = ex.InnerException;
                }
            };
        }
        /// <inheritdoc />
        protected class ExtendedWebClient : WebClient
        {
            public ExtendedWebClient()
            {
                Encoding = Encoding.UTF8;
                //Headers.Add(HttpRequestHeader.ContentType, "application/json");
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