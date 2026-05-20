using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations;

/// <summary>
/// Changes <c>FgsSetupPricingMatrixLaborTier</c> and <c>FgsSetupPricingMatrixMaterialTier</c> PK to integer identity.
/// Apply: <c>Database/Scripts/20260520140000_PricingMatrixTierIntId_Up.sql</c>.
/// </summary>
public partial class PricingMatrixTierIntId : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Intentionally empty: run 20260520140000_PricingMatrixTierIntId_Up.sql against the database.
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Intentionally empty: run 20260520140000_PricingMatrixTierIntId_Down.sql against the database.
    }
}
