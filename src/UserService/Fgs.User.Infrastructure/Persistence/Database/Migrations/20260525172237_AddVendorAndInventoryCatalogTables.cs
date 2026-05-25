using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddVendorAndInventoryCatalogTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsInventoryCategory",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CategoryCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventoryCategory", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventoryCategory_TenantId_CompanyId_CategoryCode", x => new { x.TenantId, x.CompanyId, x.CategoryCode });
                    table.ForeignKey(
                        name: "FK_FgsInventoryCategory_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsInventoryItemType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    ItemTypeCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    TracksQuantity = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventoryItemType", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventoryItemType_TenantId_CompanyId_ItemTypeCode", x => new { x.TenantId, x.CompanyId, x.ItemTypeCode });
                    table.ForeignKey(
                        name: "FK_FgsInventoryItemType_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsVendor",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    VendorCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    LegalName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    VendorType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Allowed values: VENDOR, SUBCONTRACTOR"),
                    PaymentTermId = table.Column<long>(type: "bigint", nullable: true, comment: "References payment terms used for accounts payable due date calculation."),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    MobileNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Website = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    TaxIdentificationNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LicenseNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    InsurancePolicyNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Is1099Eligible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether vendor should be included in 1099 reporting."),
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
                    table.CheckConstraint("CK_FgsVendor_VendorType", "\"VendorType\" IN ('VENDOR', 'SUBCONTRACTOR')");
                    table.ForeignKey(
                        name: "FK_FgsVendor_FgsSetupPaymentTerm",
                        column: x => x.PaymentTermId,
                        principalSchema: "dbo",
                        principalTable: "FgsSetupPaymentTerm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsVendor_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores vendor and subcontractor master information for purchasing, AP, and subcontractor management.");

            migrationBuilder.CreateTable(
                name: "GloInventoryCategory",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BusinessTypeId = table.Column<int>(type: "integer", nullable: false),
                    CategoryCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloInventoryCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GloInventoryCategory_GloBusinessType",
                        column: x => x.BusinessTypeId,
                        principalSchema: "dbo",
                        principalTable: "GloBusinessType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GloInventoryItemType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemTypeCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    TracksQuantity = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloInventoryItemType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsInventorySubCategory",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    InventoryCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    SubCategoryCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventorySubCategory", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventorySubCategory_TenantId_CompanyId_InventoryCategoryId_SubCategoryCode", x => new { x.TenantId, x.CompanyId, x.InventoryCategoryId, x.SubCategoryCode });
                    table.ForeignKey(
                        name: "FK_FgsInventorySubCategory_FgsInventoryCategory",
                        column: x => x.InventoryCategoryId,
                        principalSchema: "dbo",
                        principalTable: "FgsInventoryCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventorySubCategory_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GloInventorySubCategory",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InventoryCategoryId = table.Column<int>(type: "integer", nullable: false),
                    SubCategoryCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloInventorySubCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GloInventorySubCategory_GloInventoryCategory",
                        column: x => x.InventoryCategoryId,
                        principalSchema: "dbo",
                        principalTable: "GloInventoryCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsInventoryItem",
                schema: "dbo",
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
                    UPCCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UnitOfMeasure = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TrackQuantity = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Cost = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
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
                        principalSchema: "dbo",
                        principalTable: "FgsInventoryCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryItem_FgsInventoryItemType",
                        column: x => x.InventoryItemTypeId,
                        principalSchema: "dbo",
                        principalTable: "FgsInventoryItemType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryItem_FgsInventorySubCategory",
                        column: x => x.InventorySubCategoryId,
                        principalSchema: "dbo",
                        principalTable: "FgsInventorySubCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryItem_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Inventory item master record for purchasing, sales, and stock tracking.");

            migrationBuilder.CreateTable(
                name: "FgsInventoryItemAlternate",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    AlternateInventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    AlternateType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
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
                        principalSchema: "dbo",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryItemAlternate_FgsInventoryItem_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalSchema: "dbo",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryItemAlternate_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsInventoryItemDependency",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    DependentInventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 1m),
                    DependencyType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
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
                        principalSchema: "dbo",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryItemDependency_FgsInventoryItem_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalSchema: "dbo",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryItemDependency_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsInventoryStock",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
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
                        principalSchema: "dbo",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryStock_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsVendorInventoryItem",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    VendorId = table.Column<long>(type: "bigint", nullable: false),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    VendorPartNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Vendor-specific part number for the inventory item."),
                    VendorPartName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    LastCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Last received cost from the vendor based on purchase order receiving."),
                    LastReceivedDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Last date inventory was received from the vendor."),
                    PurchaseOrderComments = table.Column<string>(type: "text", nullable: true, comment: "Comments automatically copied to purchase orders for this vendor item combination."),
                    IsPreferredVendor = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether this vendor is the preferred vendor for the inventory item."),
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
                        principalSchema: "dbo",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsVendorInventoryItem_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsVendorInventoryItem_FgsVendor",
                        column: x => x.VendorId,
                        principalSchema: "dbo",
                        principalTable: "FgsVendor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores vendor-specific inventory item relationships, vendor part information, pricing, and purchasing defaults.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryCategory_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsInventoryCategory",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_InventoryCategoryId",
                schema: "dbo",
                table: "FgsInventoryItem",
                column: "InventoryCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_InventoryItemTypeId",
                schema: "dbo",
                table: "FgsInventoryItem",
                column: "InventoryItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_InventorySubCategoryId",
                schema: "dbo",
                table: "FgsInventoryItem",
                column: "InventorySubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_TenantId_CompanyId_InventoryCategoryId",
                schema: "dbo",
                table: "FgsInventoryItem",
                columns: new[] { "TenantId", "CompanyId", "InventoryCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_TenantId_CompanyId_InventoryCategoryId_InventorySubCategoryId",
                schema: "dbo",
                table: "FgsInventoryItem",
                columns: new[] { "TenantId", "CompanyId", "InventoryCategoryId", "InventorySubCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_TenantId_CompanyId_InventoryItemTypeId",
                schema: "dbo",
                table: "FgsInventoryItem",
                columns: new[] { "TenantId", "CompanyId", "InventoryItemTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_TenantId_CompanyId_Name",
                schema: "dbo",
                table: "FgsInventoryItem",
                columns: new[] { "TenantId", "CompanyId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemAlternate_AlternateInventoryItemId",
                schema: "dbo",
                table: "FgsInventoryItemAlternate",
                column: "AlternateInventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemAlternate_InventoryItemId",
                schema: "dbo",
                table: "FgsInventoryItemAlternate",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemAlternate_TenantId_CompanyId_InventoryItemId",
                schema: "dbo",
                table: "FgsInventoryItemAlternate",
                columns: new[] { "TenantId", "CompanyId", "InventoryItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemDependency_DependentInventoryItemId",
                schema: "dbo",
                table: "FgsInventoryItemDependency",
                column: "DependentInventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemDependency_InventoryItemId",
                schema: "dbo",
                table: "FgsInventoryItemDependency",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemDependency_TenantId_CompanyId_InventoryItemId",
                schema: "dbo",
                table: "FgsInventoryItemDependency",
                columns: new[] { "TenantId", "CompanyId", "InventoryItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemType_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsInventoryItemType",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryStock_InventoryItemId",
                schema: "dbo",
                table: "FgsInventoryStock",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventorySubCategory_InventoryCategoryId",
                schema: "dbo",
                table: "FgsInventorySubCategory",
                column: "InventoryCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventorySubCategory_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsInventorySubCategory",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventorySubCategory_TenantId_CompanyId_InventoryCategoryId",
                schema: "dbo",
                table: "FgsInventorySubCategory",
                columns: new[] { "TenantId", "CompanyId", "InventoryCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendor_PaymentTermId",
                schema: "dbo",
                table: "FgsVendor",
                column: "PaymentTermId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendor_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsVendor",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendor_TenantId_CompanyId_Name",
                schema: "dbo",
                table: "FgsVendor",
                columns: new[] { "TenantId", "CompanyId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendor_TenantId_CompanyId_VendorType",
                schema: "dbo",
                table: "FgsVendor",
                columns: new[] { "TenantId", "CompanyId", "VendorType" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendorInventoryItem_InventoryItemId",
                schema: "dbo",
                table: "FgsVendorInventoryItem",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendorInventoryItem_TenantId_CompanyId_InventoryItemId",
                schema: "dbo",
                table: "FgsVendorInventoryItem",
                columns: new[] { "TenantId", "CompanyId", "InventoryItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendorInventoryItem_TenantId_CompanyId_VendorId",
                schema: "dbo",
                table: "FgsVendorInventoryItem",
                columns: new[] { "TenantId", "CompanyId", "VendorId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendorInventoryItem_VendorId",
                schema: "dbo",
                table: "FgsVendorInventoryItem",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_GloInventoryCategory_BusinessTypeId",
                schema: "dbo",
                table: "GloInventoryCategory",
                column: "BusinessTypeId");

            migrationBuilder.CreateIndex(
                name: "UQ_GloInventoryCategory_BusinessTypeId_CategoryCode",
                schema: "dbo",
                table: "GloInventoryCategory",
                columns: new[] { "BusinessTypeId", "CategoryCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_GloInventoryItemType_ItemTypeCode",
                schema: "dbo",
                table: "GloInventoryItemType",
                column: "ItemTypeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloInventorySubCategory_InventoryCategoryId",
                schema: "dbo",
                table: "GloInventorySubCategory",
                column: "InventoryCategoryId");

            migrationBuilder.CreateIndex(
                name: "UQ_GloInventorySubCategory_InventoryCategoryId_SubCategoryCode",
                schema: "dbo",
                table: "GloInventorySubCategory",
                columns: new[] { "InventoryCategoryId", "SubCategoryCode" },
                unique: true);

            migrationBuilder.Sql(
                """
                -- Glo catalog
                COMMENT ON TABLE dbo."GloInventoryItemType" IS 'Global inventory item type catalog (inventory, non-inventory, service, kit, tool).';
                COMMENT ON COLUMN dbo."GloInventoryItemType"."Id" IS 'Surrogate primary key.';
                COMMENT ON COLUMN dbo."GloInventoryItemType"."ItemTypeCode" IS 'Unique item type code.';
                COMMENT ON COLUMN dbo."GloInventoryItemType"."Name" IS 'Display name of the item type.';
                COMMENT ON COLUMN dbo."GloInventoryItemType"."Description" IS 'Description of the item type behavior.';
                COMMENT ON COLUMN dbo."GloInventoryItemType"."TracksQuantity" IS 'Indicates whether items of this type track quantity on hand.';
                COMMENT ON COLUMN dbo."GloInventoryItemType"."DisplayOrder" IS 'Sort order for UI display.';
                COMMENT ON COLUMN dbo."GloInventoryItemType"."IsActive" IS 'Indicates whether the item type is active.';
                COMMENT ON COLUMN dbo."GloInventoryItemType"."CreatedOn" IS 'UTC timestamp when the record was created.';

                COMMENT ON TABLE dbo."GloInventoryCategory" IS 'Global inventory category catalog scoped by business type.';
                COMMENT ON COLUMN dbo."GloInventoryCategory"."Id" IS 'Surrogate primary key.';
                COMMENT ON COLUMN dbo."GloInventoryCategory"."BusinessTypeId" IS 'Reference to the business type this category belongs to.';
                COMMENT ON COLUMN dbo."GloInventoryCategory"."CategoryCode" IS 'Unique category code within the business type.';
                COMMENT ON COLUMN dbo."GloInventoryCategory"."Name" IS 'Display name of the category.';
                COMMENT ON COLUMN dbo."GloInventoryCategory"."Description" IS 'Description of the category.';
                COMMENT ON COLUMN dbo."GloInventoryCategory"."DisplayOrder" IS 'Sort order for UI display.';
                COMMENT ON COLUMN dbo."GloInventoryCategory"."IsActive" IS 'Indicates whether the category is active.';
                COMMENT ON COLUMN dbo."GloInventoryCategory"."CreatedOn" IS 'UTC timestamp when the record was created.';
                COMMENT ON COLUMN dbo."GloInventoryCategory"."UpdatedOn" IS 'UTC timestamp of the last update.';
                COMMENT ON COLUMN dbo."GloInventoryCategory"."CreatedBy" IS 'User or process that created the record.';
                COMMENT ON COLUMN dbo."GloInventoryCategory"."UpdatedBy" IS 'User or process that last updated the record.';

                COMMENT ON TABLE dbo."GloInventorySubCategory" IS 'Global inventory subcategory catalog under a category.';
                COMMENT ON COLUMN dbo."GloInventorySubCategory"."Id" IS 'Surrogate primary key.';
                COMMENT ON COLUMN dbo."GloInventorySubCategory"."InventoryCategoryId" IS 'Parent inventory category.';
                COMMENT ON COLUMN dbo."GloInventorySubCategory"."SubCategoryCode" IS 'Unique subcategory code within the parent category.';
                COMMENT ON COLUMN dbo."GloInventorySubCategory"."Name" IS 'Display name of the subcategory.';
                COMMENT ON COLUMN dbo."GloInventorySubCategory"."Description" IS 'Description of the subcategory.';
                COMMENT ON COLUMN dbo."GloInventorySubCategory"."DisplayOrder" IS 'Sort order for UI display.';
                COMMENT ON COLUMN dbo."GloInventorySubCategory"."IsActive" IS 'Indicates whether the subcategory is active.';
                COMMENT ON COLUMN dbo."GloInventorySubCategory"."CreatedOn" IS 'UTC timestamp when the record was created.';

                -- Vendor
                COMMENT ON TABLE dbo."FgsVendor" IS 'Stores vendor and subcontractor master information for purchasing, AP, and subcontractor management.';
                COMMENT ON COLUMN dbo."FgsVendor"."Id" IS 'Surrogate primary key.';
                COMMENT ON COLUMN dbo."FgsVendor"."TenantId" IS 'Owning tenant identifier.';
                COMMENT ON COLUMN dbo."FgsVendor"."CompanyId" IS 'Tenant-scoped company number (maps to FgsTenantCompany.CompanyNumber).';
                COMMENT ON COLUMN dbo."FgsVendor"."VendorCode" IS 'Unique vendor code within the tenant and company scope.';
                COMMENT ON COLUMN dbo."FgsVendor"."Name" IS 'Display name of the vendor or subcontractor.';
                COMMENT ON COLUMN dbo."FgsVendor"."LegalName" IS 'Legal business name for AP and compliance.';
                COMMENT ON COLUMN dbo."FgsVendor"."VendorType" IS 'Allowed values: VENDOR, SUBCONTRACTOR';
                COMMENT ON COLUMN dbo."FgsVendor"."PaymentTermId" IS 'References payment terms used for accounts payable due date calculation.';
                COMMENT ON COLUMN dbo."FgsVendor"."Email" IS 'Primary contact email address.';
                COMMENT ON COLUMN dbo."FgsVendor"."PhoneNumber" IS 'Primary business phone number.';
                COMMENT ON COLUMN dbo."FgsVendor"."MobileNumber" IS 'Mobile contact phone number.';
                COMMENT ON COLUMN dbo."FgsVendor"."Website" IS 'Vendor website URL.';
                COMMENT ON COLUMN dbo."FgsVendor"."TaxIdentificationNumber" IS 'Tax identification number used for 1099 and AP compliance.';
                COMMENT ON COLUMN dbo."FgsVendor"."LicenseNumber" IS 'Trade or business license number.';
                COMMENT ON COLUMN dbo."FgsVendor"."InsurancePolicyNumber" IS 'Insurance policy reference for subcontractor compliance.';
                COMMENT ON COLUMN dbo."FgsVendor"."Notes" IS 'Free-form notes about the vendor.';
                COMMENT ON COLUMN dbo."FgsVendor"."Is1099Eligible" IS 'Indicates whether vendor should be included in 1099 reporting.';
                COMMENT ON COLUMN dbo."FgsVendor"."IsActive" IS 'Indicates whether the vendor record is active.';
                COMMENT ON COLUMN dbo."FgsVendor"."CreatedOn" IS 'UTC timestamp when the record was created.';
                COMMENT ON COLUMN dbo."FgsVendor"."CreatedBy" IS 'User or process that created the record.';
                COMMENT ON COLUMN dbo."FgsVendor"."UpdatedOn" IS 'UTC timestamp of the last update.';
                COMMENT ON COLUMN dbo."FgsVendor"."UpdatedBy" IS 'User or process that last updated the record.';

                COMMENT ON TABLE dbo."FgsVendorInventoryItem" IS 'Stores vendor-specific inventory item relationships, vendor part information, pricing, and purchasing defaults.';
                COMMENT ON COLUMN dbo."FgsVendorInventoryItem"."Id" IS 'Surrogate primary key.';
                COMMENT ON COLUMN dbo."FgsVendorInventoryItem"."TenantId" IS 'Owning tenant identifier.';
                COMMENT ON COLUMN dbo."FgsVendorInventoryItem"."CompanyId" IS 'Tenant-scoped company number (maps to FgsTenantCompany.CompanyNumber).';
                COMMENT ON COLUMN dbo."FgsVendorInventoryItem"."VendorId" IS 'Reference to the vendor master record.';
                COMMENT ON COLUMN dbo."FgsVendorInventoryItem"."InventoryItemId" IS 'Reference to the inventory item master record.';
                COMMENT ON COLUMN dbo."FgsVendorInventoryItem"."VendorPartNumber" IS 'Vendor-specific part number for the inventory item.';
                COMMENT ON COLUMN dbo."FgsVendorInventoryItem"."VendorPartName" IS 'Vendor-specific part name or description.';
                COMMENT ON COLUMN dbo."FgsVendorInventoryItem"."LastCost" IS 'Last received cost from the vendor based on purchase order receiving.';
                COMMENT ON COLUMN dbo."FgsVendorInventoryItem"."LastReceivedDate" IS 'Last date inventory was received from the vendor.';
                COMMENT ON COLUMN dbo."FgsVendorInventoryItem"."PurchaseOrderComments" IS 'Comments automatically copied to purchase orders for this vendor item combination.';
                COMMENT ON COLUMN dbo."FgsVendorInventoryItem"."IsPreferredVendor" IS 'Indicates whether this vendor is the preferred vendor for the inventory item.';
                COMMENT ON COLUMN dbo."FgsVendorInventoryItem"."IsActive" IS 'Indicates whether the vendor inventory item mapping is active.';
                COMMENT ON COLUMN dbo."FgsVendorInventoryItem"."CreatedOn" IS 'UTC timestamp when the record was created.';
                COMMENT ON COLUMN dbo."FgsVendorInventoryItem"."CreatedBy" IS 'User or process that created the record.';
                COMMENT ON COLUMN dbo."FgsVendorInventoryItem"."UpdatedOn" IS 'UTC timestamp of the last update.';
                COMMENT ON COLUMN dbo."FgsVendorInventoryItem"."UpdatedBy" IS 'User or process that last updated the record.';

                -- Tenant inventory catalog
                COMMENT ON TABLE dbo."FgsInventoryItemType" IS 'Tenant-scoped inventory item type catalog seeded from GloInventoryItemType.';
                COMMENT ON COLUMN dbo."FgsInventoryItemType"."Id" IS 'Surrogate primary key.';
                COMMENT ON COLUMN dbo."FgsInventoryItemType"."TenantId" IS 'Owning tenant identifier.';
                COMMENT ON COLUMN dbo."FgsInventoryItemType"."CompanyId" IS 'Tenant-scoped company number (maps to FgsTenantCompany.CompanyNumber).';
                COMMENT ON COLUMN dbo."FgsInventoryItemType"."ItemTypeCode" IS 'Unique item type code within tenant and company scope.';
                COMMENT ON COLUMN dbo."FgsInventoryItemType"."Name" IS 'Display name of the item type.';
                COMMENT ON COLUMN dbo."FgsInventoryItemType"."Description" IS 'Description of the item type behavior.';
                COMMENT ON COLUMN dbo."FgsInventoryItemType"."TracksQuantity" IS 'Indicates whether items of this type track quantity on hand.';
                COMMENT ON COLUMN dbo."FgsInventoryItemType"."DisplayOrder" IS 'Sort order for UI display.';
                COMMENT ON COLUMN dbo."FgsInventoryItemType"."IsSystem" IS 'Indicates a system-seeded record that should not be deleted.';
                COMMENT ON COLUMN dbo."FgsInventoryItemType"."IsActive" IS 'Indicates whether the item type is active.';
                COMMENT ON COLUMN dbo."FgsInventoryItemType"."CreatedOn" IS 'UTC timestamp when the record was created.';
                COMMENT ON COLUMN dbo."FgsInventoryItemType"."CreatedBy" IS 'User or process that created the record.';
                COMMENT ON COLUMN dbo."FgsInventoryItemType"."UpdatedOn" IS 'UTC timestamp of the last update.';
                COMMENT ON COLUMN dbo."FgsInventoryItemType"."UpdatedBy" IS 'User or process that last updated the record.';

                COMMENT ON TABLE dbo."FgsInventoryCategory" IS 'Tenant-scoped inventory category catalog seeded from GloInventoryCategory.';
                COMMENT ON COLUMN dbo."FgsInventoryCategory"."Id" IS 'Surrogate primary key.';
                COMMENT ON COLUMN dbo."FgsInventoryCategory"."TenantId" IS 'Owning tenant identifier.';
                COMMENT ON COLUMN dbo."FgsInventoryCategory"."CompanyId" IS 'Tenant-scoped company number (maps to FgsTenantCompany.CompanyNumber).';
                COMMENT ON COLUMN dbo."FgsInventoryCategory"."CategoryCode" IS 'Unique category code within tenant and company scope.';
                COMMENT ON COLUMN dbo."FgsInventoryCategory"."Name" IS 'Display name of the category.';
                COMMENT ON COLUMN dbo."FgsInventoryCategory"."Description" IS 'Description of the category.';
                COMMENT ON COLUMN dbo."FgsInventoryCategory"."DisplayOrder" IS 'Sort order for UI display.';
                COMMENT ON COLUMN dbo."FgsInventoryCategory"."IsSystem" IS 'Indicates a system-seeded record that should not be deleted.';
                COMMENT ON COLUMN dbo."FgsInventoryCategory"."IsActive" IS 'Indicates whether the category is active.';
                COMMENT ON COLUMN dbo."FgsInventoryCategory"."CreatedOn" IS 'UTC timestamp when the record was created.';
                COMMENT ON COLUMN dbo."FgsInventoryCategory"."CreatedBy" IS 'User or process that created the record.';
                COMMENT ON COLUMN dbo."FgsInventoryCategory"."UpdatedOn" IS 'UTC timestamp of the last update.';
                COMMENT ON COLUMN dbo."FgsInventoryCategory"."UpdatedBy" IS 'User or process that last updated the record.';

                COMMENT ON TABLE dbo."FgsInventorySubCategory" IS 'Tenant-scoped inventory subcategory catalog seeded from GloInventorySubCategory.';
                COMMENT ON COLUMN dbo."FgsInventorySubCategory"."Id" IS 'Surrogate primary key.';
                COMMENT ON COLUMN dbo."FgsInventorySubCategory"."TenantId" IS 'Owning tenant identifier.';
                COMMENT ON COLUMN dbo."FgsInventorySubCategory"."CompanyId" IS 'Tenant-scoped company number (maps to FgsTenantCompany.CompanyNumber).';
                COMMENT ON COLUMN dbo."FgsInventorySubCategory"."InventoryCategoryId" IS 'Parent tenant inventory category.';
                COMMENT ON COLUMN dbo."FgsInventorySubCategory"."SubCategoryCode" IS 'Unique subcategory code within the parent category.';
                COMMENT ON COLUMN dbo."FgsInventorySubCategory"."Name" IS 'Display name of the subcategory.';
                COMMENT ON COLUMN dbo."FgsInventorySubCategory"."Description" IS 'Description of the subcategory.';
                COMMENT ON COLUMN dbo."FgsInventorySubCategory"."DisplayOrder" IS 'Sort order for UI display.';
                COMMENT ON COLUMN dbo."FgsInventorySubCategory"."IsSystem" IS 'Indicates a system-seeded record that should not be deleted.';
                COMMENT ON COLUMN dbo."FgsInventorySubCategory"."IsActive" IS 'Indicates whether the subcategory is active.';
                COMMENT ON COLUMN dbo."FgsInventorySubCategory"."CreatedOn" IS 'UTC timestamp when the record was created.';
                COMMENT ON COLUMN dbo."FgsInventorySubCategory"."CreatedBy" IS 'User or process that created the record.';
                COMMENT ON COLUMN dbo."FgsInventorySubCategory"."UpdatedOn" IS 'UTC timestamp of the last update.';
                COMMENT ON COLUMN dbo."FgsInventorySubCategory"."UpdatedBy" IS 'User or process that last updated the record.';

                COMMENT ON TABLE dbo."FgsInventoryItem" IS 'Inventory item master record for purchasing, sales, and stock tracking.';
                COMMENT ON COLUMN dbo."FgsInventoryItem"."Id" IS 'Surrogate primary key.';
                COMMENT ON COLUMN dbo."FgsInventoryItem"."TenantId" IS 'Owning tenant identifier.';
                COMMENT ON COLUMN dbo."FgsInventoryItem"."CompanyId" IS 'Tenant-scoped company number (maps to FgsTenantCompany.CompanyNumber).';
                COMMENT ON COLUMN dbo."FgsInventoryItem"."InventoryItemTypeId" IS 'Reference to the inventory item type.';
                COMMENT ON COLUMN dbo."FgsInventoryItem"."InventoryCategoryId" IS 'Optional reference to the inventory category.';
                COMMENT ON COLUMN dbo."FgsInventoryItem"."InventorySubCategoryId" IS 'Optional reference to the inventory subcategory.';
                COMMENT ON COLUMN dbo."FgsInventoryItem"."ItemCode" IS 'Unique item code within tenant and company scope.';
                COMMENT ON COLUMN dbo."FgsInventoryItem"."Name" IS 'Display name of the inventory item.';
                COMMENT ON COLUMN dbo."FgsInventoryItem"."Description" IS 'General description of the item.';
                COMMENT ON COLUMN dbo."FgsInventoryItem"."PurchaseDescription" IS 'Description shown on purchase orders.';
                COMMENT ON COLUMN dbo."FgsInventoryItem"."SalesDescription" IS 'Description shown on sales documents.';
                COMMENT ON COLUMN dbo."FgsInventoryItem"."ManufacturerPartNumber" IS 'Manufacturer part number (MPN).';
                COMMENT ON COLUMN dbo."FgsInventoryItem"."UPCCode" IS 'Universal product code (UPC).';
                COMMENT ON COLUMN dbo."FgsInventoryItem"."UnitOfMeasure" IS 'Default unit of measure for the item.';
                COMMENT ON COLUMN dbo."FgsInventoryItem"."TrackQuantity" IS 'Indicates whether quantity on hand is tracked for this item.';
                COMMENT ON COLUMN dbo."FgsInventoryItem"."Cost" IS 'Default or standard cost of the item.';
                COMMENT ON COLUMN dbo."FgsInventoryItem"."SalesPrice" IS 'Default sales price of the item.';
                COMMENT ON COLUMN dbo."FgsInventoryItem"."DefaultTaxable" IS 'Indicates whether the item is taxable by default.';
                COMMENT ON COLUMN dbo."FgsInventoryItem"."IsActive" IS 'Indicates whether the inventory item is active.';
                COMMENT ON COLUMN dbo."FgsInventoryItem"."CreatedOn" IS 'UTC timestamp when the record was created.';
                COMMENT ON COLUMN dbo."FgsInventoryItem"."CreatedBy" IS 'User or process that created the record.';
                COMMENT ON COLUMN dbo."FgsInventoryItem"."UpdatedOn" IS 'UTC timestamp of the last update.';
                COMMENT ON COLUMN dbo."FgsInventoryItem"."UpdatedBy" IS 'User or process that last updated the record.';

                COMMENT ON TABLE dbo."FgsInventoryStock" IS 'Quantity and cost snapshot for an inventory item at a point in time.';
                COMMENT ON COLUMN dbo."FgsInventoryStock"."Id" IS 'Surrogate primary key.';
                COMMENT ON COLUMN dbo."FgsInventoryStock"."TenantId" IS 'Owning tenant identifier.';
                COMMENT ON COLUMN dbo."FgsInventoryStock"."CompanyId" IS 'Tenant-scoped company number (maps to FgsTenantCompany.CompanyNumber).';
                COMMENT ON COLUMN dbo."FgsInventoryStock"."InventoryItemId" IS 'Reference to the inventory item.';
                COMMENT ON COLUMN dbo."FgsInventoryStock"."QuantityOnHand" IS 'Physical quantity on hand.';
                COMMENT ON COLUMN dbo."FgsInventoryStock"."QuantityCommitted" IS 'Quantity committed to work orders or sales.';
                COMMENT ON COLUMN dbo."FgsInventoryStock"."QuantityAvailable" IS 'Quantity available for use (on hand minus committed).';
                COMMENT ON COLUMN dbo."FgsInventoryStock"."AverageCost" IS 'Weighted average cost of the item.';
                COMMENT ON COLUMN dbo."FgsInventoryStock"."LastCost" IS 'Most recent purchase cost.';
                COMMENT ON COLUMN dbo."FgsInventoryStock"."LastPurchaseDate" IS 'Date of the most recent purchase receipt.';
                COMMENT ON COLUMN dbo."FgsInventoryStock"."LastSoldDate" IS 'Date of the most recent sale.';
                COMMENT ON COLUMN dbo."FgsInventoryStock"."UpdatedOn" IS 'UTC timestamp of the last stock update.';

                COMMENT ON TABLE dbo."FgsInventoryItemAlternate" IS 'Alternate or equivalent inventory items for a primary item.';
                COMMENT ON COLUMN dbo."FgsInventoryItemAlternate"."Id" IS 'Surrogate primary key.';
                COMMENT ON COLUMN dbo."FgsInventoryItemAlternate"."TenantId" IS 'Owning tenant identifier.';
                COMMENT ON COLUMN dbo."FgsInventoryItemAlternate"."CompanyId" IS 'Tenant-scoped company number (maps to FgsTenantCompany.CompanyNumber).';
                COMMENT ON COLUMN dbo."FgsInventoryItemAlternate"."InventoryItemId" IS 'Primary inventory item.';
                COMMENT ON COLUMN dbo."FgsInventoryItemAlternate"."AlternateInventoryItemId" IS 'Alternate inventory item.';
                COMMENT ON COLUMN dbo."FgsInventoryItemAlternate"."AlternateType" IS 'Type of alternate relationship.';
                COMMENT ON COLUMN dbo."FgsInventoryItemAlternate"."PriorityOrder" IS 'Priority when multiple alternates exist.';
                COMMENT ON COLUMN dbo."FgsInventoryItemAlternate"."Notes" IS 'Notes about the alternate relationship.';
                COMMENT ON COLUMN dbo."FgsInventoryItemAlternate"."IsActive" IS 'Indicates whether the alternate mapping is active.';
                COMMENT ON COLUMN dbo."FgsInventoryItemAlternate"."CreatedOn" IS 'UTC timestamp when the record was created.';
                COMMENT ON COLUMN dbo."FgsInventoryItemAlternate"."CreatedBy" IS 'User or process that created the record.';
                COMMENT ON COLUMN dbo."FgsInventoryItemAlternate"."UpdatedOn" IS 'UTC timestamp of the last update.';
                COMMENT ON COLUMN dbo."FgsInventoryItemAlternate"."UpdatedBy" IS 'User or process that last updated the record.';

                COMMENT ON TABLE dbo."FgsInventoryItemDependency" IS 'Dependent parts required or recommended with a primary inventory item.';
                COMMENT ON COLUMN dbo."FgsInventoryItemDependency"."Id" IS 'Surrogate primary key.';
                COMMENT ON COLUMN dbo."FgsInventoryItemDependency"."TenantId" IS 'Owning tenant identifier.';
                COMMENT ON COLUMN dbo."FgsInventoryItemDependency"."CompanyId" IS 'Tenant-scoped company number (maps to FgsTenantCompany.CompanyNumber).';
                COMMENT ON COLUMN dbo."FgsInventoryItemDependency"."InventoryItemId" IS 'Primary inventory item.';
                COMMENT ON COLUMN dbo."FgsInventoryItemDependency"."DependentInventoryItemId" IS 'Dependent inventory item.';
                COMMENT ON COLUMN dbo."FgsInventoryItemDependency"."Quantity" IS 'Quantity of the dependent item required.';
                COMMENT ON COLUMN dbo."FgsInventoryItemDependency"."DependencyType" IS 'Type of dependency relationship.';
                COMMENT ON COLUMN dbo."FgsInventoryItemDependency"."IsRequired" IS 'Indicates whether the dependent item is required.';
                COMMENT ON COLUMN dbo."FgsInventoryItemDependency"."Notes" IS 'Notes about the dependency.';
                COMMENT ON COLUMN dbo."FgsInventoryItemDependency"."DisplayOrder" IS 'Sort order for UI display.';
                COMMENT ON COLUMN dbo."FgsInventoryItemDependency"."IsActive" IS 'Indicates whether the dependency is active.';
                COMMENT ON COLUMN dbo."FgsInventoryItemDependency"."CreatedOn" IS 'UTC timestamp when the record was created.';
                COMMENT ON COLUMN dbo."FgsInventoryItemDependency"."CreatedBy" IS 'User or process that created the record.';
                COMMENT ON COLUMN dbo."FgsInventoryItemDependency"."UpdatedOn" IS 'UTC timestamp of the last update.';
                COMMENT ON COLUMN dbo."FgsInventoryItemDependency"."UpdatedBy" IS 'User or process that last updated the record.';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsInventoryItemAlternate",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsInventoryItemDependency",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsInventoryStock",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsVendorInventoryItem",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloInventoryItemType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloInventorySubCategory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsInventoryItem",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsVendor",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloInventoryCategory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsInventoryItemType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsInventorySubCategory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsInventoryCategory",
                schema: "dbo");
        }
    }
}
