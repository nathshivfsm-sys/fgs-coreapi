using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Scheduling.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddDispatchSchedulingSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsCrew",
                schema: "dispatch",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    CrewCode = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false, comment: "Short unique crew code used on dispatch boards, reports and integrations."),
                    CrewName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Display name of the crew."),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Optional crew description."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the crew is available for scheduling and dispatching."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true, comment: "User who last updated the record.")
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
                name: "FgsPayrollPayPeriod",
                schema: "dispatch",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    PayPeriodCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, comment: "Human-readable payroll period code such as 2026-PP12, 2026-06A or 2026-06B."),
                    PeriodStartDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "Inclusive payroll period start date."),
                    PeriodEndDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "Inclusive payroll period end date."),
                    PayrollStatusId = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Payroll status. 1=Open, 2=Calculated, 3=Approved, 4=Exported."),
                    CalculatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time payroll calculations were generated."),
                    CalculatedBy = table.Column<long>(type: "bigint", nullable: true, comment: "User who generated payroll calculations."),
                    ApprovedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time payroll was approved."),
                    ApprovedBy = table.Column<long>(type: "bigint", nullable: true, comment: "User who approved payroll."),
                    ExportedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time payroll was exported."),
                    ExportedBy = table.Column<long>(type: "bigint", nullable: true, comment: "User who exported payroll."),
                    ExportReference = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Optional external payroll batch number, export file identifier or payroll provider reference."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsPayrollPayPeriod", x => x.Id);
                    table.CheckConstraint("CK_FgsPayrollPayPeriod_Approved", "(\"ApprovedOn\" IS NULL AND \"ApprovedBy\" IS NULL) OR (\"ApprovedOn\" IS NOT NULL AND \"ApprovedBy\" IS NOT NULL)");
                    table.CheckConstraint("CK_FgsPayrollPayPeriod_Calculated", "(\"CalculatedOn\" IS NULL AND \"CalculatedBy\" IS NULL) OR (\"CalculatedOn\" IS NOT NULL AND \"CalculatedBy\" IS NOT NULL)");
                    table.CheckConstraint("CK_FgsPayrollPayPeriod_DateRange", "\"PeriodEndDate\" >= \"PeriodStartDate\"");
                    table.CheckConstraint("CK_FgsPayrollPayPeriod_Exported", "(\"ExportedOn\" IS NULL AND \"ExportedBy\" IS NULL) OR (\"ExportedOn\" IS NOT NULL AND \"ExportedBy\" IS NOT NULL)");
                    table.CheckConstraint("CK_FgsPayrollPayPeriod_Status", "\"PayrollStatusId\" IN (1, 2, 3, 4)");
                    table.ForeignKey(
                        name: "FK_FgsPayrollPayPeriod_FgsTenantCompanyCache",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dispatch",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Defines payroll processing periods used to calculate, approve and export payroll.");

            migrationBuilder.CreateTable(
                name: "FgsWorkOrder",
                schema: "dispatch",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkOrderNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Unique work order number within tenant and company."),
                    ProjectId = table.Column<long>(type: "bigint", nullable: true, comment: "Optional project identifier. References project service; no FK by design."),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false, comment: "Customer identifier. References CRM service; no FK by design."),
                    LocationId = table.Column<long>(type: "bigint", nullable: false, comment: "Service location identifier. References CRM service; no FK by design."),
                    ServiceAgreementId = table.Column<long>(type: "bigint", nullable: true, comment: "Service agreement identifier. References service agreement service; no FK by design."),
                    ServiceAgreementVisitId = table.Column<long>(type: "bigint", nullable: true, comment: "Service agreement visit identifier. References service agreement service; no FK by design."),
                    Break1Id = table.Column<long>(type: "bigint", nullable: true, comment: "Primary break classification identifier. References setup service; no FK by design."),
                    Break2Id = table.Column<long>(type: "bigint", nullable: true, comment: "Secondary break classification identifier. References setup service; no FK by design."),
                    JobTypeId = table.Column<long>(type: "bigint", nullable: false, comment: "Job type identifier. References setup.FgsJobType through application logic; no FK by design."),
                    PriorityId = table.Column<long>(type: "bigint", nullable: false, comment: "Priority identifier. References setup service; no FK by design."),
                    WorkOrderStatusId = table.Column<long>(type: "bigint", nullable: false, comment: "Work order status. New, Started, Completed, or Cancelled."),
                    WorkOrderResolutionId = table.Column<long>(type: "bigint", nullable: true, comment: "Completion or cancellation reason identifier. References setup service; no FK by design."),
                    TimeSlotId = table.Column<long>(type: "bigint", nullable: true, comment: "Promised time window. References setup.FgsSetupTimeSlot through application logic; no FK by design."),
                    CustomerPO = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Customer purchase order reference."),
                    PersonCalling = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Name of person who called to request service."),
                    PersonCallingPhoneNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true, comment: "Phone number of person who called."),
                    ContactPerson = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Onsite contact person name."),
                    ContactPersonPhoneNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true, comment: "Onsite contact person phone number."),
                    ProblemDescription = table.Column<string>(type: "text", nullable: true, comment: "Customer problem description."),
                    Note = table.Column<string>(type: "text", nullable: true, comment: "Special instructions for technicians."),
                    MaterialPricingMatrixId = table.Column<long>(type: "bigint", nullable: true, comment: "Material pricing matrix identifier. References setup service; no FK by design."),
                    LaborPricingMatrixId = table.Column<long>(type: "bigint", nullable: true, comment: "Labor pricing matrix identifier. References setup service; no FK by design."),
                    OtherPricingMatrixId = table.Column<long>(type: "bigint", nullable: true, comment: "Other pricing matrix identifier. References setup service; no FK by design."),
                    PaymentMethodId = table.Column<long>(type: "bigint", nullable: true, comment: "Payment method identifier. References setup service; no FK by design."),
                    EstimatedHours = table.Column<decimal>(type: "numeric(8,2)", nullable: true, comment: "Estimated hours for the work order."),
                    RequestedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, comment: "Date and time the work order was requested."),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Work order start date and time."),
                    EndDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Work order end date and time."),
                    Source = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Source of the work order such as Manual, Portal, API, Corrigo, ServiceChannel, Verizon, AHS, etc."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsWorkOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsWorkOrder_TenantCompany",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dispatch",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Master work order record representing a customer service request that can be scheduled through one or more appointments.");

            migrationBuilder.CreateTable(
                name: "FgsAppointment",
                schema: "dispatch",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SourceTypeId = table.Column<short>(type: "smallint", nullable: false, comment: "Source type. Typically Lead, Opportunity or Work Order."),
                    SourceId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the source record."),
                    CrewId = table.Column<long>(type: "bigint", nullable: true, comment: "Scheduled crew assigned to the appointment."),
                    CustomerContactName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Contact name used for appointment reminders and confirmations."),
                    ServiceDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "Customer promised service date."),
                    ScheduledTime = table.Column<TimeOnly>(type: "time", nullable: false, comment: "Customer promised local appointment time."),
                    EstimatedHours = table.Column<decimal>(type: "numeric(8,2)", nullable: false, comment: "Estimated appointment duration used for scheduling and dispatch planning."),
                    AppointmentStatusId = table.Column<short>(type: "smallint", nullable: false, comment: "Appointment status. 1=Unassigned, 2=Open, 3=Completed."),
                    CustomerApprovedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time customer approved the appointment visit."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsAppointment", x => x.Id);
                    table.CheckConstraint("CK_FgsAppointment_EstimatedHours", "\"EstimatedHours\" > 0");
                    table.CheckConstraint("CK_FgsAppointment_Status", "\"AppointmentStatusId\" IN (1, 2, 3)");
                    table.ForeignKey(
                        name: "FK_FgsAppointment_Crew",
                        columns: x => new { x.TenantId, x.CompanyId, x.CrewId },
                        principalSchema: "dispatch",
                        principalTable: "FgsCrew",
                        principalColumns: new[] { "TenantId", "CompanyId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsAppointment_TenantCompany",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dispatch",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Represents a scheduled customer visit for a lead, opportunity or work order.");

            migrationBuilder.CreateTable(
                name: "FgsCrewMember",
                schema: "dispatch",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    CrewId = table.Column<long>(type: "bigint", nullable: false, comment: "Crew associated with the technician."),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false, comment: "Employee assigned to the crew. References user service; no FK by design."),
                    IsLead = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the employee is the lead technician or foreman for the crew."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true, comment: "User who last updated the record.")
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

            migrationBuilder.CreateTable(
                name: "FgsPayroll",
                schema: "dispatch",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    PayPeriodId = table.Column<long>(type: "bigint", nullable: false, comment: "Payroll pay period associated with this payroll record."),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false, comment: "Employee associated with this payroll record. References user service; no FK by design."),
                    EmployeeNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Employee number snapshot captured at payroll calculation time."),
                    EmployeeName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Employee name snapshot captured at payroll calculation time."),
                    RegularHours = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Regular hours included in payroll calculation."),
                    OvertimeHours = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Overtime hours included in payroll calculation."),
                    DoubleTimeHours = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Double-time hours included in payroll calculation."),
                    RegularRate = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 0m, comment: "Regular pay rate snapshot at calculation time."),
                    OvertimeRate = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 0m, comment: "Overtime pay rate snapshot at calculation time."),
                    DoubleTimeRate = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 0m, comment: "Double-time pay rate snapshot at calculation time."),
                    RegularAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Regular pay amount."),
                    OvertimeAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Overtime pay amount."),
                    DoubleTimeAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Double-time pay amount."),
                    CommissionAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Commission amount included in payroll."),
                    BonusAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Bonus amount included in payroll."),
                    AdjustmentAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Positive or negative payroll adjustment amount."),
                    BurdenTypeId = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false, defaultValue: "P", comment: "Burden calculation method. P=Percent, F=Fixed Amount."),
                    BurdenValue = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 0m, comment: "Burden percentage or fixed amount snapshot used during payroll calculation."),
                    BurdenAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Calculated burden amount used for costing and profitability reporting."),
                    GrossPayAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Total gross pay exported to the payroll provider."),
                    SignatureFileId = table.Column<long>(type: "bigint", nullable: true, comment: "Reference to employee payroll acknowledgement signature document."),
                    SignedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time payroll acknowledgement was signed."),
                    SignedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Name of person who signed the payroll acknowledgement."),
                    Notes = table.Column<string>(type: "text", nullable: true, comment: "Optional payroll notes, explanations and adjustment reasons."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsPayroll", x => x.Id);
                    table.CheckConstraint("CK_FgsPayroll_BurdenType", "\"BurdenTypeId\" IN ('P', 'F')");
                    table.CheckConstraint("CK_FgsPayroll_Signature", "(\"SignedOn\" IS NULL AND \"SignatureFileId\" IS NULL AND \"SignedBy\" IS NULL) OR (\"SignedOn\" IS NOT NULL AND \"SignatureFileId\" IS NOT NULL AND \"SignedBy\" IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_FgsPayroll_FgsPayrollPayPeriod",
                        column: x => x.PayPeriodId,
                        principalSchema: "dispatch",
                        principalTable: "FgsPayrollPayPeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsPayroll_FgsTenantCompanyCache",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dispatch",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores payroll results for a single employee within a payroll pay period.");

            migrationBuilder.CreateTable(
                name: "FgsWorkOrderAsset",
                schema: "dispatch",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    WorkOrderId = table.Column<long>(type: "bigint", nullable: false, comment: "Parent work order identifier."),
                    AssetId = table.Column<long>(type: "bigint", nullable: false, comment: "Asset identifier. References asset service; no FK by design."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsWorkOrderAsset", x => new { x.TenantId, x.CompanyId, x.WorkOrderId, x.AssetId });
                    table.ForeignKey(
                        name: "FK_FgsWorkOrderAsset_TenantCompany",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dispatch",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsWorkOrderAsset_WorkOrder",
                        column: x => x.WorkOrderId,
                        principalSchema: "dispatch",
                        principalTable: "FgsWorkOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Associates assets with a work order.");

            migrationBuilder.CreateTable(
                name: "FgsWorkOrderIntegration",
                schema: "dispatch",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkOrderId = table.Column<long>(type: "bigint", nullable: true, comment: "Dispatch work order created from this integration record."),
                    IntegrationName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Integration source such as Corrigo, ServiceChannel, Verizon, AHS, etc."),
                    ExternalId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Primary identifier from the external system."),
                    ExternalWorkOrderNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "External work order number visible to users in the external system."),
                    ReceivedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, comment: "Date and time the payload was received from the external system."),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Received", comment: "Current processing status of the imported work order."),
                    Payload = table.Column<string>(type: "jsonb", nullable: false, comment: "Raw JSON payload received from the external system."),
                    ProcessedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was processed or booked into dispatch."),
                    ProcessedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User that processed or booked the work order."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsWorkOrderIntegration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsWorkOrderIntegration_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dispatch",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsWorkOrderIntegration_WorkOrder",
                        column: x => x.WorkOrderId,
                        principalSchema: "dispatch",
                        principalTable: "FgsWorkOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "Stores externally received work orders and their raw payloads before they are reviewed and booked into dispatch.");

            migrationBuilder.CreateTable(
                name: "FgsWorkOrderItem",
                schema: "dispatch",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkOrderId = table.Column<long>(type: "bigint", nullable: false, comment: "Parent work order identifier."),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: true, comment: "Inventory item identifier. May be NULL when the item is manually entered."),
                    ItemName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Item name used when the item does not exist in the inventory catalog."),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Additional item description or technician notes."),
                    Quantity = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 1.0m, comment: "Quantity of material used on the work order."),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 1, comment: "Display order within the work order item list."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsWorkOrderItem", x => x.Id);
                    table.CheckConstraint("CK_FgsWorkOrderItem_Item", "\"InventoryItemId\" IS NOT NULL OR COALESCE(TRIM(BOTH FROM \"ItemName\"), '') <> ''");
                    table.CheckConstraint("CK_FgsWorkOrderItem_Quantity", "\"Quantity\" > 0");
                    table.ForeignKey(
                        name: "FK_FgsWorkOrderItem_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dispatch",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsWorkOrderItem_WorkOrder",
                        column: x => x.WorkOrderId,
                        principalSchema: "dispatch",
                        principalTable: "FgsWorkOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Stores materials used on a work order. Items may come from the inventory catalog or be entered manually. Customer billing is stored separately on invoice lines.");

            migrationBuilder.CreateTable(
                name: "FgsAppointmentAssignment",
                schema: "dispatch",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AppointmentId = table.Column<long>(type: "bigint", nullable: false, comment: "Appointment associated with the assignment."),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false, comment: "Employee assigned to the appointment. References user service; no FK by design."),
                    CrewId = table.Column<long>(type: "bigint", nullable: true, comment: "Crew assignment snapshot at the time of scheduling."),
                    ServiceDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "Scheduled service date for the technician assignment."),
                    ScheduledTime = table.Column<TimeOnly>(type: "time", nullable: false, comment: "Scheduled local start time for the technician assignment."),
                    EstimatedHours = table.Column<decimal>(type: "numeric(8,2)", nullable: false, comment: "Estimated hours assigned to the technician."),
                    ActualStartOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "System-maintained start timestamp derived from assignment events."),
                    ActualEndOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "System-maintained end timestamp derived from assignment events."),
                    AssignedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the technician was assigned."),
                    AssignedBy = table.Column<long>(type: "bigint", nullable: false, comment: "User who assigned the technician."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsAppointmentAssignment", x => x.Id);
                    table.CheckConstraint("CK_FgsAppointmentAssignment_EstimatedHours", "\"EstimatedHours\" > 0");
                    table.ForeignKey(
                        name: "FK_FgsAppointmentAssignment_Appointment",
                        column: x => x.AppointmentId,
                        principalSchema: "dispatch",
                        principalTable: "FgsAppointment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsAppointmentAssignment_Crew",
                        columns: x => new { x.TenantId, x.CompanyId, x.CrewId },
                        principalSchema: "dispatch",
                        principalTable: "FgsCrew",
                        principalColumns: new[] { "TenantId", "CompanyId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsAppointmentAssignment_TenantCompany",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dispatch",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Represents a technician assigned to a scheduled appointment.");

            migrationBuilder.CreateTable(
                name: "FgsPayrollLine",
                schema: "dispatch",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    PayrollId = table.Column<long>(type: "bigint", nullable: false, comment: "Parent payroll record."),
                    PayrollLineTypeId = table.Column<short>(type: "smallint", nullable: false, comment: "Payroll line type. 1=Commission, 2=Bonus, 3=Adjustment."),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false, comment: "User-facing payroll line description."),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Positive or negative payroll line amount."),
                    Notes = table.Column<string>(type: "text", nullable: true, comment: "Optional notes and explanation for the payroll line."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsPayrollLine", x => x.Id);
                    table.CheckConstraint("CK_FgsPayrollLine_Type", "\"PayrollLineTypeId\" IN (1, 2, 3)");
                    table.ForeignKey(
                        name: "FK_FgsPayrollLine_FgsPayroll",
                        column: x => x.PayrollId,
                        principalSchema: "dispatch",
                        principalTable: "FgsPayroll",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsPayrollLine_FgsTenantCompanyCache",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dispatch",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores payroll detail lines associated with a payroll record.");

            migrationBuilder.CreateTable(
                name: "FgsAppointmentAssignmentEvent",
                schema: "dispatch",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    AssignmentId = table.Column<long>(type: "bigint", nullable: true, comment: "Appointment assignment associated with the event. NULL for technician-only events such as On Duty, Off Duty, Lunch Start and Lunch End."),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false, comment: "Employee associated with the event. References user service; no FK by design."),
                    ServiceDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "Business service date associated with the event. Used for overnight work and payroll calculations."),
                    EventTypeId = table.Column<short>(type: "smallint", nullable: false, comment: "References glo.GloAppointmentAssignmentEventType.EventTypeId."),
                    EventOccurredOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, comment: "Actual timestamp when the event occurred."),
                    EnteredByOffice = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates the event was entered or reconstructed by office staff rather than captured by the technician."),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Optional notes entered by office staff or technician."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsAppointmentAssignmentEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsAppointmentAssignmentEvent_FgsAppointmentAssignment",
                        column: x => x.AssignmentId,
                        principalSchema: "dispatch",
                        principalTable: "FgsAppointmentAssignment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsAppointmentAssignmentEvent_FgsTenantCompanyCache",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dispatch",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores technician activity events used for dispatch tracking, payroll calculations, utilization reporting and technician history.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsAppointment_Crew",
                schema: "dispatch",
                table: "FgsAppointment",
                columns: new[] { "TenantId", "CompanyId", "CrewId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAppointment_ServiceDate",
                schema: "dispatch",
                table: "FgsAppointment",
                columns: new[] { "TenantId", "CompanyId", "ServiceDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAppointment_Source",
                schema: "dispatch",
                table: "FgsAppointment",
                columns: new[] { "TenantId", "CompanyId", "SourceTypeId", "SourceId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAppointment_Status",
                schema: "dispatch",
                table: "FgsAppointment",
                columns: new[] { "TenantId", "CompanyId", "AppointmentStatusId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAppointmentAssignment_Appointment",
                schema: "dispatch",
                table: "FgsAppointmentAssignment",
                columns: new[] { "TenantId", "CompanyId", "AppointmentId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAppointmentAssignment_AppointmentId",
                schema: "dispatch",
                table: "FgsAppointmentAssignment",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsAppointmentAssignment_Crew",
                schema: "dispatch",
                table: "FgsAppointmentAssignment",
                columns: new[] { "TenantId", "CompanyId", "CrewId", "ServiceDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAppointmentAssignment_Employee",
                schema: "dispatch",
                table: "FgsAppointmentAssignment",
                columns: new[] { "TenantId", "CompanyId", "EmployeeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAppointmentAssignment_EmployeeSchedule",
                schema: "dispatch",
                table: "FgsAppointmentAssignment",
                columns: new[] { "TenantId", "CompanyId", "EmployeeId", "ServiceDate", "ScheduledTime" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAppointmentAssignment_Overlap",
                schema: "dispatch",
                table: "FgsAppointmentAssignment",
                columns: new[] { "TenantId", "CompanyId", "EmployeeId", "ActualStartOn", "ActualEndOn" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAppointmentAssignment_ServiceDate",
                schema: "dispatch",
                table: "FgsAppointmentAssignment",
                columns: new[] { "TenantId", "CompanyId", "ServiceDate" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsAppointmentAssignment_AppointmentEmployee",
                schema: "dispatch",
                table: "FgsAppointmentAssignment",
                columns: new[] { "TenantId", "CompanyId", "AppointmentId", "EmployeeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsAppointmentAssignmentEvent_Assignment",
                schema: "dispatch",
                table: "FgsAppointmentAssignmentEvent",
                columns: new[] { "TenantId", "CompanyId", "AssignmentId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAppointmentAssignmentEvent_AssignmentEventOccurredOn",
                schema: "dispatch",
                table: "FgsAppointmentAssignmentEvent",
                columns: new[] { "TenantId", "CompanyId", "AssignmentId", "EventOccurredOn" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAppointmentAssignmentEvent_AssignmentId",
                schema: "dispatch",
                table: "FgsAppointmentAssignmentEvent",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsAppointmentAssignmentEvent_Employee",
                schema: "dispatch",
                table: "FgsAppointmentAssignmentEvent",
                columns: new[] { "TenantId", "CompanyId", "EmployeeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAppointmentAssignmentEvent_EmployeeEventOccurredOn",
                schema: "dispatch",
                table: "FgsAppointmentAssignmentEvent",
                columns: new[] { "TenantId", "CompanyId", "EmployeeId", "EventOccurredOn" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAppointmentAssignmentEvent_EventType",
                schema: "dispatch",
                table: "FgsAppointmentAssignmentEvent",
                columns: new[] { "TenantId", "CompanyId", "EventTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAppointmentAssignmentEvent_ServiceDate",
                schema: "dispatch",
                table: "FgsAppointmentAssignmentEvent",
                columns: new[] { "TenantId", "CompanyId", "ServiceDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsAppointmentAssignmentEvent_TenantCompany",
                schema: "dispatch",
                table: "FgsAppointmentAssignmentEvent",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsAppointmentAssignmentEvent_NoDuplicates",
                schema: "dispatch",
                table: "FgsAppointmentAssignmentEvent",
                columns: new[] { "TenantId", "CompanyId", "EmployeeId", "EventTypeId", "EventOccurredOn" },
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_FgsPayroll_Employee",
                schema: "dispatch",
                table: "FgsPayroll",
                columns: new[] { "TenantId", "CompanyId", "EmployeeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPayroll_PayPeriod",
                schema: "dispatch",
                table: "FgsPayroll",
                columns: new[] { "TenantId", "CompanyId", "PayPeriodId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPayroll_PayPeriodId",
                schema: "dispatch",
                table: "FgsPayroll",
                column: "PayPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsPayroll_SignedOn",
                schema: "dispatch",
                table: "FgsPayroll",
                columns: new[] { "TenantId", "CompanyId", "SignedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPayroll_TenantCompany",
                schema: "dispatch",
                table: "FgsPayroll",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsPayroll_PayPeriodEmployee",
                schema: "dispatch",
                table: "FgsPayroll",
                columns: new[] { "TenantId", "CompanyId", "PayPeriodId", "EmployeeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsPayrollLine_Payroll",
                schema: "dispatch",
                table: "FgsPayrollLine",
                columns: new[] { "TenantId", "CompanyId", "PayrollId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPayrollLine_PayrollId",
                schema: "dispatch",
                table: "FgsPayrollLine",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsPayrollLine_PayrollType",
                schema: "dispatch",
                table: "FgsPayrollLine",
                columns: new[] { "TenantId", "CompanyId", "PayrollId", "PayrollLineTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPayrollLine_TenantCompany",
                schema: "dispatch",
                table: "FgsPayrollLine",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPayrollLine_Type",
                schema: "dispatch",
                table: "FgsPayrollLine",
                columns: new[] { "TenantId", "CompanyId", "PayrollLineTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPayrollPayPeriod_EndDate",
                schema: "dispatch",
                table: "FgsPayrollPayPeriod",
                columns: new[] { "TenantId", "CompanyId", "PeriodEndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPayrollPayPeriod_StartDate",
                schema: "dispatch",
                table: "FgsPayrollPayPeriod",
                columns: new[] { "TenantId", "CompanyId", "PeriodStartDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPayrollPayPeriod_Status",
                schema: "dispatch",
                table: "FgsPayrollPayPeriod",
                columns: new[] { "TenantId", "CompanyId", "PayrollStatusId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPayrollPayPeriod_TenantCompany",
                schema: "dispatch",
                table: "FgsPayrollPayPeriod",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsPayrollPayPeriod_DateRange",
                schema: "dispatch",
                table: "FgsPayrollPayPeriod",
                columns: new[] { "TenantId", "CompanyId", "PeriodStartDate", "PeriodEndDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_FgsPayrollPayPeriod_PayPeriodCode",
                schema: "dispatch",
                table: "FgsPayrollPayPeriod",
                columns: new[] { "TenantId", "CompanyId", "PayPeriodCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsWorkOrder_Customer",
                schema: "dispatch",
                table: "FgsWorkOrder",
                columns: new[] { "TenantId", "CompanyId", "CustomerId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsWorkOrder_JobType",
                schema: "dispatch",
                table: "FgsWorkOrder",
                columns: new[] { "TenantId", "CompanyId", "JobTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsWorkOrder_Location",
                schema: "dispatch",
                table: "FgsWorkOrder",
                columns: new[] { "TenantId", "CompanyId", "LocationId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsWorkOrder_Priority",
                schema: "dispatch",
                table: "FgsWorkOrder",
                columns: new[] { "TenantId", "CompanyId", "PriorityId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsWorkOrder_Project",
                schema: "dispatch",
                table: "FgsWorkOrder",
                columns: new[] { "TenantId", "CompanyId", "ProjectId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsWorkOrder_RequestedOn",
                schema: "dispatch",
                table: "FgsWorkOrder",
                columns: new[] { "TenantId", "CompanyId", "RequestedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsWorkOrder_ServiceAgreement",
                schema: "dispatch",
                table: "FgsWorkOrder",
                columns: new[] { "TenantId", "CompanyId", "ServiceAgreementId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsWorkOrder_Source",
                schema: "dispatch",
                table: "FgsWorkOrder",
                columns: new[] { "TenantId", "CompanyId", "Source" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsWorkOrder_Status",
                schema: "dispatch",
                table: "FgsWorkOrder",
                columns: new[] { "TenantId", "CompanyId", "WorkOrderStatusId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsWorkOrder_TenantCompany",
                schema: "dispatch",
                table: "FgsWorkOrder",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsWorkOrder_TimeSlot",
                schema: "dispatch",
                table: "FgsWorkOrder",
                columns: new[] { "TenantId", "CompanyId", "TimeSlotId" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsWorkOrder_WorkOrderNumber",
                schema: "dispatch",
                table: "FgsWorkOrder",
                columns: new[] { "TenantId", "CompanyId", "WorkOrderNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsWorkOrderAsset_WorkOrderId",
                schema: "dispatch",
                table: "FgsWorkOrderAsset",
                column: "WorkOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsWorkOrderIntegration_ReceivedOn",
                schema: "dispatch",
                table: "FgsWorkOrderIntegration",
                columns: new[] { "TenantId", "CompanyId", "ReceivedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsWorkOrderIntegration_Status",
                schema: "dispatch",
                table: "FgsWorkOrderIntegration",
                columns: new[] { "TenantId", "CompanyId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsWorkOrderIntegration_TenantId_CompanyId",
                schema: "dispatch",
                table: "FgsWorkOrderIntegration",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsWorkOrderIntegration_WorkOrderId",
                schema: "dispatch",
                table: "FgsWorkOrderIntegration",
                columns: new[] { "TenantId", "CompanyId", "WorkOrderId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsWorkOrderIntegration_WorkOrderId1",
                schema: "dispatch",
                table: "FgsWorkOrderIntegration",
                column: "WorkOrderId");

            migrationBuilder.CreateIndex(
                name: "UQ_FgsWorkOrderIntegration_External",
                schema: "dispatch",
                table: "FgsWorkOrderIntegration",
                columns: new[] { "TenantId", "CompanyId", "IntegrationName", "ExternalId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsWorkOrderItem_DisplayOrder",
                schema: "dispatch",
                table: "FgsWorkOrderItem",
                columns: new[] { "TenantId", "CompanyId", "WorkOrderId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsWorkOrderItem_InventoryItemId",
                schema: "dispatch",
                table: "FgsWorkOrderItem",
                columns: new[] { "TenantId", "CompanyId", "InventoryItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsWorkOrderItem_TenantId_CompanyId",
                schema: "dispatch",
                table: "FgsWorkOrderItem",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsWorkOrderItem_WorkOrderId",
                schema: "dispatch",
                table: "FgsWorkOrderItem",
                columns: new[] { "TenantId", "CompanyId", "WorkOrderId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsWorkOrderItem_WorkOrderId1",
                schema: "dispatch",
                table: "FgsWorkOrderItem",
                column: "WorkOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsAppointmentAssignmentEvent",
                schema: "dispatch");

            migrationBuilder.DropTable(
                name: "FgsCrewMember",
                schema: "dispatch");

            migrationBuilder.DropTable(
                name: "FgsPayrollLine",
                schema: "dispatch");

            migrationBuilder.DropTable(
                name: "FgsWorkOrderAsset",
                schema: "dispatch");

            migrationBuilder.DropTable(
                name: "FgsWorkOrderIntegration",
                schema: "dispatch");

            migrationBuilder.DropTable(
                name: "FgsWorkOrderItem",
                schema: "dispatch");

            migrationBuilder.DropTable(
                name: "FgsAppointmentAssignment",
                schema: "dispatch");

            migrationBuilder.DropTable(
                name: "FgsPayroll",
                schema: "dispatch");

            migrationBuilder.DropTable(
                name: "FgsWorkOrder",
                schema: "dispatch");

            migrationBuilder.DropTable(
                name: "FgsAppointment",
                schema: "dispatch");

            migrationBuilder.DropTable(
                name: "FgsPayrollPayPeriod",
                schema: "dispatch");

            migrationBuilder.DropTable(
                name: "FgsCrew",
                schema: "dispatch");
        }
    }
}
