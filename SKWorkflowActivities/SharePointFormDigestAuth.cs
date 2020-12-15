using System;
using System.Linq;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Net;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Text;

namespace SKWorkflowActivities
{
    public class SharePointFormDigestAuth : CodeActivity
    {
        [RequiredArgument]
        [Input("Username")]
        public InArgument<string> Username { get; set; }

        [RequiredArgument]
        [Input("Password")]
        public InArgument<string> Password { get; set; }

        [RequiredArgument]
        [Input("End Point (Site)")]
        [Default("https://yourdomain.sharepoint.com/sites/mysite")]
        public InArgument<string> EndPoint { get; set; }

        [RequiredArgument]
        [Input("STS EndPoint")]
        [Default("https://login.microsoftonline.com/extSTS.srf")]
        public InArgument<string> StsEndPoint { get; set; }

        [RequiredArgument]
        [Input("Sign-in Url")]
        [Default("https://yourdomain.sharepoint.com/_forms/default.aspx?wa=wsignin1.0")]
        public InArgument<string> SignInUrl { get; set; }

        [RequiredArgument]
        [Input("Browser Host")]
        [Default("yourdomain.sharepoint.com")]
        public InArgument<string> BrowserHost { get; set; }

        [RequiredArgument]
        [Input("Browser User Agent")]
        [Default("Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.86 Safari/537.36")]
        public InArgument<string> BrowserUserAgent { get; set; }

        [Output("Cookies")]
        public OutArgument<string> HeaderCookies { get; set; }

        [Output("X-RequestDigest")]
        public OutArgument<string> HeaderXRequestDigest { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            var tracingService = executionContext.GetExtension<ITracingService>();

            var username = Username.Get(executionContext);
            var password = Password.Get(executionContext);
            var endPoint = EndPoint.Get(executionContext);
            var stsEndpoint = StsEndPoint.Get(executionContext);
            var signInUrl = SignInUrl.Get(executionContext);
            var browserHost = BrowserHost.Get(executionContext); //e.g. mycompany.sharepoint.com
            var browserUserAgent = BrowserUserAgent.Get(executionContext);

            var cookies = new CookieContainer();
            //var SMAL = "[SMAL XML]"; 

            string SMAL = @"<s:Envelope xmlns:s=""http://www.w3.org/2003/05/soap-envelope""
                                xmlns:a=""http://www.w3.org/2005/08/addressing"" xmlns:u=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">
                                <s:Header>
                                    <a:Action s:mustUnderstand=""1"">http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue</a:Action>
                                    <a:ReplyTo>
                                        <a:Address>http://www.w3.org/2005/08/addressing/anonymous</a:Address>
                                    </a:ReplyTo>
                                    <a:To s:mustUnderstand=""1"">https://login.microsoftonline.com/extSTS.srf</a:To>
                                    <o:Security s:mustUnderstand=""1"" xmlns:o=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"">
                                        <o:UsernameToken>
                                            <o:Username>" + username + @"</o:Username>
                                            <o:Password>" + password + @"</o:Password>
                                        </o:UsernameToken>
                                    </o:Security>
                                </s:Header>
                                <s:Body>
                                    <t:RequestSecurityToken xmlns:t=""http://schemas.xmlsoap.org/ws/2005/02/trust"">
                                        <wsp:AppliesTo xmlns:wsp=""http://schemas.xmlsoap.org/ws/2004/09/policy"">
                                            <a:EndpointReference>
                                                <a:Address>" + endPoint + @"</a:Address>
                                            </a:EndpointReference>
                                        </wsp:AppliesTo>
                                        <t:KeyType>http://schemas.xmlsoap.org/ws/2005/05/identity/NoProofKey</t:KeyType>
                                        <t:RequestType>http://schemas.xmlsoap.org/ws/2005/02/trust/Issue</t:RequestType>
                                        <t:TokenType>urn:oasis:names:tc:SAML:1.0:assertion</t:TokenType>
                                    </t:RequestSecurityToken>
                                </s:Body>
                            </s:Envelope>";
            var payLoadSts = SMAL.Replace("{USERNAME}", username).Replace("{PASSWORD}", password).Replace("{ENDPOINTREFERENCE}", endPoint);



            // webrequest 0
            // webrequest 0: get the RpsContextCookie
            // webrequest 0
            string requiredAuthUrl = string.Format("{0}/_layouts/15/Authenticate.aspx?Source={1}", endPoint, WebUtility.UrlDecode(endPoint));
            HttpWebRequest request0 = (HttpWebRequest)WebRequest.Create(requiredAuthUrl);
            request0.Accept = "*/*";
            request0.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-US");
            request0.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            request0.UserAgent = browserUserAgent;
            request0.AllowAutoRedirect = false;
            request0.CookieContainer = cookies;

            var response0 = (HttpWebResponse)request0.GetResponse();

            // webrequest 1
            // webrequest 1: login
            // webrequest 1
            var request1 = (HttpWebRequest)WebRequest.Create(stsEndpoint);
            request1.CookieContainer = cookies;
            var data = Encoding.ASCII.GetBytes(payLoadSts);

            request1.Method = "POST";
            request1.ContentType = "application/xml";
            request1.ContentLength = data.Length;

            using (var stream1 = request1.GetRequestStream())
            {
                stream1.Write(data, 0, data.Length);
            }

            // Response 1
            var response1 = (HttpWebResponse)request1.GetResponse();
            var responseString = new StreamReader(response1.GetResponseStream()).ReadToEnd();

            // Get BinarySecurityToken
            var xData = XDocument.Parse(responseString);
            var namespaceManager = new XmlNamespaceManager(new NameTable());
            namespaceManager.AddNamespace("S", "http://www.w3.org/2003/05/soap-envelope");
            namespaceManager.AddNamespace("wst", "http://schemas.xmlsoap.org/ws/2005/02/trust");
            namespaceManager.AddNamespace("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            var BinarySecurityToken = xData.XPathSelectElement("/S:Envelope/S:Body/wst:RequestSecurityTokenResponse/wst:RequestedSecurityToken/wsse:BinarySecurityToken", namespaceManager);

            // webrequest 2
            // webrequest 2 //_forms/default.aspx?wa=wsignin1.0
            // webrequest 2
            var request2 = (HttpWebRequest)WebRequest.Create(signInUrl);

            var data2 = Encoding.ASCII.GetBytes(BinarySecurityToken.Value);
            request2.Method = "POST";
            request2.Accept = "*/*";
            request2.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-US");
            request2.ContentType = "application/x-www-form-urlencoded";
            request2.UserAgent = browserUserAgent;
            request2.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            request2.Host = browserHost;
            request2.ContentLength = data2.Length;
            request2.CookieContainer = cookies;

            using (var stream2 = request2.GetRequestStream())
            {
                stream2.Write(data2, 0, data2.Length);
            }

            // Response 2
            var response2 = (HttpWebResponse)request2.GetResponse();
            var responseString2 = new StreamReader(response2.GetResponseStream()).ReadToEnd();
            // webrequest 3
            // webrequest 3: get X-RequestDigest
            // webrequest 3
            string restUrl3 = string.Format("{0}/_api/contextinfo", endPoint);
            var request3 = (HttpWebRequest)WebRequest.Create(restUrl3);
            request3.CookieContainer = cookies;
            request3.Method = "POST";
            request3.ContentLength = 0;

            // Response 3
            string formDigest = string.Empty;
            using (var response3 = (HttpWebResponse)request3.GetResponse())
            {
                using (var reader = new StreamReader(response3.GetResponseStream()))
                {
                    var result = reader.ReadToEnd();

                    // parse the ContextInfo response
                    var resultXml = XDocument.Parse(result);

                    // get the form digest value
                    var x = from y in resultXml.Descendants()
                            where y.Name == XName.Get("FormDigestValue", "http://schemas.microsoft.com/ado/2007/08/dataservices")
                    select y;
                    formDigest = x.First().Value;
                }
            }

            HeaderCookies.Set(executionContext, cookies.GetCookieHeader(new Uri(restUrl3)));
            HeaderXRequestDigest.Set(executionContext, formDigest);
            return;
            ////Rest Call 2
            //var restUrl = string.Format("{0}/sites/{sitetitleinurl}/_api/Web/GetFileByServerRelativeUrl(‘/sites/{sitetitleinurl}/Shared Documents/PPM Actuals & Forecasts/Approved Actuals (QV data).xlsx’)/$value", endPoint);
            //var req = (HttpWebRequest)WebRequest.Create(restUrl);
            //req.CookieContainer = cookies;
            ////req.CookieContainer.Add(resp.Cookies);
            //req.Method = "GET";
            //req.Accept = "application/json; odata=verbose";
            //req.ContentType = "application/json;odata=verbose";
            //req.Headers.Add("X-RequestDigest", formDigest);
            ////req.Headers.Add("Cookie", resp.Cookies[0].Value);
            ////req.Headers.Add("Cookie", resp.Cookies[1].Value);
            //var resp = (HttpWebResponse)req.GetResponse();
            //Console.WriteLine("Read Complete.");
            //Console.ReadLine();
            //var respStr = new StreamReader(resp.GetResponseStream()).ReadToEnd();
            //Console.WriteLine(respStr);
            //Console.ReadLine();

        }
    }
}
