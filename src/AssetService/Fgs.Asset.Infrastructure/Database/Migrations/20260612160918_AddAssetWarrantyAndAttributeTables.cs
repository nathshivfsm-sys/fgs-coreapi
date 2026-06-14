using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Asset.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddAssetWarrantyAndAttributeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsAssetAttribute",
                schema: "asset",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique asset attribute identifier.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssetTypeId = table.Column<long>(type: "bigint", nullable: false, comment: "Asset type that owns this attribute definition."),
                    AttributeCode = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: false, comment: "Unique attribute code within the asset type. Stored in uppercase."),
                    AttributeName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Display name shown to users."),
                    InputType = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false, comment: "Input type. Valid values: TEXT, TEXTAREA, INTEGER, DECIMAL, DATE, BOOLEAN, DROPDOWN."),
                    DefaultOptionId = table.Column<long>(type: "bigint", nullable: true, comment: "Default dropdown option when InputType is DROPDOWN."),
                    DefaultValueText = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Default text value."),
                    DefaultValueInteger = table.Column<int>(type: "integer", nullable: true, comment: "Default integer value."),
                    DefaultValueDecimal = table.Column<decimal>(type: "numeric(18,4)", nullable: true, comment: "Default decimal value."),
                    DefaultValueDate = table.Column<DateOnly>(type: "date", nullable: true, comment: "Default date value."),
                    DefaultValueBoolean = table.Column<bool>(type: "boolean", nullable: true, comment: "Default boolean value."),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether a value must be supplied when creating or updating an asset."),
                    IsSearchable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the attribute should be available in asset search and filtering screens."),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 0, comment: "Controls the display order of attributes within the asset type."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the attribute definition is active."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsAssetAttribute", x => x.Id);
                    table.CheckConstraint("CK_FgsAssetAttribute_AttributeCode_Upper", "\"AttributeCode\" = upper(\"AttributeCode\")");
                    table.CheckConstraint("CK_FgsAssetAttribute_InputType_Upper", "\"InputType\" = upper(\"InputType\")");
                    table.ForeignKey(
                        name: "FK_FgsAssetAttribute_AssetType",
                        column: x => x.AssetTypeId,
                        principalSchema: "asset",
                        principalTable: "FgsAssetType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsAssetAttribute_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "asset",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Defines custom asset attributes that can be assigned to specific asset types.");

            migrationBuilder.CreateTable(
                name: "FgsAssetWarranty",
                schema: "asset",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique warranty record identifier.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssetId = table.Column<long>(type: "bigint", nullable: false, comment: "Asset covered by the warranty."),
                    WarrantyType = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: false, comment: "Warranty type such as MANUFACTURER, MANUFACTURER_EXTENDED, LABOR, LABOR_EXTENDED, PARTS, COMPRESSOR, HEAT_EXCHANGER, INSTALLATION, or OTHER."),
                    WarrantyProvider = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Manufacturer, contractor, dealer, or third-party organization providing the warranty coverage."),
                    WarrantyNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Warranty contract number, policy number, or manufacturer warranty identifier."),
                    RegistrationNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Warranty registration confirmation number provided by the warranty issuer."),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "Date warranty coverage becomes effective."),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "Date warranty coverage expires."),
                    CoverageDescription = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true, comment: "Detailed description of warranty coverage including covered components, labor coverage, exclusions, reimbursement limitations, registration requirements, and special warranty terms."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsAssetWarranty", x => x.Id);
                    table.CheckConstraint("CK_FgsAssetWarranty_DateRange", "\"EndDate\" >= \"StartDate\"");
                    table.CheckConstraint("CK_FgsAssetWarranty_WarrantyType_Upper", "\"WarrantyType\" = upper(\"WarrantyType\")");
                    table.ForeignKey(
                        name: "FK_FgsAssetWarranty_Asset",
                        column: x => x.AssetId,
                        principalSchema: "asset",
                        principalTable: "FgsAsset",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsAssetWarranty_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "asset",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores warranty coverage associated with customer assets and equipment.");

            migrationBuilder.CreateTable(
                name: "FgsAssetAttributeOption",
                schema: "asset",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique asset attribute option identifier.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssetAttributeId = table.Column<long>(type: "bigint", nullable: false, comment: "Asset attribute definition that owns this option."),
                    OptionCode = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: false, comment: "Unique option code within the asset attribute. Stored in uppercase."),
                    OptionName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Display name shown to users."),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 0, comment: "Controls the order in which options are displayed to users."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the option is available for selection on new or updated assets."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsAssetAttributeOption", x => x.Id);
                    table.CheckConstraint("CK_FgsAssetAttributeOption_OptionCode_Upper", "\"OptionCode\" = upper(\"OptionCode\")");
                    table.ForeignKey(
                        name: "FK_FgsAssetAttributeOption_AssetAttribute",
                        column: x => x.AssetAttributeId,
                        principalSchema: "asset",
                        principalTable: "FgsAssetAttribute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsAssetAttributeOption_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "asset",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores selectable dropdown values for asset attributes.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetAttribute_AssetTypeId",
                schema: "asset",
                table: "FgsAssetAttribute",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetAttribute_TenantId_CompanyId",
                schema: "asset",
                table: "FgsAssetAttribute",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetAttribute_TenantId_CompanyId_AssetTypeId",
                schema: "asset",
                table: "FgsAssetAttribute",
                columns: new[] { "TenantId", "CompanyId", "AssetTypeId" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsAssetAttribute_TenantCompanyAssetTypeCode",
                schema: "asset",
                table: "FgsAssetAttribute",
                columns: new[] { "TenantId", "CompanyId", "AssetTypeId", "AttributeCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetAttributeOption_AssetAttributeId",
                schema: "asset",
                table: "FgsAssetAttributeOption",
                column: "AssetAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetAttributeOption_TenantId_CompanyId",
                schema: "asset",
                table: "FgsAssetAttributeOption",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetAttributeOption_TenantId_CompanyId_AssetAttributeId",
                schema: "asset",
                table: "FgsAssetAttributeOption",
                columns: new[] { "TenantId", "CompanyId", "AssetAttributeId" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsAssetAttributeOption_Code",
                schema: "asset",
                table: "FgsAssetAttributeOption",
                columns: new[] { "TenantId", "CompanyId", "AssetAttributeId", "OptionCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_FgsAssetAttributeOption_Name",
                schema: "asset",
                table: "FgsAssetAttributeOption",
                columns: new[] { "TenantId", "CompanyId", "AssetAttributeId", "OptionName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetWarranty_TenantId_CompanyId",
                schema: "asset",
                table: "FgsAssetWarranty",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetWarranty_TenantId_CompanyId_AssetId",
                schema: "asset",
                table: "FgsAssetWarranty",
                columns: new[] { "TenantId", "CompanyId", "AssetId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetWarranty_TenantId_CompanyId_EndDate",
                schema: "asset",
                table: "FgsAssetWarranty",
                columns: new[] { "TenantId", "CompanyId", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetWarranty_TenantId_CompanyId_StartDate_EndDate",
                schema: "asset",
                table: "FgsAssetWarranty",
                columns: new[] { "TenantId", "CompanyId", "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetWarranty_TenantId_CompanyId_WarrantyType",
                schema: "asset",
                table: "FgsAssetWarranty",
                columns: new[] { "TenantId", "CompanyId", "WarrantyType" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsAssetWarranty_AssetId_WarrantyType_StartDate",
                schema: "asset",
                table: "FgsAssetWarranty",
                columns: new[] { "AssetId", "WarrantyType", "StartDate" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsAssetAttributeOption",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "FgsAssetWarranty",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "FgsAssetAttribute",
                schema: "asset");
        }
    }
}
