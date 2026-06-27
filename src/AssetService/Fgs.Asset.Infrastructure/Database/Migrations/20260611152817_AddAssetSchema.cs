using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Asset.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddAssetSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "asset");

            migrationBuilder.CreateTable(
                name: "FgsTenantCompanyCache",
                schema: "asset",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier mapped from tenant.FgsTenantCompany.CompanyNumber."),
                    CompanyGuid = table.Column<Guid>(type: "uuid", nullable: false, comment: "Globally unique company identifier used by integrations and external systems."),
                    CompanyCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Unique company code within a tenant."),
                    CompanyName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Display name of the company."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, comment: "Indicates whether the company is active."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Timestamp of the most recent synchronization from tenant.FgsTenantCompany.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsTenantCompanyCache", x => new { x.TenantId, x.CompanyId });
                },
                comment: "Local cache of tenant company information used by the asset schema to eliminate cross-schema dependencies on tenant.FgsTenantCompany.");

            migrationBuilder.CreateTable(
                name: "FgsAssetManufacturer",
                schema: "asset",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique asset manufacturer identifier.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: false, comment: "Unique manufacturer code within the tenant company. Stored in uppercase."),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Manufacturer name displayed to users."),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Optional description of the manufacturer."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the manufacturer is active and available for selection."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsAssetManufacturer", x => x.Id);
                    table.CheckConstraint("CK_FgsAssetManufacturer_Code_Upper", "\"Code\" = upper(\"Code\")");
                    table.ForeignKey(
                        name: "FK_FgsAssetManufacturer_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "asset",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores equipment manufacturers available for asset management.");

            migrationBuilder.CreateTable(
                name: "FgsAssetStatus",
                schema: "asset",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique asset status identifier.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: false, comment: "Unique status code within the tenant company. Stored in uppercase."),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Display name of the asset status."),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Optional description of the asset status."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the status is active and available for selection."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsAssetStatus", x => x.Id);
                    table.CheckConstraint("CK_FgsAssetStatus_Code_Upper", "\"Code\" = upper(\"Code\")");
                    table.ForeignKey(
                        name: "FK_FgsAssetStatus_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "asset",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores lifecycle status values used to classify service assets.");

            migrationBuilder.CreateTable(
                name: "FgsAssetType",
                schema: "asset",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique asset type identifier.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(75)", maxLength: 75, nullable: false, comment: "Unique asset type code within the tenant company. Stored in uppercase."),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Display name of the asset type."),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Optional description of the asset type."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the asset type is active and available for selection."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsAssetType", x => x.Id);
                    table.CheckConstraint("CK_FgsAssetType_Code_Upper", "\"Code\" = upper(\"Code\")");
                    table.ForeignKey(
                        name: "FK_FgsAssetType_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "asset",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Defines equipment classifications used by service assets.");

            migrationBuilder.CreateTable(
                name: "FgsServiceLocationCache",
                schema: "asset",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    ServiceLocationId = table.Column<long>(type: "bigint", nullable: false, comment: "Service location identifier mapped from crm.CrmServiceLocation.Id."),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false, comment: "Customer identifier that owns the service location."),
                    LocationSequence = table.Column<int>(type: "integer", nullable: false, comment: "Sequential location number within the customer."),
                    LocationNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "User-visible service location number."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Timestamp of the most recent synchronization from crm.CrmServiceLocation.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsServiceLocationCache", x => new { x.TenantId, x.CompanyId, x.ServiceLocationId });
                    table.ForeignKey(
                        name: "FK_FgsServiceLocationCache_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "asset",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Local cache of CRM service location information used by the asset schema to eliminate cross-schema dependencies on crm.CrmServiceLocation.");

            migrationBuilder.CreateTable(
                name: "FgsAssetModel",
                schema: "asset",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique asset model identifier.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssetTypeId = table.Column<long>(type: "bigint", nullable: false, comment: "Asset type associated with this model."),
                    AssetManufacturerId = table.Column<long>(type: "bigint", nullable: false, comment: "Manufacturer associated with this model."),
                    ModelNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Manufacturer model number."),
                    ModelDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, comment: "Description of the model."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the model is active and available for selection."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsAssetModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsAssetModel_AssetManufacturer",
                        column: x => x.AssetManufacturerId,
                        principalSchema: "asset",
                        principalTable: "FgsAssetManufacturer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsAssetModel_AssetType",
                        column: x => x.AssetTypeId,
                        principalSchema: "asset",
                        principalTable: "FgsAssetType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsAssetModel_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "asset",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Catalog of equipment models that may be associated with service assets.");

            migrationBuilder.CreateTable(
                name: "FgsAsset",
                schema: "asset",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique asset identifier.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssetGuid = table.Column<Guid>(type: "uuid", nullable: false, comment: "Globally unique asset identifier used by integrations and external systems."),
                    AssetNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "User-visible asset number within the tenant company."),
                    ServiceLocationId = table.Column<long>(type: "bigint", nullable: false, comment: "Service location where the asset is installed."),
                    AssetTypeId = table.Column<long>(type: "bigint", nullable: true, comment: "Optional catalog asset type reference."),
                    AssetManufacturerId = table.Column<long>(type: "bigint", nullable: true, comment: "Optional catalog manufacturer reference."),
                    AssetModelId = table.Column<long>(type: "bigint", nullable: true, comment: "Optional catalog model reference."),
                    AssetDescription = table.Column<string>(type: "text", nullable: true, comment: "Internal asset description."),
                    CustomerAssetNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Customer-provided asset number or tag."),
                    CustomerAssetName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Customer-provided asset name."),
                    ManufacturerName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Free-text manufacturer name when not linked to catalog."),
                    ModelNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Free-text model number when not linked to catalog."),
                    SerialNumber = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Equipment serial number."),
                    ManufactureDate = table.Column<DateOnly>(type: "date", nullable: true, comment: "Date the equipment was manufactured."),
                    InstallDate = table.Column<DateOnly>(type: "date", nullable: true, comment: "Date the asset was installed at the service location."),
                    InstalledWorkOrderId = table.Column<long>(type: "bigint", nullable: true, comment: "Work order that installed the asset. References job service; no FK by design."),
                    IsInstalledByCompany = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the asset was installed by the service company."),
                    AssetStatusId = table.Column<long>(type: "bigint", nullable: false, comment: "Current lifecycle status of the asset."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the asset record is active."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsAsset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsAsset_AssetManufacturer",
                        column: x => x.AssetManufacturerId,
                        principalSchema: "asset",
                        principalTable: "FgsAssetManufacturer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsAsset_AssetModel",
                        column: x => x.AssetModelId,
                        principalSchema: "asset",
                        principalTable: "FgsAssetModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsAsset_AssetStatus",
                        column: x => x.AssetStatusId,
                        principalSchema: "asset",
                        principalTable: "FgsAssetStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsAsset_AssetType",
                        column: x => x.AssetTypeId,
                        principalSchema: "asset",
                        principalTable: "FgsAssetType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsAsset_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "asset",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsAsset_ServiceLocationCache",
                        columns: x => new { x.TenantId, x.CompanyId, x.ServiceLocationId },
                        principalSchema: "asset",
                        principalTable: "FgsServiceLocationCache",
                        principalColumns: new[] { "TenantId", "CompanyId", "ServiceLocationId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores customer-owned equipment and installed assets at service locations.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsAsset_AssetManufacturerId",
                schema: "asset",
                table: "FgsAsset",
                column: "AssetManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsAsset_AssetModelId",
                schema: "asset",
                table: "FgsAsset",
                column: "AssetModelId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsAsset_AssetStatusId",
                schema: "asset",
                table: "FgsAsset",
                column: "AssetStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsAsset_AssetTypeId",
                schema: "asset",
                table: "FgsAsset",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsAsset_TenantId_CompanyId",
                schema: "asset",
                table: "FgsAsset",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAsset_TenantId_CompanyId_AssetManufacturerId",
                schema: "asset",
                table: "FgsAsset",
                columns: new[] { "TenantId", "CompanyId", "AssetManufacturerId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAsset_TenantId_CompanyId_AssetModelId",
                schema: "asset",
                table: "FgsAsset",
                columns: new[] { "TenantId", "CompanyId", "AssetModelId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAsset_TenantId_CompanyId_AssetStatusId",
                schema: "asset",
                table: "FgsAsset",
                columns: new[] { "TenantId", "CompanyId", "AssetStatusId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAsset_TenantId_CompanyId_AssetTypeId",
                schema: "asset",
                table: "FgsAsset",
                columns: new[] { "TenantId", "CompanyId", "AssetTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAsset_TenantId_CompanyId_InstalledWorkOrderId",
                schema: "asset",
                table: "FgsAsset",
                columns: new[] { "TenantId", "CompanyId", "InstalledWorkOrderId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAsset_TenantId_CompanyId_IsActive",
                schema: "asset",
                table: "FgsAsset",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAsset_TenantId_CompanyId_IsInstalledByCompany",
                schema: "asset",
                table: "FgsAsset",
                columns: new[] { "TenantId", "CompanyId", "IsInstalledByCompany" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAsset_TenantId_CompanyId_SerialNumber",
                schema: "asset",
                table: "FgsAsset",
                columns: new[] { "TenantId", "CompanyId", "SerialNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAsset_TenantId_CompanyId_ServiceLocationId",
                schema: "asset",
                table: "FgsAsset",
                columns: new[] { "TenantId", "CompanyId", "ServiceLocationId" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsAsset_AssetGuid",
                schema: "asset",
                table: "FgsAsset",
                column: "AssetGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_FgsAsset_TenantId_CompanyId_AssetNumber",
                schema: "asset",
                table: "FgsAsset",
                columns: new[] { "TenantId", "CompanyId", "AssetNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetManufacturer_TenantId_CompanyId",
                schema: "asset",
                table: "FgsAssetManufacturer",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetManufacturer_TenantId_CompanyId_IsActive",
                schema: "asset",
                table: "FgsAssetManufacturer",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetManufacturer_TenantId_CompanyId_Name",
                schema: "asset",
                table: "FgsAssetManufacturer",
                columns: new[] { "TenantId", "CompanyId", "Name" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsAssetManufacturer_TenantId_CompanyId_Code",
                schema: "asset",
                table: "FgsAssetManufacturer",
                columns: new[] { "TenantId", "CompanyId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetModel_AssetManufacturerId",
                schema: "asset",
                table: "FgsAssetModel",
                column: "AssetManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetModel_AssetTypeId",
                schema: "asset",
                table: "FgsAssetModel",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetModel_TenantId_CompanyId",
                schema: "asset",
                table: "FgsAssetModel",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetModel_TenantId_CompanyId_AssetManufacturerId",
                schema: "asset",
                table: "FgsAssetModel",
                columns: new[] { "TenantId", "CompanyId", "AssetManufacturerId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetModel_TenantId_CompanyId_AssetTypeId",
                schema: "asset",
                table: "FgsAssetModel",
                columns: new[] { "TenantId", "CompanyId", "AssetTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetModel_TenantId_CompanyId_AssetTypeId_AssetManufacturerId",
                schema: "asset",
                table: "FgsAssetModel",
                columns: new[] { "TenantId", "CompanyId", "AssetTypeId", "AssetManufacturerId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetModel_TenantId_CompanyId_IsActive",
                schema: "asset",
                table: "FgsAssetModel",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsAssetModel_TenantCompanyManufacturerModelNumber",
                schema: "asset",
                table: "FgsAssetModel",
                columns: new[] { "TenantId", "CompanyId", "AssetManufacturerId", "ModelNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetStatus_TenantId_CompanyId",
                schema: "asset",
                table: "FgsAssetStatus",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetStatus_TenantId_CompanyId_IsActive",
                schema: "asset",
                table: "FgsAssetStatus",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetStatus_TenantId_CompanyId_Name",
                schema: "asset",
                table: "FgsAssetStatus",
                columns: new[] { "TenantId", "CompanyId", "Name" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsAssetStatus_TenantId_CompanyId_Code",
                schema: "asset",
                table: "FgsAssetStatus",
                columns: new[] { "TenantId", "CompanyId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetType_TenantId_CompanyId",
                schema: "asset",
                table: "FgsAssetType",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetType_TenantId_CompanyId_IsActive",
                schema: "asset",
                table: "FgsAssetType",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAssetType_TenantId_CompanyId_Name",
                schema: "asset",
                table: "FgsAssetType",
                columns: new[] { "TenantId", "CompanyId", "Name" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsAssetType_TenantId_CompanyId_Code",
                schema: "asset",
                table: "FgsAssetType",
                columns: new[] { "TenantId", "CompanyId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceLocationCache_TenantId_CompanyId_CustomerId",
                schema: "asset",
                table: "FgsServiceLocationCache",
                columns: new[] { "TenantId", "CompanyId", "CustomerId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceLocationCache_TenantId_CompanyId_LocationNumber",
                schema: "asset",
                table: "FgsServiceLocationCache",
                columns: new[] { "TenantId", "CompanyId", "LocationNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsTenantCompanyCache_CompanyName",
                schema: "asset",
                table: "FgsTenantCompanyCache",
                column: "CompanyName");

            migrationBuilder.CreateIndex(
                name: "IX_FgsTenantCompanyCache_IsActive",
                schema: "asset",
                table: "FgsTenantCompanyCache",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "UQ_FgsTenantCompanyCache_CompanyGuid",
                schema: "asset",
                table: "FgsTenantCompanyCache",
                column: "CompanyGuid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsAsset",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "FgsAssetModel",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "FgsAssetStatus",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "FgsServiceLocationCache",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "FgsAssetManufacturer",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "FgsAssetType",
                schema: "asset");

            migrationBuilder.DropTable(
                name: "FgsTenantCompanyCache",
                schema: "asset");
        }
    }
}
