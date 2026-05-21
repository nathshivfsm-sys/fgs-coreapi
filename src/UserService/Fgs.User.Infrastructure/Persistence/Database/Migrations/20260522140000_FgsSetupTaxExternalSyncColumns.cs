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
        // Intentionally empty: applied in 20260521145233_GloSetupTenantStatusAndTenantIdBigint.
        // Manual deploy: Database/Scripts/20260522140000_FgsSetupTaxExternalSyncColumns_Up.sql
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Intentionally empty: run 20260522140000_FgsSetupTaxExternalSyncColumns_Down.sql against the database.
    }
}
