using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFgsEmployeeAddressIdToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Existing bigint values cannot map to FgsLocation (uuid). Clear then convert.
            migrationBuilder.Sql(
                """
                ALTER TABLE setup."FgsEmployee"
                ALTER COLUMN "AddressId" DROP DEFAULT;

                ALTER TABLE setup."FgsEmployee"
                ALTER COLUMN "AddressId" TYPE uuid
                USING NULL;

                COMMENT ON COLUMN setup."FgsEmployee"."AddressId"
                    IS 'Optional reference to the employee mailing or home address in FgsLocation. No FK by design.';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                ALTER TABLE setup."FgsEmployee"
                ALTER COLUMN "AddressId" DROP DEFAULT;

                ALTER TABLE setup."FgsEmployee"
                ALTER COLUMN "AddressId" TYPE bigint
                USING NULL;

                COMMENT ON COLUMN setup."FgsEmployee"."AddressId"
                    IS 'Reference to the employee mailing or home address record. No FK by design.';
                """);
        }
    }
}
