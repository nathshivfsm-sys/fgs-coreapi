using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Persistence.Database.Migrations;

/// <summary>
/// <c>GloSetupTenantStatus</c>, <c>FgsTenant.FgsTenantStatusId</c>, and TenantId bigint alignment.
/// For manual deploy also run:
/// <c>Database/Scripts/20260521145233_GloSetupTenantStatusAndTenantIdBigint_Up.sql</c>
/// (seed rows) before relying on the FK default.
/// </summary>
public partial class GloSetupTenantStatusAndTenantIdBigint : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "dbo",
                table: "FgsInvitation");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsUserRole",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsUser",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsTenantServiceSetup",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsTenantCompany",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsTenant",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<short>(
                name: "FgsTenantStatusId",
                schema: "dbo",
                table: "FgsTenant",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1)
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupZone",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTitleOfCourtesy",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTimeSlot",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTechTrade",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTechSkillLevel",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTaxDetail",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTaxAuthority",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTax",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "ExternalSystemId",
                schema: "dbo",
                table: "FgsSetupTax",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowTaxDetail",
                schema: "dbo",
                table: "FgsSetupTax",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SyncToken",
                schema: "dbo",
                table: "FgsSetupTax",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupServiceAssetType",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupServiceAssetModelReference",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupServiceAssetManufacturer",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPricingMatrixOther",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPricingMatrixMaterialTier",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupPricingMatrixMaterialTier",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPricingMatrixLaborTier",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupPricingMatrixLaborTier",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPricingMatrixLabor",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPricingMatrix",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPostalCode",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPaymentTerm",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupGLBreakTechTrade",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupDescription",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupCommunicationTemplate",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsRole",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsResolutionCode",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsLocation",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsInvitation",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateTable(
                name: "FgsFile",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
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

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsCredentialSecret",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsCredentialProvider",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsCredentialAudit",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateTable(
                name: "GloSetupTenantStatus",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloSetupTenantStatus", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FgsTenant_FgsTenantStatusId",
                schema: "dbo",
                table: "FgsTenant",
                column: "FgsTenantStatusId");

            migrationBuilder.CreateIndex(
                name: "UX_GloSetupTenantStatus_Name",
                schema: "dbo",
                table: "GloSetupTenantStatus",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsTenant_GloSetupTenantStatus",
                schema: "dbo",
                table: "FgsTenant",
                column: "FgsTenantStatusId",
                principalSchema: "dbo",
                principalTable: "GloSetupTenantStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FgsTenant_GloSetupTenantStatus",
                schema: "dbo",
                table: "FgsTenant");

            migrationBuilder.DropTable(
                name: "GloSetupTenantStatus",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_FgsTenant_FgsTenantStatusId",
                schema: "dbo",
                table: "FgsTenant");

            migrationBuilder.DropColumn(
                name: "FgsTenantStatusId",
                schema: "dbo",
                table: "FgsTenant");

            migrationBuilder.DropColumn(
                name: "ExternalSystemId",
                schema: "dbo",
                table: "FgsSetupTax");

            migrationBuilder.DropColumn(
                name: "ShowTaxDetail",
                schema: "dbo",
                table: "FgsSetupTax");

            migrationBuilder.DropColumn(
                name: "SyncToken",
                schema: "dbo",
                table: "FgsSetupTax");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsUserRole",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsUser",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsTenantServiceSetup",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsTenantCompany",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "dbo",
                table: "FgsTenant",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupZone",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTitleOfCourtesy",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTimeSlot",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTechTrade",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTechSkillLevel",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTaxDetail",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTaxAuthority",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupTax",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupServiceAssetType",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupServiceAssetModelReference",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupServiceAssetManufacturer",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPricingMatrixOther",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPricingMatrixMaterialTier",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupPricingMatrixMaterialTier",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPricingMatrixLaborTier",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupPricingMatrixLaborTier",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPricingMatrixLabor",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPricingMatrix",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPostalCode",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPaymentTerm",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupGLBreakTechTrade",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupDescription",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupCommunicationTemplate",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsRole",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsResolutionCode",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsLocation",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsInvitation",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "dbo",
                table: "FgsInvitation",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.DropTable(
                name: "FgsFile",
                schema: "dbo");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsCredentialSecret",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsCredentialProvider",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsCredentialAudit",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
    }
}
