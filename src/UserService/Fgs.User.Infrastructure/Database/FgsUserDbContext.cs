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

    public DbSet<FgsTenantCompanyConfiguration> FgsTenantCompanyConfigurations => Set<FgsTenantCompanyConfiguration>();

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

    public DbSet<FgsSetupCommunicationToken> FgsSetupCommunicationTokens => Set<FgsSetupCommunicationToken>();

    public DbSet<FgsSetupPaymentMethod> FgsSetupPaymentMethods => Set<FgsSetupPaymentMethod>();

    public DbSet<FgsSetupPaymentTerm> FgsSetupPaymentTerms => Set<FgsSetupPaymentTerm>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(FgsSchema);

        ConfigureFgsTenant(modelBuilder);
        ConfigureFgsTenantCompany(modelBuilder);
        ConfigureFgsTenantCompanyConfiguration(modelBuilder);
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
        MapSetupEntity<FgsSetupCommunicationToken>(modelBuilder, "FgsSetupCommunicationToken");
        MapSetupEntity<FgsSetupPaymentMethod>(modelBuilder, "FgsSetupPaymentMethod");
        MapSetupEntity<FgsSetupPaymentTerm>(modelBuilder, "FgsSetupPaymentTerm");
    }

    private static void MapSetupEntity<TEntity>(ModelBuilder modelBuilder, string tableName)
        where TEntity : FgsTenantCompanySetupEntityBase
    {
        modelBuilder.Entity<TEntity>(entity =>
        {
            entity.ToTable(tableName);
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.TenantId, e.CompanyId });
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
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

    private static void ConfigureFgsTenantCompanyConfiguration(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FgsTenantCompanyConfiguration>(entity =>
        {
            entity.ToTable("FgsTenantCompanyConfiguration");
            entity.HasKey(e => new { e.TenantId, e.CompanyId });
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
            entity.HasIndex(e => e.EntityTypeId);
            entity.Property(e => e.AddressLine1).HasMaxLength(200);
            entity.Property(e => e.AddressLine2).HasMaxLength(200);
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
            entity.HasIndex(e => new { e.TenantId, e.Code }).IsUnique();
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
            entity.Property(e => e.VaultProvider).HasMaxLength(100);
            entity.Property(e => e.SecretName).HasMaxLength(500);
            entity.Property(e => e.SecretArn).HasMaxLength(1000);
            entity.Property(e => e.RegionName).HasMaxLength(100);
            entity.Property(e => e.KmsKeyArn).HasMaxLength(1000);
            entity.Property(e => e.Remarks).HasMaxLength(1000);
            entity.Property(e => e.RotatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.LastValidatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
        });
    }

    private static void ConfigureFgsCredentialAudit(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FgsCredentialAudit>(entity =>
        {
            entity.ToTable("FgsCredentialAudit");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ActionType).HasMaxLength(100);
            entity.Property(e => e.Remarks).HasMaxLength(1000);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
        });
    }
}
