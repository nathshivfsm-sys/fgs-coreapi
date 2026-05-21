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
        // Intentionally empty: applied in 20260521145233_GloSetupTenantStatusAndTenantIdBigint.
        // Manual deploy: Database/Scripts/20260522130000_RemoveFgsInvitationIsDeleted_Up.sql
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Intentionally empty: run 20260522130000_RemoveFgsInvitationIsDeleted_Down.sql against the database.
    }
}
