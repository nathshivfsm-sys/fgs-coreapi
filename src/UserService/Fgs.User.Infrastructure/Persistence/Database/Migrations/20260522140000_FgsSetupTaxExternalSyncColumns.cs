using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.User.Infrastructure.Persistence.Database.Migrations;

/// <summary>
/// Adds external sync and tax-detail display columns to <c>FgsSetupTax</c>.
/// Apply: <c>Database/Scripts/20260522140000_FgsSetupTaxExternalSyncColumns_Up.sql</c>.
/// </summary>
public partial class FgsSetupTaxExternalSyncColumns : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "ExternalSystemId",
            schema: "dbo",
            table: "FgsSetupTax",
            type: "character varying(200)",
            maxLength: 200,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "SyncToken",
            schema: "dbo",
            table: "FgsSetupTax",
            type: "character varying(100)",
            maxLength: 100,
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "ShowTaxDetail",
            schema: "dbo",
            table: "FgsSetupTax",
            type: "boolean",
            nullable: false,
            defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "ShowTaxDetail",
            schema: "dbo",
            table: "FgsSetupTax");

        migrationBuilder.DropColumn(
            name: "SyncToken",
            schema: "dbo",
            table: "FgsSetupTax");

        migrationBuilder.DropColumn(
            name: "ExternalSystemId",
            schema: "dbo",
            table: "FgsSetupTax");
    }
}
