using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Persistence.Database.Configurations;
using Fgs.User.Infrastructure.Persistence.Database.Schemas;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Persistence.Database.DbContexts;

/// <summary>
/// Code-first EF Core context for FGS user / platform entities (PostgreSQL domain schemas).
/// Entity mappings live in <see cref="Configurations"/> (one file per entity).
/// </summary>
public class FgsUserDbContext : FgsTenantFilteredDbContext
{
    /// <summary>Schema for EF Core migration history (<c>shared</c>).</summary>
    public const string MigrationHistorySchema = FgsDatabaseSchemas.MigrationHistory;

    /// <summary>Legacy alias; use <see cref="MigrationHistorySchema"/>.</summary>
    [Obsolete("Use MigrationHistorySchema. dbo is no longer the default schema.")]
    public const string FgsSchema = MigrationHistorySchema;

    public FgsUserDbContext(
        DbContextOptions<FgsUserDbContext> options,
        ITenantContextAccessor tenantContextAccessor)
        : base(options, tenantContextAccessor)
    {
    }

    public DbSet<FgsTenant> FgsTenants => Set<FgsTenant>();

    public DbSet<FgsTenantCompany> FgsTenantCompanies => Set<FgsTenantCompany>();

    public DbSet<FgsTenantServiceSetup> FgsTenantServiceSetups => Set<FgsTenantServiceSetup>();

    public DbSet<FgsLocation> FgsLocations => Set<FgsLocation>();

    public DbSet<FgsCredential> FgsCredentials => Set<FgsCredential>();

    public DbSet<FgsCredentialAudit> FgsCredentialAudits => Set<FgsCredentialAudit>();

    public DbSet<GloCredential> GloCredentials => Set<GloCredential>();

    public DbSet<GloCredentialProviderType> GloCredentialProviderTypes => Set<GloCredentialProviderType>();

    public DbSet<FgsSetupTechTrade> FgsSetupTechTrades => Set<FgsSetupTechTrade>();

    public DbSet<FgsSetupTechSkillLevel> FgsSetupTechSkillLevels => Set<FgsSetupTechSkillLevel>();

    public DbSet<FgsSetupTimeSlot> FgsSetupTimeSlots => Set<FgsSetupTimeSlot>();

    public DbSet<FgsSetupZone> FgsSetupZones => Set<FgsSetupZone>();

    public DbSet<FgsSetupPostalCode> FgsSetupPostalCodes => Set<FgsSetupPostalCode>();

    public DbSet<FgsSetupTax> FgsSetupTaxes => Set<FgsSetupTax>();

    public DbSet<FgsSetupTaxAuthority> FgsSetupTaxAuthorities => Set<FgsSetupTaxAuthority>();

    public DbSet<FgsSetupTaxDetail> FgsSetupTaxDetails => Set<FgsSetupTaxDetail>();

    public DbSet<FgsSetupTitleOfCourtesy> FgsSetupTitlesOfCourtesy => Set<FgsSetupTitleOfCourtesy>();

    public DbSet<FgsSetupDescription> FgsSetupDescriptions => Set<FgsSetupDescription>();

    public DbSet<FgsSetupServiceAssetType> FgsSetupServiceAssetTypes => Set<FgsSetupServiceAssetType>();

    public DbSet<FgsSetupServiceAssetManufacturer> FgsSetupServiceAssetManufacturers =>
        Set<FgsSetupServiceAssetManufacturer>();

    public DbSet<FgsSetupServiceAssetModelReference> FgsSetupServiceAssetModelReferences =>
        Set<FgsSetupServiceAssetModelReference>();

    public DbSet<FgsResolutionCode> FgsResolutionCodes => Set<FgsResolutionCode>();

    public DbSet<FgsSetupPricingMatrix> FgsSetupPricingMatrices => Set<FgsSetupPricingMatrix>();

    public DbSet<FgsSetupPricingMatrixLabor> FgsSetupPricingMatrixLabors => Set<FgsSetupPricingMatrixLabor>();

    public DbSet<FgsSetupPricingMatrixLaborTier> FgsSetupPricingMatrixLaborTiers => Set<FgsSetupPricingMatrixLaborTier>();

    public DbSet<FgsSetupPricingMatrixMaterialTier> FgsSetupPricingMatrixMaterialTiers =>
        Set<FgsSetupPricingMatrixMaterialTier>();

    public DbSet<FgsSetupPricingMatrixOther> FgsSetupPricingMatrixOthers => Set<FgsSetupPricingMatrixOther>();

    public DbSet<FgsSetupGLBreak> FgsSetupGLBreaks => Set<FgsSetupGLBreak>();

    public DbSet<FgsSetupGLBreakTrade> FgsSetupGLBreakTrades => Set<FgsSetupGLBreakTrade>();

    public DbSet<FgsSetupCommunicationTemplate> FgsSetupCommunicationTemplates =>
        Set<FgsSetupCommunicationTemplate>();

    public DbSet<FgsSetupPaymentMethod> FgsSetupPaymentMethods => Set<FgsSetupPaymentMethod>();

    public DbSet<FgsSetupPaymentTerm> FgsSetupPaymentTerms => Set<FgsSetupPaymentTerm>();

    public DbSet<FgsBillingCategory> FgsBillingCategories => Set<FgsBillingCategory>();

    public DbSet<FgsBusinessType> FgsBusinessTypes => Set<FgsBusinessType>();

    public DbSet<FgsSetupLaborRateType> FgsSetupLaborRateTypes => Set<FgsSetupLaborRateType>();

    public DbSet<GloSetupPaymentTerm> GloSetupPaymentTerms => Set<GloSetupPaymentTerm>();

    public DbSet<GloInventoryItemType> GloInventoryItemTypes => Set<GloInventoryItemType>();

    public DbSet<GloInventoryCategory> GloInventoryCategories => Set<GloInventoryCategory>();

    public DbSet<GloInventorySubCategory> GloInventorySubCategories => Set<GloInventorySubCategory>();

    public DbSet<FgsInventoryItemType> FgsInventoryItemTypes => Set<FgsInventoryItemType>();

    public DbSet<FgsInventoryCategory> FgsInventoryCategories => Set<FgsInventoryCategory>();

    public DbSet<FgsInventorySubCategory> FgsInventorySubCategories => Set<FgsInventorySubCategory>();

    public DbSet<FgsInventoryItem> FgsInventoryItems => Set<FgsInventoryItem>();

    public DbSet<FgsInventoryStock> FgsInventoryStocks => Set<FgsInventoryStock>();

    public DbSet<FgsInventoryItemAlternate> FgsInventoryItemAlternates => Set<FgsInventoryItemAlternate>();

    public DbSet<FgsInventoryItemDependency> FgsInventoryItemDependencies => Set<FgsInventoryItemDependency>();

    public DbSet<FgsVendor> FgsVendors => Set<FgsVendor>();

    public DbSet<FgsVendorInventoryItem> FgsVendorInventoryItems => Set<FgsVendorInventoryItem>();

    public DbSet<FgsWarehouse> FgsWarehouses => Set<FgsWarehouse>();

    public DbSet<FgsVehicle> FgsVehicles => Set<FgsVehicle>();

    public DbSet<FgsVehicleMaintenance> FgsVehicleMaintenances => Set<FgsVehicleMaintenance>();

    public DbSet<GloVehicleMaintenanceType> GloVehicleMaintenanceTypes => Set<GloVehicleMaintenanceType>();

    public DbSet<GloMasterEntityType> GloMasterEntityTypes => Set<GloMasterEntityType>();

    public DbSet<GloCommunicationToken> GloCommunicationTokens => Set<GloCommunicationToken>();

    public DbSet<GloCommunicationTemplate> GloCommunicationTemplates => Set<GloCommunicationTemplate>();

    public DbSet<GloCommunicationTemplateToken> GloCommunicationTemplateTokens =>
        Set<GloCommunicationTemplateToken>();

    public DbSet<GloTimeCardOption> GloTimeCardOptions => Set<GloTimeCardOption>();

    public DbSet<GloPaymentMethodType> GloPaymentMethodTypes => Set<GloPaymentMethodType>();

    public DbSet<GloCountry> GloCountries => Set<GloCountry>();

    public DbSet<GloStateProvince> GloStateProvinces => Set<GloStateProvince>();

    public DbSet<GloAccountingIntegrationType> GloAccountingIntegrationTypes => Set<GloAccountingIntegrationType>();

    public DbSet<GloBusinessType> GloBusinessTypes => Set<GloBusinessType>();

    public DbSet<GloTrade> GloTrades => Set<GloTrade>();

    public DbSet<GloSkill> GloSkills => Set<GloSkill>();

    public DbSet<GloLeadSource> GloLeadSources => Set<GloLeadSource>();

    public DbSet<FgsLeadSource> FgsLeadSources => Set<FgsLeadSource>();

    public DbSet<GloZone> GloZones => Set<GloZone>();

    public DbSet<GloJobTypeCategory> GloJobTypeCategories => Set<GloJobTypeCategory>();

    public DbSet<GloJobTypeSubCategory> GloJobTypeSubCategories => Set<GloJobTypeSubCategory>();

    public DbSet<FgsJobTypeCategory> FgsJobTypeCategories => Set<FgsJobTypeCategory>();

    public DbSet<FgsJobTypeSubCategory> FgsJobTypeSubCategories => Set<FgsJobTypeSubCategory>();

    public DbSet<FgsJobType> FgsJobTypes => Set<FgsJobType>();

    public DbSet<GloUnitOfMeasure> GloUnitOfMeasures => Set<GloUnitOfMeasure>();

    public DbSet<GloTag> GloTags => Set<GloTag>();

    public DbSet<FgsTag> FgsTags => Set<FgsTag>();

    public DbSet<FgsEntityTag> FgsEntityTags => Set<FgsEntityTag>();

    public DbSet<FgsTagEntityType> FgsTagEntityTypes => Set<FgsTagEntityType>();

    public DbSet<GloTitleOfCourtesy> GloTitlesOfCourtesy => Set<GloTitleOfCourtesy>();

    public DbSet<GloSetupTenantStatus> GloSetupTenantStatuses => Set<GloSetupTenantStatus>();

    public DbSet<GloLanguage> GloLanguages => Set<GloLanguage>();

    public DbSet<GloBillingCategory> GloBillingCategories => Set<GloBillingCategory>();

    public DbSet<GloLocationType> GloLocationTypes => Set<GloLocationType>();

    public DbSet<GloResolutionType> GloResolutionTypes => Set<GloResolutionType>();

    public DbSet<GloRole> GloRoles => Set<GloRole>();

    public DbSet<GloSetupDescriptionType> GloSetupDescriptionTypes => Set<GloSetupDescriptionType>();

    public DbSet<GloSetupLaborRateType> GloSetupLaborRateTypes => Set<GloSetupLaborRateType>();

    public DbSet<FgsUser> FgsUsers => Set<FgsUser>();

    public DbSet<FgsUserRole> FgsUserRoles => Set<FgsUserRole>();

    public DbSet<FgsRole> FgsRoles => Set<FgsRole>();

    public DbSet<FgsInvitation> FgsInvitations => Set<FgsInvitation>();

    public DbSet<FgsFile> FgsFiles => Set<FgsFile>();

    public DbSet<GloOutboxMessage> GloOutboxMessages => Set<GloOutboxMessage>();

    public DbSet<GloSeedTableMapping> GloSeedTableMappings => Set<GloSeedTableMapping>();

    public DbSet<GloSeedTableColumnMapping> GloSeedTableColumnMappings => Set<GloSeedTableColumnMapping>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FgsUserDbContext).Assembly);
        EntitySchemaRegistry.ApplySchemas(modelBuilder);
        ConfigureAuditActorColumns(modelBuilder);
        ApplyFgsTenantQueryFilters(modelBuilder);
        ApplyFgsNullableTenantCompanyQueryFilter<FgsSetupCommunicationTemplate>(modelBuilder);
    }

    private static void ConfigureAuditActorColumns(ModelBuilder modelBuilder)
    {
        const int maxLength = 100;
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var createdBy = entityType.FindProperty("CreatedBy");
            if (createdBy?.ClrType == typeof(string)
                && !string.Equals(createdBy.GetColumnType(), "bigint", StringComparison.OrdinalIgnoreCase))
            {
                createdBy.SetMaxLength(maxLength);
            }

            var updatedBy = entityType.FindProperty("UpdatedBy");
            if (updatedBy?.ClrType == typeof(string)
                && !string.Equals(updatedBy.GetColumnType(), "bigint", StringComparison.OrdinalIgnoreCase))
            {
                updatedBy.SetMaxLength(maxLength);
            }
        }
    }
}
