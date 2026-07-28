using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Database.Configurations;
using Fgs.Setup.Infrastructure.Database.Schemas;
using Fgs.MultiTenancy;
using Fgs.MultiTenancy.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Database;

/// <summary>
/// Code-first EF Core context for FGS setup entities (PostgreSQL domain schemas).
/// Entity mappings live in <c>Configurations</c> (one file per entity).
/// </summary>
public class FgsSetupDbContext : FgsTenantFilteredDbContext
{
    /// <summary>Schema for EF Core migration history (<c>setup</c>).</summary>
    public const string MigrationHistorySchema = FgsDatabaseSchemas.MigrationHistory;

    /// <summary>Legacy alias; use <see cref="MigrationHistorySchema"/>.</summary>
    [Obsolete("Use MigrationHistorySchema. dbo is no longer the default schema.")]
    public const string FgsSchema = MigrationHistorySchema;

    public FgsSetupDbContext(
        DbContextOptions<FgsSetupDbContext> options,
        ITenantContextAccessor tenantContextAccessor)
        : base(options, tenantContextAccessor)
    {
    }

    public DbSet<FgsCredential> FgsCredentials => Set<FgsCredential>();

    public DbSet<FgsCrew> FgsCrews => Set<FgsCrew>();

    public DbSet<FgsCrewMember> FgsCrewMembers => Set<FgsCrewMember>();

    public DbSet<FgsEmployee> FgsEmployees => Set<FgsEmployee>();

    public DbSet<FgsEmployeeTechnicianProfile> FgsEmployeeTechnicianProfiles =>
        Set<FgsEmployeeTechnicianProfile>();

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

    public DbSet<FgsResolutionCode> FgsResolutionCodes => Set<FgsResolutionCode>();

    public DbSet<FgsSetupPricingMatrix> FgsSetupPricingMatrices => Set<FgsSetupPricingMatrix>();

    public DbSet<FgsSetupPricingMatrixLabor> FgsSetupPricingMatrixLabors => Set<FgsSetupPricingMatrixLabor>();

    public DbSet<FgsSetupPricingMatrixLaborTier> FgsSetupPricingMatrixLaborTiers => Set<FgsSetupPricingMatrixLaborTier>();

    public DbSet<FgsSetupPricingMatrixMaterialTier> FgsSetupPricingMatrixMaterialTiers =>
        Set<FgsSetupPricingMatrixMaterialTier>();

    public DbSet<FgsSetupPricingMatrixOther> FgsSetupPricingMatrixOthers => Set<FgsSetupPricingMatrixOther>();

    public DbSet<FgsUniversalPricingService> FgsUniversalPricingServices => Set<FgsUniversalPricingService>();

    public DbSet<FgsUniversalMatrixTier> FgsUniversalMatrixTiers => Set<FgsUniversalMatrixTier>();

    public DbSet<FgsUniversalMatrixSizeTier> FgsUniversalMatrixSizeTiers => Set<FgsUniversalMatrixSizeTier>();

    public DbSet<FgsUniversalMatrixItem> FgsUniversalMatrixItems => Set<FgsUniversalMatrixItem>();

    public DbSet<FgsUniversalMatrixFrequencyDiscount> FgsUniversalMatrixFrequencyDiscounts =>
        Set<FgsUniversalMatrixFrequencyDiscount>();

    public DbSet<FgsUniversalMatrixOneTimeFee> FgsUniversalMatrixOneTimeFees => Set<FgsUniversalMatrixOneTimeFee>();

    public DbSet<FgsUniversalMatrixAddOn> FgsUniversalMatrixAddOns => Set<FgsUniversalMatrixAddOn>();

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

    public DbSet<GloInventoryTransactionSourceType> GloInventoryTransactionSourceTypes =>
        Set<GloInventoryTransactionSourceType>();

    public DbSet<GloInventoryTransactionType> GloInventoryTransactionTypes =>
        Set<GloInventoryTransactionType>();

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

    public DbSet<GloLeadStatus> GloLeadStatuses => Set<GloLeadStatus>();

    public DbSet<GloAppointmentAssignmentEventType> GloAppointmentAssignmentEventTypes =>
        Set<GloAppointmentAssignmentEventType>();

    public DbSet<FgsLeadStatus> FgsLeadStatuses => Set<FgsLeadStatus>();

    public DbSet<GloLeadDisqualificationReason> GloLeadDisqualificationReasons => Set<GloLeadDisqualificationReason>();

    public DbSet<FgsLeadDisqualificationReason> FgsLeadDisqualificationReasons => Set<FgsLeadDisqualificationReason>();

    public DbSet<GloEstimateFlavor> GloEstimateFlavors => Set<GloEstimateFlavor>();

    public DbSet<GloEstimateStatus> GloEstimateStatuses => Set<GloEstimateStatus>();

    public DbSet<GloSalesPipelineStatus> GloSalesPipelineStatuses => Set<GloSalesPipelineStatus>();

    public DbSet<FgsSalesPipelineStatus> FgsSalesPipelineStatuses => Set<FgsSalesPipelineStatus>();

    public DbSet<GloSalesDispositionReason> GloSalesDispositionReasons => Set<GloSalesDispositionReason>();

    public DbSet<FgsSalesDispositionReason> FgsSalesDispositionReasons => Set<FgsSalesDispositionReason>();

    public DbSet<GloSalesActivityType> GloSalesActivityTypes => Set<GloSalesActivityType>();

    public DbSet<FgsSalesActivityType> FgsSalesActivityTypes => Set<FgsSalesActivityType>();

    public DbSet<GloSalesActivityOutcome> GloSalesActivityOutcomes => Set<GloSalesActivityOutcome>();

    public DbSet<FgsSalesActivityOutcome> FgsSalesActivityOutcomes => Set<FgsSalesActivityOutcome>();

    public DbSet<GloZone> GloZones => Set<GloZone>();

    public DbSet<GloJobTypeCategory> GloJobTypeCategories => Set<GloJobTypeCategory>();

    public DbSet<FgsJobCategory> FgsJobCategories => Set<FgsJobCategory>();

    public DbSet<FgsJobTypeCategory> FgsJobTypeCategories => Set<FgsJobTypeCategory>();

    public DbSet<FgsJobTypeTask> FgsJobTypeTasks => Set<FgsJobTypeTask>();

    public DbSet<FgsJobType> FgsJobTypes => Set<FgsJobType>();

    public DbSet<FgsPriceBook> FgsPriceBooks => Set<FgsPriceBook>();

    public DbSet<FgsPriceBookItem> FgsPriceBookItems => Set<FgsPriceBookItem>();

    public DbSet<GloUnitOfMeasure> GloUnitOfMeasures => Set<GloUnitOfMeasure>();

    public DbSet<GloUniversalPricingService> GloUniversalPricingServices => Set<GloUniversalPricingService>();

    public DbSet<GloUniversalMatrixTier> GloUniversalMatrixTiers => Set<GloUniversalMatrixTier>();

    public DbSet<GloUniversalMatrixSizeTier> GloUniversalMatrixSizeTiers => Set<GloUniversalMatrixSizeTier>();

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

    public DbSet<GloOutboxMessage> GloOutboxMessages => Set<GloOutboxMessage>();

    public DbSet<SetupOutboxMessage> SetupOutboxMessages => Set<SetupOutboxMessage>();

    public DbSet<GloSeedTableMapping> GloSeedTableMappings => Set<GloSeedTableMapping>();

    public DbSet<GloSeedTableColumnMapping> GloSeedTableColumnMappings => Set<GloSeedTableColumnMapping>();

    public DbSet<FgsSetupServiceAgreementTemplate> FgsSetupServiceAgreementTemplates =>
        Set<FgsSetupServiceAgreementTemplate>();

    public DbSet<FgsSetupServiceAgreementPricingComponent> FgsSetupServiceAgreementPricingComponents =>
        Set<FgsSetupServiceAgreementPricingComponent>();

    public DbSet<FgsSetupServiceAgreementTemplatePricingComponent> FgsSetupServiceAgreementTemplatePricingComponents =>
        Set<FgsSetupServiceAgreementTemplatePricingComponent>();

    public DbSet<FgsSetupServiceAgreementTemplateCoverage> FgsSetupServiceAgreementTemplateCoverages =>
        Set<FgsSetupServiceAgreementTemplateCoverage>();

    public DbSet<FgsLocation> FgsLocations => Set<FgsLocation>();

    public DbSet<FgsTenantCompanyCache> FgsTenantCompanyCaches => Set<FgsTenantCompanyCache>();

    public DbSet<GloCredentialProviderTypeCache> GloCredentialProviderTypeCaches => Set<GloCredentialProviderTypeCache>();

    public DbSet<GloResolutionTypeCache> GloResolutionTypeCaches => Set<GloResolutionTypeCache>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FgsSetupDbContext).Assembly);
        EntitySchemaRegistry.ApplySchemas(modelBuilder);
        FgsSetupDbContextConfigurationExtensions.ApplyTenantCompanyCacheForeignKeys(modelBuilder);
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
            if (createdBy?.ClrType == typeof(string))
            {
                createdBy.SetMaxLength(maxLength);
            }

            var updatedBy = entityType.FindProperty("UpdatedBy");
            if (updatedBy?.ClrType == typeof(string))
            {
                updatedBy.SetMaxLength(maxLength);
            }
        }
    }
}
