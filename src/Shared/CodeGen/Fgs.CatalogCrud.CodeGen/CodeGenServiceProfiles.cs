namespace Fgs.CatalogCrud.CodeGen;

internal static class CodeGenServiceProfiles
{
    public static CodeGenOptions CreateSetupDefaults(string repoRoot) => new()
    {
        Service = "Setup",
        InfrastructurePath = Path.Combine(repoRoot, "src", "SetupService", "Fgs.Setup.Infrastructure"),
        ApplicationPath = Path.Combine(repoRoot, "src", "SetupService", "Fgs.Setup.Application"),
        ApiPath = Path.Combine(repoRoot, "src", "SetupService", "Fgs.Setup.API"),
        ApplicationNamespace = "Fgs.Setup.Application",
        ApiNamespace = "Fgs.Setup.API",
        DomainProjectPath = Path.Combine(repoRoot, "src", "SetupService", "Fgs.Setup.Domain", "Fgs.Setup.Domain.csproj"),
        EntityNamespace = "Fgs.Setup.Domain.Entities",
        EntityNamePrefix = "Fgs",
        DefaultSchema = "setup",
        ExcludedEntities =
        [
            "FgsCredential",
            "FgsTenantCompanyCache",
            "FgsEntityTag",
            "FgsTagEntityType",
            "FgsSetupGLBreakTrade",
            "FgsInventoryStock"
        ],
        ResolveVariant = ResolveSetupVariant,
        ResolveSwaggerTag = ResolveSetupSwaggerTag
    };

    private static CatalogEntityVariant ResolveSetupVariant(Type entityType) =>
        entityType.Name switch
        {
            "FgsVehicleMaintenance" => CatalogEntityVariant.HardDeleteScoped,
            "FgsSetupCommunicationTemplate" => CatalogEntityVariant.NullableTenantScope,
            "FgsTag" => CatalogEntityVariant.ScopedManualAudit,
            _ => entityType.GetProperty("Id")?.PropertyType == typeof(Guid)
                ? CatalogEntityVariant.StandardGuid
                : CatalogEntityVariant.StandardLong
        };

    private static string ResolveSetupSwaggerTag(string entityName) =>
        entityName switch
        {
            "FgsBillingCategory" or "FgsSetupPaymentMethod" or "FgsSetupPaymentTerm" or "FgsSetupLaborRateType" => "Setup - Billing",
            "FgsBusinessType" => "Setup - Business",
            "FgsSetupTax" or "FgsSetupTaxAuthority" or "FgsSetupTaxDetail" or "FgsSetupPostalCode" => "Setup - Tax",
            "FgsSetupZone" or "FgsSetupTimeSlot" => "Setup - Zone",
            "FgsSetupTechTrade" or "FgsSetupTechSkillLevel" or "FgsSetupTitleOfCourtesy" or "FgsSetupDescription" => "Setup - Technician",
            var name when name.StartsWith("FgsSetupPricingMatrix", StringComparison.Ordinal) => "Setup - Pricing",
            "FgsSetupGLBreak" => "Setup - GL",
            "FgsLeadStatus" or "FgsLeadDisqualificationReason" => "Setup - Leads",
            "FgsSalesPipelineStatus" or "FgsSalesDispositionReason" or "FgsSalesActivityType" or "FgsSalesActivityOutcome" => "Setup - Sales",
            var name when name.StartsWith("FgsSetupServiceAgreement", StringComparison.Ordinal) => "Setup - ServiceAgreements",
            "FgsSetupCommunicationTemplate" or "FgsResolutionCode" => "Setup - Communication",
            "FgsJobTypeCategory" or "FgsJobTypeSubCategory" or "FgsJobType" or "FgsLeadSource" => "Setup - JobTypes",
            var name when name.StartsWith("FgsInventory", StringComparison.Ordinal) => "Setup - Inventory",
            "FgsVendor" or "FgsVendorInventoryItem" => "Setup - Vendors",
            "FgsWarehouse" => "Setup - Warehouses",
            "FgsVehicle" or "FgsVehicleMaintenance" => "Setup - Vehicles",
            "FgsTag" => "Setup - Tags",
            _ => "Setup - Catalog"
        };
}
