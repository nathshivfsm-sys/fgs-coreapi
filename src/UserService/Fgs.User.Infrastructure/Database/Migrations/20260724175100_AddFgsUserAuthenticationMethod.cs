using Fgs.User.Domain.Enums;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations;

[DbContext(typeof(FgsUserDbContext))]
[Migration("20260724175100_AddFgsUserAuthenticationMethod")]
public partial class AddFgsUserAuthenticationMethod : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<short>(
            name: "AuthenticationMethod",
            schema: "identity",
            table: "FgsUser",
            type: "smallint",
            nullable: false,
            defaultValue: (short)AuthenticationMethod.PasswordOrEmailOtp);

        migrationBuilder.AddCheckConstraint(
            name: "CK_FgsUser_AuthenticationMethod",
            schema: "identity",
            table: "FgsUser",
            sql: "\"AuthenticationMethod\" IN (1, 2, 3, 4, 5)");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropCheckConstraint(
            name: "CK_FgsUser_AuthenticationMethod",
            schema: "identity",
            table: "FgsUser");

        migrationBuilder.DropColumn(
            name: "AuthenticationMethod",
            schema: "identity",
            table: "FgsUser");
    }
}
