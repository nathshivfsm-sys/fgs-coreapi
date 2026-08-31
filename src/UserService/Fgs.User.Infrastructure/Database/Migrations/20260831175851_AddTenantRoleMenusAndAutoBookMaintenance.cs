using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantRoleMenusAndAutoBookMaintenance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AutoBookMaintenanceScheduleCalls",
                schema: "tenant",
                table: "FgsTenantServiceSetup",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Controls whether maintenance schedule calls are automatically booked. TRUE = maintenance schedule calls are automatically booked; FALSE = maintenance schedule calls must be booked manually.");

            migrationBuilder.CreateTable(
                name: "FgsRoleMenu",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier for the role menu assignment.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant that owns the role menu assignment."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company within the tenant that owns the role menu assignment."),
                    RoleId = table.Column<long>(type: "bigint", nullable: false, comment: "Role that is granted access to the menu item."),
                    MenuId = table.Column<int>(type: "integer", nullable: false, comment: "Global menu item that the role is allowed to access."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls the display order of the menu item for the role."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, comment: "Date and time the role menu assignment was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or system that created the role menu assignment."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the role menu assignment was last modified."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or system that last modified the role menu assignment."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the role currently has access to the menu item.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsRoleMenu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsRoleMenu_FgsRole",
                        column: x => x.RoleId,
                        principalSchema: "identity",
                        principalTable: "FgsRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsRoleMenu_FgsTenantCompanyCache",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "identity",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores the menu items assigned to each role within a tenant company and defines which navigation items the role can access.");

            migrationBuilder.CreateTable(
                name: "FgsTenantMenu",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier for the tenant menu assignment.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant that owns the menu assignment."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company within the tenant that receives the menu item."),
                    MenuId = table.Column<int>(type: "integer", nullable: false, comment: "Global menu item assigned to the tenant company."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls the display order of the menu item for the tenant company."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, comment: "Date and time the tenant menu assignment was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or system that created the tenant menu assignment."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the tenant menu assignment was last modified."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or system that last modified the tenant menu assignment."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the menu item is currently available to the tenant company.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsTenantMenu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsTenantMenu_FgsTenantCompanyCache",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "identity",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores the menu items enabled for a company within a tenant based on the tenant subscription and available platform features.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsRoleMenu_RoleId",
                schema: "identity",
                table: "FgsRoleMenu",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsRoleMenu_TenantId_CompanyId_RoleId_IsActive",
                schema: "identity",
                table: "FgsRoleMenu",
                columns: new[] { "TenantId", "CompanyId", "RoleId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsRoleMenu_TenantId_CompanyId_RoleId_MenuId",
                schema: "identity",
                table: "FgsRoleMenu",
                columns: new[] { "TenantId", "CompanyId", "RoleId", "MenuId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsTenantMenu_TenantId_CompanyId_IsActive",
                schema: "identity",
                table: "FgsTenantMenu",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsTenantMenu_TenantId_CompanyId_MenuId",
                schema: "identity",
                table: "FgsTenantMenu",
                columns: new[] { "TenantId", "CompanyId", "MenuId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsRoleMenu",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "FgsTenantMenu",
                schema: "identity");

            migrationBuilder.DropColumn(
                name: "AutoBookMaintenanceScheduleCalls",
                schema: "tenant",
                table: "FgsTenantServiceSetup");
        }
    }
}
