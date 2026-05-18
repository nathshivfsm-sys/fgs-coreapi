using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Fgs.User.Infrastructure.Database;

/// <summary>
/// Code-first EF Core context for FGS user / platform entities (PostgreSQL schema <c>dbo</c>).
/// </summary>
public partial class FgsUserDbContext : DbContext
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

    public DbSet<FgsSetupPriceSheet> FgsSetupPriceSheets => Set<FgsSetupPriceSheet>();

    public DbSet<FgsSetupPriceSheetLabor> FgsSetupPriceSheetLabors => Set<FgsSetupPriceSheetLabor>();

    public DbSet<FgsSetupPriceSheetLaborTier> FgsSetupPriceSheetLaborTiers => Set<FgsSetupPriceSheetLaborTier>();

    public DbSet<FgsSetupPriceSheetMaterial> FgsSetupPriceSheetMaterials => Set<FgsSetupPriceSheetMaterial>();

    public DbSet<FgsSetupPriceSheetOther> FgsSetupPriceSheetOthers => Set<FgsSetupPriceSheetOther>();

    public DbSet<FgsSetupGLBreak> FgsSetupGLBreaks => Set<FgsSetupGLBreak>();

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

    public DbSet<FgsOutboxMessage> FgsOutboxMessages => Set<FgsOutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(FgsSchema);

        ConfigureGloMasterEntityType(modelBuilder);
        ConfigureGloCommunicationToken(modelBuilder);
        ConfigureGloTimeCardOption(modelBuilder);
        ConfigureGloPaymentMethodType(modelBuilder);
        ConfigureGloCountry(modelBuilder);
        ConfigureGloStateProvince(modelBuilder);
        ConfigureGloAccountingIntegrationType(modelBuilder);
        ConfigureGloBusinessType(modelBuilder);
        ConfigureGloLanguage(modelBuilder);
        ConfigureGloBillingCategory(modelBuilder);
        ConfigureGloLocationType(modelBuilder);
        ConfigureGloCredentialCategory(modelBuilder);
        ConfigureGloCredentialProviderType(modelBuilder);
        ConfigureGloResolutionType(modelBuilder);
        ConfigureGloRole(modelBuilder);
        ConfigureGloSetupDescriptionType(modelBuilder);
        ConfigureGloSetupLaborRateType(modelBuilder);

        ConfigureFgsUser(modelBuilder);
        ConfigureFgsUserRole(modelBuilder);
        ConfigureFgsRole(modelBuilder);
        ConfigureFgsInvitation(modelBuilder);
        ConfigureFgsOutboxMessage(modelBuilder);

        ConfigureFgsTenant(modelBuilder);
        ConfigureFgsTenantCompany(modelBuilder);
        ConfigureFgsTenantServiceSetup(modelBuilder);
        ConfigureFgsLocation(modelBuilder);
        ConfigureFgsCredentialProvider(modelBuilder);
        ConfigureFgsCredentialProviderConfiguration(modelBuilder);
        ConfigureFgsCredentialSecret(modelBuilder);
        ConfigureFgsCredentialAudit(modelBuilder);

        MapSetupEntity<FgsSetupTechTrade>(modelBuilder, "FgsSetupTechTrade");
        MapSetupEntity<FgsSetupTechSkillLevel>(modelBuilder, "FgsSetupTechSkillLevel");
        MapSetupEntity<FgsSetupTimeSlot>(modelBuilder, "FgsSetupTimeSlot");
        MapSetupEntity<FgsSetupZone>(modelBuilder, "FgsSetupZone");
        MapSetupEntity<FgsSetupPostalCode>(modelBuilder, "FgsSetupPostalCode");
        MapSetupEntity<FgsSetupTax>(modelBuilder, "FgsSetupTax");
        MapSetupEntity<FgsSetupTaxAuthority>(modelBuilder, "FgsSetupTaxAuthority");
        MapSetupEntity<FgsSetupTaxDetail>(modelBuilder, "FgsSetupTaxDetail");
        MapSetupEntity<FgsSetupTitleOfCourtesy>(modelBuilder, "FgsSetupTitleOfCourtesy");
        MapSetupEntity<FgsSetupDescription>(modelBuilder, "FgsSetupDescription");
        MapSetupEntity<FgsSetupServiceAssetType>(modelBuilder, "FgsSetupServiceAssetType");
        MapSetupEntity<FgsSetupServiceAssetManufacturer>(modelBuilder, "FgsSetupServiceAssetManufacturer");
        MapSetupEntity<FgsSetupPriceSheet>(modelBuilder, "FgsSetupPriceSheet");
        MapSetupEntity<FgsSetupPriceSheetLabor>(modelBuilder, "FgsSetupPriceSheetLabor");
        MapSetupEntity<FgsSetupPriceSheetLaborTier>(modelBuilder, "FgsSetupPriceSheetLaborTier");
        MapSetupEntity<FgsSetupPriceSheetMaterial>(modelBuilder, "FgsSetupPriceSheetMaterial");
        MapSetupEntity<FgsSetupPriceSheetOther>(modelBuilder, "FgsSetupPriceSheetOther");
        MapSetupEntity<FgsSetupGLBreak>(modelBuilder, "FgsSetupGLBreak");
        MapSetupEntity<FgsSetupGLBreakTechTrade>(modelBuilder, "FgsSetupGLBreakTechTrade");
        MapSetupEntity<FgsSetupCommunicationTemplate>(modelBuilder, "FgsSetupCommunicationTemplate");
        MapSetupEntity<FgsSetupPaymentMethod>(modelBuilder, "FgsSetupPaymentMethod");
        MapSetupEntity<FgsSetupPaymentTerm>(modelBuilder, "FgsSetupPaymentTerm");

        ConfigureFgsSetupPriceSheetLaborRelationship(modelBuilder);
        ConfigureFgsSetupGLBreakTechTradeRelationships(modelBuilder);
        ConfigureAuditActorColumns(modelBuilder);
        ConfigureCleanUpTables(modelBuilder);
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

    private static void MapSetupEntity<TEntity>(ModelBuilder modelBuilder, string tableName)
        where TEntity : FgsTenantCompanySetupEntityBase
    {
        modelBuilder.Entity<TEntity>(entity =>
        {
            entity.ToTable(tableName);
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnOrder(0);
            entity.Property(e => e.TenantId).HasColumnOrder(1);
            entity.Property(e => e.CompanyId).HasColumnOrder(2);
            entity.HasIndex(e => new { e.TenantId, e.CompanyId });
            entity.HasOne<FgsTenantCompany>()
                .WithMany()
                .HasForeignKey(e => new { e.TenantId, e.CompanyId })
                .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyNumber })
                .OnDelete(DeleteBehavior.Restrict);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        });
    }

    private static void ConfigureGloMasterEntityType(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GloMasterEntityType>(entity =>
        {
            entity.ToTable("GloMasterEntityType");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        });
    }

    private static void ConfigureGloCommunicationToken(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GloCommunicationToken>(entity =>
        {
            entity.ToTable("GloCommunicationToken");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        });
    }

    private static void ConfigureGloTimeCardOption(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GloTimeCardOption>(entity =>
        {
            entity.ToTable("GloTimeCardOption");
            entity.HasKey(e => e.Id);
        });
    }

    private static void ConfigureGloPaymentMethodType(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GloPaymentMethodType>(entity =>
        {
            entity.ToTable("GloPaymentMethodType");
            entity.HasKey(e => e.Id);
        });
    }

    private static void ConfigureGloCountry(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GloCountry>(entity =>
        {
            entity.ToTable("GloCountry");
            entity.HasKey(e => e.CountryCode);
            entity.Property(e => e.CountryCode).HasMaxLength(2);
            entity.Property(e => e.CountryName).HasMaxLength(100);
            entity.Property(e => e.CurrencyCode).HasMaxLength(3);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });
    }

    private static void ConfigureGloStateProvince(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GloStateProvince>(entity =>
        {
            entity.ToTable("GloStateProvince");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityByDefaultColumn();
            entity.Property(e => e.CountryCode).HasMaxLength(2);
            entity.Property(e => e.StateProvinceCode).HasMaxLength(10);
            entity.Property(e => e.StateProvinceName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasIndex(e => new { e.CountryCode, e.StateProvinceCode })
                .IsUnique()
                .HasDatabaseName("UQ_GloStateProvince");
            entity.HasOne(e => e.Country)
                .WithMany()
                .HasForeignKey(e => e.CountryCode)
                .HasConstraintName("FK_GloStateProvince_Country")
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureGloAccountingIntegrationType(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GloAccountingIntegrationType>(entity =>
        {
            entity.ToTable("GloAccountingIntegrationType");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        });
    }

    private static void ConfigureGloBusinessType(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GloBusinessType>(entity =>
        {
            entity.ToTable("GloBusinessType");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        });
    }

    private static void ConfigureGloLanguage(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GloLanguage>(entity =>
        {
            entity.ToTable("GloLanguage");
            entity.HasKey(e => e.LanguageCode);
            entity.Property(e => e.LanguageCode).HasMaxLength(5);
            entity.Property(e => e.LanguageName).HasMaxLength(100);
            entity.Property(e => e.CultureCode).HasMaxLength(10);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });
    }

    private static void ConfigureGloBillingCategory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GloBillingCategory>(entity =>
        {
            entity.ToTable("GloBillingCategory");
            entity.HasKey(e => e.BillingCategoryType);
            entity.Property(e => e.BillingCategoryType).HasMaxLength(2);
            entity.Property(e => e.BillingCategoryName).HasMaxLength(100);
        });
    }

    private static void ConfigureGloLocationType(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GloLocationType>(entity =>
        {
            entity.ToTable("GloLocationType");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        });
    }

    private static void ConfigureGloCredentialCategory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GloCredentialCategory>(entity =>
        {
            entity.ToTable("GloCredentialCategory");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        });
    }

    private static void ConfigureGloCredentialProviderType(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GloCredentialProviderType>(entity =>
        {
            entity.ToTable("GloCredentialProviderType");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        });
    }

    private static void ConfigureGloResolutionType(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GloResolutionType>(entity =>
        {
            entity.ToTable("GloResolutionType");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ResolutionTypeCode).HasMaxLength(50);
            entity.Property(e => e.ResolutionTypeName).HasMaxLength(200);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        });
    }

    private static void ConfigureGloRole(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GloRole>(entity =>
        {
            entity.ToTable("GloRole");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasColumnType("smallint")
                .UseIdentityByDefaultColumn();
            entity.HasIndex(e => e.RoleCode).IsUnique();
            entity.Property(e => e.RoleCode).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.RoleLevel).HasMaxLength(20);
            entity.Property(e => e.SortOrder).HasColumnType("smallint");
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        });
    }

    private static void ConfigureGloSetupDescriptionType(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GloSetupDescriptionType>(entity =>
        {
            entity.ToTable("GloSetupDescriptionType");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        });
    }

    private static void ConfigureGloSetupLaborRateType(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GloSetupLaborRateType>(entity =>
        {
            entity.ToTable("GloSetupLaborRateType");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        });
    }

    private static void ConfigureFgsRole(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FgsRole>(entity =>
        {
            entity.ToTable("FgsRole");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnOrder(0);
            entity.Property(e => e.TenantId).HasColumnOrder(1);
            entity.Property(e => e.CompanyId).HasColumnOrder(2);
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.RoleCode }).IsUnique();
            entity.Property(e => e.RoleCode).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.HasOne<FgsTenantCompany>()
                .WithMany()
                .HasForeignKey(e => new { e.TenantId, e.CompanyId })
                .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyNumber })
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<GloRole>()
                .WithMany()
                .HasForeignKey(e => e.GloRoleId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        });
    }

    private static void ConfigureFgsUserRole(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FgsUserRole>(entity =>
        {
            entity.ToTable("FgsUserRole");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => new { e.TenantId, e.CompanyId });
            entity.HasIndex(e => e.GloRoleId);
            entity.HasIndex(e => e.FgsRoleId);
            entity.HasIndex(e => new { e.UserId, e.GloRoleId })
                .IsUnique()
                .HasFilter("\"GloRoleId\" IS NOT NULL");
            entity.HasIndex(e => new { e.UserId, e.FgsRoleId })
                .IsUnique()
                .HasFilter("\"FgsRoleId\" IS NOT NULL");
            entity.ToTable(t => t.HasCheckConstraint(
                "CK_FgsUserRole_OnlyOneRole",
                "(\"GloRoleId\" IS NOT NULL AND \"FgsRoleId\" IS NULL) OR (\"GloRoleId\" IS NULL AND \"FgsRoleId\" IS NOT NULL)"));
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne<FgsTenantCompany>()
                .WithMany()
                .HasForeignKey(e => new { e.TenantId, e.CompanyId })
                .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyNumber })
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.GloRole)
                .WithMany()
                .HasForeignKey(e => e.GloRoleId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.FgsRole)
                .WithMany()
                .HasForeignKey(e => e.FgsRoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureFgsUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FgsUser>(entity =>
        {
            entity.ToTable("FgsUser");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.TenantId, e.Email })
                .IsUnique()
                .HasFilter("\"IsDeleted\" = false");
            entity.Property(e => e.Email).HasMaxLength(300);
            entity.Property(e => e.DisplayName).HasMaxLength(200);
            entity.Property(e => e.EntraObjectId).HasMaxLength(100);
            entity.Property(e => e.Role).HasConversion<string>().HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
            entity.HasOne(e => e.Tenant)
                .WithMany()
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Company)
                .WithMany()
                .HasForeignKey(e => new { e.TenantId, e.CompanyId })
                .HasPrincipalKey(c => new { c.TenantId, c.CompanyNumber })
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureFgsInvitation(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FgsInvitation>(entity =>
        {
            entity.ToTable("FgsInvitation");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.TokenHash);
            entity.HasIndex(e => new { e.TenantId, e.Email, e.Status });
            entity.Property(e => e.Email).HasMaxLength(300);
            entity.Property(e => e.TokenHash).HasMaxLength(128);
            entity.Property(e => e.Status).HasConversion<string>().HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.ExpiresAtUtc).HasColumnType("timestamptz");
            entity.Property(e => e.AcceptedAtUtc).HasColumnType("timestamptz");
            entity.HasOne(e => e.User)
                .WithMany(u => u.Invitations)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureFgsOutboxMessage(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FgsOutboxMessage>(entity =>
        {
            entity.ToTable("FgsOutboxMessage");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.IdempotencyKey).IsUnique();
            entity.HasIndex(e => new { e.Status, e.CreatedOn });
            entity.Property(e => e.EventType).HasMaxLength(200);
            entity.Property(e => e.Payload).HasColumnType("jsonb");
            entity.Property(e => e.IdempotencyKey).HasMaxLength(200);
            entity.Property(e => e.CorrelationId).HasMaxLength(100);
            entity.Property(e => e.Status).HasConversion<string>().HasMaxLength(50);
            entity.Property(e => e.LastError).HasMaxLength(2000);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.ProcessedOn).HasColumnType("timestamptz");
        });
    }

    private static void ConfigureFgsTenant(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FgsTenant>(entity =>
        {
            entity.ToTable("FgsTenant");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnOrder(0);
            entity.HasIndex(e => e.TenantCode).IsUnique();
            entity.Property(e => e.TenantCode).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.LegalName).HasMaxLength(300);
            entity.Property(e => e.Email).HasMaxLength(300);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.Website).HasMaxLength(500);
            entity.Property(e => e.TimeZone).HasMaxLength(100);
            entity.Property(e => e.DefaultCurrency).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        });
    }

    private static void ConfigureFgsTenantCompany(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FgsTenantCompany>(entity =>
        {
            entity.ToTable("FgsTenantCompany");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnOrder(0);
            entity.Property(e => e.TenantId).HasColumnOrder(1);
            entity.Property(e => e.CompanyGuid).HasColumnOrder(2);
            entity.HasAlternateKey(e => new { e.TenantId, e.CompanyGuid });
            entity.HasAlternateKey(e => new { e.TenantId, e.CompanyNumber });
            entity.HasIndex(e => new { e.TenantId, e.CompanyNumber }).IsUnique();
            entity.HasIndex(e => new { e.TenantId, e.Code }).IsUnique();
            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.LegalName).HasMaxLength(300);
            entity.Property(e => e.Email).HasMaxLength(300);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.Website).HasMaxLength(500);
            entity.Property(e => e.CompanySize).HasMaxLength(20);
            entity.Property(e => e.TaxId).HasMaxLength(100);
            entity.Property(e => e.FullLogoUrl).HasMaxLength(1000);
            entity.Property(e => e.CompactLogoUrl).HasMaxLength(1000);
            entity.Property(e => e.IconLogoUrl).HasMaxLength(1000);
            entity.Property(e => e.FaviconUrl).HasMaxLength(1000);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        });
    }

    private static void ConfigureFgsTenantServiceSetup(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FgsTenantServiceSetup>(entity =>
        {
            entity.ToTable("FgsTenantServiceSetup");
            entity.HasKey(e => new { e.TenantId, e.CompanyId });
            entity.Property(e => e.TenantId).HasColumnOrder(0);
            entity.Property(e => e.CompanyId).HasColumnOrder(1);
            entity.Property(e => e.BillHoursFromDispatchOrArrive).HasMaxLength(20);
            entity.Property(e => e.InvoiceNumberPrefix).HasMaxLength(20);
            entity.Property(e => e.QuoteNumberPrefix).HasMaxLength(20);
            entity.Property(e => e.PONumberPrefix).HasMaxLength(20);
            entity.Property(e => e.WorkOrderNumberPrefix).HasMaxLength(20);
            entity.Property(e => e.InvoiceBatchNumberFormat).HasMaxLength(200);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        });
    }

    private static void ConfigureFgsLocation(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FgsLocation>(entity =>
        {
            entity.ToTable("FgsLocation");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnOrder(0);
            entity.Property(e => e.TenantId).HasColumnOrder(1);
            entity.Property(e => e.CompanyId).HasColumnOrder(2);
            entity.Property(e => e.AddressLine1).HasMaxLength(200);
            entity.Property(e => e.AddressLine2).HasMaxLength(200);
            entity.Property(e => e.AddressLine3).HasMaxLength(200);
            entity.Property(e => e.AddressLine4).HasMaxLength(200);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.State).HasMaxLength(100);
            entity.Property(e => e.County).HasMaxLength(100);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.PostalCode).HasMaxLength(20);
            entity.Property(e => e.FormattedAddress).HasMaxLength(1000);
            entity.Property(e => e.Latitude).HasPrecision(18, 10);
            entity.Property(e => e.Longitude).HasPrecision(18, 10);
            entity.Property(e => e.PlaceId).HasMaxLength(500);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        });
    }

    private static void ConfigureFgsCredentialProvider(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FgsCredentialProvider>(entity =>
        {
            entity.ToTable("FgsCredentialProvider");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnOrder(0);
            entity.Property(e => e.TenantId).HasColumnOrder(1);
            entity.Property(e => e.CompanyId).HasColumnOrder(2);
            entity.HasOne<FgsTenantCompany>()
                .WithMany()
                .HasForeignKey(e => new { e.TenantId, e.CompanyId })
                .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyNumber })
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Code }).IsUnique();
            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Environment).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        });
    }

    private static void ConfigureFgsCredentialProviderConfiguration(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FgsCredentialProviderConfiguration>(entity =>
        {
            entity.ToTable("FgsCredentialProviderConfiguration");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnOrder(0);
            entity.Property(e => e.TenantId).HasColumnOrder(1);
            entity.Property(e => e.CompanyId).HasColumnOrder(2);
            entity.Property(e => e.ConfigurationKey).HasMaxLength(200);
            entity.Property(e => e.Environment).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        });
    }

    private static void ConfigureFgsCredentialSecret(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FgsCredentialSecret>(entity =>
        {
            entity.ToTable("FgsCredentialSecret");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnOrder(0);
            entity.Property(e => e.TenantId).HasColumnOrder(1);
            entity.Property(e => e.CompanyId).HasColumnOrder(2);
            entity.Property(e => e.SecretName).HasMaxLength(200);
            entity.Property(e => e.EncryptionKeyId).HasMaxLength(500);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.LastRotatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.ExpiresOn).HasColumnType("timestamptz");
        });
    }

    private static void ConfigureFgsCredentialAudit(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FgsCredentialAudit>(entity =>
        {
            entity.ToTable("FgsCredentialAudit");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnOrder(0);
            entity.Property(e => e.TenantId).HasColumnOrder(1);
            entity.Property(e => e.CompanyId).HasColumnOrder(2);
            entity.Property(e => e.ActionType).HasMaxLength(100);
            entity.Property(e => e.Remarks).HasMaxLength(1000);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
        });
    }

    private static void ConfigureFgsSetupPriceSheetLaborRelationship(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FgsSetupPriceSheetLabor>(entity =>
        {
            entity.HasOne<GloSetupLaborRateType>()
                .WithMany()
                .HasForeignKey(e => e.FgsSetupLaborRateTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureFgsSetupGLBreakTechTradeRelationships(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FgsSetupDescription>(entity =>
        {
            entity.Property(e => e.ShortNote).HasMaxLength(30);
        });

        modelBuilder.Entity<FgsSetupGLBreakTechTrade>(entity =>
        {
            entity.HasIndex(e => new { e.FgsSetupGLBreakId, e.FgsSetupTechTradeId }).IsUnique();
            entity.HasOne(e => e.GLBreak)
                .WithMany(b => b.TechTrades)
                .HasForeignKey(e => e.FgsSetupGLBreakId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.TechTrade)
                .WithMany()
                .HasForeignKey(e => e.FgsSetupTechTradeId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

}
