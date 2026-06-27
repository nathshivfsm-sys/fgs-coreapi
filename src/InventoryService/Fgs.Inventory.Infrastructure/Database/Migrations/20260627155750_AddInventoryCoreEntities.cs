using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Inventory.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddInventoryCoreEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsInventoryCategory",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    CategoryCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Unique category code within a company."),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false, comment: "Category display name."),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Optional description."),
                    TextColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, comment: "UI text color."),
                    BackgroundColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, comment: "UI background color."),
                    DisplayIconFileId = table.Column<long>(type: "bigint", nullable: true, comment: "Display icon stored in FgsFile."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Display order."),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Seeded system record."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Created timestamp."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Created by user."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Updated timestamp."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Updated by user."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Active flag.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventoryCategory", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventoryCategory_TenantId_CompanyId_CategoryCode", x => new { x.TenantId, x.CompanyId, x.CategoryCode });
                    table.ForeignKey(
                        name: "FK_FgsInventoryCategory_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "inventory",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores the first level of inventory classification.");

            migrationBuilder.CreateTable(
                name: "FgsInventoryItemType",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier for the inventory item type.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant that owns this record."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company that owns this record."),
                    ItemTypeCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, comment: "Unique code for the inventory item type within a company."),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Display name of the inventory item type."),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Optional description of the inventory item type."),
                    TracksQuantity = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether inventory quantities are maintained for this item type."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls display order in user interfaces."),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether this is a system-defined record."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last updated the record."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the record is active.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventoryItemType", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventoryItemType_TenantId_CompanyId_ItemTypeCode", x => new { x.TenantId, x.CompanyId, x.ItemTypeCode });
                    table.ForeignKey(
                        name: "FK_FgsInventoryItemType_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "inventory",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores inventory item types used to classify inventory items and determine whether quantity is tracked.");

            migrationBuilder.CreateTable(
                name: "FgsInventoryLocation",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    InventoryLocationCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Unique inventory location code."),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Display name."),
                    InventoryLocationType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, comment: "WAREHOUSE, TRUCK, TRAILER, JOBSITE, CONSIGNMENT or VENDOR."),
                    ParentInventoryLocationId = table.Column<long>(type: "bigint", nullable: true, comment: "Optional parent inventory location."),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Description or notes."),
                    Address1 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Address line 1."),
                    Address2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Address line 2."),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "City."),
                    StateProvince = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "State or province."),
                    PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, comment: "Postal code."),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Country."),
                    ContactName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true, comment: "Primary contact."),
                    PhoneNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Contact phone."),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Contact email."),
                    TextColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, comment: "UI text color."),
                    BackgroundColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, comment: "UI background color."),
                    DisplayIconFileId = table.Column<long>(type: "bigint", nullable: true, comment: "Display icon file identifier."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Display order."),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Default inventory location."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Active flag.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventoryLocation", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventoryLocation_TenantId_CompanyId_InventoryLocationCode", x => new { x.TenantId, x.CompanyId, x.InventoryLocationCode });
                    table.CheckConstraint("CK_FgsInventoryLocation_InventoryLocationType", "\"InventoryLocationType\" IN ('WAREHOUSE', 'TRUCK', 'TRAILER', 'JOBSITE', 'CONSIGNMENT', 'VENDOR')");
                    table.ForeignKey(
                        name: "FK_FgsInventoryLocation_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "inventory",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryLocation_ParentInventoryLocation",
                        column: x => x.ParentInventoryLocationId,
                        principalSchema: "inventory",
                        principalTable: "FgsInventoryLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores all inventory locations including warehouses, trucks, trailers, job sites, vendor locations and consignment locations.");

            migrationBuilder.CreateTable(
                name: "FgsVendor",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    VendorCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    LegalName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    VendorType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    VendorStatus = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, defaultValue: "ACTIVE"),
                    VendorAccountNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PaymentTermId = table.Column<long>(type: "bigint", nullable: true, comment: "References setup payment terms; scalar only — no cross-schema FK."),
                    ContactName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    ContactTitle = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    PurchaseOrderEmail = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    MobileNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    FaxNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Website = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Address1 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Address2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    StateProvince = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TaxIdNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LicenseNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    InsurancePolicyNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Is1099Eligible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsVendor", x => x.Id);
                    table.UniqueConstraint("UQ_FgsVendor_TenantId_CompanyId_VendorCode", x => new { x.TenantId, x.CompanyId, x.VendorCode });
                    table.CheckConstraint("CK_FgsVendor_VendorStatus", "\"VendorStatus\" IN ('ACTIVE', 'INACTIVE', 'ON_HOLD')");
                    table.CheckConstraint("CK_FgsVendor_VendorType", "\"VendorType\" IN ('VENDOR', 'SUBCONTRACTOR')");
                    table.ForeignKey(
                        name: "FK_FgsVendor_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "inventory",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores vendor and subcontractor master information for purchasing, AP, and subcontractor management.");

            migrationBuilder.CreateTable(
                name: "FgsInventorySubCategory",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    InventoryCategoryId = table.Column<long>(type: "bigint", nullable: false, comment: "Parent inventory category."),
                    SubCategoryCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Unique code within a category."),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false, comment: "Display name."),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Description."),
                    TextColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, comment: "UI text color."),
                    BackgroundColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, comment: "UI background color."),
                    DisplayIconFileId = table.Column<long>(type: "bigint", nullable: true, comment: "Icon file identifier."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Display order."),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "System record."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Created date/time."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Created by."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Updated date/time."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Updated by."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Active flag.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventorySubCategory", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventorySubCategory_TenantId_CompanyId_InventoryCategoryId_SubCategoryCode", x => new { x.TenantId, x.CompanyId, x.InventoryCategoryId, x.SubCategoryCode });
                    table.ForeignKey(
                        name: "FK_FgsInventorySubCategory_FgsInventoryCategory",
                        column: x => x.InventoryCategoryId,
                        principalSchema: "inventory",
                        principalTable: "FgsInventoryCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventorySubCategory_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "inventory",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores inventory sub-categories used to classify inventory items under a parent category.");

            migrationBuilder.CreateTable(
                name: "FgsPurchaseOrder",
                schema: "inventory",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PurchaseOrderNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "User-visible purchase order number."),
                    VendorId = table.Column<long>(type: "bigint", nullable: false),
                    PurchaseOrderStatus = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, defaultValue: "OPEN", comment: "OPEN, PARTIAL, RECEIVED, CLOSED or CANCELLED."),
                    PurchaseOrderDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    ExpectedDeliveryDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    RequestedByEmployeeId = table.Column<long>(type: "bigint", nullable: true, comment: "Employee requesting the purchase."),
                    RequestedByName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true, comment: "Snapshot of the requester name when the purchase order was created."),
                    BuyerEmployeeId = table.Column<long>(type: "bigint", nullable: true, comment: "Employee responsible for purchasing and vendor follow-up."),
                    ShipToInventoryLocationId = table.Column<long>(type: "bigint", nullable: true, comment: "Inventory location receiving the shipment."),
                    ShipToServiceLocationId = table.Column<long>(type: "bigint", nullable: true, comment: "Service location or job site receiving the shipment."),
                    ShipToName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ShipToAddress1 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ShipToAddress2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ShipToCity = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ShipToStateProvince = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ShipToPostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ShipToCountry = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    VendorReferenceNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    VendorContactName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    VendorEmail = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    VendorPhoneNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Subtotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    DiscountAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    TaxableAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    PurchaseTaxJson = table.Column<string>(type: "jsonb", nullable: true, comment: "JSON tax breakdown supporting multiple tax jurisdictions such as GST, PST, HST, VAT and Sales Tax."),
                    FreightAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    OtherCharges = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    VendorNotes = table.Column<string>(type: "text", nullable: true, comment: "Notes printed on the purchase order for the vendor."),
                    InternalNotes = table.Column<string>(type: "text", nullable: true, comment: "Internal office notes not printed on the purchase order."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsPurchaseOrder", x => x.Id);
                    table.UniqueConstraint("UQ_FgsPurchaseOrder_TenantId_CompanyId_PurchaseOrderNumber", x => new { x.TenantId, x.CompanyId, x.PurchaseOrderNumber });
                    table.CheckConstraint("CK_FgsPurchaseOrder_PurchaseOrderStatus", "\"PurchaseOrderStatus\" IN ('OPEN', 'PARTIAL', 'RECEIVED', 'CLOSED', 'CANCELLED')");
                    table.ForeignKey(
                        name: "FK_FgsPurchaseOrder_FgsInventoryLocation",
                        column: x => x.ShipToInventoryLocationId,
                        principalSchema: "inventory",
                        principalTable: "FgsInventoryLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsPurchaseOrder_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "inventory",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsPurchaseOrder_FgsVendor",
                        column: x => x.VendorId,
                        principalSchema: "inventory",
                        principalTable: "FgsVendor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores purchase order header information including vendor, shipping destination, tax summary and purchasing details.");

            migrationBuilder.CreateTable(
                name: "FgsInventoryItem",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    InventoryItemTypeId = table.Column<long>(type: "bigint", nullable: false),
                    InventoryCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    InventorySubCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    ItemCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    PurchaseDescription = table.Column<string>(type: "text", nullable: true),
                    SalesDescription = table.Column<string>(type: "text", nullable: true),
                    ManufacturerPartNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ManufacturerName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Sku = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UPCCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UnitOfMeasure = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TracksInventory = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    UnitCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    StandardUnitCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    SalesPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    DefaultTaxable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventoryItem", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventoryItem_TenantId_CompanyId_ItemCode", x => new { x.TenantId, x.CompanyId, x.ItemCode });
                    table.ForeignKey(
                        name: "FK_FgsInventoryItem_FgsInventoryCategory",
                        column: x => x.InventoryCategoryId,
                        principalSchema: "inventory",
                        principalTable: "FgsInventoryCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryItem_FgsInventoryItemType",
                        column: x => x.InventoryItemTypeId,
                        principalSchema: "inventory",
                        principalTable: "FgsInventoryItemType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryItem_FgsInventorySubCategory",
                        column: x => x.InventorySubCategoryId,
                        principalSchema: "inventory",
                        principalTable: "FgsInventorySubCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryItem_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "inventory",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Inventory item master record for purchasing, sales, and stock tracking.");

            migrationBuilder.CreateTable(
                name: "FgsInventoryItemAlternate",
                schema: "inventory",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    AlternateInventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    PriorityOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventoryItemAlternate", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventoryItemAlternate_TenantId_CompanyId_InventoryItemId_AlternateInventoryItemId", x => new { x.TenantId, x.CompanyId, x.InventoryItemId, x.AlternateInventoryItemId });
                    table.ForeignKey(
                        name: "FK_FgsInventoryItemAlternate_FgsInventoryItem_AlternateInventoryItemId",
                        column: x => x.AlternateInventoryItemId,
                        principalSchema: "inventory",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryItemAlternate_FgsInventoryItem_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalSchema: "inventory",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryItemAlternate_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "inventory",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsInventoryItemDependency",
                schema: "inventory",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    DependentInventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 1m),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventoryItemDependency", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventoryItemDependency_TenantId_CompanyId_InventoryItemId_DependentInventoryItemId", x => new { x.TenantId, x.CompanyId, x.InventoryItemId, x.DependentInventoryItemId });
                    table.ForeignKey(
                        name: "FK_FgsInventoryItemDependency_FgsInventoryItem_DependentInventoryItemId",
                        column: x => x.DependentInventoryItemId,
                        principalSchema: "inventory",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryItemDependency_FgsInventoryItem_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalSchema: "inventory",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryItemDependency_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "inventory",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsInventoryStock",
                schema: "inventory",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    QuantityOnHand = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 0m),
                    QuantityCommitted = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 0m),
                    QuantityAvailable = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 0m),
                    AverageCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    LastCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    LastPurchaseDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    LastSoldDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventoryStock", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventoryStock_TenantId_CompanyId_InventoryItemId", x => new { x.TenantId, x.CompanyId, x.InventoryItemId });
                    table.ForeignKey(
                        name: "FK_FgsInventoryStock_FgsInventoryItem",
                        column: x => x.InventoryItemId,
                        principalSchema: "inventory",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryStock_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "inventory",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsInventoryTransaction",
                schema: "inventory",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TransactionNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    TransactionType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    FromInventoryLocationId = table.Column<long>(type: "bigint", nullable: true),
                    ToInventoryLocationId = table.Column<long>(type: "bigint", nullable: true),
                    UnitCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    TransactionDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    ReferenceType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    ReferenceId = table.Column<long>(type: "bigint", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventoryTransaction", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventoryTransaction_TenantId_CompanyId_TransactionNumber", x => new { x.TenantId, x.CompanyId, x.TransactionNumber });
                    table.CheckConstraint("CK_FgsInventoryTransaction_TransactionType", "\"TransactionType\" IN ('INITIAL', 'PURCHASE_RECEIPT', 'TRANSFER', 'USAGE', 'ADJUSTMENT', 'RETURN', 'PHYSICAL_COUNT')");
                    table.ForeignKey(
                        name: "FK_FgsInventoryTransaction_FgsInventoryItem",
                        column: x => x.InventoryItemId,
                        principalSchema: "inventory",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryTransaction_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "inventory",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryTransaction_FromInventoryLocation",
                        column: x => x.FromInventoryLocationId,
                        principalSchema: "inventory",
                        principalTable: "FgsInventoryLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryTransaction_ToInventoryLocation",
                        column: x => x.ToInventoryLocationId,
                        principalSchema: "inventory",
                        principalTable: "FgsInventoryLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores an immutable audit log of every inventory movement between inventory locations or into and out of inventory.");

            migrationBuilder.CreateTable(
                name: "FgsPurchaseOrderDetail",
                schema: "inventory",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PurchaseOrderId = table.Column<long>(type: "bigint", nullable: false),
                    LineNumber = table.Column<short>(type: "smallint", nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    VendorPartNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ItemDescription = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "Description printed on the purchase order."),
                    UnitOfMeasureCode = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    OrderedQuantity = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 0m),
                    ReceivedQuantity = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 0m),
                    UnitCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Unit cost at the time the purchase order was created."),
                    DiscountAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    IsTaxable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    ExtendedAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Extended amount calculated from quantity, unit cost and discount before document-level taxes and freight."),
                    ExpectedDeliveryDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsPurchaseOrderDetail", x => x.Id);
                    table.UniqueConstraint("UQ_FgsPurchaseOrderDetail_TenantId_CompanyId_PurchaseOrderId_LineNumber", x => new { x.TenantId, x.CompanyId, x.PurchaseOrderId, x.LineNumber });
                    table.ForeignKey(
                        name: "FK_FgsPurchaseOrderDetail_FgsInventoryItem",
                        column: x => x.ItemId,
                        principalSchema: "inventory",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsPurchaseOrderDetail_FgsPurchaseOrder",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "inventory",
                        principalTable: "FgsPurchaseOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsPurchaseOrderDetail_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "inventory",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores purchase order line items for inventory purchased from vendors.");

            migrationBuilder.CreateTable(
                name: "FgsVendorInventoryItem",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    VendorId = table.Column<long>(type: "bigint", nullable: false),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    VendorPartNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Vendor's part number used when purchasing this inventory item."),
                    VendorPartName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Vendor's description of the inventory item as it appears on catalogs or purchase orders."),
                    LastCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Most recent purchase cost from this vendor for the inventory item."),
                    LastReceivedDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date the inventory item was last received from this vendor."),
                    PurchaseOrderComments = table.Column<string>(type: "text", nullable: true, comment: "Vendor-specific notes automatically included or displayed during purchase order creation for this inventory item."),
                    VendorPriority = table.Column<short>(type: "smallint", nullable: false, comment: "Specifies the purchasing priority for this vendor and inventory item combination. Lower numbers indicate higher priority."),
                    LeadTimeDays = table.Column<short>(type: "smallint", nullable: true, comment: "Expected number of days required for the vendor to deliver the inventory item after the purchase order is placed."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsVendorInventoryItem", x => x.Id);
                    table.UniqueConstraint("UQ_FgsVendorInventoryItem_TenantId_CompanyId_VendorId_InventoryItemId", x => new { x.TenantId, x.CompanyId, x.VendorId, x.InventoryItemId });
                    table.ForeignKey(
                        name: "FK_FgsVendorInventoryItem_FgsInventoryItem",
                        column: x => x.InventoryItemId,
                        principalSchema: "inventory",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsVendorInventoryItem_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "inventory",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsVendorInventoryItem_FgsVendor",
                        column: x => x.VendorId,
                        principalSchema: "inventory",
                        principalTable: "FgsVendor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores vendor-specific purchasing information for inventory items, including vendor part numbers, descriptions, pricing, purchasing priority, lead times, and other information used during purchase order creation and inventory replenishment.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryCategory_TenantId_CompanyId",
                schema: "inventory",
                table: "FgsInventoryCategory",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryCategory_TenantId_CompanyId_IsActive",
                schema: "inventory",
                table: "FgsInventoryCategory",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryCategory_TenantId_CompanyId_Name",
                schema: "inventory",
                table: "FgsInventoryCategory",
                columns: new[] { "TenantId", "CompanyId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_InventoryCategoryId",
                schema: "inventory",
                table: "FgsInventoryItem",
                columns: new[] { "TenantId", "CompanyId", "InventoryCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_InventoryCategoryId1",
                schema: "inventory",
                table: "FgsInventoryItem",
                column: "InventoryCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_InventoryItemTypeId",
                schema: "inventory",
                table: "FgsInventoryItem",
                column: "InventoryItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_InventorySubCategoryId",
                schema: "inventory",
                table: "FgsInventoryItem",
                columns: new[] { "TenantId", "CompanyId", "InventoryItemTypeId", "InventorySubCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_InventorySubCategoryId1",
                schema: "inventory",
                table: "FgsInventoryItem",
                column: "InventorySubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_TenantId_CompanyId_InventoryItemTypeId",
                schema: "inventory",
                table: "FgsInventoryItem",
                columns: new[] { "TenantId", "CompanyId", "InventoryItemTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_TenantId_CompanyId_Name",
                schema: "inventory",
                table: "FgsInventoryItem",
                columns: new[] { "TenantId", "CompanyId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemAlternate_AlternateInventoryItemId",
                schema: "inventory",
                table: "FgsInventoryItemAlternate",
                column: "AlternateInventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemAlternate_InventoryItemId",
                schema: "inventory",
                table: "FgsInventoryItemAlternate",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemAlternate_TenantId_CompanyId_InventoryItemId",
                schema: "inventory",
                table: "FgsInventoryItemAlternate",
                columns: new[] { "TenantId", "CompanyId", "InventoryItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemDependency_DependentInventoryItemId",
                schema: "inventory",
                table: "FgsInventoryItemDependency",
                column: "DependentInventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemDependency_InventoryItemId",
                schema: "inventory",
                table: "FgsInventoryItemDependency",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemDependency_TenantId_CompanyId_InventoryItemId",
                schema: "inventory",
                table: "FgsInventoryItemDependency",
                columns: new[] { "TenantId", "CompanyId", "InventoryItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemType_TenantId_CompanyId",
                schema: "inventory",
                table: "FgsInventoryItemType",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemType_TenantId_CompanyId_IsActive",
                schema: "inventory",
                table: "FgsInventoryItemType",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemType_TenantId_CompanyId_Name",
                schema: "inventory",
                table: "FgsInventoryItemType",
                columns: new[] { "TenantId", "CompanyId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryLocation_ParentInventoryLocationId",
                schema: "inventory",
                table: "FgsInventoryLocation",
                column: "ParentInventoryLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryLocation_TenantId_CompanyId",
                schema: "inventory",
                table: "FgsInventoryLocation",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryLocation_TenantId_CompanyId_InventoryLocationType",
                schema: "inventory",
                table: "FgsInventoryLocation",
                columns: new[] { "TenantId", "CompanyId", "InventoryLocationType" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryLocation_TenantId_CompanyId_Name",
                schema: "inventory",
                table: "FgsInventoryLocation",
                columns: new[] { "TenantId", "CompanyId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryStock_InventoryItemId",
                schema: "inventory",
                table: "FgsInventoryStock",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryStock_TenantId_CompanyId",
                schema: "inventory",
                table: "FgsInventoryStock",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryStock_TenantId_CompanyId_InventoryItemId",
                schema: "inventory",
                table: "FgsInventoryStock",
                columns: new[] { "TenantId", "CompanyId", "InventoryItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryStock_TenantId_CompanyId_LastPurchaseDate",
                schema: "inventory",
                table: "FgsInventoryStock",
                columns: new[] { "TenantId", "CompanyId", "LastPurchaseDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryStock_TenantId_CompanyId_LastSoldDate",
                schema: "inventory",
                table: "FgsInventoryStock",
                columns: new[] { "TenantId", "CompanyId", "LastSoldDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventorySubCategory_InventoryCategoryId",
                schema: "inventory",
                table: "FgsInventorySubCategory",
                column: "InventoryCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventorySubCategory_TenantId_CompanyId",
                schema: "inventory",
                table: "FgsInventorySubCategory",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventorySubCategory_TenantId_CompanyId_InventoryCategoryId",
                schema: "inventory",
                table: "FgsInventorySubCategory",
                columns: new[] { "TenantId", "CompanyId", "InventoryCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventorySubCategory_TenantId_CompanyId_IsActive",
                schema: "inventory",
                table: "FgsInventorySubCategory",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventorySubCategory_TenantId_CompanyId_Name",
                schema: "inventory",
                table: "FgsInventorySubCategory",
                columns: new[] { "TenantId", "CompanyId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryTransaction_FromInventoryLocationId",
                schema: "inventory",
                table: "FgsInventoryTransaction",
                column: "FromInventoryLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryTransaction_InventoryItemId",
                schema: "inventory",
                table: "FgsInventoryTransaction",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryTransaction_TenantId_CompanyId",
                schema: "inventory",
                table: "FgsInventoryTransaction",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryTransaction_TenantId_CompanyId_InventoryItemId",
                schema: "inventory",
                table: "FgsInventoryTransaction",
                columns: new[] { "TenantId", "CompanyId", "InventoryItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryTransaction_TenantId_CompanyId_ReferenceType_ReferenceId",
                schema: "inventory",
                table: "FgsInventoryTransaction",
                columns: new[] { "TenantId", "CompanyId", "ReferenceType", "ReferenceId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryTransaction_TenantId_CompanyId_TransactionDate",
                schema: "inventory",
                table: "FgsInventoryTransaction",
                columns: new[] { "TenantId", "CompanyId", "TransactionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryTransaction_TenantId_CompanyId_TransactionType",
                schema: "inventory",
                table: "FgsInventoryTransaction",
                columns: new[] { "TenantId", "CompanyId", "TransactionType" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryTransaction_ToInventoryLocationId",
                schema: "inventory",
                table: "FgsInventoryTransaction",
                column: "ToInventoryLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsPurchaseOrder_ShipToInventoryLocationId",
                schema: "inventory",
                table: "FgsPurchaseOrder",
                column: "ShipToInventoryLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsPurchaseOrder_TenantId_CompanyId",
                schema: "inventory",
                table: "FgsPurchaseOrder",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPurchaseOrder_TenantId_CompanyId_BuyerEmployeeId",
                schema: "inventory",
                table: "FgsPurchaseOrder",
                columns: new[] { "TenantId", "CompanyId", "BuyerEmployeeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPurchaseOrder_TenantId_CompanyId_PurchaseOrderDate",
                schema: "inventory",
                table: "FgsPurchaseOrder",
                columns: new[] { "TenantId", "CompanyId", "PurchaseOrderDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPurchaseOrder_TenantId_CompanyId_PurchaseOrderStatus",
                schema: "inventory",
                table: "FgsPurchaseOrder",
                columns: new[] { "TenantId", "CompanyId", "PurchaseOrderStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPurchaseOrder_TenantId_CompanyId_VendorId",
                schema: "inventory",
                table: "FgsPurchaseOrder",
                columns: new[] { "TenantId", "CompanyId", "VendorId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPurchaseOrder_VendorId",
                schema: "inventory",
                table: "FgsPurchaseOrder",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsPurchaseOrderDetail_ItemId",
                schema: "inventory",
                table: "FgsPurchaseOrderDetail",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsPurchaseOrderDetail_PurchaseOrderId",
                schema: "inventory",
                table: "FgsPurchaseOrderDetail",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsPurchaseOrderDetail_TenantId_CompanyId",
                schema: "inventory",
                table: "FgsPurchaseOrderDetail",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPurchaseOrderDetail_TenantId_CompanyId_ItemId",
                schema: "inventory",
                table: "FgsPurchaseOrderDetail",
                columns: new[] { "TenantId", "CompanyId", "ItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPurchaseOrderDetail_TenantId_CompanyId_PurchaseOrderId",
                schema: "inventory",
                table: "FgsPurchaseOrderDetail",
                columns: new[] { "TenantId", "CompanyId", "PurchaseOrderId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendor_TenantId_CompanyId",
                schema: "inventory",
                table: "FgsVendor",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendor_TenantId_CompanyId_Name",
                schema: "inventory",
                table: "FgsVendor",
                columns: new[] { "TenantId", "CompanyId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendor_TenantId_CompanyId_PhoneNumber",
                schema: "inventory",
                table: "FgsVendor",
                columns: new[] { "TenantId", "CompanyId", "PhoneNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendor_TenantId_CompanyId_VendorContactName",
                schema: "inventory",
                table: "FgsVendor",
                columns: new[] { "TenantId", "CompanyId", "ContactName" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendorInventoryItem_InventoryItemId",
                schema: "inventory",
                table: "FgsVendorInventoryItem",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendorInventoryItem_TenantId_CompanyId_InventoryItemId",
                schema: "inventory",
                table: "FgsVendorInventoryItem",
                columns: new[] { "TenantId", "CompanyId", "InventoryItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendorInventoryItem_TenantId_CompanyId_VendorId",
                schema: "inventory",
                table: "FgsVendorInventoryItem",
                columns: new[] { "TenantId", "CompanyId", "VendorId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendorInventoryItem_VendorId",
                schema: "inventory",
                table: "FgsVendorInventoryItem",
                column: "VendorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsInventoryItemAlternate",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "FgsInventoryItemDependency",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "FgsInventoryStock",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "FgsInventoryTransaction",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "FgsPurchaseOrderDetail",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "FgsVendorInventoryItem",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "FgsPurchaseOrder",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "FgsInventoryItem",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "FgsInventoryLocation",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "FgsVendor",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "FgsInventoryItemType",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "FgsInventorySubCategory",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "FgsInventoryCategory",
                schema: "inventory");
        }
    }
}
