using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Persistence.Database.Schemas;

/// <summary>
/// Maps EF entities to PostgreSQL domain schemas.
/// All <c>Glo*</c> tables belong in the <see cref="FgsDatabaseSchemas.Glo"/> schema.
/// Tenant/company setup tables belong in <see cref="FgsDatabaseSchemas.Setup"/>.
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
        [typeof(GloCommunicationTemplate)] = FgsDatabaseSchemas.Glo,
        [typeof(GloCommunicationTemplateToken)] = FgsDatabaseSchemas.Glo,
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

        // setup — tenant/company configuration (billing, crm, dispatch, inventory, integration, notification)
        [typeof(FgsLeadSource)] = FgsDatabaseSchemas.Setup,
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
        [typeof(FgsCredentialProvider)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsCredentialProviderConfiguration)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsCredentialSecret)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupCommunicationTemplate)] = FgsDatabaseSchemas.Setup,
        [typeof(FgsSetupDescription)] = FgsDatabaseSchemas.Setup,

        // shared — cross-domain infrastructure (Fgs* only)
        [typeof(FgsFile)] = FgsDatabaseSchemas.Shared,
        [typeof(FgsLocation)] = FgsDatabaseSchemas.Shared,
        [typeof(FgsTag)] = FgsDatabaseSchemas.Shared,
        [typeof(FgsEntityTag)] = FgsDatabaseSchemas.Shared,
        [typeof(FgsTagEntityType)] = FgsDatabaseSchemas.Shared,

        // audit — audit trails
        [typeof(FgsCredentialAudit)] = FgsDatabaseSchemas.Audit,
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
