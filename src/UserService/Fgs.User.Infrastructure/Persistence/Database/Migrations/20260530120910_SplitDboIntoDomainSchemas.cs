using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.User.Infrastructure.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class SplitDboIntoDomainSchemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "billing");

            migrationBuilder.EnsureSchema(
                name: "dispatch");

            migrationBuilder.EnsureSchema(
                name: "audit");

            migrationBuilder.EnsureSchema(
                name: "integration");

            migrationBuilder.EnsureSchema(
                name: "shared");

            migrationBuilder.EnsureSchema(
                name: "inventory");

            migrationBuilder.EnsureSchema(
                name: "identity");

            migrationBuilder.EnsureSchema(
                name: "crm");

            migrationBuilder.EnsureSchema(
                name: "notification");

            migrationBuilder.EnsureSchema(
                name: "tenant");

            migrationBuilder.EnsureSchema(
                name: "glo");

            migrationBuilder.Sql(
                """
                ALTER TABLE IF EXISTS dbo."__EFMigrationsHistory" SET SCHEMA shared;
                """);

            migrationBuilder.RenameTable(
                name: "GloZone",
                schema: "dbo",
                newName: "GloZone",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloUnitOfMeasure",
                schema: "dbo",
                newName: "GloUnitOfMeasure",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloTrade",
                schema: "dbo",
                newName: "GloTrade",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloTitleOfCourtesy",
                schema: "dbo",
                newName: "GloTitleOfCourtesy",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloTimeCardOption",
                schema: "dbo",
                newName: "GloTimeCardOption",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "GloTag",
                schema: "dbo",
                newName: "GloTag",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloStateProvince",
                schema: "dbo",
                newName: "GloStateProvince",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloSkill",
                schema: "dbo",
                newName: "GloSkill",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloSetupTenantStatus",
                schema: "dbo",
                newName: "GloSetupTenantStatus",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloSetupPaymentTerm",
                schema: "dbo",
                newName: "GloSetupPaymentTerm",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloSetupLaborRateType",
                schema: "dbo",
                newName: "GloSetupLaborRateType",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloSetupDescriptionType",
                schema: "dbo",
                newName: "GloSetupDescriptionType",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloSeedTableMapping",
                schema: "dbo",
                newName: "GloSeedTableMapping",
                newSchema: "tenant");

            migrationBuilder.RenameTable(
                name: "GloSeedTableColumnMapping",
                schema: "dbo",
                newName: "GloSeedTableColumnMapping",
                newSchema: "tenant");

            migrationBuilder.RenameTable(
                name: "GloRole",
                schema: "dbo",
                newName: "GloRole",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "GloResolutionType",
                schema: "dbo",
                newName: "GloResolutionType",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloPaymentMethodType",
                schema: "dbo",
                newName: "GloPaymentMethodType",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloOutboxMessage",
                schema: "dbo",
                newName: "GloOutboxMessage",
                newSchema: "shared");

            migrationBuilder.RenameTable(
                name: "GloMasterEntityType",
                schema: "dbo",
                newName: "GloMasterEntityType",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloLocationType",
                schema: "dbo",
                newName: "GloLocationType",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloLeadSource",
                schema: "dbo",
                newName: "GloLeadSource",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloLanguage",
                schema: "dbo",
                newName: "GloLanguage",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloJobTypeSubCategory",
                schema: "dbo",
                newName: "GloJobTypeSubCategory",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloJobTypeCategory",
                schema: "dbo",
                newName: "GloJobTypeCategory",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloInventorySubCategory",
                schema: "dbo",
                newName: "GloInventorySubCategory",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloInventoryItemType",
                schema: "dbo",
                newName: "GloInventoryItemType",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloInventoryCategory",
                schema: "dbo",
                newName: "GloInventoryCategory",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloCredentialProviderType",
                schema: "dbo",
                newName: "GloCredentialProviderType",
                newSchema: "integration");

            migrationBuilder.RenameTable(
                name: "GloCredentialCategory",
                schema: "dbo",
                newName: "GloCredentialCategory",
                newSchema: "integration");

            migrationBuilder.RenameTable(
                name: "GloCountry",
                schema: "dbo",
                newName: "GloCountry",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloCommunicationToken",
                schema: "dbo",
                newName: "GloCommunicationToken",
                newSchema: "notification");

            migrationBuilder.RenameTable(
                name: "GloBusinessType",
                schema: "dbo",
                newName: "GloBusinessType",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloBillingCategory",
                schema: "dbo",
                newName: "GloBillingCategory",
                newSchema: "glo");

            migrationBuilder.RenameTable(
                name: "GloAccountingIntegrationType",
                schema: "dbo",
                newName: "GloAccountingIntegrationType",
                newSchema: "integration");

            migrationBuilder.RenameTable(
                name: "FgsVendorInventoryItem",
                schema: "dbo",
                newName: "FgsVendorInventoryItem",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "FgsVendor",
                schema: "dbo",
                newName: "FgsVendor",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "FgsUserRole",
                schema: "dbo",
                newName: "FgsUserRole",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "FgsUser",
                schema: "dbo",
                newName: "FgsUser",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "FgsTenantServiceSetup",
                schema: "dbo",
                newName: "FgsTenantServiceSetup",
                newSchema: "tenant");

            migrationBuilder.RenameTable(
                name: "FgsTenantCompany",
                schema: "dbo",
                newName: "FgsTenantCompany",
                newSchema: "tenant");

            migrationBuilder.RenameTable(
                name: "FgsTenant",
                schema: "dbo",
                newName: "FgsTenant",
                newSchema: "tenant");

            migrationBuilder.RenameTable(
                name: "FgsTagEntityType",
                schema: "dbo",
                newName: "FgsTagEntityType",
                newSchema: "shared");

            migrationBuilder.RenameTable(
                name: "FgsTag",
                schema: "dbo",
                newName: "FgsTag",
                newSchema: "shared");

            migrationBuilder.RenameTable(
                name: "FgsSetupZone",
                schema: "dbo",
                newName: "FgsSetupZone",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsSetupTitleOfCourtesy",
                schema: "dbo",
                newName: "FgsSetupTitleOfCourtesy",
                newSchema: "crm");

            migrationBuilder.RenameTable(
                name: "FgsSetupTimeSlot",
                schema: "dbo",
                newName: "FgsSetupTimeSlot",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsSetupTechTrade",
                schema: "dbo",
                newName: "FgsSetupTechTrade",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsSetupTechSkillLevel",
                schema: "dbo",
                newName: "FgsSetupTechSkillLevel",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsSetupTaxDetail",
                schema: "dbo",
                newName: "FgsSetupTaxDetail",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupTaxAuthority",
                schema: "dbo",
                newName: "FgsSetupTaxAuthority",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupTax",
                schema: "dbo",
                newName: "FgsSetupTax",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupServiceAssetType",
                schema: "dbo",
                newName: "FgsSetupServiceAssetType",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsSetupServiceAssetModelReference",
                schema: "dbo",
                newName: "FgsSetupServiceAssetModelReference",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsSetupServiceAssetManufacturer",
                schema: "dbo",
                newName: "FgsSetupServiceAssetManufacturer",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsSetupPricingMatrixOther",
                schema: "dbo",
                newName: "FgsSetupPricingMatrixOther",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupPricingMatrixMaterialTier",
                schema: "dbo",
                newName: "FgsSetupPricingMatrixMaterialTier",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupPricingMatrixLaborTier",
                schema: "dbo",
                newName: "FgsSetupPricingMatrixLaborTier",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupPricingMatrixLabor",
                schema: "dbo",
                newName: "FgsSetupPricingMatrixLabor",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupPricingMatrix",
                schema: "dbo",
                newName: "FgsSetupPricingMatrix",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupPostalCode",
                schema: "dbo",
                newName: "FgsSetupPostalCode",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsSetupPaymentTerm",
                schema: "dbo",
                newName: "FgsSetupPaymentTerm",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupPaymentMethod",
                schema: "dbo",
                newName: "FgsSetupPaymentMethod",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupLaborRateType",
                schema: "dbo",
                newName: "FgsSetupLaborRateType",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupGLBreakTrade",
                schema: "dbo",
                newName: "FgsSetupGLBreakTrade",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupGLBreak",
                schema: "dbo",
                newName: "FgsSetupGLBreak",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupDescription",
                schema: "dbo",
                newName: "FgsSetupDescription",
                newSchema: "notification");

            migrationBuilder.RenameTable(
                name: "FgsSetupCommunicationTemplate",
                schema: "dbo",
                newName: "FgsSetupCommunicationTemplate",
                newSchema: "notification");

            migrationBuilder.RenameTable(
                name: "FgsRole",
                schema: "dbo",
                newName: "FgsRole",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "FgsResolutionCode",
                schema: "dbo",
                newName: "FgsResolutionCode",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsLocation",
                schema: "dbo",
                newName: "FgsLocation",
                newSchema: "shared");

            migrationBuilder.RenameTable(
                name: "FgsLeadSource",
                schema: "dbo",
                newName: "FgsLeadSource",
                newSchema: "crm");

            migrationBuilder.RenameTable(
                name: "FgsJobTypeSubCategory",
                schema: "dbo",
                newName: "FgsJobTypeSubCategory",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsJobTypeCategory",
                schema: "dbo",
                newName: "FgsJobTypeCategory",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsJobType",
                schema: "dbo",
                newName: "FgsJobType",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsInvitation",
                schema: "dbo",
                newName: "FgsInvitation",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "FgsInventorySubCategory",
                schema: "dbo",
                newName: "FgsInventorySubCategory",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "FgsInventoryStock",
                schema: "dbo",
                newName: "FgsInventoryStock",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "FgsInventoryItemType",
                schema: "dbo",
                newName: "FgsInventoryItemType",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "FgsInventoryItemDependency",
                schema: "dbo",
                newName: "FgsInventoryItemDependency",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "FgsInventoryItemAlternate",
                schema: "dbo",
                newName: "FgsInventoryItemAlternate",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "FgsInventoryItem",
                schema: "dbo",
                newName: "FgsInventoryItem",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "FgsInventoryCategory",
                schema: "dbo",
                newName: "FgsInventoryCategory",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "FgsFile",
                schema: "dbo",
                newName: "FgsFile",
                newSchema: "shared");

            migrationBuilder.RenameTable(
                name: "FgsEntityTag",
                schema: "dbo",
                newName: "FgsEntityTag",
                newSchema: "shared");

            migrationBuilder.RenameTable(
                name: "FgsCredentialSecret",
                schema: "dbo",
                newName: "FgsCredentialSecret",
                newSchema: "integration");

            migrationBuilder.RenameTable(
                name: "FgsCredentialProviderConfiguration",
                schema: "dbo",
                newName: "FgsCredentialProviderConfiguration",
                newSchema: "integration");

            migrationBuilder.RenameTable(
                name: "FgsCredentialProvider",
                schema: "dbo",
                newName: "FgsCredentialProvider",
                newSchema: "integration");

            migrationBuilder.RenameTable(
                name: "FgsCredentialAudit",
                schema: "dbo",
                newName: "FgsCredentialAudit",
                newSchema: "audit");

            migrationBuilder.RenameTable(
                name: "FgsBusinessType",
                schema: "dbo",
                newName: "FgsBusinessType",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsBillingCategory",
                schema: "dbo",
                newName: "FgsBillingCategory",
                newSchema: "billing");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "GloZone",
                schema: "glo",
                newName: "GloZone",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloUnitOfMeasure",
                schema: "glo",
                newName: "GloUnitOfMeasure",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloTrade",
                schema: "glo",
                newName: "GloTrade",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloTitleOfCourtesy",
                schema: "glo",
                newName: "GloTitleOfCourtesy",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloTimeCardOption",
                schema: "dispatch",
                newName: "GloTimeCardOption",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloTag",
                schema: "glo",
                newName: "GloTag",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloStateProvince",
                schema: "glo",
                newName: "GloStateProvince",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloSkill",
                schema: "glo",
                newName: "GloSkill",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloSetupTenantStatus",
                schema: "glo",
                newName: "GloSetupTenantStatus",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloSetupPaymentTerm",
                schema: "glo",
                newName: "GloSetupPaymentTerm",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloSetupLaborRateType",
                schema: "glo",
                newName: "GloSetupLaborRateType",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloSetupDescriptionType",
                schema: "glo",
                newName: "GloSetupDescriptionType",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloSeedTableMapping",
                schema: "tenant",
                newName: "GloSeedTableMapping",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloSeedTableColumnMapping",
                schema: "tenant",
                newName: "GloSeedTableColumnMapping",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloRole",
                schema: "identity",
                newName: "GloRole",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloResolutionType",
                schema: "glo",
                newName: "GloResolutionType",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloPaymentMethodType",
                schema: "glo",
                newName: "GloPaymentMethodType",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloOutboxMessage",
                schema: "shared",
                newName: "GloOutboxMessage",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloMasterEntityType",
                schema: "glo",
                newName: "GloMasterEntityType",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloLocationType",
                schema: "glo",
                newName: "GloLocationType",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloLeadSource",
                schema: "glo",
                newName: "GloLeadSource",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloLanguage",
                schema: "glo",
                newName: "GloLanguage",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloJobTypeSubCategory",
                schema: "glo",
                newName: "GloJobTypeSubCategory",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloJobTypeCategory",
                schema: "glo",
                newName: "GloJobTypeCategory",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloInventorySubCategory",
                schema: "glo",
                newName: "GloInventorySubCategory",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloInventoryItemType",
                schema: "glo",
                newName: "GloInventoryItemType",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloInventoryCategory",
                schema: "glo",
                newName: "GloInventoryCategory",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloCredentialProviderType",
                schema: "integration",
                newName: "GloCredentialProviderType",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloCredentialCategory",
                schema: "integration",
                newName: "GloCredentialCategory",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloCountry",
                schema: "glo",
                newName: "GloCountry",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloCommunicationToken",
                schema: "notification",
                newName: "GloCommunicationToken",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloBusinessType",
                schema: "glo",
                newName: "GloBusinessType",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloBillingCategory",
                schema: "glo",
                newName: "GloBillingCategory",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "GloAccountingIntegrationType",
                schema: "integration",
                newName: "GloAccountingIntegrationType",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsVendorInventoryItem",
                schema: "inventory",
                newName: "FgsVendorInventoryItem",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsVendor",
                schema: "inventory",
                newName: "FgsVendor",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsUserRole",
                schema: "identity",
                newName: "FgsUserRole",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsUser",
                schema: "identity",
                newName: "FgsUser",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsTenantServiceSetup",
                schema: "tenant",
                newName: "FgsTenantServiceSetup",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsTenantCompany",
                schema: "tenant",
                newName: "FgsTenantCompany",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsTenant",
                schema: "tenant",
                newName: "FgsTenant",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsTagEntityType",
                schema: "shared",
                newName: "FgsTagEntityType",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsTag",
                schema: "shared",
                newName: "FgsTag",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsSetupZone",
                schema: "dispatch",
                newName: "FgsSetupZone",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsSetupTitleOfCourtesy",
                schema: "crm",
                newName: "FgsSetupTitleOfCourtesy",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsSetupTimeSlot",
                schema: "dispatch",
                newName: "FgsSetupTimeSlot",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsSetupTechTrade",
                schema: "dispatch",
                newName: "FgsSetupTechTrade",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsSetupTechSkillLevel",
                schema: "dispatch",
                newName: "FgsSetupTechSkillLevel",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsSetupTaxDetail",
                schema: "billing",
                newName: "FgsSetupTaxDetail",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsSetupTaxAuthority",
                schema: "billing",
                newName: "FgsSetupTaxAuthority",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsSetupTax",
                schema: "billing",
                newName: "FgsSetupTax",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsSetupServiceAssetType",
                schema: "dispatch",
                newName: "FgsSetupServiceAssetType",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsSetupServiceAssetModelReference",
                schema: "dispatch",
                newName: "FgsSetupServiceAssetModelReference",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsSetupServiceAssetManufacturer",
                schema: "dispatch",
                newName: "FgsSetupServiceAssetManufacturer",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsSetupPricingMatrixOther",
                schema: "billing",
                newName: "FgsSetupPricingMatrixOther",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsSetupPricingMatrixMaterialTier",
                schema: "billing",
                newName: "FgsSetupPricingMatrixMaterialTier",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsSetupPricingMatrixLaborTier",
                schema: "billing",
                newName: "FgsSetupPricingMatrixLaborTier",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsSetupPricingMatrixLabor",
                schema: "billing",
                newName: "FgsSetupPricingMatrixLabor",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsSetupPricingMatrix",
                schema: "billing",
                newName: "FgsSetupPricingMatrix",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsSetupPostalCode",
                schema: "dispatch",
                newName: "FgsSetupPostalCode",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsSetupPaymentTerm",
                schema: "billing",
                newName: "FgsSetupPaymentTerm",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsSetupPaymentMethod",
                schema: "billing",
                newName: "FgsSetupPaymentMethod",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsSetupLaborRateType",
                schema: "billing",
                newName: "FgsSetupLaborRateType",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsSetupGLBreakTrade",
                schema: "billing",
                newName: "FgsSetupGLBreakTrade",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsSetupGLBreak",
                schema: "billing",
                newName: "FgsSetupGLBreak",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsSetupDescription",
                schema: "notification",
                newName: "FgsSetupDescription",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsSetupCommunicationTemplate",
                schema: "notification",
                newName: "FgsSetupCommunicationTemplate",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsRole",
                schema: "identity",
                newName: "FgsRole",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsResolutionCode",
                schema: "dispatch",
                newName: "FgsResolutionCode",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsLocation",
                schema: "shared",
                newName: "FgsLocation",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsLeadSource",
                schema: "crm",
                newName: "FgsLeadSource",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsJobTypeSubCategory",
                schema: "dispatch",
                newName: "FgsJobTypeSubCategory",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsJobTypeCategory",
                schema: "dispatch",
                newName: "FgsJobTypeCategory",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsJobType",
                schema: "dispatch",
                newName: "FgsJobType",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsInvitation",
                schema: "identity",
                newName: "FgsInvitation",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsInventorySubCategory",
                schema: "inventory",
                newName: "FgsInventorySubCategory",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsInventoryStock",
                schema: "inventory",
                newName: "FgsInventoryStock",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsInventoryItemType",
                schema: "inventory",
                newName: "FgsInventoryItemType",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsInventoryItemDependency",
                schema: "inventory",
                newName: "FgsInventoryItemDependency",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsInventoryItemAlternate",
                schema: "inventory",
                newName: "FgsInventoryItemAlternate",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsInventoryItem",
                schema: "inventory",
                newName: "FgsInventoryItem",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsInventoryCategory",
                schema: "inventory",
                newName: "FgsInventoryCategory",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsFile",
                schema: "shared",
                newName: "FgsFile",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsEntityTag",
                schema: "shared",
                newName: "FgsEntityTag",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsCredentialSecret",
                schema: "integration",
                newName: "FgsCredentialSecret",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsCredentialProviderConfiguration",
                schema: "integration",
                newName: "FgsCredentialProviderConfiguration",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsCredentialProvider",
                schema: "integration",
                newName: "FgsCredentialProvider",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsCredentialAudit",
                schema: "audit",
                newName: "FgsCredentialAudit",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsBusinessType",
                schema: "dispatch",
                newName: "FgsBusinessType",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "FgsBillingCategory",
                schema: "billing",
                newName: "FgsBillingCategory",
                newSchema: "dbo");

            migrationBuilder.Sql(
                """
                ALTER TABLE IF EXISTS shared."__EFMigrationsHistory" SET SCHEMA dbo;
                """);
        }
    }
}
