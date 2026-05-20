using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.User.Infrastructure.Persistence.Database.Migrations;

/// <summary>
/// Pricing matrix tables (replaces FgsSetupPriceSheet*). Apply the paired SQL script:
/// <c>Database/Scripts/20260520120000_PricingMatrix_Up.sql</c>.
/// </summary>
public partial class PricingMatrix : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Intentionally empty: run 20260520120000_PricingMatrix_Up.sql against the database.
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Intentionally empty: run 20260520120000_PricingMatrix_Down.sql against the database.
    }
}
