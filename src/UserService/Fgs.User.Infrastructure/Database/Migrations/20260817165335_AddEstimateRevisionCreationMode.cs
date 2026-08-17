using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddEstimateRevisionCreationMode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EstimateRevisionCreationMode",
                schema: "tenant",
                table: "FgsTenantServiceSetup",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "OnDemand",
                comment: "Controls when estimate revisions are created. Valid values: OnDemand = user manually creates a revision; OnPostSignatureChange = automatically creates a revision when a signed estimate is changed.");

            migrationBuilder.AddCheckConstraint(
                name: "CK_FgsTenantServiceSetup_EstimateRevisionCreationMode",
                schema: "tenant",
                table: "FgsTenantServiceSetup",
                sql: "\"EstimateRevisionCreationMode\" IN ('OnDemand', 'OnPostSignatureChange')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_FgsTenantServiceSetup_EstimateRevisionCreationMode",
                schema: "tenant",
                table: "FgsTenantServiceSetup");

            migrationBuilder.DropColumn(
                name: "EstimateRevisionCreationMode",
                schema: "tenant",
                table: "FgsTenantServiceSetup");
        }
    }
}
