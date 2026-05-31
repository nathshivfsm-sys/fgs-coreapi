using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.User.Infrastructure.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class MoveDomainTablesToSetupSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "setup");

            migrationBuilder.RenameTable(
                name: "FgsVendorInventoryItem",
                schema: "inventory",
                newName: "FgsVendorInventoryItem",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsVendor",
                schema: "inventory",
                newName: "FgsVendor",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsSetupZone",
                schema: "dispatch",
                newName: "FgsSetupZone",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsSetupTitleOfCourtesy",
                schema: "crm",
                newName: "FgsSetupTitleOfCourtesy",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsSetupTimeSlot",
                schema: "dispatch",
                newName: "FgsSetupTimeSlot",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsSetupTechTrade",
                schema: "dispatch",
                newName: "FgsSetupTechTrade",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsSetupTechSkillLevel",
                schema: "dispatch",
                newName: "FgsSetupTechSkillLevel",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsSetupTaxDetail",
                schema: "billing",
                newName: "FgsSetupTaxDetail",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsSetupTaxAuthority",
                schema: "billing",
                newName: "FgsSetupTaxAuthority",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsSetupTax",
                schema: "billing",
                newName: "FgsSetupTax",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsSetupServiceAssetType",
                schema: "dispatch",
                newName: "FgsSetupServiceAssetType",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsSetupServiceAssetModelReference",
                schema: "dispatch",
                newName: "FgsSetupServiceAssetModelReference",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsSetupServiceAssetManufacturer",
                schema: "dispatch",
                newName: "FgsSetupServiceAssetManufacturer",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsSetupPricingMatrixOther",
                schema: "billing",
                newName: "FgsSetupPricingMatrixOther",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsSetupPricingMatrixMaterialTier",
                schema: "billing",
                newName: "FgsSetupPricingMatrixMaterialTier",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsSetupPricingMatrixLaborTier",
                schema: "billing",
                newName: "FgsSetupPricingMatrixLaborTier",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsSetupPricingMatrixLabor",
                schema: "billing",
                newName: "FgsSetupPricingMatrixLabor",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsSetupPricingMatrix",
                schema: "billing",
                newName: "FgsSetupPricingMatrix",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsSetupPostalCode",
                schema: "dispatch",
                newName: "FgsSetupPostalCode",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsSetupPaymentTerm",
                schema: "billing",
                newName: "FgsSetupPaymentTerm",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsSetupPaymentMethod",
                schema: "billing",
                newName: "FgsSetupPaymentMethod",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsSetupLaborRateType",
                schema: "billing",
                newName: "FgsSetupLaborRateType",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsSetupGLBreakTrade",
                schema: "billing",
                newName: "FgsSetupGLBreakTrade",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsSetupGLBreak",
                schema: "billing",
                newName: "FgsSetupGLBreak",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsSetupDescription",
                schema: "notification",
                newName: "FgsSetupDescription",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsSetupCommunicationTemplate",
                schema: "notification",
                newName: "FgsSetupCommunicationTemplate",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsResolutionCode",
                schema: "dispatch",
                newName: "FgsResolutionCode",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsLeadSource",
                schema: "crm",
                newName: "FgsLeadSource",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsJobTypeSubCategory",
                schema: "dispatch",
                newName: "FgsJobTypeSubCategory",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsJobTypeCategory",
                schema: "dispatch",
                newName: "FgsJobTypeCategory",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsJobType",
                schema: "dispatch",
                newName: "FgsJobType",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsInventorySubCategory",
                schema: "inventory",
                newName: "FgsInventorySubCategory",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsInventoryStock",
                schema: "inventory",
                newName: "FgsInventoryStock",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsInventoryItemType",
                schema: "inventory",
                newName: "FgsInventoryItemType",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsInventoryItemDependency",
                schema: "inventory",
                newName: "FgsInventoryItemDependency",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsInventoryItemAlternate",
                schema: "inventory",
                newName: "FgsInventoryItemAlternate",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsInventoryItem",
                schema: "inventory",
                newName: "FgsInventoryItem",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsInventoryCategory",
                schema: "inventory",
                newName: "FgsInventoryCategory",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsCredentialSecret",
                schema: "integration",
                newName: "FgsCredentialSecret",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsCredentialProviderConfiguration",
                schema: "integration",
                newName: "FgsCredentialProviderConfiguration",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsCredentialProvider",
                schema: "integration",
                newName: "FgsCredentialProvider",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsBusinessType",
                schema: "dispatch",
                newName: "FgsBusinessType",
                newSchema: "setup");

            migrationBuilder.RenameTable(
                name: "FgsBillingCategory",
                schema: "billing",
                newName: "FgsBillingCategory",
                newSchema: "setup");

            migrationBuilder.Sql(
                """
                UPDATE glo."GloSeedTableMapping"
                SET "TargetSchemaName" = 'setup'
                WHERE "TargetSchemaName" IN ('billing', 'crm', 'dispatch', 'integration', 'inventory', 'notification');
                """);

            migrationBuilder.Sql(
                """
                DROP SCHEMA IF EXISTS billing;
                DROP SCHEMA IF EXISTS crm;
                DROP SCHEMA IF EXISTS dispatch;
                DROP SCHEMA IF EXISTS integration;
                DROP SCHEMA IF EXISTS inventory;
                DROP SCHEMA IF EXISTS notification;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "billing");

            migrationBuilder.EnsureSchema(
                name: "dispatch");

            migrationBuilder.EnsureSchema(
                name: "integration");

            migrationBuilder.EnsureSchema(
                name: "inventory");

            migrationBuilder.EnsureSchema(
                name: "crm");

            migrationBuilder.EnsureSchema(
                name: "notification");

            migrationBuilder.RenameTable(
                name: "FgsVendorInventoryItem",
                schema: "setup",
                newName: "FgsVendorInventoryItem",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "FgsVendor",
                schema: "setup",
                newName: "FgsVendor",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "FgsSetupZone",
                schema: "setup",
                newName: "FgsSetupZone",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsSetupTitleOfCourtesy",
                schema: "setup",
                newName: "FgsSetupTitleOfCourtesy",
                newSchema: "crm");

            migrationBuilder.RenameTable(
                name: "FgsSetupTimeSlot",
                schema: "setup",
                newName: "FgsSetupTimeSlot",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsSetupTechTrade",
                schema: "setup",
                newName: "FgsSetupTechTrade",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsSetupTechSkillLevel",
                schema: "setup",
                newName: "FgsSetupTechSkillLevel",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsSetupTaxDetail",
                schema: "setup",
                newName: "FgsSetupTaxDetail",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupTaxAuthority",
                schema: "setup",
                newName: "FgsSetupTaxAuthority",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupTax",
                schema: "setup",
                newName: "FgsSetupTax",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupServiceAssetType",
                schema: "setup",
                newName: "FgsSetupServiceAssetType",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsSetupServiceAssetModelReference",
                schema: "setup",
                newName: "FgsSetupServiceAssetModelReference",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsSetupServiceAssetManufacturer",
                schema: "setup",
                newName: "FgsSetupServiceAssetManufacturer",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsSetupPricingMatrixOther",
                schema: "setup",
                newName: "FgsSetupPricingMatrixOther",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupPricingMatrixMaterialTier",
                schema: "setup",
                newName: "FgsSetupPricingMatrixMaterialTier",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupPricingMatrixLaborTier",
                schema: "setup",
                newName: "FgsSetupPricingMatrixLaborTier",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupPricingMatrixLabor",
                schema: "setup",
                newName: "FgsSetupPricingMatrixLabor",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupPricingMatrix",
                schema: "setup",
                newName: "FgsSetupPricingMatrix",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupPostalCode",
                schema: "setup",
                newName: "FgsSetupPostalCode",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsSetupPaymentTerm",
                schema: "setup",
                newName: "FgsSetupPaymentTerm",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupPaymentMethod",
                schema: "setup",
                newName: "FgsSetupPaymentMethod",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupLaborRateType",
                schema: "setup",
                newName: "FgsSetupLaborRateType",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupGLBreakTrade",
                schema: "setup",
                newName: "FgsSetupGLBreakTrade",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupGLBreak",
                schema: "setup",
                newName: "FgsSetupGLBreak",
                newSchema: "billing");

            migrationBuilder.RenameTable(
                name: "FgsSetupDescription",
                schema: "setup",
                newName: "FgsSetupDescription",
                newSchema: "notification");

            migrationBuilder.RenameTable(
                name: "FgsSetupCommunicationTemplate",
                schema: "setup",
                newName: "FgsSetupCommunicationTemplate",
                newSchema: "notification");

            migrationBuilder.RenameTable(
                name: "FgsResolutionCode",
                schema: "setup",
                newName: "FgsResolutionCode",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsLeadSource",
                schema: "setup",
                newName: "FgsLeadSource",
                newSchema: "crm");

            migrationBuilder.RenameTable(
                name: "FgsJobTypeSubCategory",
                schema: "setup",
                newName: "FgsJobTypeSubCategory",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsJobTypeCategory",
                schema: "setup",
                newName: "FgsJobTypeCategory",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsJobType",
                schema: "setup",
                newName: "FgsJobType",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsInventorySubCategory",
                schema: "setup",
                newName: "FgsInventorySubCategory",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "FgsInventoryStock",
                schema: "setup",
                newName: "FgsInventoryStock",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "FgsInventoryItemType",
                schema: "setup",
                newName: "FgsInventoryItemType",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "FgsInventoryItemDependency",
                schema: "setup",
                newName: "FgsInventoryItemDependency",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "FgsInventoryItemAlternate",
                schema: "setup",
                newName: "FgsInventoryItemAlternate",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "FgsInventoryItem",
                schema: "setup",
                newName: "FgsInventoryItem",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "FgsInventoryCategory",
                schema: "setup",
                newName: "FgsInventoryCategory",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "FgsCredentialSecret",
                schema: "setup",
                newName: "FgsCredentialSecret",
                newSchema: "integration");

            migrationBuilder.RenameTable(
                name: "FgsCredentialProviderConfiguration",
                schema: "setup",
                newName: "FgsCredentialProviderConfiguration",
                newSchema: "integration");

            migrationBuilder.RenameTable(
                name: "FgsCredentialProvider",
                schema: "setup",
                newName: "FgsCredentialProvider",
                newSchema: "integration");

            migrationBuilder.RenameTable(
                name: "FgsBusinessType",
                schema: "setup",
                newName: "FgsBusinessType",
                newSchema: "dispatch");

            migrationBuilder.RenameTable(
                name: "FgsBillingCategory",
                schema: "setup",
                newName: "FgsBillingCategory",
                newSchema: "billing");

            migrationBuilder.Sql(
                """
                UPDATE glo."GloSeedTableMapping"
                SET "TargetSchemaName" = CASE "TargetTableName"
                    WHEN 'FgsBillingCategory' THEN 'billing'
                    WHEN 'FgsSetupPaymentMethod' THEN 'billing'
                    WHEN 'FgsSetupLaborRateType' THEN 'billing'
                    WHEN 'FgsSetupPaymentTerm' THEN 'billing'
                    WHEN 'FgsLeadSource' THEN 'crm'
                    WHEN 'FgsSetupTitleOfCourtesy' THEN 'crm'
                    WHEN 'FgsJobTypeCategory' THEN 'dispatch'
                    WHEN 'FgsJobTypeSubCategory' THEN 'dispatch'
                    WHEN 'FgsResolutionCode' THEN 'dispatch'
                    WHEN 'FgsSetupTechSkillLevel' THEN 'dispatch'
                    WHEN 'FgsSetupTechTrade' THEN 'dispatch'
                    WHEN 'FgsSetupZone' THEN 'dispatch'
                    ELSE "TargetSchemaName"
                END
                WHERE "TargetSchemaName" = 'setup';
                """);
        }
    }
}
