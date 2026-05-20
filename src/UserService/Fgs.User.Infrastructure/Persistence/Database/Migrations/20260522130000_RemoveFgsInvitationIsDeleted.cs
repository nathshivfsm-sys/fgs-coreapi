using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.User.Infrastructure.Persistence.Database.Migrations;

/// <summary>
/// Drops soft-delete column from <c>FgsInvitation</c> (not used on invitations).
/// Apply: <c>Database/Scripts/20260522130000_RemoveFgsInvitationIsDeleted_Up.sql</c>.
/// </summary>
public partial class RemoveFgsInvitationIsDeleted : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsDeleted",
            schema: "dbo",
            table: "FgsInvitation");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsDeleted",
            schema: "dbo",
            table: "FgsInvitation",
            type: "boolean",
            nullable: false,
            defaultValue: false);
    }
}
