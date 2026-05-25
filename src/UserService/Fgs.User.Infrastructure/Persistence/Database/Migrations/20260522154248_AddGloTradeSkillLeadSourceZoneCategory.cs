using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddGloTradeSkillLeadSourceZoneCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsLeadSource",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    SourceCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SourceName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsLeadSource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsLeadSource_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GloCategory",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    BusinessTypeId = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloCategory", x => x.Id);
                    table.CheckConstraint("CK_GloCategory_Code_Upper", "\"Code\" = upper(\"Code\")");
                    table.ForeignKey(
                        name: "FK_GloCategory_GloBusinessType_BusinessTypeId",
                        column: x => x.BusinessTypeId,
                        principalSchema: "dbo",
                        principalTable: "GloBusinessType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GloLeadSource",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SourceCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SourceName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloLeadSource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloSubCategory",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloSubCategory", x => x.Id);
                    table.CheckConstraint("CK_GloSubCategory_Code_Upper", "\"Code\" = upper(\"Code\")");
                });

            migrationBuilder.CreateTable(
                name: "GloTrade",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BusinessTypeId = table.Column<int>(type: "integer", nullable: false),
                    TradeCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TradeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloTrade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GloTrade_GloBusinessType_BusinessTypeId",
                        column: x => x.BusinessTypeId,
                        principalSchema: "dbo",
                        principalTable: "GloBusinessType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GloZone",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloZone", x => x.Id);
                    table.CheckConstraint("CK_GloZone_Code_Upper", "\"Code\" = upper(\"Code\")");
                });

            migrationBuilder.CreateTable(
                name: "GloCategorySubCategory",
                schema: "dbo",
                columns: table => new
                {
                    BusinessTypeId = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<short>(type: "smallint", nullable: false),
                    SubCategoryId = table.Column<short>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloCategorySubCategory", x => new { x.BusinessTypeId, x.CategoryId, x.SubCategoryId });
                    table.ForeignKey(
                        name: "FK_GloCategorySubCategory_GloBusinessType_BusinessTypeId",
                        column: x => x.BusinessTypeId,
                        principalSchema: "dbo",
                        principalTable: "GloBusinessType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GloCategorySubCategory_GloCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "dbo",
                        principalTable: "GloCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GloCategorySubCategory_GloSubCategory_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalSchema: "dbo",
                        principalTable: "GloSubCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GloSkill",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BusinessTypeId = table.Column<int>(type: "integer", nullable: false),
                    TradeId = table.Column<short>(type: "smallint", nullable: false),
                    SkillCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SkillName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    RequiresCertification = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloSkill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GloSkill_GloBusinessType_BusinessTypeId",
                        column: x => x.BusinessTypeId,
                        principalSchema: "dbo",
                        principalTable: "GloBusinessType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GloSkill_GloTrade_TradeId",
                        column: x => x.TradeId,
                        principalSchema: "dbo",
                        principalTable: "GloTrade",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "UX_FgsLeadSource_TenantId_CompanyId_SourceCode",
                schema: "dbo",
                table: "FgsLeadSource",
                columns: new[] { "TenantId", "CompanyId", "SourceCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_GloCategory_BusinessTypeId_Code",
                schema: "dbo",
                table: "GloCategory",
                columns: new[] { "BusinessTypeId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloCategorySubCategory_CategoryId",
                schema: "dbo",
                table: "GloCategorySubCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_GloCategorySubCategory_SubCategoryId",
                schema: "dbo",
                table: "GloCategorySubCategory",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "UX_GloLeadSource_SourceCode",
                schema: "dbo",
                table: "GloLeadSource",
                column: "SourceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloSkill_BusinessTypeId",
                schema: "dbo",
                table: "GloSkill",
                column: "BusinessTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GloSkill_TradeId",
                schema: "dbo",
                table: "GloSkill",
                column: "TradeId");

            migrationBuilder.CreateIndex(
                name: "UX_GloSkill_SkillCode",
                schema: "dbo",
                table: "GloSkill",
                column: "SkillCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_GloSubCategory_Code",
                schema: "dbo",
                table: "GloSubCategory",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloTrade_BusinessTypeId",
                schema: "dbo",
                table: "GloTrade",
                column: "BusinessTypeId");

            migrationBuilder.CreateIndex(
                name: "UX_GloTrade_TradeCode",
                schema: "dbo",
                table: "GloTrade",
                column: "TradeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_GloZone_Code",
                schema: "dbo",
                table: "GloZone",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsLeadSource",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloCategorySubCategory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloLeadSource",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloSkill",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloZone",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloCategory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloSubCategory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloTrade",
                schema: "dbo");
        }
    }
}
