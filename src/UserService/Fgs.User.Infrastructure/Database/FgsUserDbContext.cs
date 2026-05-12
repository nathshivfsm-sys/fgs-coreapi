using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Database;

/// <summary>
/// Code-first EF Core context for FGS user / platform entities (PostgreSQL schema <c>dbo</c>).
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

    public DbSet<FgsSetupServiceAssetModelSerialDescription> FgsSetupServiceAssetModelSerialDescriptions =>
        Set<FgsSetupServiceAssetModelSerialDescription>();

    public DbSet<FgsSetupServiceAssetMedia> FgsSetupServiceAssetMedia => Set<FgsSetupServiceAssetMedia>();

    public DbSet<FgsSetupPriceSheet> FgsSetupPriceSheets => Set<FgsSetupPriceSheet>();

    public DbSet<FgsSetupPriceSheetLabor> FgsSetupPriceSheetLabors => Set<FgsSetupPriceSheetLabor>();

    public DbSet<FgsSetupPriceSheetLaborTier> FgsSetupPriceSheetLaborTiers => Set<FgsSetupPriceSheetLaborTier>();

    public DbSet<FgsSetupPriceSheetMaterial> FgsSetupPriceSheetMaterials => Set<FgsSetupPriceSheetMaterial>();

    public DbSet<FgsSetupPriceSheetMaterialRange> FgsSetupPriceSheetMaterialRanges =>
        Set<FgsSetupPriceSheetMaterialRange>();

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(FgsSchema);

        ConfigureGloMasterEntityType(modelBuilder);
        ConfigureGloCommunicationToken(modelBuilder);
        ConfigureGloTimeCardOption(modelBuilder);
        ConfigureGloPaymentMethodType(modelBuilder);

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
        MapSetupEntity<FgsSetupServiceAssetModelSerialDescription>(
            modelBuilder,
            "FgsSetupServiceAssetModelSerialDescription");
        MapSetupEntity<FgsSetupServiceAssetMedia>(modelBuilder, "FgsSetupServiceAssetMedia");
        MapSetupEntity<FgsSetupPriceSheet>(modelBuilder, "FgsSetupPriceSheet");
        MapSetupEntity<FgsSetupPriceSheetLabor>(modelBuilder, "FgsSetupPriceSheetLabor");
        MapSetupEntity<FgsSetupPriceSheetLaborTier>(modelBuilder, "FgsSetupPriceSheetLaborTier");
        MapSetupEntity<FgsSetupPriceSheetMaterial>(modelBuilder, "FgsSetupPriceSheetMaterial");
        MapSetupEntity<FgsSetupPriceSheetMaterialRange>(modelBuilder, "FgsSetupPriceSheetMaterialRange");
        MapSetupEntity<FgsSetupPriceSheetOther>(modelBuilder, "FgsSetupPriceSheetOther");
        MapSetupEntity<FgsSetupGLBreak>(modelBuilder, "FgsSetupGLBreak");
        MapSetupEntity<FgsSetupCommunicationTemplate>(modelBuilder, "FgsSetupCommunicationTemplate");
        MapSetupEntity<FgsSetupPaymentMethod>(modelBuilder, "FgsSetupPaymentMethod");
        MapSetupEntity<FgsSetupPaymentTerm>(modelBuilder, "FgsSetupPaymentTerm");

        ConfigureFgsSetupPaymentMethodRelationship(modelBuilder);
        ConfigureTenantCompanyScopedIndexes(modelBuilder);
    }

    private static void MapSetupEntity<TEntity>(ModelBuilder modelBuilder, string tableName)
        where TEntity : FgsTenantCompanySetupEntityBase
    {
        modelBuilder.Entity<TEntity>(entity =>
        {
            entity.ToTable(tableName);
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.TenantId, e.CompanyId });
            entity.HasOne<FgsTenantCompany>()
                .WithMany()
                .HasForeignKey(e => new { e.TenantId, e.CompanyId })
                .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyGuid })
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
            entity.HasIndex(e => e.Code).IsUnique();
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
            entity.HasIndex(e => e.TokenCode).IsUnique();
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
            entity.HasIndex(e => e.Code).IsUnique();
        });
    }

    private static void ConfigureGloPaymentMethodType(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GloPaymentMethodType>(entity =>
        {
            entity.ToTable("GloPaymentMethodType");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
        });
    }

    private static void ConfigureFgsTenant(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FgsTenant>(entity =>
        {
            entity.ToTable("FgsTenant");
            entity.HasKey(e => e.Id);
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
            entity.HasAlternateKey(e => new { e.TenantId, e.CompanyGuid });
            entity.HasIndex(e => new { e.TenantId, e.CompanyNumber }).IsUnique();
            entity.HasIndex(e => new { e.TenantId, e.Code }).IsUnique();
            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.LegalName).HasMaxLength(300);
            entity.Property(e => e.Email).HasMaxLength(300);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.Website).HasMaxLength(500);
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
            entity.HasOne<FgsTenantCompany>()
                .WithMany()
                .HasForeignKey(e => new { e.TenantId, e.CompanyId })
                .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyGuid })
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<GloTimeCardOption>()
                .WithMany()
                .HasForeignKey(e => e.GloTimeCardOptionId)
                .OnDelete(DeleteBehavior.Restrict);
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
            entity.HasOne<FgsTenantCompany>()
                .WithMany()
                .HasForeignKey(e => new { e.TenantId, e.CompanyId })
                .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyGuid })
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<GloMasterEntityType>()
                .WithMany()
                .HasForeignKey(e => e.MasterEntityTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.MasterEntityTypeId, e.EntityNumber });
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
            entity.HasOne<FgsTenantCompany>()
                .WithMany()
                .HasForeignKey(e => new { e.TenantId, e.CompanyId })
                .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyGuid })
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
            entity.HasOne<FgsTenantCompany>()
                .WithMany()
                .HasForeignKey(e => new { e.TenantId, e.CompanyId })
                .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyGuid })
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<FgsCredentialProvider>()
                .WithMany()
                .HasForeignKey(e => e.CredentialProviderId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(e => new { e.TenantId, e.CompanyId });
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CredentialProviderId });
            entity.HasIndex(
                    e => new
                    {
                        e.TenantId,
                        e.CompanyId,
                        e.CredentialProviderId,
                        e.ConfigurationKey,
                        e.Environment
                    })
                .IsUnique();
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
            entity.HasOne<FgsTenantCompany>()
                .WithMany()
                .HasForeignKey(e => new { e.TenantId, e.CompanyId })
                .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyGuid })
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<FgsCredentialProvider>()
                .WithMany()
                .HasForeignKey(e => e.CredentialProviderId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(e => new { e.TenantId, e.CompanyId });
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CredentialProviderId });
            entity.HasIndex(e => e.IsActive);
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CredentialProviderId, e.SecretName, e.VersionNo })
                .IsUnique();
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
            entity.HasOne<FgsTenantCompany>()
                .WithMany()
                .HasForeignKey(e => new { e.TenantId, e.CompanyId })
                .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyGuid })
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<FgsCredentialSecret>()
                .WithMany()
                .HasForeignKey(e => e.CredentialSecretId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(e => new { e.TenantId, e.CompanyId });
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CredentialSecretId });
            entity.HasIndex(
                    e => new
                    {
                        e.TenantId,
                        e.CompanyId,
                        e.CredentialSecretId,
                        e.ActionType,
                        e.NewVersionNo
                    })
                .IsUnique();
            entity.Property(e => e.ActionType).HasMaxLength(100);
            entity.Property(e => e.Remarks).HasMaxLength(1000);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        });
    }

    private static void ConfigureFgsSetupPaymentMethodRelationship(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FgsSetupPaymentMethod>(entity =>
        {
            entity.HasOne<GloPaymentMethodType>()
                .WithMany()
                .HasForeignKey(e => e.GloPaymentMethodTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.GloPaymentMethodTypeId }).IsUnique();
        });
    }

    private static void ConfigureTenantCompanyScopedIndexes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FgsSetupZone>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Code });
        });

        modelBuilder.Entity<FgsSetupTechTrade>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.TradeCode });
        });

        modelBuilder.Entity<FgsSetupTimeSlot>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Code });
        });

        modelBuilder.Entity<FgsSetupTechSkillLevel>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Code });
        });

        modelBuilder.Entity<FgsSetupTitleOfCourtesy>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Code });
        });

        modelBuilder.Entity<FgsSetupServiceAssetType>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Code });
        });

        modelBuilder.Entity<FgsSetupServiceAssetManufacturer>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Code });
        });

        modelBuilder.Entity<FgsSetupTax>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.TaxCode });
        });

        modelBuilder.Entity<FgsSetupTaxAuthority>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Code });
        });

        modelBuilder.Entity<FgsSetupDescription>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.DescriptionTypeCode });
        });

        modelBuilder.Entity<FgsSetupPostalCode>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.PostalCode });
        });

        modelBuilder.Entity<FgsSetupPriceSheet>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Code });
        });

        modelBuilder.Entity<FgsSetupPriceSheetMaterial>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Code });
        });

        modelBuilder.Entity<FgsSetupPriceSheetOther>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CategoryCode });
        });

        modelBuilder.Entity<FgsSetupGLBreak>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Code }).IsUnique();
        });

        modelBuilder.Entity<FgsSetupCommunicationTemplate>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.TemplateType, e.Code }).IsUnique();
        });

        modelBuilder.Entity<FgsSetupPaymentTerm>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.Name }).IsUnique();
        });
    }
}
