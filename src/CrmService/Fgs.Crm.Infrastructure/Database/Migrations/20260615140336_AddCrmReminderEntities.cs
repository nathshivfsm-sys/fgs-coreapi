using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Crm.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCrmReminderEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CrmReminder",
                schema: "crm",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntityId = table.Column<int>(type: "integer", nullable: true, comment: "Related entity identifier."),
                    EntityValue = table.Column<long>(type: "bigint", nullable: true, comment: "Primary key value of the related business record."),
                    PriorityId = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)2, comment: "Priority. 1=Low, 2=Normal, 3=High, 4=Critical."),
                    StatusId = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Status. 1=Open, 2=Completed, 3=Cancelled."),
                    Subject = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false, comment: "Reminder subject."),
                    ReminderText = table.Column<string>(type: "text", nullable: false, comment: "Reminder details, notes, instructions, or comments."),
                    DueOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, comment: "Date and time the reminder is due."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User identifier of the user who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User identifier of the user who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmReminder", x => x.Id);
                    table.CheckConstraint("CK_CrmReminder_Entity", "(\"EntityId\" IS NULL AND \"EntityValue\" IS NULL) OR (\"EntityId\" IS NOT NULL AND \"EntityValue\" IS NOT NULL)");
                    table.CheckConstraint("CK_CrmReminder_PriorityId", "\"PriorityId\" IN (1, 2, 3, 4)");
                    table.CheckConstraint("CK_CrmReminder_StatusId", "\"StatusId\" IN (1, 2, 3)");
                    table.ForeignKey(
                        name: "FK_CrmReminder_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "crm",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores reminders assigned to users or roles for follow-up, review, approval, notification, and workflow activities.");

            migrationBuilder.CreateTable(
                name: "CrmReminderAssignment",
                schema: "crm",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReminderId = table.Column<long>(type: "bigint", nullable: false, comment: "Related reminder identifier."),
                    UserId = table.Column<long>(type: "bigint", nullable: true, comment: "Assigned user identifier."),
                    RoleId = table.Column<long>(type: "bigint", nullable: true, comment: "Assigned role identifier."),
                    ResponseText = table.Column<string>(type: "text", nullable: true, comment: "Response or completion notes entered by the assignee."),
                    CompletedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the assignment was completed."),
                    CompletedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User identifier of the user who completed the reminder."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User identifier of the user who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User identifier of the user who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmReminderAssignment", x => x.Id);
                    table.CheckConstraint("CK_CrmReminderAssignment_Assignee", "(\"UserId\" IS NOT NULL AND \"RoleId\" IS NULL) OR (\"UserId\" IS NULL AND \"RoleId\" IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_CrmReminderAssignment_CrmReminder_ReminderId",
                        column: x => x.ReminderId,
                        principalSchema: "crm",
                        principalTable: "CrmReminder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CrmReminderAssignment_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "crm",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores user and role assignments for reminders.");

            migrationBuilder.CreateIndex(
                name: "IX_CrmReminder_CreatedBy",
                schema: "crm",
                table: "CrmReminder",
                columns: new[] { "TenantId", "CompanyId", "CreatedBy" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmReminder_EntityId_EntityValue",
                schema: "crm",
                table: "CrmReminder",
                columns: new[] { "TenantId", "CompanyId", "EntityId", "EntityValue" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmReminder_StatusId_DueOn",
                schema: "crm",
                table: "CrmReminder",
                columns: new[] { "TenantId", "CompanyId", "StatusId", "DueOn" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmReminder_TenantId_CompanyId",
                schema: "crm",
                table: "CrmReminder",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmReminderAssignment_ReminderId",
                schema: "crm",
                table: "CrmReminderAssignment",
                columns: new[] { "TenantId", "CompanyId", "ReminderId" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmReminderAssignment_ReminderId1",
                schema: "crm",
                table: "CrmReminderAssignment",
                column: "ReminderId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmReminderAssignment_RoleId",
                schema: "crm",
                table: "CrmReminderAssignment",
                columns: new[] { "TenantId", "CompanyId", "RoleId" },
                filter: "\"RoleId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CrmReminderAssignment_TenantId_CompanyId",
                schema: "crm",
                table: "CrmReminderAssignment",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmReminderAssignment_UserId",
                schema: "crm",
                table: "CrmReminderAssignment",
                columns: new[] { "TenantId", "CompanyId", "UserId" },
                filter: "\"UserId\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrmReminderAssignment",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "CrmReminder",
                schema: "crm");
        }
    }
}
