using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class RenameJobTypeCategoriesAddEntityTagRemoveUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE dbo."GloSubCategory" RENAME TO "GloJobTypeSubCategory";
                ALTER TABLE dbo."GloCategory" RENAME TO "GloJobTypeCategory";
                ALTER TABLE dbo."GloCategorySubCategory" RENAME TO "GloJobTypeCategorySubCategory";

                ALTER TABLE dbo."GloJobTypeSubCategory" RENAME CONSTRAINT "PK_GloSubCategory" TO "PK_GloJobTypeSubCategory";
                ALTER INDEX dbo."UQ_GloSubCategory_Code" RENAME TO "UQ_GloJobTypeSubCategory_Code";
                ALTER TABLE dbo."GloJobTypeSubCategory" RENAME CONSTRAINT "CK_GloSubCategory_Code_Upper" TO "CK_GloJobTypeSubCategory_Code_Upper";

                ALTER TABLE dbo."GloJobTypeCategory" RENAME CONSTRAINT "PK_GloCategory" TO "PK_GloJobTypeCategory";
                ALTER INDEX dbo."UQ_GloCategory_BusinessTypeId_Code" RENAME TO "UQ_GloJobTypeCategory_BusinessTypeId_Code";
                ALTER TABLE dbo."GloJobTypeCategory" RENAME CONSTRAINT "CK_GloCategory_Code_Upper" TO "CK_GloJobTypeCategory_Code_Upper";
                ALTER TABLE dbo."GloJobTypeCategory" RENAME CONSTRAINT "FK_GloCategory_GloBusinessType_BusinessTypeId" TO "FK_GloJobTypeCategory_GloBusinessType_BusinessTypeId";

                ALTER TABLE dbo."GloJobTypeCategorySubCategory" RENAME CONSTRAINT "PK_GloCategorySubCategory" TO "PK_GloJobTypeCategorySubCategory";
                ALTER TABLE dbo."GloJobTypeCategorySubCategory" RENAME CONSTRAINT "FK_GloCategorySubCategory_GloBusinessType_BusinessTypeId" TO "FK_GloJobTypeCategorySubCategory_GloBusinessType_BusinessTypeId";
                ALTER TABLE dbo."GloJobTypeCategorySubCategory" RENAME CONSTRAINT "FK_GloCategorySubCategory_GloCategory_CategoryId" TO "FK_GloJobTypeCategorySubCategory_GloJobTypeCategory_CategoryId";
                ALTER TABLE dbo."GloJobTypeCategorySubCategory" RENAME CONSTRAINT "FK_GloCategorySubCategory_GloSubCategory_SubCategoryId" TO "FK_GloJobTypeCategorySubCategory_GloJobTypeSubCategory_SubCategoryId";
                ALTER INDEX dbo."IX_GloCategorySubCategory_CategoryId" RENAME TO "IX_GloJobTypeCategorySubCategory_CategoryId";
                ALTER INDEX dbo."IX_GloCategorySubCategory_SubCategoryId" RENAME TO "IX_GloJobTypeCategorySubCategory_SubCategoryId";
                """);

            migrationBuilder.Sql("""
                INSERT INTO dbo."FgsUserRole" ("UserId", "TenantId", "CompanyId", "GloRoleId", "CreatedOn")
                SELECT u."Id", u."TenantId", u."CompanyId", r."Id", NOW()
                FROM dbo."FgsUser" u
                INNER JOIN dbo."GloRole" r ON r."RoleCode" = 'TENANT_ADMIN'
                WHERE u."Role" = 'Admin'
                  AND NOT EXISTS (
                      SELECT 1
                      FROM dbo."FgsUserRole" ur
                      WHERE ur."UserId" = u."Id"
                        AND ur."GloRoleId" = r."Id");
                """);

            migrationBuilder.DropColumn(
                name: "Role",
                schema: "dbo",
                table: "FgsUser");

            migrationBuilder.CreateTable(
                name: "FgsEntityTag",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    TagId = table.Column<long>(type: "bigint", nullable: false),
                    MasterEntityTypeId = table.Column<int>(type: "integer", nullable: false),
                    EntityId = table.Column<long>(type: "bigint", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsEntityTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsEntityTag_FgsTag_TagId",
                        column: x => x.TagId,
                        principalSchema: "dbo",
                        principalTable: "FgsTag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsEntityTag_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsEntityTag_GloMasterEntityType_MasterEntityTypeId",
                        column: x => x.MasterEntityTypeId,
                        principalSchema: "dbo",
                        principalTable: "GloMasterEntityType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsTagEntityType",
                schema: "dbo",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    TagId = table.Column<long>(type: "bigint", nullable: false),
                    MasterEntityTypeId = table.Column<int>(type: "integer", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsTagEntityType", x => new { x.TenantId, x.CompanyId, x.TagId, x.MasterEntityTypeId });
                    table.ForeignKey(
                        name: "FK_FgsTagEntityType_FgsTag_TagId",
                        column: x => x.TagId,
                        principalSchema: "dbo",
                        principalTable: "FgsTag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsTagEntityType_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsTagEntityType_GloMasterEntityType_MasterEntityTypeId",
                        column: x => x.MasterEntityTypeId,
                        principalSchema: "dbo",
                        principalTable: "GloMasterEntityType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEntityTag_CreatedOn",
                schema: "dbo",
                table: "FgsEntityTag",
                column: "CreatedOn",
                descending: new[] { true });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEntityTag_Entity",
                schema: "dbo",
                table: "FgsEntityTag",
                columns: new[] { "TenantId", "CompanyId", "MasterEntityTypeId", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEntityTag_MasterEntityTypeId",
                schema: "dbo",
                table: "FgsEntityTag",
                columns: new[] { "TenantId", "CompanyId", "MasterEntityTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEntityTag_TagId",
                schema: "dbo",
                table: "FgsEntityTag",
                columns: new[] { "TenantId", "CompanyId", "TagId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsEntityTag_TenantId_CompanyId_TagId_MasterEntityTypeId_EntityId",
                schema: "dbo",
                table: "FgsEntityTag",
                columns: new[] { "TenantId", "CompanyId", "TagId", "MasterEntityTypeId", "EntityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsTagEntityType_IsDefault",
                schema: "dbo",
                table: "FgsTagEntityType",
                columns: new[] { "TenantId", "CompanyId", "MasterEntityTypeId", "IsDefault" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsTagEntityType_MasterEntityTypeId",
                schema: "dbo",
                table: "FgsTagEntityType",
                columns: new[] { "TenantId", "CompanyId", "MasterEntityTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsTagEntityType_TagId",
                schema: "dbo",
                table: "FgsTagEntityType",
                columns: new[] { "TenantId", "CompanyId", "TagId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsEntityTag",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsTagEntityType",
                schema: "dbo");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                schema: "dbo",
                table: "FgsUser",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Admin");

            migrationBuilder.Sql("""
                ALTER TABLE dbo."GloJobTypeSubCategory" RENAME TO "GloSubCategory";
                ALTER TABLE dbo."GloJobTypeCategory" RENAME TO "GloCategory";
                ALTER TABLE dbo."GloJobTypeCategorySubCategory" RENAME TO "GloCategorySubCategory";

                ALTER TABLE dbo."GloSubCategory" RENAME CONSTRAINT "PK_GloJobTypeSubCategory" TO "PK_GloSubCategory";
                ALTER INDEX dbo."UQ_GloJobTypeSubCategory_Code" RENAME TO "UQ_GloSubCategory_Code";
                ALTER TABLE dbo."GloSubCategory" RENAME CONSTRAINT "CK_GloJobTypeSubCategory_Code_Upper" TO "CK_GloSubCategory_Code_Upper";

                ALTER TABLE dbo."GloCategory" RENAME CONSTRAINT "PK_GloJobTypeCategory" TO "PK_GloCategory";
                ALTER INDEX dbo."UQ_GloJobTypeCategory_BusinessTypeId_Code" RENAME TO "UQ_GloCategory_BusinessTypeId_Code";
                ALTER TABLE dbo."GloCategory" RENAME CONSTRAINT "CK_GloJobTypeCategory_Code_Upper" TO "CK_GloCategory_Code_Upper";
                ALTER TABLE dbo."GloCategory" RENAME CONSTRAINT "FK_GloJobTypeCategory_GloBusinessType_BusinessTypeId" TO "FK_GloCategory_GloBusinessType_BusinessTypeId";

                ALTER TABLE dbo."GloCategorySubCategory" RENAME CONSTRAINT "PK_GloJobTypeCategorySubCategory" TO "PK_GloCategorySubCategory";
                ALTER TABLE dbo."GloCategorySubCategory" RENAME CONSTRAINT "FK_GloJobTypeCategorySubCategory_GloBusinessType_BusinessTypeId" TO "FK_GloCategorySubCategory_GloBusinessType_BusinessTypeId";
                ALTER TABLE dbo."GloCategorySubCategory" RENAME CONSTRAINT "FK_GloJobTypeCategorySubCategory_GloJobTypeCategory_CategoryId" TO "FK_GloCategorySubCategory_GloCategory_CategoryId";
                ALTER TABLE dbo."GloCategorySubCategory" RENAME CONSTRAINT "FK_GloJobTypeCategorySubCategory_GloJobTypeSubCategory_SubCategoryId" TO "FK_GloCategorySubCategory_GloSubCategory_SubCategoryId";
                ALTER INDEX dbo."IX_GloJobTypeCategorySubCategory_CategoryId" RENAME TO "IX_GloCategorySubCategory_CategoryId";
                ALTER INDEX dbo."IX_GloJobTypeCategorySubCategory_SubCategoryId" RENAME TO "IX_GloCategorySubCategory_SubCategoryId";
                """);
        }
    }
}
