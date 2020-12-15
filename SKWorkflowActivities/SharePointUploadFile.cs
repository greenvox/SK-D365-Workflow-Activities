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
    public class SharePointUploadFile : CodeActivity
    {
        [RequiredArgument]
        [Input("Username")]
        public InArgument<string> Username { get; set; }

        [RequiredArgument]
        [Input("Password")]
        public InArgument<string> Password { get; set; }

        [RequiredArgument]
        [Input("File Body")]
        public InArgument<string> DocumentBody { get; set; }

        [RequiredArgument]
        [Input("File Name with Extension")]
        [Default("myfile.docx")]
        public InArgument<string> FileName { get; set; }

        [RequiredArgument]
        [Input("Browser Host")]
        [Default("yourdomain.sharepoint.com")]
        public InArgument<string> BrowserHost { get; set; }
        [RequiredArgument]
        [Input("Site URL")]
        [Default("https://yourdomain.sharepoint.com/sites/mysite")]
        public InArgument<string> EndPoint { get; set; }

        [RequiredArgument]
        [Input("Document Library")]
        [Default("Documents")]
        public InArgument<string> DocumentLibrary { get; set; }

        [RequiredArgument]
        [Input("Browser User Agent")]
        [Default("Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.86 Safari/537.36")]
        public InArgument<string> BrowserUserAgent { get; set; }

        [Output("Response")]
        public OutArgument<string> Response { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            var tracingService = executionContext.GetExtension<ITracingService>();

            var username = Username.Get(executionContext);
            var password = Password.Get(executionContext);
            var endPoint = EndPoint.Get(executionContext);
            var browserHost = BrowserHost.Get(executionContext); //e.g. mycompany.sharepoint.com
            var signInUrl = "https://" + browserHost + "/_forms/default.aspx?wa=wsignin1.0"; //SignInUrl.Get(executionContext);
            var browserUserAgent = BrowserUserAgent.Get(executionContext);
            var fileName = FileName.Get(executionContext);
            var documentLibrary = DocumentLibrary.Get(executionContext);
            var documentBody = DocumentBody.Get(executionContext);

            var stsEndpoint = "https://login.microsoftonline.us/extSTS.srf";
            if (browserHost.EndsWith("us"))
            {
            stsEndpoint = "https://login.microsoftonline.us/extSTS.srf";
            }


            var digestHeaders = SharepointUtility.GetDigestHeaders(username, password, endPoint, browserHost, signInUrl, stsEndpoint, browserUserAgent);

            var cookies = digestHeaders.Cookies;
            var formDigest = digestHeaders.FormDigest;
            byte[] byteArray = Convert.FromBase64String(documentBody);
            //byte[] byteArray = Encoding.UTF8.GetBytes(documentBody);

            //Upload call
            var restUrl = string.Format("{0}/_api/web/lists/getbytitle('{1}')/rootfolder/files/add(url='{2}',overwrite=true)", endPoint, documentLibrary, fileName);
            var req = (HttpWebRequest)WebRequest.Create(restUrl);
            req.CookieContainer = cookies;
            req.Method = "POST";
            //req.ContentType = "application/json;odata=verbose";
            req.ContentType = "application/x-www-form-urlencoded";
            req.Headers.Add("X-RequestDigest", formDigest);
            req.ContentLength = byteArray.Length;
            Stream dataStream = req.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse res = req.GetResponse();

            String responseString;
            using (Stream stream = res.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                responseString = reader.ReadToEnd();
            }

            // Get relativeUrl
            var xData = XDocument.Parse(responseString);
            var namespaceManager = new XmlNamespaceManager(new NameTable());
            namespaceManager.AddNamespace("S", "http://www.w3.org/2005/Atom");
            namespaceManager.AddNamespace("D", "http://schemas.microsoft.com/ado/2007/08/dataservices");
            namespaceManager.AddNamespace("M", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
            var relativeUrl = xData.XPathSelectElement("/S:entry/S:content/M:properties/D:ServerRelativeUrl", namespaceManager);

            var absoluteUrl = string.Format("https://{0}{1}", browserHost, relativeUrl.Value);

            Response.Set(executionContext, absoluteUrl);
        }
    }
}
