using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCacheTablesAndRewireFks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FgsCredential_GloCredentialProviderType",
                schema: "setup",
                table: "FgsCredential");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsResolutionCode_GloResType",
                schema: "setup",
                table: "FgsResolutionCode");

            migrationBuilder.DropColumn(
                name: "LocationId",
                schema: "setup",
                table: "FgsWarehouse");

            migrationBuilder.AddColumn<Guid>(
                name: "AddressId",
                schema: "setup",
                table: "FgsWarehouse",
                type: "uuid",
                nullable: true,
                comment: "Optional reference to the warehouse address record.");

            migrationBuilder.CreateTable(
                name: "FgsTenantCompanyCache",
                schema: "setup",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the tenant that owns the company."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier used throughout FSM. Maps to tenant.FgsTenantCompany.CompanyNumber."),
                    CompanyGuid = table.Column<Guid>(type: "uuid", nullable: false, comment: "Globally unique public identifier used by external integrations and APIs."),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Human-readable company code."),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Display name of the company."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, comment: "Indicates whether the company is active."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Timestamp of the most recent synchronization from tenant.FgsTenantCompany.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsTenantCompanyCache", x => new { x.TenantId, x.CompanyId });
                },
                comment: "Local cache of tenant company identity information used for CompanyGuid resolution and elimination of cross-schema dependencies.");

            migrationBuilder.CreateTable(
                name: "GloCredentialProviderTypeCache",
                schema: "setup",
                columns: table => new
                {
                    ProviderTypeId = table.Column<int>(type: "integer", nullable: false, comment: "Identifier from glo.GloCredentialProviderType.Id."),
                    ProviderCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "System unique provider code used by application logic and integration services."),
                    ProviderName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "User friendly provider name displayed in setup screens."),
                    ConfigurationSchema = table.Column<string>(type: "jsonb", nullable: false, comment: "JSON schema used by the UI to dynamically render provider configuration fields and perform validation."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, comment: "Indicates whether the provider can be selected for new credential configurations."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Timestamp of the most recent synchronization from glo.GloCredentialProviderType.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloCredentialProviderTypeCache", x => x.ProviderTypeId);
                },
                comment: "Local cache of globally defined credential providers used to eliminate cross-schema dependencies.");

            migrationBuilder.CreateTable(
                name: "GloResolutionTypeCache",
                schema: "setup",
                columns: table => new
                {
                    ResolutionTypeId = table.Column<int>(type: "integer", nullable: false, comment: "Identifier from glo.GloResolutionType.Id."),
                    ResolutionTypeCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "System unique resolution type code."),
                    ResolutionTypeName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "User friendly resolution type name displayed in setup screens."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, comment: "Indicates whether the resolution type can be used for new configurations."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Timestamp of the most recent synchronization from glo.GloResolutionType.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloResolutionTypeCache", x => x.ResolutionTypeId);
                },
                comment: "Local cache of globally defined resolution types used to eliminate cross-schema dependencies.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsTenantCompanyCache_TenantId_Code",
                schema: "setup",
                table: "FgsTenantCompanyCache",
                columns: new[] { "TenantId", "Code" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsTenantCompanyCache_TenantId_Name",
                schema: "setup",
                table: "FgsTenantCompanyCache",
                columns: new[] { "TenantId", "Name" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsTenantCompanyCache_CompanyGuid",
                schema: "setup",
                table: "FgsTenantCompanyCache",
                column: "CompanyGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloCredentialProviderTypeCache_ProviderName",
                schema: "setup",
                table: "GloCredentialProviderTypeCache",
                column: "ProviderName");

            migrationBuilder.CreateIndex(
                name: "UQ_GloCredentialProviderTypeCache_ProviderCode",
                schema: "setup",
                table: "GloCredentialProviderTypeCache",
                column: "ProviderCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloResolutionTypeCache_ResolutionTypeName",
                schema: "setup",
                table: "GloResolutionTypeCache",
                column: "ResolutionTypeName");

            migrationBuilder.CreateIndex(
                name: "UQ_GloResolutionTypeCache_ResolutionTypeCode",
                schema: "setup",
                table: "GloResolutionTypeCache",
                column: "ResolutionTypeCode",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsBillingCategory_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsBillingCategory",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsBusinessType_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsBusinessType",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql(
                """
                ALTER TABLE IF EXISTS setup."FgsCredential"
                    DROP CONSTRAINT IF EXISTS "FK_FgsCredential_FgsTenantCompany_TenantId_CompanyId";
                """);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsCredential_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsCredential",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsCredential_GloCredentialProviderTypeCache_ProviderTypeId",
                schema: "setup",
                table: "FgsCredential",
                column: "CredentialProviderTypeId",
                principalSchema: "setup",
                principalTable: "GloCredentialProviderTypeCache",
                principalColumn: "ProviderTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsEntityTag_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsEntityTag",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsInventoryCategory_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsInventoryCategory",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsInventoryItem_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsInventoryItem",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsInventoryItemAlternate_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsInventoryItemAlternate",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsInventoryItemDependency_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsInventoryItemDependency",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsInventoryItemType_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsInventoryItemType",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsInventoryStock_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsInventoryStock",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsInventorySubCategory_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsInventorySubCategory",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsJobType_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsJobType",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsJobTypeCategory_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsJobTypeCategory",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsJobTypeSubCategory_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsJobTypeSubCategory",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsLeadSource_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsLeadSource",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsResolutionCode_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsResolutionCode",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsResolutionCode_GloResolutionTypeCache_ResolutionTypeId",
                schema: "setup",
                table: "FgsResolutionCode",
                column: "GloResolutionTypeId",
                principalSchema: "setup",
                principalTable: "GloResolutionTypeCache",
                principalColumn: "ResolutionTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupDescription_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupDescription",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupGLBreak_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupGLBreak",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupGLBreakTrade_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupGLBreakTrade",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupLaborRateType_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupLaborRateType",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupPaymentMethod_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupPaymentMethod",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupPaymentTerm_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupPaymentTerm",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupPostalCode_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupPostalCode",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupPricingMatrix_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupPricingMatrix",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupPricingMatrixLabor_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupPricingMatrixLabor",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupPricingMatrixLaborTier_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupPricingMatrixLaborTier",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupPricingMatrixMaterialTier_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupPricingMatrixOther_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupPricingMatrixOther",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupServiceAssetManufacturer_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupServiceAssetManufacturer",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupServiceAssetModelReference_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupServiceAssetModelReference",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupServiceAssetType_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupServiceAssetType",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupTax_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupTax",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupTaxAuthority_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupTaxAuthority",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupTaxDetail_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupTaxDetail",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupTechSkillLevel_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupTechSkillLevel",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupTechTrade_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupTechTrade",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupTimeSlot_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupTimeSlot",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupTitleOfCourtesy_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupTitleOfCourtesy",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupZone_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupZone",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsTag_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsTag",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsTagEntityType_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsTagEntityType",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsVehicle_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsVehicle",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsVehicleMaintenance_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsVehicleMaintenance",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsVendor_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsVendor",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsVendorInventoryItem_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsVendorInventoryItem",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsWarehouse_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsWarehouse",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "setup",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FgsBillingCategory_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsBillingCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsBusinessType_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsBusinessType");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsCredential_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsCredential");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsCredential_GloCredentialProviderTypeCache_ProviderTypeId",
                schema: "setup",
                table: "FgsCredential");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsEntityTag_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsEntityTag");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsInventoryCategory_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsInventoryCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsInventoryItem_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsInventoryItem");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsInventoryItemAlternate_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsInventoryItemAlternate");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsInventoryItemDependency_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsInventoryItemDependency");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsInventoryItemType_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsInventoryItemType");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsInventoryStock_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsInventoryStock");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsInventorySubCategory_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsInventorySubCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsJobType_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsJobType");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsJobTypeCategory_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsJobTypeCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsJobTypeSubCategory_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsJobTypeSubCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsLeadSource_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsLeadSource");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsResolutionCode_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsResolutionCode");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsResolutionCode_GloResolutionTypeCache_ResolutionTypeId",
                schema: "setup",
                table: "FgsResolutionCode");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupDescription_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupDescription");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupGLBreak_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupGLBreak");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupGLBreakTrade_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupGLBreakTrade");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupLaborRateType_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupLaborRateType");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupPaymentMethod_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupPaymentMethod");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupPaymentTerm_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupPaymentTerm");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupPostalCode_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupPostalCode");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupPricingMatrix_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupPricingMatrix");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupPricingMatrixLabor_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupPricingMatrixLabor");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupPricingMatrixLaborTier_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupPricingMatrixLaborTier");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupPricingMatrixMaterialTier_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupPricingMatrixOther_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupPricingMatrixOther");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupServiceAssetManufacturer_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupServiceAssetManufacturer");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupServiceAssetModelReference_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupServiceAssetModelReference");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupServiceAssetType_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupServiceAssetType");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupTax_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupTax");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupTaxAuthority_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupTaxAuthority");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupTaxDetail_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupTaxDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupTechSkillLevel_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupTechSkillLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupTechTrade_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupTechTrade");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupTimeSlot_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupTimeSlot");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupTitleOfCourtesy_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupTitleOfCourtesy");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupZone_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupZone");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsTag_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsTag");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsTagEntityType_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsTagEntityType");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsVehicle_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsVehicle");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsVehicleMaintenance_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsVehicleMaintenance");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsVendor_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsVendor");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsVendorInventoryItem_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsVendorInventoryItem");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsWarehouse_FgsTenantCompanyCache_TenantId_CompanyId",
                schema: "setup",
                table: "FgsWarehouse");

            migrationBuilder.DropTable(
                name: "FgsTenantCompanyCache",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "GloCredentialProviderTypeCache",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "GloResolutionTypeCache",
                schema: "setup");

            migrationBuilder.DropColumn(
                name: "AddressId",
                schema: "setup",
                table: "FgsWarehouse");

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                schema: "setup",
                table: "FgsWarehouse",
                type: "uuid",
                nullable: true,
                comment: "Optional reference to the physical address or geo location in FgsLocation.");

            migrationBuilder.AddForeignKey(
                name: "FK_FgsCredential_GloCredentialProviderType",
                schema: "setup",
                table: "FgsCredential",
                column: "CredentialProviderTypeId",
                principalSchema: "glo",
                principalTable: "GloCredentialProviderType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsResolutionCode_GloResType",
                schema: "setup",
                table: "FgsResolutionCode",
                column: "GloResolutionTypeId",
                principalSchema: "glo",
                principalTable: "GloResolutionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
