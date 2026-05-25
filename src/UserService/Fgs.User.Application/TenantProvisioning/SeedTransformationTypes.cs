namespace Fgs.User.Application.TenantProvisioning;

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
    }

    public static class SqlFunctions
    {
        public const string CurrentTimestamp = "NOW()";
    }

    public static class ErrorMessages
    {
        public const string SourceColumnRequiredFormat =
            "Column mapping {0} requires SourceColumnName when TransformationType is null.";

        public const string UnsupportedTransformationFormat =
            "Unsupported transformation type '{0}' on column mapping {1}.";
    }
}
