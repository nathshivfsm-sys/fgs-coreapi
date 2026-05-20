using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations;

/// <summary>
/// Aligns the EF model snapshot with catalog unique/check constraints and column defaults
/// (UX_GloRole_RoleCode, UQ_GloSetupDescriptionType_Code, UQ_GloSetupLaborRateType_Name, etc.).
/// Apply: <c>Database/Scripts/20260519155306_AlignGloCatalogModel_Up.sql</c>.
/// </summary>
public partial class AlignGloCatalogModel : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Intentionally empty: run 20260519155306_AlignGloCatalogModel_Up.sql against the database.
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Intentionally empty: run 20260519155306_AlignGloCatalogModel_Down.sql against the database.
    }
}
