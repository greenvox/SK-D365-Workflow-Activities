using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SKWorkflowActivities
{
    /// <summary>
    ///     (Credit: Mitch Milam at XrmCoaches.com)
    ///     Common functions to support code activities
    /// </summary>
    public class CrmUtility
    {
        public static EntityReference GetCrmAdminUser(IOrganizationService service)
        {
            var query = new QueryByAttribute(SystemUser.EntityLogicalName);

            query.AddAttributeValue("fullname", "CRM Admin");

            return service.RetrieveMultiple(query).Entities.FirstOrDefault()?.ToEntityReference();
        }

        public static EntityReference CreateEntityReferenceFromString(string value)
        {
            // 0  1                                     2                                    
            // 8, DF108675-DF2B-E911-A964-000D3A3B535A,\"Sarfraz Khan\"]

            const int systemUserType = 8;
            const string entityTypeName = SystemUser.EntityLogicalName;

            var fields = value.Split(',', '[');

            var convertible = int.TryParse(fields[0], out var entityType);

            if (!convertible)
            {
                return null;
            }

            var entityId = new Guid(fields[1]);

            return entityType != systemUserType ? null : new EntityReference(entityTypeName, entityId);
        }

        public static Entity GetEntityByAttribute(IOrganizationService service, string entityName, string fieldName, string fieldValue, ColumnSet columnSet)
        {
            var query = new QueryByAttribute(entityName)
            {
                ColumnSet = columnSet
            };

            query.AddAttributeValue(fieldName, fieldValue);

            return service.RetrieveMultiple(query).Entities.FirstOrDefault();
        }

        public static string GetStringByValueUsingFetch(IOrganizationService service, string entityName, string attributeName, string filter, string fieldName, string fieldValue, string fetchFilters)
        {
            var fetchXml = $@"
                    <fetch>
                      <entity name='{entityName}'>
                        <attribute name='{attributeName}' />
                        <filter type='{filter}'>
                          <condition attribute='{fieldName}' operator='eq' value='{fieldValue}'/>
                          {fetchFilters}  
                        </filter>
                      </entity>
                    </fetch>";

            var result = service.RetrieveMultiple(new FetchExpression(fetchXml)).Entities.FirstOrDefault();

            return result == null ? string.Empty : result.GetString(attributeName);
        }
        
    }
}
