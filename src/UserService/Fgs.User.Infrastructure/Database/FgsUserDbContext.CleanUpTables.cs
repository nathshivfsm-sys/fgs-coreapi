using Fgs.User.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Database;

public partial class FgsUserDbContext
{
    private static void ConfigureCleanUpTables(ModelBuilder modelBuilder)
    {
        ConfigureFgsResolutionCode(modelBuilder);
        ConfigureFgsSetupServiceAssetModelReference(modelBuilder);
        ConfigureCleanUpTableUniqueConstraints(modelBuilder);
        ConfigureCleanUpTableSupportingIndexes(modelBuilder);
        ConfigureCleanUpTableCheckConstraints(modelBuilder);
        ConfigureCleanUpTableForeignKeys(modelBuilder);
    }

    private static void ConfigureFgsResolutionCode(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FgsResolutionCode>(entity =>
        {
            entity.ToTable("FgsResolutionCode");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnOrder(0);
            entity.Property(e => e.TenantId).HasColumnOrder(1);
            entity.Property(e => e.CompanyId).HasColumnOrder(2);
            entity.HasOne<FgsTenantCompany>()
                .WithMany()
                .HasForeignKey(e => new { e.TenantId, e.CompanyId })
                .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyNumber })
                .HasConstraintName("FK_FgsResolutionCode_Company")
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.ResolutionType)
                .WithMany()
                .HasForeignKey(e => e.GloResolutionTypeId)
                .HasConstraintName("FK_FgsResolutionCode_GloResType")
                .OnDelete(DeleteBehavior.Restrict);
            entity.Property(e => e.ResolutionCode).HasMaxLength(50);
            entity.Property(e => e.ResolutionName).HasMaxLength(200);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
            entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.ResolutionCode })
                .HasName("UQ_FgsResolutionCode_Code");
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.GloResolutionTypeId })
                .HasDatabaseName("IX_FgsResolutionCode_GloResType");
            entity.ToTable(t => t.HasCheckConstraint(
                "CK_FgsResolutionCode_Code_Upper",
                "\"ResolutionCode\" = UPPER(\"ResolutionCode\")"));
        });
    }

    private static void ConfigureFgsSetupServiceAssetModelReference(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FgsSetupServiceAssetModelReference>(entity =>
        {
            entity.ToTable("FgsSetupServiceAssetModelReference");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnOrder(0);
            entity.Property(e => e.TenantId).HasColumnOrder(1);
            entity.Property(e => e.CompanyId).HasColumnOrder(2);
            entity.Property(e => e.UrlsJson).HasColumnType("jsonb");
            entity.HasOne<FgsTenantCompany>()
                .WithMany()
                .HasForeignKey(e => new { e.TenantId, e.CompanyId })
                .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyNumber })
                .HasConstraintName("FK_FgsSvcAssetModelRef_Company")
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.ServiceAssetType)
                .WithMany()
                .HasForeignKey(e => e.FgsSetupServiceAssetTypeId)
                .HasConstraintName("FK_FgsSvcAssetModelRef_AssetType")
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.ServiceAssetManufacturer)
                .WithMany()
                .HasForeignKey(e => e.FgsSetupServiceAssetManufacturerId)
                .HasConstraintName("FK_FgsSvcAssetModelRef_Mfr")
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(e => e.FgsSetupServiceAssetTypeId)
                .HasDatabaseName("IX_FgsSvcAssetModelRef_TypeId");
            entity.HasIndex(e => e.FgsSetupServiceAssetManufacturerId)
                .HasDatabaseName("IX_FgsSvcAssetModelRef_MfrId");
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.FgsSetupServiceAssetManufacturerId })
                .HasDatabaseName("IX_FgsSvcAssetModelRef_Mfr");
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.FgsSetupServiceAssetTypeId })
                .HasDatabaseName("IX_FgsSvcAssetModelRef_Type");
            entity.HasIndex(e => new
                {
                    e.TenantId,
                    e.CompanyId,
                    e.FgsSetupServiceAssetTypeId,
                    e.FgsSetupServiceAssetManufacturerId
                })
                .HasDatabaseName("IX_FgsSvcAssetModelRef_TypeMfr");
            entity.HasIndex(e => e.UrlsJson)
                .HasDatabaseName("IX_ServiceAsset_UrlsJson")
                .HasMethod("gin");
            entity.Property(e => e.CreatedOn).HasColumnType("timestamptz");
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamptz");
            entity.ToTable(t => t.HasCheckConstraint(
                "CK_FgsSvcAssetModelRef_UrlsJson",
                "\"UrlsJson\" IS NULL OR jsonb_typeof(\"UrlsJson\") = 'array'"));
        });
    }

    private static void ConfigureCleanUpTableUniqueConstraints(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GloMasterEntityType>(entity =>
            entity.HasAlternateKey(e => e.Code).HasName("UQ_GloMasterEntityType_Code"));

        modelBuilder.Entity<GloCommunicationToken>(entity =>
            entity.HasAlternateKey(e => e.TokenCode).HasName("UQ_GloCommunicationToken_TokenCode"));

        modelBuilder.Entity<GloPaymentMethodType>(entity =>
            entity.HasAlternateKey(e => e.Code).HasName("UQ_GloPaymentMethodType_Code"));

        modelBuilder.Entity<GloTimeCardOption>(entity =>
            entity.HasAlternateKey(e => e.Code).HasName("UQ_GloTimeCardOption_Code"));

        modelBuilder.Entity<GloResolutionType>(entity =>
            entity.HasAlternateKey(e => e.ResolutionTypeCode).HasName("UQ_GloResolutionType_Code"));

        modelBuilder.Entity<FgsSetupZone>(entity =>
            entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Code }).HasName("UQ_FgsSetupZone"));

        modelBuilder.Entity<FgsSetupTitleOfCourtesy>(entity =>
            entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Code }).HasName("UQ_FgsSetupTitleOfCourtesy"));

        modelBuilder.Entity<FgsSetupTimeSlot>(entity =>
            entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Code }).HasName("UQ_FgsSetupTimeSlot"));

        modelBuilder.Entity<FgsSetupTechTrade>(entity =>
            entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.TradeCode }).HasName("UQ_FgsSetupTechTrade"));

        modelBuilder.Entity<FgsSetupTechSkillLevel>(entity =>
            entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Code }).HasName("UQ_FgsSetupTechSkillLevel"));

        modelBuilder.Entity<FgsSetupTaxAuthority>(entity =>
            entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Code }).HasName("UQ_FgsSetupTaxAuthority"));

        modelBuilder.Entity<FgsSetupTax>(entity =>
            entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.TaxCode }).HasName("UQ_FgsSetupTax"));

        modelBuilder.Entity<FgsSetupServiceAssetType>(entity =>
            entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Code }).HasName("UQ_FgsSetupServiceAssetType"));

        modelBuilder.Entity<FgsSetupServiceAssetManufacturer>(entity =>
            entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Code }).HasName("UQ_FgsSetupServiceAssetManufacturer"));

        modelBuilder.Entity<FgsSetupPriceSheetOther>(entity =>
            entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.FgsSetupPriceSheetId, e.CategoryCode })
                .HasName("UQ_FgsSetupPricingMatrixOther"));

        modelBuilder.Entity<FgsSetupPriceSheet>(entity =>
            entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Code }).HasName("UQ_FgsSetupPricingMatrix"));

        modelBuilder.Entity<FgsSetupPostalCode>(entity =>
            entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.PostalCode }).HasName("UQ_FgsSetupPostalCode"));

        modelBuilder.Entity<FgsSetupPaymentTerm>(entity =>
            entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Name }).HasName("UQ_FgsSetupPaymentTerm"));

        modelBuilder.Entity<FgsSetupPaymentMethod>(entity =>
            entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.GloPaymentMethodTypeId })
                .HasName("UQ_FgsSetupPaymentMethod"));

        modelBuilder.Entity<FgsSetupGLBreak>(entity =>
            entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.Code }).HasName("UQ_FgsSetupGLBreak"));

        modelBuilder.Entity<FgsSetupCommunicationTemplate>(entity =>
            entity.HasAlternateKey(e => new { e.TenantId, e.CompanyId, e.TemplateType, e.Code })
                .HasName("UQ_FgsSetupCommunicationTemplate"));

        modelBuilder.Entity<FgsCredentialProviderConfiguration>(entity =>
            entity.HasIndex(e => new
                {
                    e.TenantId,
                    e.CompanyId,
                    e.CredentialProviderId,
                    e.ConfigurationKey,
                    e.Environment
                })
                .IsUnique()
                .HasDatabaseName("UQ_FgsCredentialProviderConfiguration"));

        modelBuilder.Entity<FgsCredentialSecret>(entity =>
            entity.HasIndex(e => new
                {
                    e.TenantId,
                    e.CompanyId,
                    e.CredentialProviderId,
                    e.SecretName,
                    e.VersionNo
                })
                .IsUnique()
                .HasDatabaseName("UQ_FgsCredentialSecret"));

        modelBuilder.Entity<FgsCredentialAudit>(entity =>
            entity.HasIndex(e => new
                {
                    e.TenantId,
                    e.CompanyId,
                    e.CredentialSecretId,
                    e.ActionType,
                    e.NewVersionNo
                })
                .IsUnique()
                .HasDatabaseName("UQ_FgsCredentialAudit"));
    }

    private static void ConfigureCleanUpTableSupportingIndexes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FgsLocation>(entity =>
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.MasterEntityTypeId, e.EntityNumber })
                .HasDatabaseName("IX_FgsLocation_Tenant_Company_Entity"));

        modelBuilder.Entity<FgsCredentialAudit>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.CompanyId })
                .HasDatabaseName("IX_FgsCredentialAudit_Tenant_Company");
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CredentialSecretId })
                .HasDatabaseName("IX_FgsCredentialAudit_Tenant_Company_Cred");
        });

        modelBuilder.Entity<FgsCredentialProviderConfiguration>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.CompanyId })
                .HasDatabaseName("IX_FgsCredProvCfg_Tenant_Company");
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CredentialProviderId })
                .HasDatabaseName("IX_FgsCredProvCfg_Tenant_Company_Prov");
        });

        modelBuilder.Entity<FgsCredentialSecret>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.CompanyId })
                .HasDatabaseName("IX_FgsCredentialSecret_Tenant_Company");
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.CredentialProviderId })
                .HasDatabaseName("IX_FgsCredentialSecret_Tenant_Company_Prov");
            entity.HasIndex(e => e.IsActive)
                .HasDatabaseName("IX_FgsCredentialSecret_IsActive");
        });

        modelBuilder.Entity<FgsSetupTitleOfCourtesy>(entity =>
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.SortOrder })
                .HasDatabaseName("IX_FgsSetupTitleOfCourtesy_SortOrder"));

        modelBuilder.Entity<FgsSetupTimeSlot>(entity =>
        {
            entity.HasIndex(e => e.FgsSetupZoneId)
                .HasDatabaseName("IX_FgsSetupTimeSlot_ZoneId");
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.FgsSetupZoneId })
                .HasDatabaseName("IX_FgsSetupTimeSlot_Zone");
        });

        modelBuilder.Entity<FgsSetupTechTrade>(entity =>
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.SortOrder })
                .HasDatabaseName("IX_FgsSetupTechTrade_SortOrder"));

        modelBuilder.Entity<FgsSetupTechSkillLevel>(entity =>
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.SortOrder })
                .HasDatabaseName("IX_FgsSetupTechSkillLevel_SortOrder"));

        modelBuilder.Entity<FgsSetupTaxAuthority>(entity =>
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.RegionCode })
                .HasDatabaseName("IX_FgsSetupTaxAuthority_RegionCode"));

        modelBuilder.Entity<FgsSetupTaxDetail>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.FgsSetupTaxId, e.EffectiveFromDate, e.EffectiveToDate })
                .HasDatabaseName("IX_FgsSetupTaxDetail_Tax");
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.FgsSetupTaxAuthorityId })
                .HasDatabaseName("IX_FgsSetupTaxDetail_TaxAuth");
            entity.HasIndex(e => new { e.EffectiveFromDate, e.EffectiveToDate })
                .HasDatabaseName("IX_FgsSetupTaxDetail_EffectiveDates");
            entity.HasIndex(e => e.FgsSetupTaxId)
                .HasDatabaseName("IX_FgsSetupTaxDetail_TaxId");
            entity.HasIndex(e => e.FgsSetupTaxAuthorityId)
                .HasDatabaseName("IX_FgsSetupTaxDetail_TaxAuthId");
        });

        modelBuilder.Entity<FgsSetupDescription>(entity =>
        {
            entity.HasIndex(e => new { e.TenantId, e.CompanyId, e.DescriptionTypeCode })
                .HasDatabaseName("IX_FgsSetupDescription_Tenant_Company_Type");
            entity.HasIndex(e => e.FgsSetupTechTradeId)
                .HasDatabaseName("IX_FgsSetupDescription_TechTrade");
        });

        modelBuilder.Entity<FgsSetupPostalCode>(entity =>
        {
            entity.HasIndex(e => e.FgsSetupZoneId)
                .HasDatabaseName("IX_FgsSetupPostalCode_ZoneId");
            entity.HasIndex(e => e.FgsSetupTaxId)
                .HasDatabaseName("IX_FgsSetupPostalCode_TaxId");
        });
    }

    private static void ConfigureCleanUpTableCheckConstraints(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GloTimeCardOption>(entity =>
            entity.ToTable(t => t.HasCheckConstraint(
                "CK_GloTimeCardOption_Code_Upper",
                "\"Code\" = UPPER(\"Code\")")));

        modelBuilder.Entity<FgsTenantServiceSetup>(entity =>
        {
            entity.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_FgsTenantServiceSetup_WorkLocationRadius",
                    "\"WorkLocationRadiusForAutoArrive\" IS NULL OR \"WorkLocationRadiusForAutoArrive\" >= 0");
                t.HasCheckConstraint(
                    "CK_FgsTenantServiceSetup_OTRange",
                    "\"OTStartTime\" IS NULL OR \"OTEndTime\" IS NULL OR \"OTEndTime\" > \"OTStartTime\"");
                t.HasCheckConstraint(
                    "CK_FgsTenantServiceSetup_DTRange",
                    "\"DTStartTime\" IS NULL OR \"DTEndTime\" IS NULL OR \"DTEndTime\" > \"DTStartTime\"");
            });
        });

        modelBuilder.Entity<FgsSetupZone>(entity =>
            entity.ToTable(t => t.HasCheckConstraint(
                "CK_FgsSetupZone_Code_Upper",
                "\"Code\" = UPPER(\"Code\")")));

        modelBuilder.Entity<FgsSetupTitleOfCourtesy>(entity =>
        {
            entity.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_FgsSetupTitleOfCourtesy_Code_Upper",
                    "\"Code\" = UPPER(\"Code\")");
                t.HasCheckConstraint(
                    "CK_FgsSetupTitleOfCourtesy_SortOrder",
                    "\"SortOrder\" >= 0");
            });
        });

        modelBuilder.Entity<FgsSetupTimeSlot>(entity =>
        {
            entity.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_FgsSetupTimeSlot_Code_Upper",
                    "\"Code\" = UPPER(\"Code\")");
                t.HasCheckConstraint(
                    "CK_FgsSetupTimeSlot_TimeRange",
                    "\"EndTime\" > \"BeginTime\"");
            });
        });

        modelBuilder.Entity<FgsSetupTechTrade>(entity =>
        {
            entity.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_FgsSetupTechTrade_TradeCode_Upper",
                    "\"TradeCode\" = UPPER(\"TradeCode\")");
                t.HasCheckConstraint(
                    "CK_FgsSetupTechTrade_SortOrder",
                    "\"SortOrder\" >= 0");
            });
        });

        modelBuilder.Entity<FgsSetupTechSkillLevel>(entity =>
        {
            entity.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_FgsSetupTechSkillLevel_Code_Upper",
                    "\"Code\" = UPPER(\"Code\")");
                t.HasCheckConstraint(
                    "CK_FgsSetupTechSkillLevel_SortOrder",
                    "\"SortOrder\" >= 0");
            });
        });

        modelBuilder.Entity<FgsSetupTax>(entity =>
            entity.ToTable(t => t.HasCheckConstraint(
                "CK_FgsSetupTax_TaxCode_Upper",
                "\"TaxCode\" = UPPER(\"TaxCode\")")));

        modelBuilder.Entity<FgsSetupTaxAuthority>(entity =>
        {
            entity.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_FgsSetupTaxAuthority_Code_Upper",
                    "\"Code\" = UPPER(\"Code\")");
                t.HasCheckConstraint(
                    "CK_FgsSetupTaxAuthority_RegionCode_Upper",
                    "\"RegionCode\" IS NULL OR \"RegionCode\" = UPPER(\"RegionCode\")");
            });
        });

        modelBuilder.Entity<FgsSetupTaxDetail>(entity =>
        {
            entity.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_FgsSetupTaxDetail_TaxPercent",
                    "\"TaxPercent\" >= 0 AND \"TaxPercent\" <= 100");
                t.HasCheckConstraint(
                    "CK_FgsSetupTaxDetail_EffectiveDates",
                    "\"EffectiveToDate\" IS NULL OR \"EffectiveToDate\" >= \"EffectiveFromDate\"");
            });
        });

        modelBuilder.Entity<FgsSetupServiceAssetType>(entity =>
            entity.ToTable(t => t.HasCheckConstraint(
                "CK_FgsSetupServiceAssetType_Code_Upper",
                "\"Code\" = UPPER(\"Code\")")));

        modelBuilder.Entity<FgsSetupServiceAssetManufacturer>(entity =>
            entity.ToTable(t => t.HasCheckConstraint(
                "CK_FgsSetupServiceAssetManufacturer_Code_Upper",
                "\"Code\" = UPPER(\"Code\")")));

        modelBuilder.Entity<FgsSetupPriceSheetOther>(entity =>
        {
            entity.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_FgsSetupPricingMatrixOther_MarkupPercent",
                    "\"MarkupPercent\" IS NULL OR (\"MarkupPercent\" >= 0 AND \"MarkupPercent\" <= 100)");
                t.HasCheckConstraint(
                    "CK_FgsSetupPricingMatrixOther_DiscountPercent",
                    "\"DiscountPercent\" IS NULL OR (\"DiscountPercent\" >= 0 AND \"DiscountPercent\" <= 100)");
            });
        });
    }

    private static void ConfigureCleanUpTableForeignKeys(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FgsLocation>(entity =>
        {
            entity.HasOne<FgsTenantCompany>()
                .WithMany()
                .HasForeignKey(e => new { e.TenantId, e.CompanyId })
                .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyNumber })
                .HasConstraintName("FK_FgsLocation_Company")
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<GloMasterEntityType>()
                .WithMany()
                .HasForeignKey(e => e.MasterEntityTypeId)
                .HasConstraintName("FK_FgsLocation_MasterEntityType")
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<FgsTenantServiceSetup>(entity =>
        {
            entity.HasOne<FgsTenantCompany>()
                .WithMany()
                .HasForeignKey(e => new { e.TenantId, e.CompanyId })
                .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyNumber })
                .HasConstraintName("FK_FgsTenantServiceSetup_Company")
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<GloTimeCardOption>()
                .WithMany()
                .HasForeignKey(e => e.GloTimeCardOptionId)
                .HasConstraintName("FK_FgsTenantServiceSetup_TimeCardOption")
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<FgsCredentialAudit>(entity =>
        {
            entity.HasOne<FgsTenantCompany>()
                .WithMany()
                .HasForeignKey(e => new { e.TenantId, e.CompanyId })
                .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyNumber })
                .HasConstraintName("FK_FgsCredentialAudit_Company")
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<FgsCredentialSecret>()
                .WithMany()
                .HasForeignKey(e => e.CredentialSecretId)
                .HasConstraintName("FK_FgsCredentialAudit_CredentialSecret")
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<FgsCredentialProviderConfiguration>(entity =>
        {
            entity.HasOne<FgsTenantCompany>()
                .WithMany()
                .HasForeignKey(e => new { e.TenantId, e.CompanyId })
                .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyNumber })
                .HasConstraintName("FK_FgsCredProvCfg_Company")
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<FgsCredentialProvider>()
                .WithMany()
                .HasForeignKey(e => e.CredentialProviderId)
                .HasConstraintName("FK_FgsCredProvCfg_Provider")
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<FgsCredentialSecret>(entity =>
        {
            entity.HasOne<FgsTenantCompany>()
                .WithMany()
                .HasForeignKey(e => new { e.TenantId, e.CompanyId })
                .HasPrincipalKey(tc => new { tc.TenantId, tc.CompanyNumber })
                .HasConstraintName("FK_FgsCredentialSecret_Company")
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<FgsCredentialProvider>()
                .WithMany()
                .HasForeignKey(e => e.CredentialProviderId)
                .HasConstraintName("FK_FgsCredentialSecret_Provider")
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<FgsSetupDescription>(entity =>
            entity.HasOne<FgsSetupTechTrade>()
                .WithMany()
                .HasForeignKey(e => e.FgsSetupTechTradeId)
                .HasConstraintName("FK_FgsSetupDescription_TechTrade")
                .OnDelete(DeleteBehavior.Restrict));

        modelBuilder.Entity<FgsSetupPostalCode>(entity =>
        {
            entity.HasOne<FgsSetupZone>()
                .WithMany()
                .HasForeignKey(e => e.FgsSetupZoneId)
                .HasConstraintName("FK_FgsSetupPostalCode_Zone")
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<FgsSetupTax>()
                .WithMany()
                .HasForeignKey(e => e.FgsSetupTaxId)
                .HasConstraintName("FK_FgsSetupPostalCode_Tax")
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<FgsSetupTaxDetail>(entity =>
        {
            entity.HasOne<FgsSetupTax>()
                .WithMany()
                .HasForeignKey(e => e.FgsSetupTaxId)
                .HasConstraintName("FK_FgsSetupTaxDetail_Tax")
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<FgsSetupTaxAuthority>()
                .WithMany()
                .HasForeignKey(e => e.FgsSetupTaxAuthorityId)
                .HasConstraintName("FK_FgsSetupTaxDetail_TaxAuth")
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<FgsSetupTimeSlot>(entity =>
            entity.HasOne<FgsSetupZone>()
                .WithMany()
                .HasForeignKey(e => e.FgsSetupZoneId)
                .HasConstraintName("FK_FgsSetupTimeSlot_Zone")
                .OnDelete(DeleteBehavior.Restrict));

        modelBuilder.Entity<FgsSetupPaymentMethod>(entity =>
            entity.HasOne<GloPaymentMethodType>()
                .WithMany()
                .HasForeignKey(e => e.GloPaymentMethodTypeId)
                .HasConstraintName("FK_FgsSetupPaymentMethod_GloPayType")
                .OnDelete(DeleteBehavior.Restrict));
    }
}
