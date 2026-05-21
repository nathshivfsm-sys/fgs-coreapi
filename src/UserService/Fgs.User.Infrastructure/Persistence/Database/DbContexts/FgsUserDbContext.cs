using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Persistence.Database.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Persistence.Database.DbContexts;

/// <summary>
/// Code-first EF Core context for FGS user / platform entities (PostgreSQL schema <c>dbo</c>).
/// Entity mappings live in <see cref="Configurations"/> (one file per entity).
/// </summary>
public class FgsUserDbContext : DbContext
{
    public const string FgsSchema = "dbo";

    public FgsUserDbContext(DbContextOptions<FgsUserDbContext> options)
        : base(options)
    {
    }

    public DbSet<FgsTenant> FgsTenants => Set<FgsTenant>();

    public DbSet<FgsTenantCompany> FgsTenantCompanies => Set<FgsTenantCompany>();

    public DbSet<FgsTenantServiceSetup> FgsTenantServiceSetups => Set<FgsTenantServiceSetup>();

    public DbSet<FgsLocation> FgsLocations => Set<FgsLocation>();

    public DbSet<FgsCredentialProvider> FgsCredentialProviders => Set<FgsCredentialProvider>();

    public DbSet<FgsCredentialProviderConfiguration> FgsCredentialProviderConfigurations =>
        Set<FgsCredentialProviderConfiguration>();

    public DbSet<FgsCredentialSecret> FgsCredentialSecrets => Set<FgsCredentialSecret>();

    public DbSet<FgsCredentialAudit> FgsCredentialAudits => Set<FgsCredentialAudit>();

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

    public DbSet<FgsSetupGLBreakTechTrade> FgsSetupGLBreakTechTrades => Set<FgsSetupGLBreakTechTrade>();

    public DbSet<FgsSetupCommunicationTemplate> FgsSetupCommunicationTemplates =>
        Set<FgsSetupCommunicationTemplate>();

    public DbSet<FgsSetupPaymentMethod> FgsSetupPaymentMethods => Set<FgsSetupPaymentMethod>();

    public DbSet<FgsSetupPaymentTerm> FgsSetupPaymentTerms => Set<FgsSetupPaymentTerm>();

    public DbSet<GloMasterEntityType> GloMasterEntityTypes => Set<GloMasterEntityType>();

    public DbSet<GloCommunicationToken> GloCommunicationTokens => Set<GloCommunicationToken>();

    public DbSet<GloTimeCardOption> GloTimeCardOptions => Set<GloTimeCardOption>();

    public DbSet<GloPaymentMethodType> GloPaymentMethodTypes => Set<GloPaymentMethodType>();

    public DbSet<GloCountry> GloCountries => Set<GloCountry>();

    public DbSet<GloStateProvince> GloStateProvinces => Set<GloStateProvince>();

    public DbSet<GloAccountingIntegrationType> GloAccountingIntegrationTypes => Set<GloAccountingIntegrationType>();

    public DbSet<GloBusinessType> GloBusinessTypes => Set<GloBusinessType>();

    public DbSet<GloSetupTenantStatus> GloSetupTenantStatuses => Set<GloSetupTenantStatus>();

    public DbSet<GloLanguage> GloLanguages => Set<GloLanguage>();

    public DbSet<GloBillingCategory> GloBillingCategories => Set<GloBillingCategory>();

    public DbSet<GloLocationType> GloLocationTypes => Set<GloLocationType>();

    public DbSet<GloCredentialCategory> GloCredentialCategories => Set<GloCredentialCategory>();

    public DbSet<GloCredentialProviderType> GloCredentialProviderTypes => Set<GloCredentialProviderType>();

    public DbSet<GloResolutionType> GloResolutionTypes => Set<GloResolutionType>();

    public DbSet<GloRole> GloRoles => Set<GloRole>();

    public DbSet<GloSetupDescriptionType> GloSetupDescriptionTypes => Set<GloSetupDescriptionType>();

    public DbSet<GloSetupLaborRateType> GloSetupLaborRateTypes => Set<GloSetupLaborRateType>();

    public DbSet<FgsUser> FgsUsers => Set<FgsUser>();

    public DbSet<FgsUserRole> FgsUserRoles => Set<FgsUserRole>();

    public DbSet<FgsRole> FgsRoles => Set<FgsRole>();

    public DbSet<FgsInvitation> FgsInvitations => Set<FgsInvitation>();

    public DbSet<FgsFile> FgsFiles => Set<FgsFile>();

    public DbSet<FgsOutboxMessage> FgsOutboxMessages => Set<FgsOutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(FgsSchema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FgsUserDbContext).Assembly);
        ConfigureAuditActorColumns(modelBuilder);
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
