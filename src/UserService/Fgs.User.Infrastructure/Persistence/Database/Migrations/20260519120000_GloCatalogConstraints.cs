using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.User.Infrastructure.Persistence.Database.Migrations;

/// <summary>
/// Catalog constraint alignment. Apply the paired SQL script:
/// <c>Database/Scripts/20260519120000_GloCatalogConstraints_Up.sql</c>.
/// </summary>
public partial class GloCatalogConstraints : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Intentionally empty: run 20260519120000_GloCatalogConstraints_Up.sql against the database.
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Intentionally empty: run 20260519120000_GloCatalogConstraints_Down.sql against the database.
    }
}
