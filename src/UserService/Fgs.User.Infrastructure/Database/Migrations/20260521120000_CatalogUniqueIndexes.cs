using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations;

/// <summary>
/// UX_* unique constraints on GloAccountingIntegrationType, GloBusinessType, FgsTenantCompany.
/// Apply: <c>Database/Scripts/20260521120000_CatalogUniqueIndexes_Up.sql</c>.
/// </summary>
public partial class CatalogUniqueIndexes : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Intentionally empty: run 20260521120000_CatalogUniqueIndexes_Up.sql against the database.
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Intentionally empty: run 20260521120000_CatalogUniqueIndexes_Down.sql against the database.
    }
}
