using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddMenuAndTermsConditionEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsTermsCondition",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Surrogate primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant that owns the terms and conditions."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company within the tenant that owns the terms and conditions."),
                    Code = table.Column<string>(type: "text", nullable: false, comment: "Code identifying the terms and conditions definition. Multiple versions can exist for the same code."),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Display name of the terms and conditions."),
                    VersionNumber = table.Column<int>(type: "integer", nullable: false, comment: "Sequential version number of the terms and conditions."),
                    TermsText = table.Column<string>(type: "text", nullable: false, comment: "Complete terms and conditions text for this version."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, comment: "UTC timestamp when the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "UTC timestamp when the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that last updated the record."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, comment: "Indicates whether the terms and conditions version is active.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsTermsCondition", x => x.Id);
                    table.UniqueConstraint("UQ_FgsTermsCondition", x => new { x.TenantId, x.CompanyId, x.Code, x.VersionNumber });
                    table.CheckConstraint("CK_FgsTermsCondition_VersionNumber", "\"VersionNumber\" > 0");
                    table.ForeignKey(
                        name: "FK_FgsTermsCondition_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores terms and conditions definitions and their versions for use across estimates, invoices, work authorizations, signatures, and other business entities.");

            migrationBuilder.CreateTable(
                name: "GloMenu",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Unique identifier for the menu item.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MenuCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Unique system-defined code identifying the menu item."),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Display name of the menu item shown to users."),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Description of the menu item and its purpose."),
                    ParentMenuId = table.Column<int>(type: "integer", nullable: true, comment: "References the parent menu item when this menu is a child item; NULL for top-level menus."),
                    MenuType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, comment: "Defines the type of menu item, such as a menu group or navigable page."),
                    Route = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Application route used to navigate to the menu item when applicable."),
                    Icon = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "UI icon identifier associated with the menu item."),
                    SortOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0, comment: "Determines the display order of the menu item within its parent menu."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the menu item is currently active and available for tenant configuration."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "timezone('utc', now())", comment: "UTC timestamp when the menu item was created.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloMenu", x => x.Id);
                    table.UniqueConstraint("UX_GloMenu_MenuCode", x => x.MenuCode);
                    table.CheckConstraint("CK_GloMenu_MenuCode_NotEmpty", "length(trim(\"MenuCode\")) > 0");
                    table.CheckConstraint("CK_GloMenu_Name_NotEmpty", "length(trim(\"Name\")) > 0");
                    table.ForeignKey(
                        name: "FK_GloMenu_ParentMenu",
                        column: x => x.ParentMenuId,
                        principalSchema: "glo",
                        principalTable: "GloMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Global master definition of application menus and navigation items available across the FSM platform.");

            migrationBuilder.CreateTable(
                name: "FgsEntityDefaultTermsCondition",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Surrogate primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant that owns the entity terms and conditions configuration."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company within the tenant for which the default terms and conditions are configured."),
                    EntityType = table.Column<string>(type: "text", nullable: false, comment: "Entity type to which the default terms and conditions version applies, such as Invoice, Estimate, WorkAuthorization, or Signature."),
                    TermsConditionId = table.Column<long>(type: "bigint", nullable: false, comment: "Reference to the specific terms and conditions version that is the default for the entity type."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, comment: "UTC timestamp when the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "UTC timestamp when the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that last updated the record."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, comment: "Indicates whether the default entity terms and conditions mapping is active.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsEntityDefaultTermsCondition", x => x.Id);
                    table.UniqueConstraint("UQ_FgsEntityDefaultTermsCondition", x => new { x.TenantId, x.CompanyId, x.EntityType });
                    table.ForeignKey(
                        name: "FK_FgsEntityDefaultTermsCondition_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsEntityDefaultTermsCondition_FgsTermsCondition_TermsConditionId",
                        column: x => x.TermsConditionId,
                        principalSchema: "setup",
                        principalTable: "FgsTermsCondition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores the default terms and conditions version assigned to each supported entity type for a tenant and company.");

            migrationBuilder.CreateTable(
                name: "GloRoleMenu",
                schema: "glo",
                columns: table => new
                {
                    RoleId = table.Column<short>(type: "smallint", nullable: false, comment: "References the global standard role to which the menu item is assigned."),
                    MenuId = table.Column<int>(type: "integer", nullable: false, comment: "References the global menu item assigned to the role."),
                    SortOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0, comment: "Determines the display order of the menu item for the role."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether this default role-to-menu assignment is active and should be included when seeding tenant role menu assignments."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "timezone('utc', now())", comment: "UTC timestamp when the role-to-menu assignment was created.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloRoleMenu", x => new { x.RoleId, x.MenuId });
                    table.ForeignKey(
                        name: "FK_GloRoleMenu_Menu",
                        column: x => x.MenuId,
                        principalSchema: "glo",
                        principalTable: "GloMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GloRoleMenu_Role",
                        column: x => x.RoleId,
                        principalSchema: "glo",
                        principalTable: "GloRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Global default mapping of standard roles to menu items used to seed tenant role menu assignments during onboarding.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEntityDefaultTermsCondition_TenantId_CompanyId",
                schema: "setup",
                table: "FgsEntityDefaultTermsCondition",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEntityDefaultTermsCondition_TermsConditionId",
                schema: "setup",
                table: "FgsEntityDefaultTermsCondition",
                column: "TermsConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsTermsCondition_TenantId_CompanyId_Code",
                schema: "setup",
                table: "FgsTermsCondition",
                columns: new[] { "TenantId", "CompanyId", "Code" });

            migrationBuilder.CreateIndex(
                name: "IX_GloMenu_ParentMenuId",
                schema: "glo",
                table: "GloMenu",
                column: "ParentMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_GloRoleMenu_MenuId",
                schema: "glo",
                table: "GloRoleMenu",
                column: "MenuId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsEntityDefaultTermsCondition",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "GloRoleMenu",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "FgsTermsCondition",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "GloMenu",
                schema: "glo");
        }
    }
}
