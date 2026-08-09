using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddPostalCodeGeoTripChargeDropTimeCardOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GloTimeCardOption",
                schema: "glo");

            migrationBuilder.AlterColumn<bool>(
                name: "IsLaborTierStructure",
                schema: "setup",
                table: "FgsSetupPricingMatrix",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Indicates whether labor pricing in this pricing matrix is based on labor tiers. When false, standard labor pricing rules are applied. When true, labor charges are calculated using the configured labor tier structure.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "City",
                schema: "setup",
                table: "FgsSetupPostalCode",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                comment: "Primary city or municipality associated with the postal code.");

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                schema: "setup",
                table: "FgsSetupPostalCode",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "",
                comment: "ISO 3166-1 alpha-2 country code associated with the postal code (for example, US, CA, MX).");

            migrationBuilder.AddColumn<string>(
                name: "StateProvinceCode",
                schema: "setup",
                table: "FgsSetupPostalCode",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                comment: "State, province, or territory code associated with the postal code (for example, TX, ON, BC).");

            migrationBuilder.AddColumn<decimal>(
                name: "TripChargeAmount",
                schema: "setup",
                table: "FgsSetupPostalCode",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                comment: "Default trip charge applied when providing service to this postal code. Used by dispatching, estimating, and pricing calculations.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                schema: "setup",
                table: "FgsSetupPostalCode");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                schema: "setup",
                table: "FgsSetupPostalCode");

            migrationBuilder.DropColumn(
                name: "StateProvinceCode",
                schema: "setup",
                table: "FgsSetupPostalCode");

            migrationBuilder.DropColumn(
                name: "TripChargeAmount",
                schema: "setup",
                table: "FgsSetupPostalCode");

            migrationBuilder.AlterColumn<bool>(
                name: "IsLaborTierStructure",
                schema: "setup",
                table: "FgsSetupPricingMatrix",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false,
                oldComment: "Indicates whether labor pricing in this pricing matrix is based on labor tiers. When false, standard labor pricing rules are applied. When true, labor charges are calculated using the configured labor tier structure.");

            migrationBuilder.CreateTable(
                name: "GloTimeCardOption",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloTimeCardOption", x => x.Id);
                    table.UniqueConstraint("UQ_GloTimeCardOption_Code", x => x.Code);
                    table.CheckConstraint("CK_GloTimeCardOption_Code_Upper", "\"Code\" = UPPER(\"Code\")");
                });
        }
    }
}
