using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFgsTenantCompanySchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessTypeId",
                schema: "tenant",
                table: "FgsTenantCompany");

            migrationBuilder.DropColumn(
                name: "FaviconUrl",
                schema: "tenant",
                table: "FgsTenantCompany");

            migrationBuilder.DropColumn(
                name: "IconLogoUrl",
                schema: "tenant",
                table: "FgsTenantCompany");

            migrationBuilder.Sql(
                """
                ALTER TABLE tenant."FgsTenantCompany"
                ALTER COLUMN "FullLogoUrl" TYPE bigint
                USING "FullLogoUrl"::bigint;
                """);

            migrationBuilder.Sql(
                """
                ALTER TABLE tenant."FgsTenantCompany"
                ALTER COLUMN "CompactLogoUrl" TYPE bigint
                USING "CompactLogoUrl"::bigint;
                """);

            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                schema: "tenant",
                table: "FgsTenantCompany",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeZone",
                schema: "tenant",
                table: "FgsTenantCompany");

            migrationBuilder.AlterColumn<string>(
                name: "FullLogoUrl",
                schema: "tenant",
                table: "FgsTenantCompany",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompactLogoUrl",
                schema: "tenant",
                table: "FgsTenantCompany",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BusinessTypeId",
                schema: "tenant",
                table: "FgsTenantCompany",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FaviconUrl",
                schema: "tenant",
                table: "FgsTenantCompany",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconLogoUrl",
                schema: "tenant",
                table: "FgsTenantCompany",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);
        }
    }
}
