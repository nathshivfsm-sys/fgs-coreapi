using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSetupServiceAssetCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DROP TABLE IF EXISTS setup.""FgsSetupServiceAssetModelReference"" CASCADE;
DROP TABLE IF EXISTS setup.""FgsSetupServiceAssetManufacturer"" CASCADE;
DROP TABLE IF EXISTS setup.""FgsSetupServiceAssetType"" CASCADE;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsSetupServiceAssetManufacturer",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupServiceAssetManufacturer", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupServiceAssetManufacturer", x => new { x.TenantId, x.CompanyId, x.Code });
                    table.CheckConstraint("CK_FgsSetupServiceAssetManufacturer_Code_Upper", "\"Code\" = UPPER(\"Code\")");
                    table.ForeignKey(
                        name: "FK_FgsSetupServiceAssetManufacturer_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupServiceAssetType",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupServiceAssetType", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupServiceAssetType", x => new { x.TenantId, x.CompanyId, x.Code });
                    table.CheckConstraint("CK_FgsSetupServiceAssetType_Code_Upper", "\"Code\" = UPPER(\"Code\")");
                    table.ForeignKey(
                        name: "FK_FgsSetupServiceAssetType_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupServiceAssetModelReference",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    FgsSetupServiceAssetManufacturerId = table.Column<long>(type: "bigint", nullable: false),
                    FgsSetupServiceAssetTypeId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ModelDescription = table.Column<string>(type: "text", nullable: false),
                    ModelNumber = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    SerialNumberPattern = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UrlsJson = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupServiceAssetModelReference", x => x.Id);
                    table.CheckConstraint("CK_FgsSvcAssetModelRef_UrlsJson", "\"UrlsJson\" IS NULL OR jsonb_typeof(\"UrlsJson\") = 'array'");
                    table.ForeignKey(
                        name: "FK_FgsSetupServiceAssetModelReference_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsSvcAssetModelRef_AssetType",
                        column: x => x.FgsSetupServiceAssetTypeId,
                        principalSchema: "setup",
                        principalTable: "FgsSetupServiceAssetType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsSvcAssetModelRef_Mfr",
                        column: x => x.FgsSetupServiceAssetManufacturerId,
                        principalSchema: "setup",
                        principalTable: "FgsSetupServiceAssetManufacturer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupServiceAssetManufacturer_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupServiceAssetManufacturer",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupServiceAssetModelReference_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupServiceAssetModelReference",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSvcAssetModelRef_Mfr",
                schema: "setup",
                table: "FgsSetupServiceAssetModelReference",
                columns: new[] { "TenantId", "CompanyId", "FgsSetupServiceAssetManufacturerId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSvcAssetModelRef_MfrId",
                schema: "setup",
                table: "FgsSetupServiceAssetModelReference",
                column: "FgsSetupServiceAssetManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSvcAssetModelRef_Type",
                schema: "setup",
                table: "FgsSetupServiceAssetModelReference",
                columns: new[] { "TenantId", "CompanyId", "FgsSetupServiceAssetTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSvcAssetModelRef_TypeId",
                schema: "setup",
                table: "FgsSetupServiceAssetModelReference",
                column: "FgsSetupServiceAssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSvcAssetModelRef_TypeMfr",
                schema: "setup",
                table: "FgsSetupServiceAssetModelReference",
                columns: new[] { "TenantId", "CompanyId", "FgsSetupServiceAssetTypeId", "FgsSetupServiceAssetManufacturerId" });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAsset_UrlsJson",
                schema: "setup",
                table: "FgsSetupServiceAssetModelReference",
                column: "UrlsJson")
                .Annotation("Npgsql:IndexMethod", "gin");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupServiceAssetType_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupServiceAssetType",
                columns: new[] { "TenantId", "CompanyId" });
        }
    }
}
