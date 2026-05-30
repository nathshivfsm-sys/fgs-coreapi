using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelCommentsAndIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "GloBillingCategory",
                schema: "dbo",
                comment: "Global billing line category lookup used during tenant provisioning (equipment, labor, tax, etc.).");

            migrationBuilder.AlterTable(
                name: "FgsSetupGLBreakTrade",
                schema: "dbo",
                comment: "Stores trade-to-GL-break mappings used for financial segmentation and reporting.");

            migrationBuilder.AlterTable(
                name: "FgsSetupGLBreak",
                schema: "dbo",
                comment: "Stores GL break configuration for financial reporting segmentation by trade, division, branch, or organizational unit.");

            migrationBuilder.AlterColumn<short>(
                name: "DisplayOrder",
                schema: "dbo",
                table: "GloBillingCategory",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1,
                comment: "Controls sorting/display order of billing categories in dropdowns and setup screens.",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)1);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "GloBillingCategory",
                type: "text",
                nullable: true,
                comment: "Optional description of how the billing category is used.",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BillingCategoryName",
                schema: "dbo",
                table: "GloBillingCategory",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                comment: "Display name of the billing category.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "BillingCategoryType",
                schema: "dbo",
                table: "GloBillingCategory",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                comment: "Short billing category code (primary key), e.g. IN, LB, TX.",
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2);

            migrationBuilder.AlterColumn<string>(
                name: "TradeCode",
                schema: "dbo",
                table: "FgsSetupGLBreakTrade",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                comment: "Technician or operational trade code associated with the GL break such as HVAC, Plumbing, Electrical, or Drain.",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupGLBreakTrade",
                type: "bigint",
                nullable: false,
                comment: "Owning tenant identifier.",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "GLBreakId",
                schema: "dbo",
                table: "FgsSetupGLBreakTrade",
                type: "bigint",
                nullable: false,
                comment: "Reference to the associated GL break configuration.",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "dbo",
                table: "FgsSetupGLBreakTrade",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()",
                comment: "UTC timestamp when the record was created.",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                schema: "dbo",
                table: "FgsSetupGLBreakTrade",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "User or process that created the record.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupGLBreakTrade",
                type: "bigint",
                nullable: false,
                comment: "Tenant-scoped company number.",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupGLBreakTrade",
                type: "bigint",
                nullable: false,
                comment: "Surrogate primary key.",
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "timestamptz",
                nullable: true,
                comment: "UTC timestamp when the record was last updated.",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "User or process that last updated the record.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "text",
                nullable: false,
                comment: "Display name of the GL break.",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<long>(
                name: "LogoFileId",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "bigint",
                nullable: true,
                comment: "Optional reference to uploaded logo file in FgsFile.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "boolean",
                nullable: false,
                comment: "Indicates whether the GL break is active.",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "timestamptz",
                nullable: false,
                comment: "UTC timestamp when the record was created.",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "User or process that created the record.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "text",
                nullable: false,
                comment: "Unique GL break code within tenant, company, and break level scope.",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<short>(
                name: "BreakLevel",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "smallint",
                nullable: false,
                comment: "Break hierarchy level. Allowed values: 1 or 2.",
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<string>(
                name: "BreakLabel",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "text",
                nullable: true,
                comment: "Optional label displayed in UI and financial documents.",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "uuid",
                nullable: true,
                comment: "Optional reference to branch or break address in FgsLocation.",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "bigint",
                nullable: false,
                comment: "Surrogate primary key.",
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "GloBillingCategory",
                schema: "dbo",
                oldComment: "Global billing line category lookup used during tenant provisioning (equipment, labor, tax, etc.).");

            migrationBuilder.AlterTable(
                name: "FgsSetupGLBreakTrade",
                schema: "dbo",
                oldComment: "Stores trade-to-GL-break mappings used for financial segmentation and reporting.");

            migrationBuilder.AlterTable(
                name: "FgsSetupGLBreak",
                schema: "dbo",
                oldComment: "Stores GL break configuration for financial reporting segmentation by trade, division, branch, or organizational unit.");

            migrationBuilder.AlterColumn<short>(
                name: "DisplayOrder",
                schema: "dbo",
                table: "GloBillingCategory",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)1,
                oldComment: "Controls sorting/display order of billing categories in dropdowns and setup screens.");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "dbo",
                table: "GloBillingCategory",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Optional description of how the billing category is used.");

            migrationBuilder.AlterColumn<string>(
                name: "BillingCategoryName",
                schema: "dbo",
                table: "GloBillingCategory",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldComment: "Display name of the billing category.");

            migrationBuilder.AlterColumn<string>(
                name: "BillingCategoryType",
                schema: "dbo",
                table: "GloBillingCategory",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2,
                oldComment: "Short billing category code (primary key), e.g. IN, LB, TX.");

            migrationBuilder.AlterColumn<string>(
                name: "TradeCode",
                schema: "dbo",
                table: "FgsSetupGLBreakTrade",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldComment: "Technician or operational trade code associated with the GL break such as HVAC, Plumbing, Electrical, or Drain.");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsSetupGLBreakTrade",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Owning tenant identifier.");

            migrationBuilder.AlterColumn<long>(
                name: "GLBreakId",
                schema: "dbo",
                table: "FgsSetupGLBreakTrade",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Reference to the associated GL break configuration.");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "dbo",
                table: "FgsSetupGLBreakTrade",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "now()",
                oldComment: "UTC timestamp when the record was created.");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                schema: "dbo",
                table: "FgsSetupGLBreakTrade",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "User or process that created the record.");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsSetupGLBreakTrade",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Tenant-scoped company number.");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupGLBreakTrade",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Surrogate primary key.")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "timestamptz",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldNullable: true,
                oldComment: "UTC timestamp when the record was last updated.");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "User or process that last updated the record.");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Display name of the GL break.");

            migrationBuilder.AlterColumn<long>(
                name: "LogoFileId",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Optional reference to uploaded logo file in FgsFile.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "Indicates whether the GL break is active.");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "timestamptz",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldComment: "UTC timestamp when the record was created.");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "User or process that created the record.");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Unique GL break code within tenant, company, and break level scope.");

            migrationBuilder.AlterColumn<short>(
                name: "BreakLevel",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldComment: "Break hierarchy level. Allowed values: 1 or 2.");

            migrationBuilder.AlterColumn<string>(
                name: "BreakLabel",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Optional label displayed in UI and financial documents.");

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "Optional reference to branch or break address in FgsLocation.");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Surrogate primary key.")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
