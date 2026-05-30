using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddBillingCategoryAllowToPickAndGloCommunicationTokenSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "UQ_FgsBillingCategory_TenantId_CompanyId_BillingCategoryType",
                schema: "dbo",
                table: "FgsBillingCategory");

            migrationBuilder.AlterTable(
                name: "FgsBillingCategory",
                schema: "dbo",
                comment: "Stores tenant/company specific billing categories used for invoicing, service billing, maintenance plans, and other billing operations. Seeded initially from GloBillingCategory but fully managed by each tenant/company independently.");

            migrationBuilder.AddColumn<string>(
                name: "SourceDatabaseName",
                schema: "dbo",
                table: "GloCommunicationToken",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceSchemaName",
                schema: "dbo",
                table: "GloCommunicationToken",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "AllowToPick",
                schema: "dbo",
                table: "GloBillingCategory",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                comment: "Determines whether office users are allowed to manually select this billing category during estimate, invoice, or billing entry. Categories such as tax may be system controlled and not manually selectable.");

            migrationBuilder.AddColumn<bool>(
                name: "ShowToFieldTech",
                schema: "dbo",
                table: "GloBillingCategory",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                comment: "Determines whether field technicians can view/select this billing category in mobile and field workflows.");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "dbo",
                table: "FgsBillingCategory",
                type: "timestamptz",
                nullable: true,
                comment: "Date and time the billing category record was last updated.",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                schema: "dbo",
                table: "FgsBillingCategory",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "User identifier that last updated the billing category record.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsBillingCategory",
                type: "bigint",
                nullable: false,
                comment: "Tenant identifier owning this billing category.",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<bool>(
                name: "ShowToFieldTech",
                schema: "dbo",
                table: "FgsBillingCategory",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Indicates whether the billing category is visible to field technicians in mobile and field service applications.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsSystemDefined",
                schema: "dbo",
                table: "FgsBillingCategory",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Indicates whether the billing category was system seeded or manually created by the tenant/company.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "dbo",
                table: "FgsBillingCategory",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                comment: "Indicates whether the billing category is active and available for use.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<short>(
                name: "DisplayOrder",
                schema: "dbo",
                table: "FgsBillingCategory",
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
                table: "FgsBillingCategory",
                type: "text",
                nullable: true,
                comment: "Optional internal description or notes for the billing category.",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "dbo",
                table: "FgsBillingCategory",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()",
                comment: "Date and time the billing category record was created.",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                schema: "dbo",
                table: "FgsBillingCategory",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "User identifier that created the billing category record.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsBillingCategory",
                type: "bigint",
                nullable: false,
                comment: "Company identifier within the tenant owning this billing category.",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "BillingCategoryType",
                schema: "dbo",
                table: "FgsBillingCategory",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                comment: "Short billing category code such as IN, PM, SR, or other tenant-defined values.",
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2);

            migrationBuilder.AlterColumn<string>(
                name: "BillingCategoryName",
                schema: "dbo",
                table: "FgsBillingCategory",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                comment: "Display name of the billing category shown throughout the application.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsBillingCategory",
                type: "bigint",
                nullable: false,
                comment: "Primary key identity of the billing category record.",
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<bool>(
                name: "AllowToPick",
                schema: "dbo",
                table: "FgsBillingCategory",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                comment: "Determines whether office users are allowed to manually select this billing category during estimate, invoice, or billing entry. Categories such as tax may be system controlled and not manually selectable.");

            migrationBuilder.AddUniqueConstraint(
                name: "UQ_FgsBillingCategory_TenantId_CompanyId_BillingCategoryType",
                schema: "dbo",
                table: "FgsBillingCategory",
                columns: new[] { "TenantId", "CompanyId", "BillingCategoryType", "BillingCategoryName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "UQ_FgsBillingCategory_TenantId_CompanyId_BillingCategoryType",
                schema: "dbo",
                table: "FgsBillingCategory");

            migrationBuilder.DropColumn(
                name: "SourceDatabaseName",
                schema: "dbo",
                table: "GloCommunicationToken");

            migrationBuilder.DropColumn(
                name: "SourceSchemaName",
                schema: "dbo",
                table: "GloCommunicationToken");

            migrationBuilder.DropColumn(
                name: "AllowToPick",
                schema: "dbo",
                table: "GloBillingCategory");

            migrationBuilder.DropColumn(
                name: "ShowToFieldTech",
                schema: "dbo",
                table: "GloBillingCategory");

            migrationBuilder.DropColumn(
                name: "AllowToPick",
                schema: "dbo",
                table: "FgsBillingCategory");

            migrationBuilder.AlterTable(
                name: "FgsBillingCategory",
                schema: "dbo",
                oldComment: "Stores tenant/company specific billing categories used for invoicing, service billing, maintenance plans, and other billing operations. Seeded initially from GloBillingCategory but fully managed by each tenant/company independently.");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "dbo",
                table: "FgsBillingCategory",
                type: "timestamptz",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldNullable: true,
                oldComment: "Date and time the billing category record was last updated.");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                schema: "dbo",
                table: "FgsBillingCategory",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "User identifier that last updated the billing category record.");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "dbo",
                table: "FgsBillingCategory",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Tenant identifier owning this billing category.");

            migrationBuilder.AlterColumn<bool>(
                name: "ShowToFieldTech",
                schema: "dbo",
                table: "FgsBillingCategory",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false,
                oldComment: "Indicates whether the billing category is visible to field technicians in mobile and field service applications.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSystemDefined",
                schema: "dbo",
                table: "FgsBillingCategory",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false,
                oldComment: "Indicates whether the billing category was system seeded or manually created by the tenant/company.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "dbo",
                table: "FgsBillingCategory",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true,
                oldComment: "Indicates whether the billing category is active and available for use.");

            migrationBuilder.AlterColumn<short>(
                name: "DisplayOrder",
                schema: "dbo",
                table: "FgsBillingCategory",
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
                table: "FgsBillingCategory",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Optional internal description or notes for the billing category.");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "dbo",
                table: "FgsBillingCategory",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "now()",
                oldComment: "Date and time the billing category record was created.");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                schema: "dbo",
                table: "FgsBillingCategory",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "User identifier that created the billing category record.");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "dbo",
                table: "FgsBillingCategory",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Company identifier within the tenant owning this billing category.");

            migrationBuilder.AlterColumn<string>(
                name: "BillingCategoryType",
                schema: "dbo",
                table: "FgsBillingCategory",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2,
                oldComment: "Short billing category code such as IN, PM, SR, or other tenant-defined values.");

            migrationBuilder.AlterColumn<string>(
                name: "BillingCategoryName",
                schema: "dbo",
                table: "FgsBillingCategory",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldComment: "Display name of the billing category shown throughout the application.");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "dbo",
                table: "FgsBillingCategory",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Primary key identity of the billing category record.")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddUniqueConstraint(
                name: "UQ_FgsBillingCategory_TenantId_CompanyId_BillingCategoryType",
                schema: "dbo",
                table: "FgsBillingCategory",
                columns: new[] { "TenantId", "CompanyId", "BillingCategoryType" });
        }
    }
}
