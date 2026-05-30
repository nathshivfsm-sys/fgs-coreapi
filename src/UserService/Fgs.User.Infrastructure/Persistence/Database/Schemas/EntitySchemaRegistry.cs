using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Persistence.Database.Schemas;

/// <summary>
/// Maps EF entities to PostgreSQL domain schemas.
/// All <c>Glo*</c> tables belong in the <see cref="FgsDatabaseSchemas.Glo"/> schema.
/// </summary>
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
        // glo — all Glo* global platform lookups and master content
        [typeof(GloAccountingIntegrationType)] = FgsDatabaseSchemas.Glo,
        [typeof(GloBillingCategory)] = FgsDatabaseSchemas.Glo,
        [typeof(GloBusinessType)] = FgsDatabaseSchemas.Glo,
        [typeof(GloCommunicationToken)] = FgsDatabaseSchemas.Glo,
        [typeof(GloCountry)] = FgsDatabaseSchemas.Glo,
        [typeof(GloCredentialCategory)] = FgsDatabaseSchemas.Glo,
        [typeof(GloCredentialProviderType)] = FgsDatabaseSchemas.Glo,
        [typeof(GloInventoryCategory)] = FgsDatabaseSchemas.Glo,
        [typeof(GloInventoryItemType)] = FgsDatabaseSchemas.Glo,
        [typeof(GloInventorySubCategory)] = FgsDatabaseSchemas.Glo,
        [typeof(GloJobTypeCategory)] = FgsDatabaseSchemas.Glo,
        [typeof(GloJobTypeSubCategory)] = FgsDatabaseSchemas.Glo,
        [typeof(GloLanguage)] = FgsDatabaseSchemas.Glo,
        [typeof(GloLeadSource)] = FgsDatabaseSchemas.Glo,
        [typeof(GloLocationType)] = FgsDatabaseSchemas.Glo,
        [typeof(GloMasterEntityType)] = FgsDatabaseSchemas.Glo,
        [typeof(GloOutboxMessage)] = FgsDatabaseSchemas.Glo,
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
        [typeof(GloZone)] = FgsDatabaseSchemas.Glo,

        // identity — authentication and authorization (Fgs* only)
        [typeof(FgsUser)] = FgsDatabaseSchemas.Identity,
        [typeof(FgsUserRole)] = FgsDatabaseSchemas.Identity,
        [typeof(FgsRole)] = FgsDatabaseSchemas.Identity,
        [typeof(FgsInvitation)] = FgsDatabaseSchemas.Identity,

        // tenant — tenant/company management
        [typeof(FgsTenant)] = FgsDatabaseSchemas.Tenant,
        [typeof(FgsTenantCompany)] = FgsDatabaseSchemas.Tenant,
        [typeof(FgsTenantServiceSetup)] = FgsDatabaseSchemas.Tenant,

        // crm — customer relationships
        [typeof(FgsLeadSource)] = FgsDatabaseSchemas.Crm,
        [typeof(FgsSetupTitleOfCourtesy)] = FgsDatabaseSchemas.Crm,

        // dispatch — jobs, scheduling, technicians, field service setup
        [typeof(FgsJobType)] = FgsDatabaseSchemas.Dispatch,
        [typeof(FgsJobTypeCategory)] = FgsDatabaseSchemas.Dispatch,
        [typeof(FgsJobTypeSubCategory)] = FgsDatabaseSchemas.Dispatch,
        [typeof(FgsSetupTimeSlot)] = FgsDatabaseSchemas.Dispatch,
        [typeof(FgsSetupTechTrade)] = FgsDatabaseSchemas.Dispatch,
        [typeof(FgsSetupTechSkillLevel)] = FgsDatabaseSchemas.Dispatch,
        [typeof(FgsSetupZone)] = FgsDatabaseSchemas.Dispatch,
        [typeof(FgsSetupPostalCode)] = FgsDatabaseSchemas.Dispatch,
        [typeof(FgsSetupServiceAssetType)] = FgsDatabaseSchemas.Dispatch,
        [typeof(FgsSetupServiceAssetManufacturer)] = FgsDatabaseSchemas.Dispatch,
        [typeof(FgsSetupServiceAssetModelReference)] = FgsDatabaseSchemas.Dispatch,
        [typeof(FgsResolutionCode)] = FgsDatabaseSchemas.Dispatch,
        [typeof(FgsBusinessType)] = FgsDatabaseSchemas.Dispatch,

        // billing — financial domain
        [typeof(FgsBillingCategory)] = FgsDatabaseSchemas.Billing,
        [typeof(FgsSetupGLBreak)] = FgsDatabaseSchemas.Billing,
        [typeof(FgsSetupGLBreakTrade)] = FgsDatabaseSchemas.Billing,
        [typeof(FgsSetupTax)] = FgsDatabaseSchemas.Billing,
        [typeof(FgsSetupTaxAuthority)] = FgsDatabaseSchemas.Billing,
        [typeof(FgsSetupTaxDetail)] = FgsDatabaseSchemas.Billing,
        [typeof(FgsSetupPaymentMethod)] = FgsDatabaseSchemas.Billing,
        [typeof(FgsSetupPaymentTerm)] = FgsDatabaseSchemas.Billing,
        [typeof(FgsSetupLaborRateType)] = FgsDatabaseSchemas.Billing,
        [typeof(FgsSetupPricingMatrix)] = FgsDatabaseSchemas.Billing,
        [typeof(FgsSetupPricingMatrixLabor)] = FgsDatabaseSchemas.Billing,
        [typeof(FgsSetupPricingMatrixLaborTier)] = FgsDatabaseSchemas.Billing,
        [typeof(FgsSetupPricingMatrixMaterialTier)] = FgsDatabaseSchemas.Billing,
        [typeof(FgsSetupPricingMatrixOther)] = FgsDatabaseSchemas.Billing,

        // inventory — inventory and procurement
        [typeof(FgsInventoryItemType)] = FgsDatabaseSchemas.Inventory,
        [typeof(FgsInventoryCategory)] = FgsDatabaseSchemas.Inventory,
        [typeof(FgsInventorySubCategory)] = FgsDatabaseSchemas.Inventory,
        [typeof(FgsInventoryItem)] = FgsDatabaseSchemas.Inventory,
        [typeof(FgsInventoryStock)] = FgsDatabaseSchemas.Inventory,
        [typeof(FgsInventoryItemAlternate)] = FgsDatabaseSchemas.Inventory,
        [typeof(FgsInventoryItemDependency)] = FgsDatabaseSchemas.Inventory,
        [typeof(FgsVendor)] = FgsDatabaseSchemas.Inventory,
        [typeof(FgsVendorInventoryItem)] = FgsDatabaseSchemas.Inventory,

        // shared — cross-domain infrastructure (Fgs* only)
        [typeof(FgsFile)] = FgsDatabaseSchemas.Shared,
        [typeof(FgsLocation)] = FgsDatabaseSchemas.Shared,
        [typeof(FgsTag)] = FgsDatabaseSchemas.Shared,
        [typeof(FgsEntityTag)] = FgsDatabaseSchemas.Shared,
        [typeof(FgsTagEntityType)] = FgsDatabaseSchemas.Shared,

        // audit — audit trails
        [typeof(FgsCredentialAudit)] = FgsDatabaseSchemas.Audit,

        // integration — third-party connectivity and credentials (Fgs* only)
        [typeof(FgsCredentialProvider)] = FgsDatabaseSchemas.Integration,
        [typeof(FgsCredentialProviderConfiguration)] = FgsDatabaseSchemas.Integration,
        [typeof(FgsCredentialSecret)] = FgsDatabaseSchemas.Integration,

        // notification — messaging and templates (Fgs* only)
        [typeof(FgsSetupCommunicationTemplate)] = FgsDatabaseSchemas.Notification,
        [typeof(FgsSetupDescription)] = FgsDatabaseSchemas.Notification,
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
