using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Scheduling.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class MoveCrewToSetupAndAddDispatchBoardTechnician : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FgsAppointment_Crew",
                schema: "dispatch",
                table: "FgsAppointment");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsAppointmentAssignment_Crew",
                schema: "dispatch",
                table: "FgsAppointmentAssignment");

            migrationBuilder.DropTable(
                name: "FgsCrewMember",
                schema: "dispatch");

            migrationBuilder.DropTable(
                name: "FgsCrew",
                schema: "dispatch");

            migrationBuilder.AlterColumn<long>(
                name: "CrewId",
                schema: "dispatch",
                table: "FgsAppointmentAssignment",
                type: "bigint",
                nullable: true,
                comment: "Crew assignment snapshot at the time of scheduling. References setup service; no FK by design.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Crew assignment snapshot at the time of scheduling.");

            migrationBuilder.AlterColumn<long>(
                name: "CrewId",
                schema: "dispatch",
                table: "FgsAppointment",
                type: "bigint",
                nullable: true,
                comment: "Scheduled crew assigned to the appointment. References setup service; no FK by design.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Scheduled crew assigned to the appointment.");

            migrationBuilder.CreateTable(
                name: "FgsDispatchBoardTechnician",
                schema: "dispatch",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    ServiceDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "Service date represented on the dispatch board."),
                    TechnicianProfileId = table.Column<long>(type: "bigint", nullable: false, comment: "Reference to setup.FgsEmployeeTechnicianProfile. Stored without cross-domain foreign key."),
                    TechCode = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false, comment: "Technician code snapshot used by dispatch board displays."),
                    TechName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Technician name snapshot used by dispatch board displays."),
                    CrewId = table.Column<long>(type: "bigint", nullable: true, comment: "Daily crew assignment identifier. May be overridden for a specific service date."),
                    CrewCode = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true, comment: "Crew code snapshot used for dispatch board grouping."),
                    CrewName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Crew name snapshot used for dispatch board grouping."),
                    DispatchBoardStatusId = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0, comment: "Dispatch board status. 0=Available, 1=Assigned, 2=Dispatched, 3=Arrived, 4=Waiting, 5=Completed, 6=Off Duty."),
                    IsWorking = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the technician should appear on the dispatch board for the service date."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsDispatchBoardTechnician", x => x.Id);
                    table.CheckConstraint("CK_FgsDispatchBoardTechnician_Status", "\"DispatchBoardStatusId\" IN (0, 1, 2, 3, 4, 5, 6)");
                    table.ForeignKey(
                        name: "FK_FgsDispatchBoardTechnician_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dispatch",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores daily dispatch board technician projections used for scheduling and dispatching.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsDispatchBoardTechnician_Crew",
                schema: "dispatch",
                table: "FgsDispatchBoardTechnician",
                columns: new[] { "TenantId", "CompanyId", "ServiceDate", "CrewId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsDispatchBoardTechnician_ServiceDate",
                schema: "dispatch",
                table: "FgsDispatchBoardTechnician",
                columns: new[] { "TenantId", "CompanyId", "ServiceDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsDispatchBoardTechnician_TenantCompany",
                schema: "dispatch",
                table: "FgsDispatchBoardTechnician",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsDispatchBoardTechnician_ServiceDate_TechnicianProfileId",
                schema: "dispatch",
                table: "FgsDispatchBoardTechnician",
                columns: new[] { "TenantId", "CompanyId", "ServiceDate", "TechnicianProfileId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsDispatchBoardTechnician",
                schema: "dispatch");

            migrationBuilder.AlterColumn<long>(
                name: "CrewId",
                schema: "dispatch",
                table: "FgsAppointmentAssignment",
                type: "bigint",
                nullable: true,
                comment: "Crew assignment snapshot at the time of scheduling.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Crew assignment snapshot at the time of scheduling. References setup service; no FK by design.");

            migrationBuilder.AlterColumn<long>(
                name: "CrewId",
                schema: "dispatch",
                table: "FgsAppointment",
                type: "bigint",
                nullable: true,
                comment: "Scheduled crew assigned to the appointment.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Scheduled crew assigned to the appointment. References setup service; no FK by design.");

            migrationBuilder.CreateTable(
                name: "FgsCrew",
                schema: "dispatch",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false, comment: "User who created the record."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CrewCode = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false, comment: "Short unique crew code used on dispatch boards, reports and integrations."),
                    CrewName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Display name of the crew."),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Optional crew description."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the crew is available for scheduling and dispatching."),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true, comment: "User who last updated the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsCrew", x => x.Id);
                    table.UniqueConstraint("UX_FgsCrew_TenantCompany_Id", x => new { x.TenantId, x.CompanyId, x.Id });
                    table.ForeignKey(
                        name: "FK_FgsCrew_FgsTenantCompanyCache",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dispatch",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Represents a technician crew used for scheduling, dispatching and workload management.");

            migrationBuilder.CreateTable(
                name: "FgsCrewMember",
                schema: "dispatch",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false, comment: "User who created the record."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CrewId = table.Column<long>(type: "bigint", nullable: false, comment: "Crew associated with the technician."),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false, comment: "Employee assigned to the crew. References user service; no FK by design."),
                    IsLead = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the employee is the lead technician or foreman for the crew."),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true, comment: "User who last updated the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsCrewMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsCrewMember_FgsCrew",
                        column: x => x.CrewId,
                        principalSchema: "dispatch",
                        principalTable: "FgsCrew",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsCrewMember_FgsTenantCompanyCache",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dispatch",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores technician membership within a crew.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsCrew_IsActive",
                schema: "dispatch",
                table: "FgsCrew",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsCrew_TenantCompany",
                schema: "dispatch",
                table: "FgsCrew",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsCrew_CrewCode",
                schema: "dispatch",
                table: "FgsCrew",
                columns: new[] { "TenantId", "CompanyId", "CrewCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_FgsCrew_CrewName",
                schema: "dispatch",
                table: "FgsCrew",
                columns: new[] { "TenantId", "CompanyId", "CrewName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsCrewMember_CrewId",
                schema: "dispatch",
                table: "FgsCrewMember",
                column: "CrewId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsCrewMember_IsLead",
                schema: "dispatch",
                table: "FgsCrewMember",
                columns: new[] { "TenantId", "CompanyId", "CrewId", "IsLead" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsCrewMember_TenantCompany",
                schema: "dispatch",
                table: "FgsCrewMember",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsCrewMember_Employee",
                schema: "dispatch",
                table: "FgsCrewMember",
                columns: new[] { "TenantId", "CompanyId", "EmployeeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_FgsCrewMember_LeadPerCrew",
                schema: "dispatch",
                table: "FgsCrewMember",
                columns: new[] { "TenantId", "CompanyId", "CrewId" },
                unique: true,
                filter: "\"IsLead\" = true");

            migrationBuilder.AddForeignKey(
                name: "FK_FgsAppointment_Crew",
                schema: "dispatch",
                table: "FgsAppointment",
                columns: new[] { "TenantId", "CompanyId", "CrewId" },
                principalSchema: "dispatch",
                principalTable: "FgsCrew",
                principalColumns: new[] { "TenantId", "CompanyId", "Id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsAppointmentAssignment_Crew",
                schema: "dispatch",
                table: "FgsAppointmentAssignment",
                columns: new[] { "TenantId", "CompanyId", "CrewId" },
                principalSchema: "dispatch",
                principalTable: "FgsCrew",
                principalColumns: new[] { "TenantId", "CompanyId", "Id" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
