namespace Fgs.Setup.Application.Features.TenantProvisioning;

public static class SeedTransformationTypes
{
    public const string TenantId = "TENANT_ID";
    public const string CompanyId = "COMPANY_ID";
    public const string Static = "STATIC";
    public const string CurrentTimestamp = "CURRENT_TIMESTAMP";
    public const string SeedCreatedBy = "SEED_CREATED_BY";

    public const string SeedCreatedByValue = "Data Seed";

    public static class TargetColumns
    {
        public const string TenantId = "TenantId";
        public const string CompanyId = "CompanyId";
        public const string CreatedBy = "CreatedBy";
    }

    public static class SqlParameters
    {
        public const string TenantId = "tenantId";
        public const string CompanyId = "companyId";
        public const string BusinessTypeIds = "businessTypeIds";
    }

    public static class SqlFunctions
    {
        public const string CurrentTimestamp = "NOW()";
    }

    public static class SourceColumns
    {
        public const string BusinessTypeId = "BusinessTypeId";
    }

    public static class ErrorMessages
    {
        public const string SourceColumnRequiredFormat =
            "Column mapping {0} requires SourceColumnName when TransformationType is null.";

        public const string UnsupportedTransformationFormat =
            "Unsupported transformation type '{0}' on column mapping {1}.";

        public const string SourceTableNotFoundFormat =
            "Source table {0} does not exist for seed mapping {1} ({2}).";

        public const string TargetTableNotFoundFormat =
            "Target table {0} does not exist for seed mapping {1} ({2}).";

        public const string SourceColumnNotFoundFormat =
            "Source column {0} does not exist on {1} for column mapping {2} ({3}).";

        public const string TargetColumnNotFoundFormat =
            "Target column {0} does not exist on {1} for column mapping {2} ({3}).";

        public const string DatabaseConnectionFailedFormat =
            "Unable to open connection to database '{0}' for seed mapping {1}: {2}";

        public const string DuplicateTargetColumnFormat =
            "Seed mapping {0} defines duplicate target column '{1}'.";

        public const string NoColumnMappingsFormat =
            "Seed mapping {0} has no active column mappings.";
    }
}
