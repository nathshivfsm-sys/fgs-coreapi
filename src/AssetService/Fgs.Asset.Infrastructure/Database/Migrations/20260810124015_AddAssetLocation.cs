using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Asset.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddAssetLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                ALTER TABLE asset."FgsAsset"
                ADD COLUMN IF NOT EXISTS "AssetLocation" character varying(200);

                COMMENT ON COLUMN asset."FgsAsset"."AssetLocation"
                IS 'Physical location of the asset within the service location or unit, such as Roof - Northeast Corner, Mechanical Room, 2nd Floor West Wing, or Basement.';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                ALTER TABLE asset."FgsAsset"
                DROP COLUMN IF EXISTS "AssetLocation";
                """);
        }
    }
}
