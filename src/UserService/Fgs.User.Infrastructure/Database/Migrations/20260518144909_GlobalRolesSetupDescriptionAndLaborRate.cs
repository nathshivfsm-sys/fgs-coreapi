using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class GlobalRolesSetupDescriptionAndLaborRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortNote",
                schema: "dbo",
                table: "FgsSetupDescription",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FgsSetupGLBreakTechTrade",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    FgsSetupGLBreakId = table.Column<long>(type: "bigint", nullable: false),
                    FgsSetupTechTradeId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupGLBreakTechTrade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsSetupGLBreakTechTrade_FgsSetupGLBreak_FgsSetupGLBreakId",
                        column: x => x.FgsSetupGLBreakId,
                        principalSchema: "dbo",
                        principalTable: "FgsSetupGLBreak",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsSetupGLBreakTechTrade_FgsSetupTechTrade_FgsSetupTechTrad~",
                        column: x => x.FgsSetupTechTradeId,
                        principalSchema: "dbo",
                        principalTable: "FgsSetupTechTrade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsSetupGLBreakTechTrade_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GloRole",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    RoleLevel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsAssignable = table.Column<bool>(type: "boolean", nullable: false),
                    IsSystemRole = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloSetupDescriptionType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloSetupDescriptionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloSetupLaborRateType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloSetupLaborRateType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsRole",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    RoleCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    GloRoleId = table.Column<short>(type: "smallint", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsRole_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsRole_GloRole_GloRoleId",
                        column: x => x.GloRoleId,
                        principalSchema: "dbo",
                        principalTable: "GloRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsUserRole",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    GloRoleId = table.Column<short>(type: "smallint", nullable: true),
                    FgsRoleId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsUserRole", x => x.Id);
                    table.CheckConstraint("CK_FgsUserRole_OnlyOneRole", "(\"GloRoleId\" IS NOT NULL AND \"FgsRoleId\" IS NULL) OR (\"GloRoleId\" IS NULL AND \"FgsRoleId\" IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_FgsUserRole_FgsRole_FgsRoleId",
                        column: x => x.FgsRoleId,
                        principalSchema: "dbo",
                        principalTable: "FgsRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsUserRole_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsUserRole_FgsUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "FgsUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsUserRole_GloRole_GloRoleId",
                        column: x => x.GloRoleId,
                        principalSchema: "dbo",
                        principalTable: "GloRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPriceSheetLabor_FgsSetupLaborRateTypeId",
                schema: "dbo",
                table: "FgsSetupPriceSheetLabor",
                column: "FgsSetupLaborRateTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsRole_GloRoleId",
                schema: "dbo",
                table: "FgsRole",
                column: "GloRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsRole_TenantId_CompanyId_RoleCode",
                schema: "dbo",
                table: "FgsRole",
                columns: new[] { "TenantId", "CompanyId", "RoleCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupGLBreakTechTrade_FgsSetupGLBreakId_FgsSetupTechTrad~",
                schema: "dbo",
                table: "FgsSetupGLBreakTechTrade",
                columns: new[] { "FgsSetupGLBreakId", "FgsSetupTechTradeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupGLBreakTechTrade_FgsSetupTechTradeId",
                schema: "dbo",
                table: "FgsSetupGLBreakTechTrade",
                column: "FgsSetupTechTradeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupGLBreakTechTrade_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupGLBreakTechTrade",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsUserRole_FgsRoleId",
                schema: "dbo",
                table: "FgsUserRole",
                column: "FgsRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsUserRole_GloRoleId",
                schema: "dbo",
                table: "FgsUserRole",
                column: "GloRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsUserRole_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsUserRole",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsUserRole_UserId",
                schema: "dbo",
                table: "FgsUserRole",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsUserRole_UserId_FgsRoleId",
                schema: "dbo",
                table: "FgsUserRole",
                columns: new[] { "UserId", "FgsRoleId" },
                unique: true,
                filter: "\"FgsRoleId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FgsUserRole_UserId_GloRoleId",
                schema: "dbo",
                table: "FgsUserRole",
                columns: new[] { "UserId", "GloRoleId" },
                unique: true,
                filter: "\"GloRoleId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GloRole_RoleCode",
                schema: "dbo",
                table: "GloRole",
                column: "RoleCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloSetupDescriptionType_Code",
                schema: "dbo",
                table: "GloSetupDescriptionType",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloSetupLaborRateType_Name",
                schema: "dbo",
                table: "GloSetupLaborRateType",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupPriceSheetLabor_GloSetupLaborRateType_FgsSetupLabor~",
                schema: "dbo",
                table: "FgsSetupPriceSheetLabor",
                column: "FgsSetupLaborRateTypeId",
                principalSchema: "dbo",
                principalTable: "GloSetupLaborRateType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql(
                """
                INSERT INTO dbo."FgsSetupGLBreakTechTrade"
                    ("TenantId", "CompanyId", "FgsSetupGLBreakId", "FgsSetupTechTradeId", "IsActive", "CreatedOn", "CreatedBy")
                SELECT
                    b."TenantId",
                    b."CompanyId",
                    b."Id",
                    b."FgsSetupTechTradeId",
                    true,
                    timezone('utc', now()),
                    'System'
                FROM dbo."FgsSetupGLBreak" b
                WHERE b."FgsSetupTechTradeId" IS NOT NULL
                  AND NOT EXISTS (
                      SELECT 1
                      FROM dbo."FgsSetupGLBreakTechTrade" j
                      WHERE j."FgsSetupGLBreakId" = b."Id"
                        AND j."FgsSetupTechTradeId" = b."FgsSetupTechTradeId");
                """);

            migrationBuilder.DropColumn(
                name: "FgsSetupTechTradeId",
                schema: "dbo",
                table: "FgsSetupGLBreak");

            migrationBuilder.Sql(
                """
                DO $seed$
                DECLARE
                    v_now timestamptz := timezone('utc', now());
                BEGIN
                    INSERT INTO dbo."GloRole"
                        ("RoleCode", "Name", "Description", "RoleLevel", "IsAssignable", "IsSystemRole", "SortOrder", "IsActive", "CreatedOn")
                    VALUES
                        ('SYSTEM_ADMIN', 'System Administrator', 'Full internal platform administration access.', 'SYSTEM', false, true, 1, true, v_now),
                        ('IMPLEMENTATION_SPECIALIST', 'Implementation Specialist', 'Helps onboard and configure customer tenants.', 'SYSTEM', false, true, 2, true, v_now),
                        ('SUPPORT_AGENT', 'Support Agent', 'Provides customer support and troubleshooting.', 'SYSTEM', false, true, 3, true, v_now),
                        ('BILLING_ADMIN', 'Billing Administrator', 'Manages subscriptions, invoices, and customer payments.', 'SYSTEM', false, true, 4, true, v_now),
                        ('SALES_ADMIN', 'Sales Administrator', 'Creates demo tenants and supports sales operations.', 'SYSTEM', false, true, 5, true, v_now),
                        ('READONLY_AUDITOR', 'Readonly Auditor', 'Internal audit and compliance access.', 'SYSTEM', false, true, 6, true, v_now),
                        ('TENANT_ADMIN', 'Tenant Administrator', 'Super administrator for all companies under the tenant.', 'TENANT', false, false, 10, true, v_now),
                        ('COMPANY_ADMIN', 'Company Administrator', 'Full administrator for a single company.', 'COMPANY', true, false, 20, true, v_now),
                        ('OPERATIONS_MANAGER', 'Operations Manager', 'Manages overall company operations.', 'COMPANY', true, false, 21, true, v_now),
                        ('DISPATCHER', 'Dispatcher', 'Schedules and dispatches service work.', 'COMPANY', true, false, 22, true, v_now),
                        ('BILLING', 'Billing Specialist', 'Handles invoicing and billing operations.', 'COMPANY', true, false, 23, true, v_now),
                        ('CSR', 'Customer Service Representative', 'Handles customer communication and service requests.', 'COMPANY', true, false, 24, true, v_now),
                        ('OFFICE_USER', 'Office User', 'Standard office employee with limited access.', 'COMPANY', true, false, 25, true, v_now),
                        ('SERVICE_MANAGER', 'Service Manager', 'Manages all field operations and technicians.', 'FIELD', true, false, 30, true, v_now),
                        ('FIELD_SUPERVISOR', 'Field Supervisor', 'Supervises assigned field technicians and teams.', 'FIELD', true, false, 31, true, v_now),
                        ('FIELD_TECH', 'Field Technician', 'Performs field service work and job completion.', 'FIELD', true, false, 32, true, v_now)
                    ON CONFLICT ("RoleCode") DO NOTHING;

                    INSERT INTO dbo."GloSetupDescriptionType" ("Id", "Code", "Name", "Description", "IsActive", "CreatedOn")
                    VALUES
                        (gen_random_uuid(), 'ReasonForCall', 'Reason For Call', 'Predefined reason for call descriptions', true, v_now),
                        (gen_random_uuid(), 'Recommendations', 'Recommendations', 'Predefined recommendation descriptions', true, v_now),
                        (gen_random_uuid(), 'WorkSummary', 'Work Summary', 'Predefined work summary descriptions', true, v_now),
                        (gen_random_uuid(), 'AgreementDescription', 'Agreement Description', 'Predefined agreement descriptions', true, v_now)
                    ON CONFLICT ("Code") DO NOTHING;

                    INSERT INTO dbo."GloSetupLaborRateType"
                        ("Name", "Description", "SortOrder", "IsSystem", "IsActive", "CreatedOn", "CreatedBy")
                    VALUES
                        ('Regular', 'Standard labor rate', 1, true, true, v_now, 'System'),
                        ('Overtime', 'Overtime labor rate', 2, true, true, v_now, 'System'),
                        ('Double-Time', 'Double-time labor rate', 3, true, true, v_now, 'System'),
                        ('Holiday', 'Holiday labor rate', 4, true, true, v_now, 'System'),
                        ('Weekend', 'Weekend labor rate', 5, true, true, v_now, 'System')
                    ON CONFLICT ("Name") DO NOTHING;

                    PERFORM setval(
                        pg_get_serial_sequence('dbo."GloRole"', 'Id'),
                        COALESCE((SELECT MAX("Id") FROM dbo."GloRole"), 1),
                        true);
                    PERFORM setval(
                        pg_get_serial_sequence('dbo."GloSetupLaborRateType"', 'Id'),
                        COALESCE((SELECT MAX("Id") FROM dbo."GloSetupLaborRateType"), 1),
                        true);
                END $seed$;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DELETE FROM dbo."GloSetupLaborRateType"
                WHERE "Name" IN ('Regular', 'Overtime', 'Double-Time', 'Holiday', 'Weekend');

                DELETE FROM dbo."GloSetupDescriptionType"
                WHERE "Code" IN ('ReasonForCall', 'Recommendations', 'WorkSummary', 'AgreementDescription');

                DELETE FROM dbo."GloRole"
                WHERE "RoleCode" IN (
                    'SYSTEM_ADMIN', 'IMPLEMENTATION_SPECIALIST', 'SUPPORT_AGENT', 'BILLING_ADMIN', 'SALES_ADMIN',
                    'READONLY_AUDITOR', 'TENANT_ADMIN', 'COMPANY_ADMIN', 'OPERATIONS_MANAGER', 'DISPATCHER',
                    'BILLING', 'CSR', 'OFFICE_USER', 'SERVICE_MANAGER', 'FIELD_SUPERVISOR', 'FIELD_TECH');
                """);

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupPriceSheetLabor_GloSetupLaborRateType_FgsSetupLabor~",
                schema: "dbo",
                table: "FgsSetupPriceSheetLabor");

            migrationBuilder.AddColumn<long>(
                name: "FgsSetupTechTradeId",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "bigint",
                nullable: true);

            migrationBuilder.Sql(
                """
                UPDATE dbo."FgsSetupGLBreak" b
                SET "FgsSetupTechTradeId" = j."FgsSetupTechTradeId"
                FROM (
                    SELECT DISTINCT ON ("FgsSetupGLBreakId")
                        "FgsSetupGLBreakId",
                        "FgsSetupTechTradeId"
                    FROM dbo."FgsSetupGLBreakTechTrade"
                    ORDER BY "FgsSetupGLBreakId", "Id"
                ) j
                WHERE b."Id" = j."FgsSetupGLBreakId";
                """);

            migrationBuilder.DropTable(
                name: "FgsSetupGLBreakTechTrade",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsUserRole",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloSetupDescriptionType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloSetupLaborRateType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsRole",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloRole",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupPriceSheetLabor_FgsSetupLaborRateTypeId",
                schema: "dbo",
                table: "FgsSetupPriceSheetLabor");

            migrationBuilder.DropColumn(
                name: "ShortNote",
                schema: "dbo",
                table: "FgsSetupDescription");
        }
    }
}
