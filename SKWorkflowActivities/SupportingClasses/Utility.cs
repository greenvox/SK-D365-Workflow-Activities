using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using Microsoft.Xrm.Sdk;

namespace SKWorkflowActivities
{
    public class Utility
    {
        public static List<string> SplitStringOnBrackets(string value)
        {
            var results = value.Split('[', ']').Where((item, index) => index % 2 != 0).ToList();

            return results;
        }

        public static string HandleExceptions(Exception ex)
        {
            var sb = new StringBuilder();

            switch (ex)
            {
                case WebException _:
                    sb.Append(ex.Message);
                    break;
                case FaultException<DiscoveryServiceFault> _:
                {
                    if (!(ex.InnerException is FaultException<DiscoveryServiceFault> faultException))
                    {
                        return sb.ToString();
                    }

                    sb.Append($"Timestamp:\t{faultException.Detail.Timestamp}{Environment.NewLine}");
                    sb.Append($"Code:\t{faultException.Detail.ErrorCode}{Environment.NewLine}");
                    sb.Append($"Stack Trace:\t{ex.StackTrace}{Environment.NewLine}");
                    sb.Append($"Message:\t{faultException.Detail.Message}{Environment.NewLine}");
                    sb.Append($"Inner Fault:\t{(null == faultException.Detail.InnerFault ? "Has Inner Fault" : "No Inner Fault")}{Environment.NewLine}");
                    break;
                }
                case FaultException<OrganizationServiceFault> _:
                {
                    sb.Append($"{ex.Message}{Environment.NewLine}");

                    if (ex.InnerException == null)
                    {
                        return sb.ToString();
                    }

                    sb.Append($"{ex.InnerException.Message}{Environment.NewLine}");
                    sb.Append($"Stack Trace:\t{ex.StackTrace}{Environment.NewLine}");

                    if (ex.InnerException is FaultException<OrganizationServiceFault> innerFaultException)
                    {
                        HandleInnerException(innerFaultException, sb);
                    }

                    break;
                }
                case TimeoutException _:
                    sb.Append($"Message:\t{ex.Message}{Environment.NewLine}");
                    sb.Append($"Stack Trace:\t{ex.StackTrace}{Environment.NewLine}");
                    sb.Append($"Inner Fault:\tNo Inner Fault{Environment.NewLine}");
                    break;
                default:
                {
                    sb.Append($"{ex.Message}{Environment.NewLine}");
                    sb.Append($"Stack Trace:\t{ex.StackTrace}{Environment.NewLine}");

                    if (ex.InnerException == null)
                    {
                        return sb.ToString();
                    }

                    sb.AppendFormat("{0}{1}", ex.InnerException.Message, Environment.NewLine);

                    if (ex.InnerException is FaultException<OrganizationServiceFault> innerFaultException)
                    {
                        HandleInnerException(innerFaultException, sb);
                    }

                    break;
                }
            }

            return sb.ToString();
        }

        private static void HandleInnerException(FaultException<OrganizationServiceFault> innerFaultException,
            StringBuilder sb)
        {
            sb.Append($"Timestamp:\t{innerFaultException.Detail.Timestamp}{Environment.NewLine}");
            sb.Append($"Code:\t{innerFaultException.Detail.ErrorCode}{Environment.NewLine}");
            sb.Append($"Message:\t{innerFaultException.Detail.Message}{Environment.NewLine}");
            sb.Append($"Trace:\t{innerFaultException.Detail.TraceText}{Environment.NewLine}");
            sb.Append($"Stack Trace:\t{innerFaultException.StackTrace}{Environment.NewLine}");
            sb.Append($"Inner Fault:\t{(null == innerFaultException.Detail.InnerFault ? "Has Inner Fault" : "No Inner Fault")}{Environment.NewLine}");
        }
    }
}
