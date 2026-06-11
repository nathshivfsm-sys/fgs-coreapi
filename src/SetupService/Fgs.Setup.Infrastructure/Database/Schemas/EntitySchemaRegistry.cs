using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Database.Schemas;

internal static class EntitySchemaRegistry
{
    private static readonly Dictionary<Type, string> EntitySchemas = BuildEntitySchemas();
    private static readonly Dictionary<string, string> TableSchemas = BuildTableSchemas();

    public static void ApplySchemas(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.ClrType is null)
            {
                continue;
            }

            if (!EntitySchemas.TryGetValue(entityType.ClrType, out var schema))
            {
                throw new InvalidOperationException(
                    $"No PostgreSQL schema mapping for entity '{entityType.ClrType.Name}'. " +
                    "Add it to EntitySchemaRegistry.");
            }

            entityType.SetSchema(schema);
        }
    }

    public static string GetSchemaForTable(string tableName) =>
        TableSchemas.TryGetValue(tableName, out var schema)
            ? schema
            : throw new InvalidOperationException(
                $"No PostgreSQL schema mapping for table '{tableName}'. Add it to EntitySchemaRegistry.");

    public static string QualifyTable(string tableName) =>
        $"{GetSchemaForTable(tableName)}.\"{tableName}\"";

    private static Dictionary<Type, string> BuildEntitySchemas() => new()
    {
        [typeof(GloAccountingIntegrationType)] = FgsDatabaseSchemas.Glo,
        [typeof(GloBillingCategory)] = FgsDatabaseSchemas.Glo,
        [typeof(GloBusinessType)] = FgsDatabaseSchemas.Glo,
        [typeof(GloCommunicationToken)] = FgsDatabaseSchemas.Glo,
        [typeof(GloCommunicationTemplate)] = FgsDatabaseSchemas.Glo,
        [typeof(GloCommunicationTemplateToken)] = FgsDatabaseSchemas.Glo,
        [typeof(GloCountry)] = FgsDatabaseSchemas.Glo,
        [typeof(GloCredential)] = FgsDatabaseSchemas.Glo,
        [typeof(GloCredentialProviderType)] = FgsDatabaseSchemas.Glo,
        [typeof(GloInventoryCategory)] = FgsDatabaseSchemas.Glo,
        [typeof(GloInventoryItemType)] = FgsDatabaseSchemas.Glo,
        [typeof(GloInventorySubCategory)] = FgsDatabaseSchemas.Glo,
        [typeof(GloJobTypeCategory)] = FgsDatabaseSchemas.Glo,
        [typeof(GloJobTypeSubCategory)] = FgsDatabaseSchemas.Glo,
        [typeof(GloLanguage)] = FgsDatabaseSchemas.Glo,
        [typeof(GloLeadSource)] = FgsDatabaseSchemas.Glo,
        [typeof(GloLeadStatus)] = FgsDatabaseSchemas.Glo,
        [typeof(GloLeadDisqualificationReason)] = FgsDatabaseSchemas.Glo,
        [typeof(GloLocationType)] = FgsDatabaseSchemas.Glo,
        [typeof(GloMasterEntityType)] = FgsDatabaseSchemas.Glo,
        [typeof(GloOutboxMessage)] = FgsDatabaseSchemas.Glo,
        [typeof(SetupOutboxMessage)] = FgsDatabaseSchemas.Setup,
        [typeof(GloPaymentMethodType)] = FgsDatabaseSchemas.Glo,
        [typeof(GloResolutionType)] = FgsDatabaseSchemas.Glo,
        [typeof(GloRole)] = FgsDatabaseSchemas.Glo,
        [typeof(GloSeedTableColumnMapping)] = FgsDatabaseSchemas.Glo,
        [typeof(GloSeedTableMapping)] = FgsDatabaseSchemas.Glo,
        [typeof(GloSetupDescriptionType)] = FgsDatabaseSchemas.Glo,
        [typeof(GloSetupLaborRateType)] = FgsDatabaseSchemas.Glo,
        [typeof(GloSetupPaymentTerm)] = FgsDatabaseSchemas.Glo,
        [typeof(GloSetupTenantStatus)] = FgsDatabaseSchemas.Glo,
        [typeof(GloSkill)] = FgsDatabaseSchemas.Glo,
        [typeof(GloStateProvince)] = FgsDatabaseSchemas.Glo,
        [typeof(GloTag)] = FgsDatabaseSchemas.Glo,
        [typeof(GloTimeCardOption)] = FgsDatabaseSchemas.Glo,
        [typeof(GloTitleOfCourtesy)] = FgsDatabaseSchemas.Glo,
        [typeof(GloTrade)] = FgsDatabaseSchemas.Glo,
        [typeof(GloUnitOfMeasure)] = FgsDatabaseSchemas.Glo,
        [typeof(GloVehicleMaintenanceType)] = FgsDatabaseSchemas.Glo,
        [typeof(GloZone)] = FgsDatabaseSchemas.Glo,
        [typeof(FgsLeadSource)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsLeadStatus)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsLeadDisqualificationReason)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupTitleOfCourtesy)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsJobType)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsJobTypeCategory)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsJobTypeSubCategory)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupTimeSlot)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupTechTrade)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupTechSkillLevel)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupZone)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupPostalCode)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupServiceAssetType)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupServiceAssetManufacturer)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupServiceAssetModelReference)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsResolutionCode)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsBusinessType)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsBillingCategory)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupGLBreak)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupGLBreakTrade)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupTax)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupTaxAuthority)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupTaxDetail)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupPaymentMethod)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupPaymentTerm)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupLaborRateType)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupPricingMatrix)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupPricingMatrixLabor)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupPricingMatrixLaborTier)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupPricingMatrixMaterialTier)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupPricingMatrixOther)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsInventoryItemType)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsInventoryCategory)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsInventorySubCategory)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsInventoryItem)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsInventoryStock)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsInventoryItemAlternate)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsInventoryItemDependency)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsVendor)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsVendorInventoryItem)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsVehicle)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsVehicleMaintenance)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsWarehouse)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsCredential)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupCommunicationTemplate)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupDescription)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsTag)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsEntityTag)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsTagEntityType)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupServiceAgreementTemplate)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupServiceAgreementPricingComponent)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupServiceAgreementTemplatePricingComponent)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupServiceAgreementTemplateCoverage)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsTenantCompanyCache)] = FgsDatabaseSchemas.Setup,
        [typeof(GloCredentialProviderTypeCache)] = FgsDatabaseSchemas.Setup,
        [typeof(GloResolutionTypeCache)] = FgsDatabaseSchemas.Setup,
    };

    private static Dictionary<string, string> BuildTableSchemas()
    {
        var tableSchemas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var (entityType, schema) in EntitySchemas)
        {
            tableSchemas[entityType.Name] = schema;
        }

        return tableSchemas;
    }
}
