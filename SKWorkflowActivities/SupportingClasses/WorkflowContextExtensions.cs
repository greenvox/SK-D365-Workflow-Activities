using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;

namespace SKWorkflowActivities
{
    public static class WorkflowContextExtensions
    {
        /// <summary>
        /// Retrieves a PreImage from the PluginExecutionContext
        /// </summary>
        /// <param name="context">The plugin execution Context</param>
        /// <param name="imageName">The name of the PreImage. If not supplied, "PreImage" will be used</param>
        /// <returns>An Entity representing the Image</returns>
        public static Entity GetPreImage(this IWorkflowContext context, string imageName = "PreImage")
        {
            if (context.PreEntityImages != null &&
                context.PreEntityImages.Contains(imageName))
            {
                return context.PreEntityImages[imageName];
            }

            return null;
        }

        /// <summary>
        /// Retrieves a PostImage from the PluginExecutionContext
        /// </summary>
        /// <param name="context">The plugin execution Context</param>
        /// <param name="imageName">The name of the PostImage. If not supplied, "PostImage" will be used</param>
        /// <returns>An Entity representing the Image</returns>
        public static Entity GetPostImage(this IWorkflowContext context, string imageName = "PostImage")
        {
            if (context.PostEntityImages != null &&
                context.PostEntityImages.Contains(imageName))
            {
                return context.PostEntityImages[imageName];
            }

            return null;
        }

        /// <summary>
        /// Retrieves a Plugin Step Image from the PluginExecutionContext.
        /// It will first try to locate the PreImage, if not found, 
        /// it will try the Post Image. If not found, it will return null.
        /// </summary>
        /// <param name="context">The plugin execution context</param>
        /// <param name="imageName">Name of the image</param>
        /// <returns>Entity Image or null</returns>
        public static Entity GetImage(this IWorkflowContext context, string imageName)
        {
            if (context.PreEntityImages != null && context.PreEntityImages.Contains(imageName))
            {
                return context.PreEntityImages[imageName];
            }

            if (context.PostEntityImages != null && context.PostEntityImages.Contains(imageName))
            {
                return context.PostEntityImages[imageName];
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public static T GetInputParameter<T>(this IWorkflowContext context, string parameterName)
        {
            if (!context.InputParameters.Contains(parameterName))
            {
                return default;
            }

            var value = context.InputParameters[parameterName];

            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public static T GetOutputParameter<T>(this IWorkflowContext context, string parameterName)
        {
            if (!context.OutputParameters.Contains(parameterName))
            {
                return default;
            }

            var value = context.OutputParameters[parameterName];

            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// Return the Target entity from the plugin execution context
        /// </summary>
        /// <param name="context">The plugin execution context</param>
        /// <returns>An Entity representing the Target</returns>
        public static Entity GetTargetEntity(this IWorkflowContext context)
        {
            return context.GetInputParameter<Entity>(ParameterName.Target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static EntityReference GetTargetEntityReference(this IWorkflowContext context)
        {
            return context.GetInputParameter<EntityReference>(ParameterName.Target);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static EntityReference GetEntityMoniker(this IWorkflowContext context)
        {
            return context.GetInputParameter<EntityReference>(ParameterName.EntityMoniker);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Relationship GetRelationship(this IWorkflowContext context)
        {
            return context.GetInputParameter<Relationship>(ParameterName.Relationship);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static EntityReferenceCollection GetRelatedEntities(this IWorkflowContext context)
        {
            return context.GetInputParameter<EntityReferenceCollection>(ParameterName.RelatedEntities);
        }

        /// <summary>
        /// Check the plugin context input parameters for a specific relationship name
        /// </summary>
        /// <param name="context">The plugin execution context</param>
        /// <param name="relationshipName">The name of the relationship to verify</param>
        /// <returns>true if the relationship exists in the plugin context</returns>
        public static bool IsRelationship(this IWorkflowContext context, string relationshipName)
        {
            var relationship = context.GetRelationship();

            return relationship != null && relationship.SchemaName.Equals(relationshipName, StringComparison.InvariantCultureIgnoreCase);
        }

    }
}
