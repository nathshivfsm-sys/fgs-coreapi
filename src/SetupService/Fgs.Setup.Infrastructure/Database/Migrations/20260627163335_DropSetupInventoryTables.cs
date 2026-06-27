using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class DropSetupInventoryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FgsVehicle_FgsWarehouse_WarehouseId",
                schema: "setup",
                table: "FgsVehicle");

            migrationBuilder.DropTable(
                name: "FgsInventoryItemAlternate",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsInventoryItemDependency",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsInventoryStock",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsVendorInventoryItem",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsWarehouse",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsInventoryItem",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsVendor",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsInventoryItemType",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsInventorySubCategory",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsInventoryCategory",
                schema: "setup");

            migrationBuilder.DropUniqueConstraint(
                name: "UQ_FgsVehicle_WarehouseId",
                schema: "setup",
                table: "FgsVehicle");

            migrationBuilder.DropIndex(
                name: "IX_FgsVehicle_TenantId_CompanyId_WarehouseId",
                schema: "setup",
                table: "FgsVehicle");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                schema: "setup",
                table: "FgsVehicle");

            migrationBuilder.AlterTable(
                name: "FgsVehicle",
                schema: "setup",
                comment: "Stores company-owned or leased vehicles used for field service operations. Each vehicle is associated with an inventory location that serves as a truck storage location.",
                oldComment: "Stores company-owned or leased vehicles used for field service operations. Each vehicle is associated with a truck warehouse that serves as an inventory location.");

            migrationBuilder.AddColumn<long>(
                name: "InventoryLocationId",
                schema: "setup",
                table: "FgsVehicle",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "Associated truck inventory location. References inventory.FgsInventoryLocation; scalar only — no cross-schema FK.");

            migrationBuilder.AddUniqueConstraint(
                name: "UQ_FgsVehicle_InventoryLocationId",
                schema: "setup",
                table: "FgsVehicle",
                column: "InventoryLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicle_TenantId_CompanyId_InventoryLocationId",
                schema: "setup",
                table: "FgsVehicle",
                columns: new[] { "TenantId", "CompanyId", "InventoryLocationId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "UQ_FgsVehicle_InventoryLocationId",
                schema: "setup",
                table: "FgsVehicle");

            migrationBuilder.DropIndex(
                name: "IX_FgsVehicle_TenantId_CompanyId_InventoryLocationId",
                schema: "setup",
                table: "FgsVehicle");

            migrationBuilder.DropColumn(
                name: "InventoryLocationId",
                schema: "setup",
                table: "FgsVehicle");

            migrationBuilder.AlterTable(
                name: "FgsVehicle",
                schema: "setup",
                comment: "Stores company-owned or leased vehicles used for field service operations. Each vehicle is associated with a truck warehouse that serves as an inventory location.",
                oldComment: "Stores company-owned or leased vehicles used for field service operations. Each vehicle is associated with an inventory location that serves as a truck storage location.");

            migrationBuilder.AddColumn<long>(
                name: "WarehouseId",
                schema: "setup",
                table: "FgsVehicle",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "Associated truck warehouse used as the vehicle inventory location.");

            migrationBuilder.AddUniqueConstraint(
                name: "UQ_FgsVehicle_WarehouseId",
                schema: "setup",
                table: "FgsVehicle",
                column: "WarehouseId");

            migrationBuilder.CreateTable(
                name: "FgsInventoryCategory",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CategoryCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventoryCategory", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventoryCategory_TenantId_CompanyId_CategoryCode", x => new { x.TenantId, x.CompanyId, x.CategoryCode });
                    table.ForeignKey(
                        name: "FK_FgsInventoryCategory_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsInventoryItemType",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ItemTypeCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TracksQuantity = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventoryItemType", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventoryItemType_TenantId_CompanyId_ItemTypeCode", x => new { x.TenantId, x.CompanyId, x.ItemTypeCode });
                    table.ForeignKey(
                        name: "FK_FgsInventoryItemType_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsVendor",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    InsurancePolicyNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Is1099Eligible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether vendor should be included in 1099 reporting."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    LegalName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    LicenseNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MobileNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    PaymentTermId = table.Column<long>(type: "bigint", nullable: true, comment: "References payment terms used for accounts payable due date calculation."),
                    PhoneNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TaxIdentificationNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    VendorCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    VendorType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Allowed values: VENDOR, SUBCONTRACTOR"),
                    Website = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsVendor", x => x.Id);
                    table.UniqueConstraint("UQ_FgsVendor_TenantId_CompanyId_VendorCode", x => new { x.TenantId, x.CompanyId, x.VendorCode });
                    table.CheckConstraint("CK_FgsVendor_VendorType", "\"VendorType\" IN ('VENDOR', 'SUBCONTRACTOR')");
                    table.ForeignKey(
                        name: "FK_FgsVendor_FgsSetupPaymentTerm",
                        column: x => x.PaymentTermId,
                        principalSchema: "setup",
                        principalTable: "FgsSetupPaymentTerm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsVendor_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores vendor and subcontractor master information for purchasing, AP, and subcontractor management.");

            migrationBuilder.CreateTable(
                name: "FgsWarehouse",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    AddressId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Optional reference to the warehouse address record."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the record."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Optional description or notes for the warehouse."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the warehouse is active and available for inventory operations."),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether this warehouse is the default inventory location for the company."),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Display name of the warehouse or inventory location."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last updated the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    WarehouseCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Unique warehouse code within the tenant and company scope."),
                    WarehouseType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, comment: "Type of inventory location. Allowed values: Warehouse, Truck, Trailer, JobSite, Consignment, Vendor.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsWarehouse", x => x.Id);
                    table.UniqueConstraint("UQ_FgsWarehouse_TenantId_CompanyId_WarehouseCode", x => new { x.TenantId, x.CompanyId, x.WarehouseCode });
                    table.CheckConstraint("CK_FgsWarehouse_WarehouseType", "\"WarehouseType\" IN ('Warehouse', 'Truck', 'Trailer', 'JobSite', 'Consignment', 'Vendor')");
                    table.ForeignKey(
                        name: "FK_FgsWarehouse_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores inventory warehouse, truck, trailer, job site, consignment, and vendor storage locations.");

            migrationBuilder.CreateTable(
                name: "FgsInventorySubCategory",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    InventoryCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    SubCategoryCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventorySubCategory", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventorySubCategory_TenantId_CompanyId_InventoryCategoryId_SubCategoryCode", x => new { x.TenantId, x.CompanyId, x.InventoryCategoryId, x.SubCategoryCode });
                    table.ForeignKey(
                        name: "FK_FgsInventorySubCategory_FgsInventoryCategory",
                        column: x => x.InventoryCategoryId,
                        principalSchema: "setup",
                        principalTable: "FgsInventoryCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventorySubCategory_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsInventoryItem",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    DefaultTaxable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    InventoryCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    InventoryItemTypeId = table.Column<long>(type: "bigint", nullable: false),
                    InventorySubCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    ItemCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ManufacturerPartNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PurchaseDescription = table.Column<string>(type: "text", nullable: true),
                    SalesDescription = table.Column<string>(type: "text", nullable: true),
                    SalesPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    TrackQuantity = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    UPCCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UnitOfMeasure = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventoryItem", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventoryItem_TenantId_CompanyId_ItemCode", x => new { x.TenantId, x.CompanyId, x.ItemCode });
                    table.ForeignKey(
                        name: "FK_FgsInventoryItem_FgsInventoryCategory",
                        column: x => x.InventoryCategoryId,
                        principalSchema: "setup",
                        principalTable: "FgsInventoryCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryItem_FgsInventoryItemType",
                        column: x => x.InventoryItemTypeId,
                        principalSchema: "setup",
                        principalTable: "FgsInventoryItemType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryItem_FgsInventorySubCategory",
                        column: x => x.InventorySubCategoryId,
                        principalSchema: "setup",
                        principalTable: "FgsInventorySubCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryItem_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Inventory item master record for purchasing, sales, and stock tracking.");

            migrationBuilder.CreateTable(
                name: "FgsInventoryItemAlternate",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AlternateInventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    AlternateType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    PriorityOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventoryItemAlternate", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventoryItemAlternate_TenantId_CompanyId_InventoryItemId_AlternateInventoryItemId", x => new { x.TenantId, x.CompanyId, x.InventoryItemId, x.AlternateInventoryItemId });
                    table.ForeignKey(
                        name: "FK_FgsInventoryItemAlternate_FgsInventoryItem_AlternateInventoryItemId",
                        column: x => x.AlternateInventoryItemId,
                        principalSchema: "setup",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryItemAlternate_FgsInventoryItem_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalSchema: "setup",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryItemAlternate_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsInventoryItemDependency",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    DependencyType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DependentInventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Quantity = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 1m),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventoryItemDependency", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventoryItemDependency_TenantId_CompanyId_InventoryItemId_DependentInventoryItemId", x => new { x.TenantId, x.CompanyId, x.InventoryItemId, x.DependentInventoryItemId });
                    table.ForeignKey(
                        name: "FK_FgsInventoryItemDependency_FgsInventoryItem_DependentInventoryItemId",
                        column: x => x.DependentInventoryItemId,
                        principalSchema: "setup",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryItemDependency_FgsInventoryItem_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalSchema: "setup",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryItemDependency_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsInventoryStock",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AverageCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    LastCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    LastPurchaseDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    LastSoldDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    QuantityAvailable = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 0m),
                    QuantityCommitted = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 0m),
                    QuantityOnHand = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 0m),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventoryStock", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventoryStock_TenantId_CompanyId_InventoryItemId", x => new { x.TenantId, x.CompanyId, x.InventoryItemId });
                    table.ForeignKey(
                        name: "FK_FgsInventoryStock_FgsInventoryItem",
                        column: x => x.InventoryItemId,
                        principalSchema: "setup",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryStock_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsVendorInventoryItem",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsPreferredVendor = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether this vendor is the preferred vendor for the inventory item."),
                    LastCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Last received cost from the vendor based on purchase order receiving."),
                    LastReceivedDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Last date inventory was received from the vendor."),
                    PurchaseOrderComments = table.Column<string>(type: "text", nullable: true, comment: "Comments automatically copied to purchase orders for this vendor item combination."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    VendorId = table.Column<long>(type: "bigint", nullable: false),
                    VendorPartName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    VendorPartNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Vendor-specific part number for the inventory item.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsVendorInventoryItem", x => x.Id);
                    table.UniqueConstraint("UQ_FgsVendorInventoryItem_TenantId_CompanyId_VendorId_InventoryItemId", x => new { x.TenantId, x.CompanyId, x.VendorId, x.InventoryItemId });
                    table.ForeignKey(
                        name: "FK_FgsVendorInventoryItem_FgsInventoryItem",
                        column: x => x.InventoryItemId,
                        principalSchema: "setup",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsVendorInventoryItem_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsVendorInventoryItem_FgsVendor",
                        column: x => x.VendorId,
                        principalSchema: "setup",
                        principalTable: "FgsVendor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores vendor-specific inventory item relationships, vendor part information, pricing, and purchasing defaults.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicle_TenantId_CompanyId_WarehouseId",
                schema: "setup",
                table: "FgsVehicle",
                columns: new[] { "TenantId", "CompanyId", "WarehouseId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryCategory_TenantId_CompanyId",
                schema: "setup",
                table: "FgsInventoryCategory",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_InventoryCategoryId",
                schema: "setup",
                table: "FgsInventoryItem",
                column: "InventoryCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_InventoryItemTypeId",
                schema: "setup",
                table: "FgsInventoryItem",
                column: "InventoryItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_InventorySubCategoryId",
                schema: "setup",
                table: "FgsInventoryItem",
                column: "InventorySubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_TenantId_CompanyId_InventoryCategoryId",
                schema: "setup",
                table: "FgsInventoryItem",
                columns: new[] { "TenantId", "CompanyId", "InventoryCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_TenantId_CompanyId_InventoryCategoryId_InventorySubCategoryId",
                schema: "setup",
                table: "FgsInventoryItem",
                columns: new[] { "TenantId", "CompanyId", "InventoryCategoryId", "InventorySubCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_TenantId_CompanyId_InventoryItemTypeId",
                schema: "setup",
                table: "FgsInventoryItem",
                columns: new[] { "TenantId", "CompanyId", "InventoryItemTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_TenantId_CompanyId_Name",
                schema: "setup",
                table: "FgsInventoryItem",
                columns: new[] { "TenantId", "CompanyId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemAlternate_AlternateInventoryItemId",
                schema: "setup",
                table: "FgsInventoryItemAlternate",
                column: "AlternateInventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemAlternate_InventoryItemId",
                schema: "setup",
                table: "FgsInventoryItemAlternate",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemAlternate_TenantId_CompanyId_InventoryItemId",
                schema: "setup",
                table: "FgsInventoryItemAlternate",
                columns: new[] { "TenantId", "CompanyId", "InventoryItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemDependency_DependentInventoryItemId",
                schema: "setup",
                table: "FgsInventoryItemDependency",
                column: "DependentInventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemDependency_InventoryItemId",
                schema: "setup",
                table: "FgsInventoryItemDependency",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemDependency_TenantId_CompanyId_InventoryItemId",
                schema: "setup",
                table: "FgsInventoryItemDependency",
                columns: new[] { "TenantId", "CompanyId", "InventoryItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemType_TenantId_CompanyId",
                schema: "setup",
                table: "FgsInventoryItemType",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryStock_InventoryItemId",
                schema: "setup",
                table: "FgsInventoryStock",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventorySubCategory_InventoryCategoryId",
                schema: "setup",
                table: "FgsInventorySubCategory",
                column: "InventoryCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventorySubCategory_TenantId_CompanyId",
                schema: "setup",
                table: "FgsInventorySubCategory",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventorySubCategory_TenantId_CompanyId_InventoryCategoryId",
                schema: "setup",
                table: "FgsInventorySubCategory",
                columns: new[] { "TenantId", "CompanyId", "InventoryCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendor_PaymentTermId",
                schema: "setup",
                table: "FgsVendor",
                column: "PaymentTermId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendor_TenantId_CompanyId",
                schema: "setup",
                table: "FgsVendor",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendor_TenantId_CompanyId_Name",
                schema: "setup",
                table: "FgsVendor",
                columns: new[] { "TenantId", "CompanyId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendor_TenantId_CompanyId_VendorType",
                schema: "setup",
                table: "FgsVendor",
                columns: new[] { "TenantId", "CompanyId", "VendorType" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendorInventoryItem_InventoryItemId",
                schema: "setup",
                table: "FgsVendorInventoryItem",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendorInventoryItem_TenantId_CompanyId_InventoryItemId",
                schema: "setup",
                table: "FgsVendorInventoryItem",
                columns: new[] { "TenantId", "CompanyId", "InventoryItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendorInventoryItem_TenantId_CompanyId_VendorId",
                schema: "setup",
                table: "FgsVendorInventoryItem",
                columns: new[] { "TenantId", "CompanyId", "VendorId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendorInventoryItem_VendorId",
                schema: "setup",
                table: "FgsVendorInventoryItem",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsWarehouse_TenantId_CompanyId_IsActive",
                schema: "setup",
                table: "FgsWarehouse",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsWarehouse_TenantId_CompanyId_WarehouseType",
                schema: "setup",
                table: "FgsWarehouse",
                columns: new[] { "TenantId", "CompanyId", "WarehouseType" });

            migrationBuilder.AddForeignKey(
                name: "FK_FgsVehicle_FgsWarehouse_WarehouseId",
                schema: "setup",
                table: "FgsVehicle",
                column: "WarehouseId",
                principalSchema: "setup",
                principalTable: "FgsWarehouse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
