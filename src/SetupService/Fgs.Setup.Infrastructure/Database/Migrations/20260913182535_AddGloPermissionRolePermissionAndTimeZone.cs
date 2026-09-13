using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddGloPermissionRolePermissionAndTimeZone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GloPermission",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier for the global permission.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PermissionCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Unique system identifier for the permission. Used internally by the application and should not be changed after creation."),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false, comment: "Display name of the permission shown to administrators."),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Optional description explaining what the permission allows the user to do."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0, comment: "Controls the display order of permissions within the permission management interface."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the permission is currently available for assignment to roles."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "timezone('utc', now())", comment: "UTC timestamp when the global permission was created.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloPermission", x => x.Id);
                    table.UniqueConstraint("UX_GloPermission_PermissionCode", x => x.PermissionCode);
                    table.CheckConstraint("CK_GloPermission_Name_NotEmpty", "length(trim(\"Name\")) > 0");
                    table.CheckConstraint("CK_GloPermission_PermissionCode_NotEmpty", "length(trim(\"PermissionCode\")) > 0");
                },
                comment: "Global catalog of permissions supported by the FGS platform. Permissions define the actions that can be assigned to security roles.");

            migrationBuilder.CreateTable(
                name: "GloTimeZone",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false, comment: "Unique identifier for the time zone.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TimeZoneCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, comment: "Short time zone abbreviation used by the application, such as EST, CST, MST, or PST."),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "User-friendly display name for the time zone."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the time zone is currently available for selection.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloTimeZone", x => x.Id);
                },
                comment: "Global reference table containing time zone options available for customer and user selection.");

            migrationBuilder.CreateTable(
                name: "GloRolePermission",
                schema: "glo",
                columns: table => new
                {
                    RoleId = table.Column<short>(type: "smallint", nullable: false, comment: "References the global standard role to which the permission is assigned."),
                    PermissionId = table.Column<long>(type: "bigint", nullable: false, comment: "References the global permission assigned to the role."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether this default role-to-permission assignment is active and should be included when seeding tenant role permission assignments."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "timezone('utc', now())", comment: "UTC timestamp when the role-to-permission assignment was created.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloRolePermission", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_GloRolePermission_Permission",
                        column: x => x.PermissionId,
                        principalSchema: "glo",
                        principalTable: "GloPermission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GloRolePermission_Role",
                        column: x => x.RoleId,
                        principalSchema: "glo",
                        principalTable: "GloRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Global default mapping of standard roles to permissions used to seed tenant role permission assignments during onboarding.");

            migrationBuilder.CreateIndex(
                name: "IX_GloPermission_Name",
                schema: "glo",
                table: "GloPermission",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_GloRolePermission_PermissionId",
                schema: "glo",
                table: "GloRolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_GloTimeZone_TimeZoneCode",
                schema: "glo",
                table: "GloTimeZone",
                column: "TimeZoneCode",
                unique: true);

            migrationBuilder.Sql(
                """
                INSERT INTO glo."GloTimeZone" ("TimeZoneCode", "Name", "IsActive")
                SELECT v."TimeZoneCode", v."Name", TRUE
                FROM (VALUES
                    ('EST', 'Eastern Time'),
                    ('CST', 'Central Time'),
                    ('MST', 'Mountain Time'),
                    ('PST', 'Pacific Time'),
                    ('AKST', 'Alaska Time'),
                    ('HST', 'Hawaii Time')
                ) AS v("TimeZoneCode", "Name")
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM glo."GloTimeZone" t
                    WHERE t."TimeZoneCode" = v."TimeZoneCode"
                );
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DELETE FROM glo."GloTimeZone"
                WHERE "TimeZoneCode" IN ('EST', 'CST', 'MST', 'PST', 'AKST', 'HST');
                """);

            migrationBuilder.DropTable(
                name: "GloRolePermission",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloTimeZone",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloPermission",
                schema: "glo");
        }
    }
}
