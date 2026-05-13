using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class DropObsoleteSetupAssetAndMaterialRangeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsSetupPriceSheetMaterialRange",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupServiceAssetMedia",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupServiceAssetModelSerialDescription",
                schema: "dbo");

            migrationBuilder.AddColumn<long>(
                name: "FgsSetupZoneId",
                schema: "dbo",
                table: "FgsSetupTimeSlot",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsExternalSystemRecord",
                schema: "dbo",
                table: "FgsSetupTaxDetail",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsExternalSystemRecord",
                schema: "dbo",
                table: "FgsSetupTaxAuthority",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsExternalSystemRecord",
                schema: "dbo",
                table: "FgsSetupTax",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "FgsSetupLaborRateTypeId",
                schema: "dbo",
                table: "FgsSetupPriceSheetLabor",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLaborRateBySkillLevel",
                schema: "dbo",
                table: "FgsSetupPriceSheet",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLaborTierStructure",
                schema: "dbo",
                table: "FgsSetupPriceSheet",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FgsSetupZoneId",
                schema: "dbo",
                table: "FgsSetupTimeSlot");

            migrationBuilder.DropColumn(
                name: "IsExternalSystemRecord",
                schema: "dbo",
                table: "FgsSetupTaxDetail");

            migrationBuilder.DropColumn(
                name: "IsExternalSystemRecord",
                schema: "dbo",
                table: "FgsSetupTaxAuthority");

            migrationBuilder.DropColumn(
                name: "IsExternalSystemRecord",
                schema: "dbo",
                table: "FgsSetupTax");

            migrationBuilder.DropColumn(
                name: "FgsSetupLaborRateTypeId",
                schema: "dbo",
                table: "FgsSetupPriceSheetLabor");

            migrationBuilder.DropColumn(
                name: "IsLaborRateBySkillLevel",
                schema: "dbo",
                table: "FgsSetupPriceSheet");

            migrationBuilder.DropColumn(
                name: "IsLaborTierStructure",
                schema: "dbo",
                table: "FgsSetupPriceSheet");

            migrationBuilder.CreateTable(
                name: "FgsSetupPriceSheetMaterialRange",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CostFrom = table.Column<decimal>(type: "numeric", nullable: false),
                    CostTo = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    FgsSetupPriceSheetMaterialId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    MarkupPercent = table.Column<decimal>(type: "numeric", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupPriceSheetMaterialRange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsSetupPriceSheetMaterialRange_FgsTenantCompany_TenantId_C~",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyGuid" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupServiceAssetMedia",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    FgsSetupServiceAssetTypeId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    MediaUrl = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupServiceAssetMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsSetupServiceAssetMedia_FgsTenantCompany_TenantId_Company~",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyGuid" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupServiceAssetModelSerialDescription",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    FgsSetupServiceAssetManufacturerId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ModelDescription = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    SerialNumberPattern = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupServiceAssetModelSerialDescription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsSetupServiceAssetModelSerialDescription_FgsTenantCompany~",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyGuid" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPriceSheetMaterialRange_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheetMaterialRange",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupServiceAssetMedia_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupServiceAssetMedia",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupServiceAssetModelSerialDescription_TenantId_Company~",
                schema: "dbo",
                table: "FgsSetupServiceAssetModelSerialDescription",
                columns: new[] { "TenantId", "CompanyId" });
        }
    }
}
