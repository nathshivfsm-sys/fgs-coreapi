using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddGloUnitOfMeasureGloTagFgsTagGloTitleOfCourtesy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsTag",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    TagCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NormalizedName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    BackgroundColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    TextColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IconFileId = table.Column<long>(type: "bigint", nullable: true),
                    UsageCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsSystemGenerated = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsTag_FgsFile_IconFileId",
                        column: x => x.IconFileId,
                        principalSchema: "dbo",
                        principalTable: "FgsFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_FgsTag_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GloTag",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    TagCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NormalizedName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    BackgroundColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    TextColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IconFileId = table.Column<long>(type: "bigint", nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsSystemGenerated = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GloTag_FgsFile_IconFileId",
                        column: x => x.IconFileId,
                        principalSchema: "dbo",
                        principalTable: "FgsFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "GloTitleOfCourtesy",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloTitleOfCourtesy", x => x.Id);
                    table.CheckConstraint("CK_GloTitleOfCourtesy_Code_Upper", "\"Code\" = upper(\"Code\")");
                    table.CheckConstraint("CK_GloTitleOfCourtesy_SortOrder", "\"SortOrder\" >= 0");
                });

            migrationBuilder.CreateTable(
                name: "GloUnitOfMeasure",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UnitCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Abbreviation = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    UnitType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DecimalPlaces = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)2),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloUnitOfMeasure", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FgsTag_IconFileId",
                schema: "dbo",
                table: "FgsTag",
                column: "IconFileId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsTag_IsActive",
                schema: "dbo",
                table: "FgsTag",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsTag_Name",
                schema: "dbo",
                table: "FgsTag",
                columns: new[] { "TenantId", "CompanyId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsTag_UsageCount",
                schema: "dbo",
                table: "FgsTag",
                columns: new[] { "TenantId", "CompanyId", "UsageCount" },
                descending: new[] { false, false, true });

            migrationBuilder.CreateIndex(
                name: "UX_FgsTag_TenantId_CompanyId_NormalizedName",
                schema: "dbo",
                table: "FgsTag",
                columns: new[] { "TenantId", "CompanyId", "NormalizedName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_FgsTag_TenantId_CompanyId_TagCode",
                schema: "dbo",
                table: "FgsTag",
                columns: new[] { "TenantId", "CompanyId", "TagCode" },
                unique: true,
                filter: "\"TagCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GloTag_DisplayOrder",
                schema: "dbo",
                table: "GloTag",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_GloTag_IconFileId",
                schema: "dbo",
                table: "GloTag",
                column: "IconFileId");

            migrationBuilder.CreateIndex(
                name: "IX_GloTag_IsActive",
                schema: "dbo",
                table: "GloTag",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_GloTag_Name",
                schema: "dbo",
                table: "GloTag",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "UX_GloTag_NormalizedName",
                schema: "dbo",
                table: "GloTag",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_GloTag_TagCode",
                schema: "dbo",
                table: "GloTag",
                column: "TagCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloTitleOfCourtesy_SortOrder",
                schema: "dbo",
                table: "GloTitleOfCourtesy",
                column: "SortOrder");

            migrationBuilder.CreateIndex(
                name: "UQ_GloTitleOfCourtesy_Code",
                schema: "dbo",
                table: "GloTitleOfCourtesy",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloUnitOfMeasure_UnitType",
                schema: "dbo",
                table: "GloUnitOfMeasure",
                column: "UnitType");

            migrationBuilder.CreateIndex(
                name: "UQ_GloUnitOfMeasure_UnitCode",
                schema: "dbo",
                table: "GloUnitOfMeasure",
                column: "UnitCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsTag",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloTag",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloTitleOfCourtesy",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloUnitOfMeasure",
                schema: "dbo");
        }
    }
}
