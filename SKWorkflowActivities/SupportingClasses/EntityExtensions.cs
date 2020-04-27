using System;
using Microsoft.Xrm.Sdk;

namespace SKWorkflowActivities
{
    public static class EntityExtensions
    {
        ///<summary> 
        /// Extension method to get an attribute value from the entity  
        /// or its image snapshot 
        ///</summary> 
        ///<typeparam name="T">The attribute type</typeparam> 
        ///<param name="entity">The primary entity</param> 
        ///<param name="attributeLogicalName">Logical name of the attribute</param> 
        ///<param name="image">Image (pre/post) of the primary entity</param> 
        ///<returns>The attribute value of type T</returns> 
        ///<remarks>If neither entity contains the attribute, returns default(T)</remarks> 
        public static T GetAttributeValue<T>(this Entity entity,
                                             string attributeLogicalName,
                                             Entity image)
        {
            return entity.GetAttributeValue(attributeLogicalName, image, default(T));
        }

        ///<summary> 
        ///Extension method to get an attribute value from the entity or image 
        ///</summary> 
        ///<typeparam name="T">The attribute type</typeparam> 
        ///<param name="entity">The primary entity</param> 
        ///<param name="attributeLogicalName">Logical name of the attribute</param> 
        ///<param name="image">Image (pre/post) of the primary entity</param> 
        ///<param name="defaultValue">The default value to use</param> 
        ///<returns>The attribute value of type T</returns> 
        public static T GetAttributeValue<T>(this Entity entity,
                                             string attributeLogicalName,
                                             Entity image,
                                             T defaultValue)
        {
            return entity.Contains(attributeLogicalName)
                ? entity.GetAttributeValue<T>(attributeLogicalName)
                : image != null && image.Contains(attributeLogicalName)
                    ? image.GetAttributeValue<T>(attributeLogicalName)
                    : defaultValue;
        }

        /// <summary>
        /// Returns a money value from an entity
        /// </summary>
        /// <param name="entity">The primary entity</param>
        /// <param name="attributeName">Logical name of the attribute</param>
        /// <param name="defaultValue">Default return value. 0 if nothing is specified</param>
        /// <returns>a CRM Money value</returns>
        public static Money GetMoney(this Entity entity, string attributeName, decimal defaultValue = 0)
        {
            var result = entity.GetAttributeValue<Money>(attributeName);

            if (result == null || result.Value == 0)
            {
                return new Money(defaultValue);
            }

            return result;
        }

        /// <summary>
        /// Returns a OptionSetValue value from an entity
        /// </summary>
        /// <param name="entity">The primary entity</param>
        /// <param name="attributeName">Logical name of the attribute</param>
        /// <returns>A CRM OptionSetValue</returns>
        public static OptionSetValue GetOptionSetValue(this Entity entity, string attributeName)
        {
            return entity.GetAttributeValue<OptionSetValue>(attributeName);
        }

        /// <summary>
        /// Returns a EntityReference value from an entity
        /// </summary>
        /// <param name="entity">The primary entity</param>
        /// <param name="attributeName">Logical name of the attribute</param>
        /// <returns>A CRM Entity Reference</returns>
        public static EntityReference GetEntityReference(this Entity entity, string attributeName)
        {
            return entity.GetAttributeValue<EntityReference>(attributeName);
        }

        /// <summary>
        /// Returns a string value from an entity
        /// </summary>
        /// <param name="entity">The primary entity</param>
        /// <param name="attributeName">Logical name of the attribute</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>A string</returns>
        public static string GetString(this Entity entity, string attributeName, string defaultValue = "")
        {
            var result = entity.GetAttributeValue<string>(attributeName);

            return string.IsNullOrEmpty(result) ? defaultValue : result;
        }

        /// <summary>
        /// Returns a boolean value from an entity
        /// </summary>
        /// <param name="entity">The primary entity</param>
        /// <param name="attributeName">Logical name of the attribute</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>A nullable boolean value</returns>
        public static bool? GetBoolean(this Entity entity, string attributeName, bool? defaultValue = null)
        {
            var result = entity.GetAttributeValue<bool?>(attributeName);

            return result ?? defaultValue;
        }

        /// <summary>
        /// Returns a decimal value from an entity
        /// </summary>
        /// <param name="entity">The primary entity</param>
        /// <param name="attributeName">Logical name of the attribute</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>A nullable decimal value</returns>
        public static decimal? GetDecimal(this Entity entity, string attributeName, decimal? defaultValue = null)
        {
            var result = entity.GetAttributeValue<decimal?>(attributeName);

            return result ?? defaultValue;
        }


        /// <summary>
        /// Returns a floating point value from an entity
        /// </summary>
        /// <param name="entity">The primary entity</param>
        /// <param name="attributeName">Logical name of the attribute</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>A nullable double value</returns>
        public static double? GetFloatingPoint(this Entity entity, string attributeName, double? defaultValue = null)
        {
            var result = entity.GetAttributeValue<double?>(attributeName);

            return result ?? defaultValue;
        }

        /// <summary>
        /// Returns a integer value from an entity
        /// </summary>
        /// <param name="entity">The primary entity</param>
        /// <param name="attributeName">Logical name of the attribute</param>
        /// <param name="defaultValue">Default return value. Null if nothing is specified</param>
        /// <returns>A nullable integer value. If the attribute is not found, a null will be returned.</returns>
        public static int? GetWholeNumber(this Entity entity, string attributeName, int? defaultValue = null)
        {
            var result = entity.GetAttributeValue<int?>(attributeName);

            return result ?? defaultValue;
        }


        /// <summary>
        /// Returns a DateTime value from an entity
        /// </summary>
        /// <param name="entity">The primary entity</param>
        /// <param name="attributeName">Logical name of the attribute</param>
        /// <param name="defaultValue">the default value</param>
        /// <returns>A nullable DateTime value</returns>
        public static DateTime? GetDateTime(this Entity entity, string attributeName, DateTime? defaultValue = null)
        {
            var result = entity.GetAttributeValue<DateTime?>(attributeName);

            return result ?? defaultValue;
        }

        /// <summary>
        /// Removes an attribute from an Entity
        /// </summary>
        /// <param name="entity">The primary entity</param>
        /// <param name="attributeName">Logical name of the attribute</param>
        public static void RemoveAttribute(this Entity entity, string attributeName)
        {
            if (entity == null || !entity.Contains(attributeName))
            {
                return;
            }

            entity.Attributes.Remove(attributeName);
        }
    }
}
