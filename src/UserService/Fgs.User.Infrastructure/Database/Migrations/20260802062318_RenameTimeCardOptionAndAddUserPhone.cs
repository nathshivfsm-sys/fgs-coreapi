using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class RenameTimeCardOptionAndAddUserPhone : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "GloTimeCardOptionId",
            schema: "tenant",
            table: "FgsTenantServiceSetup",
            newName: "TimeCardOptionId");

        migrationBuilder.AlterColumn<short>(
            name: "TimeCardOptionId",
            schema: "tenant",
            table: "FgsTenantServiceSetup",
            type: "smallint",
            nullable: false,
            comment: "Determines the technician time tracking workflow. Valid values: 1 = No formal technician time tracking workflow, 2 = Technician manually checks in and checks out, 3 = Tracks dispatch, arrival, and completion timestamps, 4 = Tracks dispatch, arrival, completion, and documentation time timestamps.",
            oldClrType: typeof(int),
            oldType: "integer");

        // Remap legacy GloTimeCardOption seed ids to new enum semantics:
        // 1=NONE→None, 2=DISPATCHARRIVECOMPLETE→DispatchArriveComplete(3), 3=CHECKINCHECKOUT→CheckInCheckOut(2)
        migrationBuilder.Sql("""
            UPDATE tenant."FgsTenantServiceSetup"
            SET "TimeCardOptionId" = CASE "TimeCardOptionId"
                WHEN 1 THEN 1
                WHEN 2 THEN 3
                WHEN 3 THEN 2
                ELSE 1
            END;
            """);

        migrationBuilder.AddCheckConstraint(
            name: "CK_FgsTenantServiceSetup_TimeCardOptionId",
            schema: "tenant",
            table: "FgsTenantServiceSetup",
            sql: "\"TimeCardOptionId\" IN (1, 2, 3, 4)");

        migrationBuilder.AddColumn<string>(
            name: "PhoneNumber",
            schema: "identity",
            table: "FgsUser",
            type: "character varying(20)",
            maxLength: 20,
            nullable: true,
            comment: "Primary phone number used for SMS notifications and one-time password (OTP) verification when multi-factor authentication (MFA) using SMS is enabled.");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "PhoneNumber",
            schema: "identity",
            table: "FgsUser");

        migrationBuilder.DropCheckConstraint(
            name: "CK_FgsTenantServiceSetup_TimeCardOptionId",
            schema: "tenant",
            table: "FgsTenantServiceSetup");

        migrationBuilder.Sql("""
            UPDATE tenant."FgsTenantServiceSetup"
            SET "TimeCardOptionId" = CASE "TimeCardOptionId"
                WHEN 1 THEN 1
                WHEN 2 THEN 3
                WHEN 3 THEN 2
                ELSE 1
            END;
            """);

        migrationBuilder.AlterColumn<int>(
            name: "TimeCardOptionId",
            schema: "tenant",
            table: "FgsTenantServiceSetup",
            type: "integer",
            nullable: false,
            oldClrType: typeof(short),
            oldType: "smallint",
            oldComment: "Determines the technician time tracking workflow. Valid values: 1 = No formal technician time tracking workflow, 2 = Technician manually checks in and checks out, 3 = Tracks dispatch, arrival, and completion timestamps, 4 = Tracks dispatch, arrival, completion, and documentation time timestamps.");

        migrationBuilder.RenameColumn(
            name: "TimeCardOptionId",
            schema: "tenant",
            table: "FgsTenantServiceSetup",
            newName: "GloTimeCardOptionId");
    }
}
