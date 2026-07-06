using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class DropCredentialKeyIdentifier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KeyIdentifier",
                schema: "glo",
                table: "GloCredential");

            migrationBuilder.DropColumn(
                name: "KeyIdentifier",
                schema: "setup",
                table: "FgsCredential");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KeyIdentifier",
                schema: "glo",
                table: "GloCredential",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KeyIdentifier",
                schema: "setup",
                table: "FgsCredential",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                comment: "AWS KMS key ARN or alias used to encrypt the Data Encryption Key.");
        }
    }
}
