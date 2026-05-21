using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.User.Infrastructure.Persistence.Database.Migrations;

/// <summary>
/// Creates <c>FgsFile</c> with storage uniqueness, entity lookup, and tag search indexes.
/// Schema is applied in <c>20260521145233_GloSetupTenantStatusAndTenantIdBigint</c> (EF) or
/// <c>Database/Scripts/20260522150000_FgsFile_Up.sql</c> (manual).
/// </summary>
public partial class FgsFile : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Intentionally empty: FgsFile DDL is applied in 20260521145233_GloSetupTenantStatusAndTenantIdBigint.
        // Manual deploy: Database/Scripts/20260522150000_FgsFile_Up.sql
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Intentionally empty: run 20260522150000_FgsFile_Down.sql against the database.
    }
}
