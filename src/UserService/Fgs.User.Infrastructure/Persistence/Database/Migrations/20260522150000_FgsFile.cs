using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Persistence.Database.Migrations;

/// <summary>
/// Creates <c>FgsFile</c> with storage uniqueness, entity lookup, and tag search indexes.
/// Apply: <c>Database/Scripts/20260522150000_FgsFile_Up.sql</c>.
/// </summary>
public partial class FgsFile : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "FgsFile",
            schema: "dbo",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                CompanyId = table.Column<long>(type: "bigint", nullable: false),
                EntityType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                EntityId = table.Column<long>(type: "bigint", nullable: false),
                BucketName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                ObjectKey = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                ThumbnailObjectKey = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                OriginalFileName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                StoredFileName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                ContentType = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                FileExtension = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                Description = table.Column<string>(type: "text", nullable: true),
                Tags = table.Column<string[]>(type: "text[]", nullable: true),
                IsVisibleToCustomer = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                IsVisibleToFieldTechnician = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                UploadedByUserId = table.Column<long>(type: "bigint", nullable: true),
                UploadedByName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                UploadedByType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FgsFile", x => x.Id);
                table.ForeignKey(
                    name: "FK_FgsFile_FgsTenantCompany_TenantId_CompanyId",
                    columns: x => new { x.TenantId, x.CompanyId },
                    principalSchema: "dbo",
                    principalTable: "FgsTenantCompany",
                    principalColumns: new[] { "TenantId", "CompanyNumber" },
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_FgsFile_Entity",
            schema: "dbo",
            table: "FgsFile",
            columns: new[] { "TenantId", "CompanyId", "EntityType", "EntityId" });

        migrationBuilder.CreateIndex(
            name: "IX_FgsFile_Tags",
            schema: "dbo",
            table: "FgsFile",
            column: "Tags")
            .Annotation("Npgsql:IndexMethod", "gin");

        migrationBuilder.CreateIndex(
            name: "IX_FgsFile_TenantId_CompanyId",
            schema: "dbo",
            table: "FgsFile",
            columns: new[] { "TenantId", "CompanyId" });

        migrationBuilder.CreateIndex(
            name: "UX_FgsFile_Bucket_ObjectKey",
            schema: "dbo",
            table: "FgsFile",
            columns: new[] { "BucketName", "ObjectKey" },
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "FgsFile",
            schema: "dbo");
    }
}
