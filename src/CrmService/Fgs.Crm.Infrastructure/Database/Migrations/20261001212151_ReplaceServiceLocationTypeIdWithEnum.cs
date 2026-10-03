using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Crm.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceServiceLocationTypeIdWithEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServiceLocationTypeId",
                schema: "crm",
                table: "CrmServiceLocation",
                newName: "ServiceLocationType");

            migrationBuilder.Sql(
                """
                UPDATE crm."CrmServiceLocation"
                SET "ServiceLocationType" = 5
                WHERE "ServiceLocationType" NOT IN (1, 2, 3, 4, 5);
                """);

            migrationBuilder.AlterColumn<short>(
                name: "ServiceLocationType",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "smallint",
                nullable: false,
                comment: "Specifies the type of the service location. Valid values: 1=Residential, 2=Commercial, 3=Industrial, 4=Government, 5=Other. Corresponds to the ServiceLocationType enum in the application.",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)0,
                oldComment: "Identifier of the service location type.");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CrmServiceLocation_ServiceLocationType",
                schema: "crm",
                table: "CrmServiceLocation",
                sql: "\"ServiceLocationType\" IN (1, 2, 3, 4, 5)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_CrmServiceLocation_ServiceLocationType",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.AlterColumn<short>(
                name: "ServiceLocationType",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0,
                comment: "Identifier of the service location type.",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldComment: "Specifies the type of the service location. Valid values: 1=Residential, 2=Commercial, 3=Industrial, 4=Government, 5=Other. Corresponds to the ServiceLocationType enum in the application.");

            migrationBuilder.RenameColumn(
                name: "ServiceLocationType",
                schema: "crm",
                table: "CrmServiceLocation",
                newName: "ServiceLocationTypeId");
        }
    }
}
