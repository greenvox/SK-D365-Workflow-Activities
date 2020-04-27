using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Dynamics_CRM_Workflow_Activity.ReferenceCode
{
    class WorkflowUtility
    {
        public static Guid GetLowestUserId(IOrganizationService service,
                                      string entityName)
        {
            const string query = @" 
       <fetch distinct='false' mapping='logical' aggregate='true' page='1' count='1'> 
            <entity name='{0}'> 
              <attribute name='{0}id' alias='record_count' aggregate='countcolumn' /> 
              <attribute name='ownerid' alias='ownerid' groupby='true' /> 
              <order alias='record_count' descending='false'/>
              </entity> 
            </fetch>";

            var fetch = string.Format(query, entityName);

            var result = service.RetrieveMultiple(new FetchExpression(fetch));

            var top = result.Entities[0];

            return ((EntityReference)((AliasedValue)top["ownerid"]).Value).Id;
        }

    }
}
