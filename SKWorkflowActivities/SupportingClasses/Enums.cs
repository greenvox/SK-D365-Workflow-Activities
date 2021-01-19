using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKWorkflowActivities
{
    public enum SdkMessageProcessingStepMode
    {
        Synchronous = 0,
        Asynchronous = 1,
    }

    public enum SdkMessageProcessingStepStage
    {
        Prevalidation = 10,
        Preoperation = 20,
        // MainOperation_Forinternaluseonly = 30,
        Postoperation = 40,
    }

    public enum sdkmessageprocessingstep_statuscode
    {
        Enabled = 1,
        Disabled = 2,
    }

    public enum SdkMessageProcessingStepSupportedDeployment
    {
        ServerOnly = 0,
        MicrosoftDynamicsCRMClientforOutlookOnly = 1,
        Both = 2,
    }

    public enum SdkMessageProcessingStepImageImageType
    {
        PreImage = 0,
        PostImage = 1,
        Both = 2,
    }

    public static class ParameterName
    {
        public const string Target = "Target";
        public const string Relationship = "Relationship";
        public const string EntityMoniker = "EntityMoniker";
        public const string RelatedEntities = "RelatedEntities";
        public const string SmallId = "id";
        public const string BigId = "Id";
        public const string ColumnSet = "ColumnSet";
        public const string RelationshipQueryCollection = "RelationshipQueryCollection";
        public const string ReturnNotifications = "ReturnNotifications";
        public const string Query = "Query";

        public const string Assignee = "Assignee";
        public const string BusinessEntity = "BusinessEntity";
        public const string BusinessEntityCollection = "BusinessEntityCollection";
        public const string CampaignId = "CampaignId";
        public const string CampaignActivityId = "CampaignActivityId";
        public const string Context = "context";
        public const string ContractId = "ContractId";
        public const string Id = "id";
        public const string EmailId = "emailid";
        public const string EndpointId = "EndpointId";
        public const string ExchangeRate = "ExchangeRate";
        public const string SubordinateId = "subordinateid";
        public const string EntityId = "EntityId";
        public const string EntityEntityReference = "entityMoniker";
        public const string FaxId = "FaxId";
        public const string ListId = "ListId";
        public const string ReturnDynamicEntities = "ReturnDynamicEntities";
        public const string RouteType = "RouteType";
        public const string State = "state";
        public const string Status = "status";
        public const string Settings = "Settings";
        public const string TeamId = "TeamId";
        public const string TemplateId = "TemplateId";
        public const string UpdateContent = "UpdateContent";
        public const string ValidationResult = "ValidationResult";
        public const string OptionalParameters = "OptionalParameters";
        public const string TriggerAttribute = "TriggerAttribute"; // Attribute which trigger workflow update message 

        public const string PreBusinessEntity = "PreBusinessEntity";
        public const string PreMasterBusinessEntity = "PreMasterBusinessEntity";
        public const string PreSubordinateBusinessEntity = "PreSubordinateBusinessEntity";
        public const string PostBusinessEntity = "PostBusinessEntity";
        public const string PostMasterBusinessEntity = "PostMasterBusinessEntity";
        public const string WorkflowId = "WorkflowId";
        public const string AsyncOperationId = "AsyncOperationId";
    }

    public static class MessageName
    {
        public const string AddItem = "AddItem";
        public const string AddListMembers = "AddListMembers";
        public const string AddMember = "AddMember";
        public const string AddMembers = "AddMembers";
        public const string AddPrincipalToQueue = "AddPrincipalToQueue";
        public const string AddPrivileges = "AddPrivileges";
        public const string AddProductToKit = "AddProductToKit";
        public const string AddRecurrence = "AddRecurrence";
        public const string AddSolutionComponent = "AddSolutionComponent";
        public const string AddSubstitute = "AddSubstitute";
        public const string AddToQueue = "AddToQueue";
        public const string AddUserToRecordTeam = "AddUserToRecordTeam";
        public const string ApplyRecordCreationAndUpdateRule = "ApplyRecordCreationAndUpdateRule";
        public const string ApplyRoutingRule = "ApplyRoutingRule";
        public const string Assign = "Assign";
        public const string AssignUserRoles = "AssignUserRoles";
        public const string Associate = "Associate";
        public const string AssociateEntities = "AssociateEntities";
        public const string AutoMapEntity = "AutoMapEntity";
        public const string BackgroundSend = "BackgroundSend";
        public const string Book = "Book";
        public const string BulkDelete = "BulkDelete";
        public const string BulkDelete2 = "BulkDelete2";
        public const string BulkDetectDuplicates = "BulkDetectDuplicates";
        public const string BulkMail = "BulkMail";
        public const string BulkOperationStatusClose = "BulkOperationStatusClose";
        public const string CalculateActualValue = "CalculateActualValue";
        public const string CalculatePrice = "CalculatePrice";
        public const string CalculateRollupField = "CalculateRollupField";
        public const string CalculateTotalTime = "CalculateTotalTime";
        public const string CanBeReferenced = "CanBeReferenced";
        public const string CanBeReferencing = "CanBeReferencing";
        public const string Cancel = "Cancel";
        public const string CanManyToMany = "CanManyToMany";
        public const string CheckIncoming = "CheckIncoming";
        public const string CheckPromote = "CheckPromote";
        public const string CleanUpBulkOperation = "CleanUpBulkOperation";
        public const string Clone = "Clone";
        public const string CloneProduct = "CloneProduct";
        public const string Close = "Close";
        public const string CompoundCreate = "CompoundCreate";
        public const string CompoundUpdate = "CompoundUpdate";
        public const string CompoundUpdateDuplicateDetectionRule = "CompoundUpdateDuplicateDetectionRule";
        public const string ConvertDateAndTimeBehavior = "ConvertDateAndTimeBehavior";
        public const string ConvertKitToProduct = "ConvertKitToProduct";
        public const string ConvertOwnerTeamToAccessTeam = "ConvertOwnerTeamToAccessTeam";
        public const string ConvertProductToKit = "ConvertProductToKit";
        public const string ConvertQuoteToSalesOrder = "ConvertQuoteToSalesOrder";
        public const string ConvertSalesOrderToInvoice = "ConvertSalesOrderToInvoice";
        public const string Copy = "Copy";
        public const string CopyCampaignResponse = "CopyCampaignResponse";
        public const string CopyDynamicListToStatic = "CopyDynamicListToStatic";
        public const string CopyMembers = "CopyMembers";
        public const string CopySystemForm = "CopySystemForm";
        public const string Create = "Create";
        public const string CreateActivities = "CreateActivities";
        public const string CreateAttribute = "CreateAttribute";
        public const string CreateEntity = "CreateEntity";
        public const string CreateEntityKey = "CreateEntityKey";
        public const string CreateException = "CreateException";
        public const string CreateInstance = "CreateInstance";
        public const string CreateManyToMany = "CreateManyToMany";
        public const string CreateOneToMany = "CreateOneToMany";
        public const string CreateOptionSet = "CreateOptionSet";
        public const string CreateWorkflowFromTemplate = "CreateWorkflowFromTemplate";
        public const string Delete = "Delete";
        public const string DeleteAttribute = "DeleteAttribute";
        public const string DeleteAuditData = "DeleteAuditData";
        public const string DeleteEntity = "DeleteEntity";
        public const string DeleteEntityKey = "DeleteEntityKey";
        public const string DeleteOpenInstances = "DeleteOpenInstances";
        public const string DeleteOptionSet = "DeleteOptionSet";
        public const string DeleteOptionValue = "DeleteOptionValue";
        public const string DeleteRelationship = "DeleteRelationship";
        public const string DeliverIncoming = "DeliverIncoming";
        public const string DeliverPromote = "DeliverPromote";
        public const string DeprovisionLanguage = "DeprovisionLanguage";
        public const string DetachFromQueue = "DetachFromQueue";
        public const string Disassociate = "Disassociate";
        public const string DisassociateEntities = "DisassociateEntities";
        public const string DistributeCampaignActivity = "DistributeCampaignActivity";
        public const string DownloadReportDefinition = "DownloadReportDefinition";
        public const string EntityExpressionToFetchXml = "EntityExpressionToFetchXml";
        public const string Execute = "Execute";
        public const string ExecuteAsync = "ExecuteAsync";
        public const string ExecuteById = "ExecuteById";
        public const string ExecuteMultiple = "ExecuteMultiple";
        public const string ExecuteTransaction = "ExecuteTransaction";
        public const string ExecuteWorkflow = "ExecuteWorkflow";
        public const string Expand = "Expand";
        public const string Export = "Export";
        public const string ExportAll = "ExportAll";
        public const string ExportCompressed = "ExportCompressed";
        public const string ExportCompressedAll = "ExportCompressedAll";
        public const string ExportCompressedTranslations = "ExportCompressedTranslations";
        public const string ExportFieldTranslation = "ExportFieldTranslation";
        public const string ExportMappings = "ExportMappings";
        public const string ExportSolution = "ExportSolution";
        public const string ExportToExcelOnline = "ExportToExcelOnline";
        public const string ExportTranslation = "ExportTranslation";
        public const string ExportTranslations = "ExportTranslations";
        public const string FetchXmlToEntityExpression = "FetchXmlToEntityExpression";
        public const string FindParent = "FindParent";
        public const string Fulfill = "Fulfill";
        public const string GenerateInvoiceFromOpportunity = "GenerateInvoiceFromOpportunity";
        public const string GenerateQuoteFromOpportunity = "GenerateQuoteFromOpportunity";
        public const string GenerateSalesOrderFromOpportunity = "GenerateSalesOrderFromOpportunity";
        public const string GenerateSocialProfile = "GenerateSocialProfile";
        public const string GetAllTimeZonesWithDisplayName = "GetAllTimeZonesWithDisplayName";
        public const string GetDecryptionKey = "GetDecryptionKey";
        public const string GetDefaultPriceLevel = "GetDefaultPriceLevel";
        public const string GetDistinctValues = "GetDistinctValues";
        public const string GetHeaderColumns = "GetHeaderColumns";
        public const string GetInvoiceProductsFromOpportunity = "GetInvoiceProductsFromOpportunity";
        public const string GetQuantityDecimal = "GetQuantityDecimal";
        public const string GetQuoteProductsFromOpportunity = "GetQuoteProductsFromOpportunity";
        public const string GetReportHistoryLimit = "GetReportHistoryLimit";
        public const string GetSalesOrderProductsFromOpportunity = "GetSalesOrderProductsFromOpportunity";
        public const string GetTimeZoneCodeByLocalizedName = "GetTimeZoneCodeByLocalizedName";
        public const string GetTrackingToken = "GetTrackingToken";
        public const string GetValidManyToMany = "GetValidManyToMany";
        public const string GetValidReferencedEntities = "GetValidReferencedEntities";
        public const string GetValidReferencingEntities = "GetValidReferencingEntities";
        public const string GrantAccess = "GrantAccess";
        public const string Handle = "Handle";
        public const string Import = "Import";
        public const string ImportAll = "ImportAll";
        public const string ImportCompressedAll = "ImportCompressedAll";
        public const string ImportCompressedTranslationsWithProgress = "ImportCompressedTranslationsWithProgress";
        public const string ImportCompressedWithProgress = "ImportCompressedWithProgress";
        public const string ImportFieldTranslation = "ImportFieldTranslation";
        public const string ImportMappings = "ImportMappings";
        public const string ImportRecords = "ImportRecords";
        public const string ImportSolution = "ImportSolution";
        public const string ImportTranslation = "ImportTranslation";
        public const string ImportTranslationsWithProgress = "ImportTranslationsWithProgress";
        public const string ImportWithProgress = "ImportWithProgress";
        public const string InitializeFrom = "InitializeFrom";
        public const string InsertOptionValue = "InsertOptionValue";
        public const string InsertStatusValue = "InsertStatusValue";
        public const string InstallSampleData = "InstallSampleData";
        public const string Instantiate = "Instantiate";
        public const string InstantiateFilters = "InstantiateFilters";
        public const string IsBackOfficeInstalled = "IsBackOfficeInstalled";
        public const string IsComponentCustomizable = "IsComponentCustomizable";
        public const string IsDataEncryptionActive = "IsDataEncryptionActive";
        public const string IsValidStateTransition = "IsValidStateTransition";
        public const string LocalTimeFromUtcTime = "LocalTimeFromUtcTime";
        public const string LockInvoicePricing = "LockInvoicePricing";
        public const string LockSalesOrderPricing = "LockSalesOrderPricing";
        public const string LogFailure = "LogFailure";
        public const string LogSuccess = "LogSuccess";
        public const string Lose = "Lose";
        public const string MakeAvailableToOrganization = "MakeAvailableToOrganization";
        public const string MakeUnavailableToOrganization = "MakeUnavailableToOrganization";
        public const string Merge = "Merge";
        public const string ModifyAccess = "ModifyAccess";
        public const string OrderOption = "OrderOption";
        public const string Parse = "Parse";
        public const string PickFromQueue = "PickFromQueue";
        public const string ProcessInbound = "ProcessInbound";
        public const string ProcessOneMemberBulkOperation = "ProcessOneMemberBulkOperation";
        public const string PropagateByExpression = "PropagateByExpression";
        public const string ProvisionLanguage = "ProvisionLanguage";
        public const string Publish = "Publish";
        public const string PublishAll = "PublishAll";
        public const string PublishProductHierarchy = "PublishProductHierarchy";
        public const string PublishTheme = "PublishTheme";
        public const string QualifyLead = "QualifyLead";
        public const string QualifyMember = "QualifyMember";
        public const string Query = "Query";
        public const string QueryMultiple = "QueryMultiple";
        public const string ReactivateEntityKey = "ReactivateEntityKey";
        public const string ReassignObjects = "ReassignObjects";
        public const string ReassignObjectsEx = "ReassignObjectsEx";
        public const string Recalculate = "Recalculate";
        public const string ReleaseToQueue = "ReleaseToQueue";
        public const string RemoveFromQueue = "RemoveFromQueue";
        public const string RemoveItem = "RemoveItem";
        public const string RemoveMember = "RemoveMember";
        public const string RemoveMembers = "RemoveMembers";
        public const string RemoveParent = "RemoveParent";
        public const string RemovePrivilege = "RemovePrivilege";
        public const string RemoveProductFromKit = "RemoveProductFromKit";
        public const string RemoveRelated = "RemoveRelated";
        public const string RemoveSolutionComponent = "RemoveSolutionComponent";
        public const string RemoveSubstitute = "RemoveSubstitute";
        public const string RemoveUserFromRecordTeam = "RemoveUserFromRecordTeam";
        public const string RemoveUserRoles = "RemoveUserRoles";
        public const string Renew = "Renew";
        public const string RenewEntitlement = "RenewEntitlement";
        public const string ReplacePrivileges = "ReplacePrivileges";
        public const string Reschedule = "Reschedule";
        public const string ResetOfflineFilters = "ResetOfflineFilters";
        public const string ResetUserFilters = "ResetUserFilters";
        public const string Retrieve = "Retrieve";
        public const string RetrieveAbsoluteAndSiteCollectionUrl = "RetrieveAbsoluteAndSiteCollectionUrl";
        public const string RetrieveAllChildUsers = "RetrieveAllChildUsers";
        public const string RetrieveAllEntities = "RetrieveAllEntities";
        public const string RetrieveAllManagedProperties = "RetrieveAllManagedProperties";
        public const string RetrieveAllOptionSets = "RetrieveAllOptionSets";
        public const string RetrieveApplicationRibbon = "RetrieveApplicationRibbon";
        public const string RetrieveAttribute = "RetrieveAttribute";
        public const string RetrieveAttributeChangeHistory = "RetrieveAttributeChangeHistory";
        public const string RetrieveAuditDetails = "RetrieveAuditDetails";
        public const string RetrieveAuditPartitionList = "RetrieveAuditPartitionList";
        public const string RetrieveAvailableLanguages = "RetrieveAvailableLanguages";
        public const string RetrieveBusinessHierarchy = "RetrieveBusinessHierarchy";
        public const string RetrieveByGroup = "RetrieveByGroup";
        public const string RetrieveByResource = "RetrieveByResource";
        public const string RetrieveByResources = "RetrieveByResources";
        public const string RetrieveByTopIncidentProduct = "RetrieveByTopIncidentProduct";
        public const string RetrieveByTopIncidentSubject = "RetrieveByTopIncidentSubject";
        public const string RetrieveCurrentOrganization = "RetrieveCurrentOrganization";
        public const string RetrieveDataEncryptionKey = "RetrieveDataEncryptionKey";
        public const string RetrieveDependenciesForDelete = "RetrieveDependenciesForDelete";
        public const string RetrieveDependenciesForUninstall = "RetrieveDependenciesForUninstall";
        public const string RetrieveDependentComponents = "RetrieveDependentComponents";
        public const string RetrieveDeploymentLicenseType = "RetrieveDeploymentLicenseType";
        public const string RetrieveDeprovisionedLanguages = "RetrieveDeprovisionedLanguages";
        public const string RetrieveDuplicates = "RetrieveDuplicates";
        public const string RetrieveEntity = "RetrieveEntity";
        public const string RetrieveEntityChanges = "RetrieveEntityChanges";
        public const string RetrieveEntityKey = "RetrieveEntityKey";
        public const string RetrieveEntityRibbon = "RetrieveEntityRibbon";
        public const string RetrieveExchangeRate = "RetrieveExchangeRate";
        public const string RetrieveFilteredForms = "RetrieveFilteredForms";
        public const string RetrieveFormattedImportJobResults = "RetrieveFormattedImportJobResults";
        public const string RetrieveFormXml = "RetrieveFormXml";
        public const string RetrieveInstalledLanguagePacks = "RetrieveInstalledLanguagePacks";
        public const string RetrieveInstalledLanguagePackVersion = "RetrieveInstalledLanguagePackVersion";
        public const string RetrieveLicenseInfo = "RetrieveLicenseInfo";
        public const string RetrieveLocLabels = "RetrieveLocLabels";
        public const string RetrieveMailboxTrackingFolders = "RetrieveMailboxTrackingFolders";
        public const string RetrieveManagedProperty = "RetrieveManagedProperty";
        public const string RetrieveMembers = "RetrieveMembers";
        public const string RetrieveMembersBulkOperation = "RetrieveMembersBulkOperation";
        public const string RetrieveMetadataChanges = "RetrieveMetadataChanges";
        public const string RetrieveMissingComponents = "RetrieveMissingComponents";
        public const string RetrieveMissingDependencies = "RetrieveMissingDependencies";
        public const string RetrieveMultiple = "RetrieveMultiple";
        public const string RetrieveOptionSet = "RetrieveOptionSet";
        public const string RetrieveOrganizationResources = "RetrieveOrganizationResources";
        public const string RetrieveParentGroups = "RetrieveParentGroups";
        public const string RetrieveParsedData = "RetrieveParsedData";
        public const string RetrievePersonalWall = "RetrievePersonalWall";
        public const string RetrievePrincipalAccess = "RetrievePrincipalAccess";
        public const string RetrievePrincipalAttributePrivileges = "RetrievePrincipalAttributePrivileges";
        public const string RetrievePrincipalSyncAttributeMappings = "RetrievePrincipalSyncAttributeMappings";
        public const string RetrievePrivilegeSet = "RetrievePrivilegeSet";
        public const string RetrieveProductProperties = "RetrieveProductProperties";
        public const string RetrieveProvisionedLanguagePackVersion = "RetrieveProvisionedLanguagePackVersion";
        public const string RetrieveProvisionedLanguages = "RetrieveProvisionedLanguages";
        public const string RetrieveRecordChangeHistory = "RetrieveRecordChangeHistory";
        public const string RetrieveRecordWall = "RetrieveRecordWall";
        public const string RetrieveRelationship = "RetrieveRelationship";
        public const string RetrieveRequiredComponents = "RetrieveRequiredComponents";
        public const string RetrieveRolePrivileges = "RetrieveRolePrivileges";
        public const string RetrieveSharedPrincipalsAndAccess = "RetrieveSharedPrincipalsAndAccess";
        public const string RetrieveSubGroups = "RetrieveSubGroups";
        public const string RetrieveSubsidiaryTeams = "RetrieveSubsidiaryTeams";
        public const string RetrieveSubsidiaryUsers = "RetrieveSubsidiaryUsers";
        public const string RetrieveTeamPrivileges = "RetrieveTeamPrivileges";
        public const string RetrieveTeams = "RetrieveTeams";
        public const string RetrieveTimestamp = "RetrieveTimestamp";
        public const string RetrieveUnpublished = "RetrieveUnpublished";
        public const string RetrieveUnpublishedMultiple = "RetrieveUnpublishedMultiple";
        public const string RetrieveUserPrivileges = "RetrieveUserPrivileges";
        public const string RetrieveUserQueues = "RetrieveUserQueues";
        public const string RetrieveUserSettings = "RetrieveUserSettings";
        public const string RetrieveVersion = "RetrieveVersion";
        public const string RevertProduct = "RevertProduct";
        public const string Revise = "Revise";
        public const string RevokeAccess = "RevokeAccess";
        public const string Rollup = "Rollup";
        public const string Route = "Route";
        public const string RouteTo = "RouteTo";
        public const string Search = "Search";
        public const string SearchByBody = "SearchByBody";
        public const string SearchByBodyLegacy = "SearchByBodyLegacy";
        public const string SearchByKeywords = "SearchByKeywords";
        public const string SearchByKeywordsLegacy = "SearchByKeywordsLegacy";
        public const string SearchByTitle = "SearchByTitle";
        public const string SearchByTitleLegacy = "SearchByTitleLegacy";
        public const string Send = "Send";
        public const string SendFromTemplate = "SendFromTemplate";
        public const string SetBusiness = "SetBusiness";
        public const string SetDataEncryptionKey = "SetDataEncryptionKey";
        public const string SetLocLabels = "SetLocLabels";
        public const string SetParent = "SetParent";
        public const string SetRelated = "SetRelated";
        public const string SetReportRelated = "SetReportRelated";
        public const string SetState = "SetState";
        public const string SetStateDynamicEntity = "SetStateDynamicEntity";
        public const string StatusUpdateBulkOperation = "StatusUpdateBulkOperation";
        public const string Transform = "Transform";
        public const string TriggerServiceEndpointCheck = "TriggerServiceEndpointCheck";
        public const string UninstallSampleData = "UninstallSampleData";
        public const string UnlockInvoicePricing = "UnlockInvoicePricing";
        public const string UnlockSalesOrderPricing = "UnlockSalesOrderPricing";
        public const string Unpublish = "Unpublish";
        public const string Update = "Update";
        public const string UpdateAttribute = "UpdateAttribute";
        public const string UpdateEntity = "UpdateEntity";
        public const string UpdateOptionSet = "UpdateOptionSet";
        public const string UpdateOptionValue = "UpdateOptionValue";
        public const string UpdateProductProperties = "UpdateProductProperties";
        public const string UpdateRelationship = "UpdateRelationship";
        public const string UpdateStateValue = "UpdateStateValue";
        public const string UpdateUserSettings = "UpdateUserSettings";
        public const string Upsert = "Upsert";
        public const string UtcTimeFromLocalTime = "UtcTimeFromLocalTime";
        public const string Validate = "Validate";
        public const string ValidateRecurrenceRule = "ValidateRecurrenceRule";
        public const string VerifyProcessStateData = "VerifyProcessStateData";
        public const string WhoAmI = "WhoAmI";
        public const string Win = "Win";
    }

    [System.Runtime.Serialization.DataContractAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "8.1.0.239")]
    public enum ImportModeCode
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Create = 0,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Update = 1,
    }


    public enum asyncoperation_statuscode
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        WaitingForResources = 0,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Waiting = 10,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        InProgress = 20,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Pausing = 21,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Canceling = 22,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Succeeded = 30,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Failed = 31,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Canceled = 32,
    }


    [System.Runtime.Serialization.DataContractAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "8.1.0.239")]
    public enum ImportFileDataDelimiterCode
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        DoubleQuote = 1,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        None = 2,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        SingleQuote = 3,
    }


    [System.Runtime.Serialization.DataContractAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "8.1.0.239")]
    public enum ImportFileProcessCode
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Process = 1,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Ignore = 2,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Internal = 3,
    }


    [System.Runtime.Serialization.DataContractAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "8.1.0.239")]
    public enum ImportFileFieldDelimiterCode
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Colon = 1,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Comma = 2,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Tab = 3,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Semicolon = 4,
    }
}
