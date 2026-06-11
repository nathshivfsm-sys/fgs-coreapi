using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Asset.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "asset");

            migrationBuilder.Sql(@"COMMENT ON SCHEMA asset IS 'Asset Management Domain

Stores customer-owned equipment and installed assets.

Examples:
- Air Conditioners
- Furnaces
- Water Heaters
- Boilers
- Generators

Assets are shared business entities referenced by:
- Service Agreements
- Estimates
- Jobs
- Invoices
- Inspections

Asset definitions such as Asset Type, Manufacturer and Model are stored in the setup schema.'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("COMMENT ON SCHEMA asset IS NULL;");
        }
    }
}
