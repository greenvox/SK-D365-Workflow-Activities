using System;
using System.Activities;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using Microsoft.Xrm.Sdk.Workflow;

namespace SKWorkflowActivities
{
    public enum ddHttpVerbs
    {
        GET,
        POST,
        PUT,
        PATCH
    }
    public class HTTPPostJsonWithHeader : CodeActivity
    {
        /// <summary>
        /// X-DocuSign-Authentication Header contains IntegratorKey, SendOnBehalfOf, Username, and Password.
        /// {"Username":"sarfraz.khan@mcaconnect.com","Password":"encrypytedordecyptedpassword","IntegratorKey":"Get from DocuSign"}
        /// </summary>
        [Input("JSON Header")]
        public InArgument<string> JsonHeader { get; set; }

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
        [Input("JSON Body")]
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
            var parameters = "";
            var body = JsonBody.Get(context);
            var headerRaw = JsonHeader.Get(context);
            var method = Method.Get(context).ToUpper();

            var header = headerRaw.Replace('"', '\"');
            //var url = WebUtility.UrlEncode(urlRaw);

            Dictionary<string, string> SerializedHeader;
            using (MemoryStream DeSerializememoryStream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Dictionary<string, string>));
                StreamWriter writer = new StreamWriter(DeSerializememoryStream);
                writer.Write(header);
                writer.Flush();

                DeSerializememoryStream.Position = 0;
                SerializedHeader = (Dictionary<string, string>)serializer.ReadObject(DeSerializememoryStream);
            }
            try
            {
                if (method == "GET")
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    foreach (var key in SerializedHeader.Keys)
                    {
                        request.Headers.Add(key, SerializedHeader[key]);
                    }
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
                        foreach (var key in SerializedHeader.Keys)
                        {
                            client.Headers.Add(key, SerializedHeader[key]);
                        }
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