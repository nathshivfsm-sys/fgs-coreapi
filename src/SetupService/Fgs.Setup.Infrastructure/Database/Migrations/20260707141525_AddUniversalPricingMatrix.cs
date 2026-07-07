using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddUniversalPricingMatrix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsUniversalPricingService",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    UniversalPricingServiceCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Logical reference to glo.GloUniversalPricingService.ServiceCode. No cross-domain foreign key is enforced."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls the display sequence of the Universal Pricing Service for the company."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the Universal Pricing Service is currently active for the company.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsUniversalPricingService", x => x.Id);
                    table.UniqueConstraint("AK_FgsUniversalPricingService_TenantId_CompanyId_Id", x => new { x.TenantId, x.CompanyId, x.Id });
                    table.ForeignKey(
                        name: "FK_FgsUniversalPricingService_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Defines Universal Pricing Services enabled and configured for a tenant company.");

            migrationBuilder.CreateTable(
                name: "GloUniversalPricingService",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Stable system code used to identify the universal pricing service across domains."),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "User-facing service name."),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Optional description of the universal pricing service."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloUniversalPricingService", x => x.Id);
                },
                comment: "Global seeded list of services supported by the Universal Pricing Matrix.");

            migrationBuilder.CreateTable(
                name: "FgsUniversalMatrixAddOn",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    UniversalPricingServiceId = table.Column<long>(type: "bigint", nullable: false, comment: "References the company-specific Universal Pricing Service configuration."),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    UnitType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Pricing unit for the add-on, such as Flat Rate, Window, or Bed."),
                    Price = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false, defaultValue: 0m, comment: "Company-specific price per add-on pricing unit."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsUniversalMatrixAddOn", x => x.Id);
                    table.CheckConstraint("CK_FgsUniversalMatrixAddOn_Price", "\"Price\" >= 0");
                    table.ForeignKey(
                        name: "FK_FgsUniversalMatrixAddOn_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsUniversalMatrixAddOn_FgsUniversalPricingService_TenantId_CompanyId_UniversalPricingServiceId",
                        columns: x => new { x.TenantId, x.CompanyId, x.UniversalPricingServiceId },
                        principalSchema: "setup",
                        principalTable: "FgsUniversalPricingService",
                        principalColumns: new[] { "TenantId", "CompanyId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores company-specific optional add-ons available within the Universal Pricing Matrix.");

            migrationBuilder.CreateTable(
                name: "FgsUniversalMatrixFrequencyDiscount",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    UniversalPricingServiceId = table.Column<long>(type: "bigint", nullable: false, comment: "References the company-specific Universal Pricing Service configuration."),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DiscountPercent = table.Column<decimal>(type: "numeric(8,4)", precision: 8, scale: 4, nullable: false, defaultValue: 0m, comment: "Percentage discount applied based on the selected service frequency."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsUniversalMatrixFrequencyDiscount", x => x.Id);
                    table.CheckConstraint("CK_FgsUniversalMatrixFrequencyDiscount_DiscountPercent", "\"DiscountPercent\" >= 0 AND \"DiscountPercent\" <= 100");
                    table.ForeignKey(
                        name: "FK_FgsUniversalMatrixFrequencyDiscount_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsUniversalMatrixFrequencyDiscount_FgsUniversalPricingService_TenantId_CompanyId_UniversalPricingServiceId",
                        columns: x => new { x.TenantId, x.CompanyId, x.UniversalPricingServiceId },
                        principalSchema: "setup",
                        principalTable: "FgsUniversalPricingService",
                        principalColumns: new[] { "TenantId", "CompanyId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores company-specific service frequency options and their discount percentages.");

            migrationBuilder.CreateTable(
                name: "FgsUniversalMatrixItem",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    UniversalPricingServiceId = table.Column<long>(type: "bigint", nullable: false, comment: "References the company-specific Universal Pricing Service configuration."),
                    ItemName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    UnitType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Pricing unit used by the matrix item, such as Flat Rate, Sqft, Linear Foot, Window, or Bed."),
                    BasePrice = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false, defaultValue: 0m, comment: "Base price before tier, size, frequency, fee, add-on, tax, or other pricing adjustments."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsUniversalMatrixItem", x => x.Id);
                    table.CheckConstraint("CK_FgsUniversalMatrixItem_BasePrice", "\"BasePrice\" >= 0");
                    table.ForeignKey(
                        name: "FK_FgsUniversalMatrixItem_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsUniversalMatrixItem_FgsUniversalPricingService_TenantId_CompanyId_UniversalPricingServiceId",
                        columns: x => new { x.TenantId, x.CompanyId, x.UniversalPricingServiceId },
                        principalSchema: "setup",
                        principalTable: "FgsUniversalPricingService",
                        principalColumns: new[] { "TenantId", "CompanyId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores company-specific Universal Pricing Matrix items and base prices.");

            migrationBuilder.CreateTable(
                name: "FgsUniversalMatrixOneTimeFee",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    UniversalPricingServiceId = table.Column<long>(type: "bigint", nullable: false, comment: "References the company-specific Universal Pricing Service configuration."),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false, defaultValue: 0m, comment: "Fixed amount of the one-time fee."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsUniversalMatrixOneTimeFee", x => x.Id);
                    table.CheckConstraint("CK_FgsUniversalMatrixOneTimeFee_Amount", "\"Amount\" >= 0");
                    table.ForeignKey(
                        name: "FK_FgsUniversalMatrixOneTimeFee_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsUniversalMatrixOneTimeFee_FgsUniversalPricingService_TenantId_CompanyId_UniversalPricingServiceId",
                        columns: x => new { x.TenantId, x.CompanyId, x.UniversalPricingServiceId },
                        principalSchema: "setup",
                        principalTable: "FgsUniversalPricingService",
                        principalColumns: new[] { "TenantId", "CompanyId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores company-specific one-time fees used by the Universal Pricing Matrix.");

            migrationBuilder.CreateTable(
                name: "FgsUniversalMatrixSizeTier",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    UniversalPricingServiceId = table.Column<long>(type: "bigint", nullable: false, comment: "References the company-specific Universal Pricing Service configuration."),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Multiplier = table.Column<decimal>(type: "numeric(8,4)", precision: 8, scale: 4, nullable: false, defaultValue: 1.0000m, comment: "Company-specific multiplier applied for this size tier."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsUniversalMatrixSizeTier", x => x.Id);
                    table.CheckConstraint("CK_FgsUniversalMatrixSizeTier_Multiplier", "\"Multiplier\" > 0");
                    table.ForeignKey(
                        name: "FK_FgsUniversalMatrixSizeTier_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsUniversalMatrixSizeTier_FgsUniversalPricingService_TenantId_CompanyId_UniversalPricingServiceId",
                        columns: x => new { x.TenantId, x.CompanyId, x.UniversalPricingServiceId },
                        principalSchema: "setup",
                        principalTable: "FgsUniversalPricingService",
                        principalColumns: new[] { "TenantId", "CompanyId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores company-specific size tiers and pricing multipliers for an enabled Universal Pricing Service.");

            migrationBuilder.CreateTable(
                name: "FgsUniversalMatrixTier",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    UniversalPricingServiceId = table.Column<long>(type: "bigint", nullable: false, comment: "References the company-specific Universal Pricing Service configuration."),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Multiplier = table.Column<decimal>(type: "numeric(8,4)", precision: 8, scale: 4, nullable: false, defaultValue: 1.0000m, comment: "Company-specific multiplier applied for this pricing tier."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsUniversalMatrixTier", x => x.Id);
                    table.CheckConstraint("CK_FgsUniversalMatrixTier_Multiplier", "\"Multiplier\" > 0");
                    table.ForeignKey(
                        name: "FK_FgsUniversalMatrixTier_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsUniversalMatrixTier_FgsUniversalPricingService_TenantId_CompanyId_UniversalPricingServiceId",
                        columns: x => new { x.TenantId, x.CompanyId, x.UniversalPricingServiceId },
                        principalSchema: "setup",
                        principalTable: "FgsUniversalPricingService",
                        principalColumns: new[] { "TenantId", "CompanyId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores company-specific pricing tiers and pricing multipliers for an enabled Universal Pricing Service.");

            migrationBuilder.CreateTable(
                name: "GloUniversalMatrixSizeTier",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UniversalPricingServiceId = table.Column<short>(type: "smallint", nullable: false, comment: "Reference to the global universal pricing service."),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Multiplier = table.Column<decimal>(type: "numeric(8,4)", precision: 8, scale: 4, nullable: false, defaultValue: 1.0000m, comment: "Multiplier applied to calculated service pricing for this size tier."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloUniversalMatrixSizeTier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GloUniversalMatrixSizeTier_GloUniversalPricingService_UniversalPricingServiceId",
                        column: x => x.UniversalPricingServiceId,
                        principalSchema: "glo",
                        principalTable: "GloUniversalPricingService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Global service size tiers and their default pricing multipliers.");

            migrationBuilder.CreateTable(
                name: "GloUniversalMatrixTier",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UniversalPricingServiceId = table.Column<short>(type: "smallint", nullable: false, comment: "Reference to the global universal pricing service."),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Multiplier = table.Column<decimal>(type: "numeric(8,4)", precision: 8, scale: 4, nullable: false, defaultValue: 1.0000m, comment: "Multiplier applied to calculated service pricing for this tier."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloUniversalMatrixTier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GloUniversalMatrixTier_GloUniversalPricingService_UniversalPricingServiceId",
                        column: x => x.UniversalPricingServiceId,
                        principalSchema: "glo",
                        principalTable: "GloUniversalPricingService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Global service pricing tiers and their default pricing multipliers.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsUniversalMatrixAddOn_TenantId_CompanyId",
                schema: "setup",
                table: "FgsUniversalMatrixAddOn",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsUniversalMatrixAddOn_TenantId_CompanyId_UniversalPricingServiceId",
                schema: "setup",
                table: "FgsUniversalMatrixAddOn",
                columns: new[] { "TenantId", "CompanyId", "UniversalPricingServiceId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsUniversalMatrixAddOn_TenantId_CompanyId_UniversalPricingServiceId_Name",
                schema: "setup",
                table: "FgsUniversalMatrixAddOn",
                columns: new[] { "TenantId", "CompanyId", "UniversalPricingServiceId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsUniversalMatrixFrequencyDiscount_TenantId_CompanyId",
                schema: "setup",
                table: "FgsUniversalMatrixFrequencyDiscount",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsUniversalMatrixFrequencyDiscount_TenantId_CompanyId_UniversalPricingServiceId",
                schema: "setup",
                table: "FgsUniversalMatrixFrequencyDiscount",
                columns: new[] { "TenantId", "CompanyId", "UniversalPricingServiceId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsUniversalMatrixFrequencyDiscount_TenantId_CompanyId_UniversalPricingServiceId_Name",
                schema: "setup",
                table: "FgsUniversalMatrixFrequencyDiscount",
                columns: new[] { "TenantId", "CompanyId", "UniversalPricingServiceId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsUniversalMatrixItem_TenantId_CompanyId",
                schema: "setup",
                table: "FgsUniversalMatrixItem",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsUniversalMatrixItem_TenantId_CompanyId_UniversalPricingServiceId",
                schema: "setup",
                table: "FgsUniversalMatrixItem",
                columns: new[] { "TenantId", "CompanyId", "UniversalPricingServiceId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsUniversalMatrixItem_TenantId_CompanyId_UniversalPricingServiceId_ItemName",
                schema: "setup",
                table: "FgsUniversalMatrixItem",
                columns: new[] { "TenantId", "CompanyId", "UniversalPricingServiceId", "ItemName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsUniversalMatrixOneTimeFee_TenantId_CompanyId",
                schema: "setup",
                table: "FgsUniversalMatrixOneTimeFee",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsUniversalMatrixOneTimeFee_TenantId_CompanyId_UniversalPricingServiceId",
                schema: "setup",
                table: "FgsUniversalMatrixOneTimeFee",
                columns: new[] { "TenantId", "CompanyId", "UniversalPricingServiceId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsUniversalMatrixOneTimeFee_TenantId_CompanyId_UniversalPricingServiceId_Name",
                schema: "setup",
                table: "FgsUniversalMatrixOneTimeFee",
                columns: new[] { "TenantId", "CompanyId", "UniversalPricingServiceId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsUniversalMatrixSizeTier_TenantId_CompanyId",
                schema: "setup",
                table: "FgsUniversalMatrixSizeTier",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsUniversalMatrixSizeTier_TenantId_CompanyId_UniversalPricingServiceId",
                schema: "setup",
                table: "FgsUniversalMatrixSizeTier",
                columns: new[] { "TenantId", "CompanyId", "UniversalPricingServiceId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsUniversalMatrixSizeTier_TenantId_CompanyId_UniversalPricingServiceId_Name",
                schema: "setup",
                table: "FgsUniversalMatrixSizeTier",
                columns: new[] { "TenantId", "CompanyId", "UniversalPricingServiceId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsUniversalMatrixTier_TenantId_CompanyId",
                schema: "setup",
                table: "FgsUniversalMatrixTier",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsUniversalMatrixTier_TenantId_CompanyId_UniversalPricingServiceId",
                schema: "setup",
                table: "FgsUniversalMatrixTier",
                columns: new[] { "TenantId", "CompanyId", "UniversalPricingServiceId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsUniversalMatrixTier_TenantId_CompanyId_UniversalPricingServiceId_Name",
                schema: "setup",
                table: "FgsUniversalMatrixTier",
                columns: new[] { "TenantId", "CompanyId", "UniversalPricingServiceId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsUniversalPricingService_TenantId_CompanyId",
                schema: "setup",
                table: "FgsUniversalPricingService",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsUniversalPricingService_TenantId_CompanyId_ServiceCode",
                schema: "setup",
                table: "FgsUniversalPricingService",
                columns: new[] { "TenantId", "CompanyId", "UniversalPricingServiceCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloUniversalMatrixSizeTier_UniversalPricingServiceId",
                schema: "glo",
                table: "GloUniversalMatrixSizeTier",
                column: "UniversalPricingServiceId");

            migrationBuilder.CreateIndex(
                name: "UX_GloUniversalMatrixSizeTier_ServiceId_Name",
                schema: "glo",
                table: "GloUniversalMatrixSizeTier",
                columns: new[] { "UniversalPricingServiceId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloUniversalMatrixTier_UniversalPricingServiceId",
                schema: "glo",
                table: "GloUniversalMatrixTier",
                column: "UniversalPricingServiceId");

            migrationBuilder.CreateIndex(
                name: "UX_GloUniversalMatrixTier_ServiceId_Name",
                schema: "glo",
                table: "GloUniversalMatrixTier",
                columns: new[] { "UniversalPricingServiceId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloUniversalPricingService_Name",
                schema: "glo",
                table: "GloUniversalPricingService",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_GloUniversalPricingService_ServiceCode",
                schema: "glo",
                table: "GloUniversalPricingService",
                column: "ServiceCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsUniversalMatrixAddOn",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsUniversalMatrixFrequencyDiscount",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsUniversalMatrixItem",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsUniversalMatrixOneTimeFee",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsUniversalMatrixSizeTier",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsUniversalMatrixTier",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "GloUniversalMatrixSizeTier",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloUniversalMatrixTier",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "FgsUniversalPricingService",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "GloUniversalPricingService",
                schema: "glo");
        }
    }
}
