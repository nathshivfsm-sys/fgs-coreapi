using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeTechnicianProfileAndCrew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "GloInventorySubCategory",
                schema: "glo",
                comment: "Global inventory sub-category catalog scoped to an inventory category.");

            migrationBuilder.AlterTable(
                name: "GloInventoryItemType",
                schema: "glo",
                comment: "Global inventory item type catalog (inventory, non-inventory, service, kit, tool).");

            migrationBuilder.AlterTable(
                name: "GloInventoryCategory",
                schema: "glo",
                comment: "Global inventory category catalog scoped to a business type.");

            migrationBuilder.AlterColumn<string>(
                name: "SubCategoryCode",
                schema: "glo",
                table: "GloInventorySubCategory",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                comment: "Unique sub-category code within the category.",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "glo",
                table: "GloInventorySubCategory",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                comment: "Display name of the sub-category.",
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "glo",
                table: "GloInventorySubCategory",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                comment: "Indicates whether the sub-category is active.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "InventoryCategoryId",
                schema: "glo",
                table: "GloInventorySubCategory",
                type: "integer",
                nullable: false,
                comment: "Parent inventory category.",
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<short>(
                name: "DisplayOrder",
                schema: "glo",
                table: "GloInventorySubCategory",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1,
                comment: "Display order.",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)1);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "glo",
                table: "GloInventorySubCategory",
                type: "text",
                nullable: true,
                comment: "Description of the sub-category.",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "glo",
                table: "GloInventorySubCategory",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()",
                comment: "Date and time the record was created.",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "glo",
                table: "GloInventorySubCategory",
                type: "integer",
                nullable: false,
                comment: "Primary key.",
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<bool>(
                name: "TracksQuantity",
                schema: "glo",
                table: "GloInventoryItemType",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Indicates whether items of this type track quantity on hand.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "glo",
                table: "GloInventoryItemType",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                comment: "Display name of the item type.",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ItemTypeCode",
                schema: "glo",
                table: "GloInventoryItemType",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                comment: "Unique item type code.",
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "glo",
                table: "GloInventoryItemType",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                comment: "Indicates whether the item type is active.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<short>(
                name: "DisplayOrder",
                schema: "glo",
                table: "GloInventoryItemType",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1,
                comment: "Display order.",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)1);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "glo",
                table: "GloInventoryItemType",
                type: "text",
                nullable: true,
                comment: "Description of the item type.",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "glo",
                table: "GloInventoryItemType",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()",
                comment: "Date and time the record was created.",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<short>(
                name: "Id",
                schema: "glo",
                table: "GloInventoryItemType",
                type: "smallint",
                nullable: false,
                comment: "Primary key.",
                oldClrType: typeof(short),
                oldType: "smallint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "glo",
                table: "GloInventoryCategory",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                comment: "Display name of the category.",
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "glo",
                table: "GloInventoryCategory",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                comment: "Indicates whether the category is active.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<short>(
                name: "DisplayOrder",
                schema: "glo",
                table: "GloInventoryCategory",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1,
                comment: "Display order.",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)1);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "glo",
                table: "GloInventoryCategory",
                type: "text",
                nullable: true,
                comment: "Description of the category.",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "glo",
                table: "GloInventoryCategory",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()",
                comment: "Date and time the record was created.",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryCode",
                schema: "glo",
                table: "GloInventoryCategory",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                comment: "Unique category code within the business type.",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "BusinessTypeId",
                schema: "glo",
                table: "GloInventoryCategory",
                type: "integer",
                nullable: false,
                comment: "Business type that owns this category.",
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "glo",
                table: "GloInventoryCategory",
                type: "integer",
                nullable: false,
                comment: "Primary key.",
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateTable(
                name: "FgsCrew",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CrewCode = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false, comment: "Unique crew code displayed on dispatch boards."),
                    CrewName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "User-friendly crew name."),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Optional crew description."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the crew is active and available for dispatching.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsCrew", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsCrew_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores technician crew definitions used for dispatching and scheduling.");

            migrationBuilder.CreateTable(
                name: "FgsEmployee",
                schema: "setup",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the tenant that owns the employee record."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the company that owns the employee record."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key identifier for the employee record.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: true, comment: "Optional reference to the system user account associated with this employee. One user may be linked to only one employee. References identity service; no FK by design."),
                    EmployeeNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Unique employee number within the company."),
                    EmployeeTypeId = table.Column<short>(type: "smallint", nullable: false, comment: "Employee classification. Typical values are Office and Technician."),
                    DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Display name used throughout the application, dispatch board, schedules, and reports."),
                    LegalFirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Employee legal first name."),
                    LegalMiddleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Employee legal middle name."),
                    LegalLastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Employee legal last name."),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: true, comment: "Employee date of birth."),
                    HireDate = table.Column<DateOnly>(type: "date", nullable: true, comment: "Date employee was hired."),
                    TerminationDate = table.Column<DateOnly>(type: "date", nullable: true, comment: "Date employee was terminated or separated from employment."),
                    StatusId = table.Column<short>(type: "smallint", nullable: false, comment: "Current employee status such as Active, Inactive, Leave of Absence, or Terminated."),
                    PersonalEmail = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Employee personal email address."),
                    OfficeEmail = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Employee company or office email address."),
                    PersonalPhone = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true, comment: "Employee personal phone number."),
                    OfficePhone = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true, comment: "Employee office phone number or extension."),
                    AddressId = table.Column<long>(type: "bigint", nullable: true, comment: "Reference to the employee mailing or home address record. No FK by design."),
                    ProfilePhotoFileId = table.Column<long>(type: "bigint", nullable: true, comment: "Identifier of the employee profile photo stored in the file repository. No FK by design."),
                    RegularRate = table.Column<decimal>(type: "numeric(18,2)", nullable: true, comment: "Standard hourly labor rate used for payroll, costing, and reporting."),
                    OvertimeRate = table.Column<decimal>(type: "numeric(18,2)", nullable: true, comment: "Overtime hourly labor rate."),
                    DoubleTimeRate = table.Column<decimal>(type: "numeric(18,2)", nullable: true, comment: "Double-time hourly labor rate."),
                    LaborBurdenTypeId = table.Column<short>(type: "smallint", nullable: true, comment: "Determines whether labor burden is expressed as a percentage or fixed amount."),
                    LaborBurdenValue = table.Column<decimal>(type: "numeric(18,2)", nullable: true, comment: "Labor burden amount or percentage used for estimating, costing, and profitability calculations."),
                    IsPurchaser = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the employee is authorized to create or approve purchase orders."),
                    Notes = table.Column<string>(type: "text", nullable: true, comment: "Internal notes related to the employee."),
                    CreatedOn = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()", comment: "Date and time the employee record was created."),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false, comment: "User who created the employee record."),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp", nullable: true, comment: "Date and time the employee record was last modified."),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true, comment: "User who last modified the employee record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsEmployee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsEmployee_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores employee master information for office and field personnel. Employees may optionally be linked to a system user account through UserId. Technician-specific operational settings are stored in FgsEmployeeTechnicianProfile.");

            migrationBuilder.CreateTable(
                name: "GloInventoryTransactionSourceType",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "System code used internally by the application."),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Display name of the inventory transaction source type."),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Description of the business process that generates inventory transactions."),
                    SortOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 1, comment: "Display order."),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the source type is active."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloInventoryTransactionSourceType", x => x.Id);
                },
                comment: "Defines business processes and source documents that generate inventory transactions.");

            migrationBuilder.CreateTable(
                name: "FgsEmployeeTechnicianProfile",
                schema: "setup",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the tenant that owns the technician profile."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the company that owns the technician profile."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key identifier for the technician profile.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false, comment: "Reference to the employee associated with this technician profile. One employee may have only one technician profile."),
                    TechCode = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false, comment: "Required unique technician code used on dispatch boards, whiteboards, reports, scheduling screens, mobile applications, and integrations."),
                    TechName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Technician name displayed to customers in appointment reminders, technician tracking pages, work orders, invoices, and customer communications."),
                    CanBeScheduled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the technician can receive appointments and appear on the dispatch board."),
                    DailyCapacityHours = table.Column<decimal>(type: "numeric(5,2)", nullable: false, defaultValue: 8.00m, comment: "Number of labor hours available per day. Used for whiteboard capacity calculations, scheduling, utilization reporting, and workforce planning."),
                    DispatchZoneId = table.Column<long>(type: "bigint", nullable: true, comment: "Default dispatch zone assigned to the technician for territory-based scheduling and routing."),
                    StartLocationTypeId = table.Column<short>(type: "smallint", nullable: false, comment: "Indicates where the technician normally starts the workday. Typical values are Office or Home."),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: true, comment: "Default daily start time used for scheduling, route planning, technician availability calculations, and capacity management."),
                    TechTradeId = table.Column<int>(type: "integer", nullable: true, comment: "Primary trade classification assigned to the technician such as HVAC, Plumbing, Electrical, Refrigeration, Landscaping, Cleaning, or Pest Control."),
                    TechSkillId = table.Column<int>(type: "integer", nullable: true, comment: "Primary skill or specialization assigned to the technician within the selected trade."),
                    TruckId = table.Column<long>(type: "bigint", nullable: true, comment: "Assigned service vehicle used by the technician. Used for dispatching, route planning, truck inventory, inventory consumption, and replenishment processes."),
                    CustomerFacingPhone = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true, comment: "Phone number displayed to customers for technician communication. May differ from the employee personal or office phone number."),
                    Notes = table.Column<string>(type: "text", nullable: true, comment: "Internal technician-specific notes used by dispatchers, supervisors, and managers."),
                    CreatedOn = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()", comment: "Date and time the technician profile was created."),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false, comment: "User who created the technician profile."),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp", nullable: true, comment: "Date and time the technician profile was last modified."),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true, comment: "User who last modified the technician profile.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsEmployeeTechnicianProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsEmployeeTechnicianProfile_FgsEmployee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "setup",
                        principalTable: "FgsEmployee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsEmployeeTechnicianProfile_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores technician-specific operational settings used by dispatching, scheduling, routing, capacity planning, inventory assignment, and customer-facing technician communications.");

            migrationBuilder.CreateTable(
                name: "GloInventoryTransactionType",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InventoryTransactionSourceTypeId = table.Column<int>(type: "integer", nullable: false, comment: "References the inventory transaction source type that generates this transaction."),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "System transaction code."),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Display name of the inventory transaction type."),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Description of the inventory transaction type."),
                    SortOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 1, comment: "Display order."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the transaction type is active."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloInventoryTransactionType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GloInventoryTransactionType_GloInventoryTransactionSourceType",
                        column: x => x.InventoryTransactionSourceTypeId,
                        principalSchema: "glo",
                        principalTable: "GloInventoryTransactionSourceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Defines specific inventory ledger transactions generated by inventory business processes.");

            migrationBuilder.CreateTable(
                name: "FgsCrewMember",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CrewId = table.Column<long>(type: "bigint", nullable: false, comment: "Crew to which the technician belongs."),
                    TechnicianProfileId = table.Column<long>(type: "bigint", nullable: false, comment: "Technician profile assigned to the crew."),
                    IsLead = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the technician is the designated lead technician for the crew."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsCrewMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsCrewMember_FgsCrew_CrewId",
                        column: x => x.CrewId,
                        principalSchema: "setup",
                        principalTable: "FgsCrew",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsCrewMember_FgsEmployeeTechnicianProfile_TechnicianProfileId",
                        column: x => x.TechnicianProfileId,
                        principalSchema: "setup",
                        principalTable: "FgsEmployeeTechnicianProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsCrewMember_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores permanent technician membership within crews.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsCrew_TenantId_CompanyId",
                schema: "setup",
                table: "FgsCrew",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsCrew_TenantId_CompanyId_CrewCode",
                schema: "setup",
                table: "FgsCrew",
                columns: new[] { "TenantId", "CompanyId", "CrewCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_FgsCrew_TenantId_CompanyId_CrewName",
                schema: "setup",
                table: "FgsCrew",
                columns: new[] { "TenantId", "CompanyId", "CrewName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsCrewMember_CrewId",
                schema: "setup",
                table: "FgsCrewMember",
                column: "CrewId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsCrewMember_TechnicianProfileId",
                schema: "setup",
                table: "FgsCrewMember",
                column: "TechnicianProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsCrewMember_TenantId_CompanyId",
                schema: "setup",
                table: "FgsCrewMember",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsCrewMember_CrewId_Lead",
                schema: "setup",
                table: "FgsCrewMember",
                columns: new[] { "CrewId", "IsLead" },
                unique: true,
                filter: "\"IsLead\" = true");

            migrationBuilder.CreateIndex(
                name: "UX_FgsCrewMember_TenantId_CompanyId_TechnicianProfileId",
                schema: "setup",
                table: "FgsCrewMember",
                columns: new[] { "TenantId", "CompanyId", "TechnicianProfileId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsEmployee_TenantId_CompanyId",
                schema: "setup",
                table: "FgsEmployee",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEmployee_TenantId_CompanyId_DisplayName",
                schema: "setup",
                table: "FgsEmployee",
                columns: new[] { "TenantId", "CompanyId", "DisplayName" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEmployee_TenantId_CompanyId_EmployeeTypeId",
                schema: "setup",
                table: "FgsEmployee",
                columns: new[] { "TenantId", "CompanyId", "EmployeeTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEmployee_TenantId_CompanyId_StatusId",
                schema: "setup",
                table: "FgsEmployee",
                columns: new[] { "TenantId", "CompanyId", "StatusId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsEmployee_TenantId_CompanyId_EmployeeNumber",
                schema: "setup",
                table: "FgsEmployee",
                columns: new[] { "TenantId", "CompanyId", "EmployeeNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_FgsEmployee_UserId",
                schema: "setup",
                table: "FgsEmployee",
                column: "UserId",
                unique: true,
                filter: "\"UserId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEmployeeTechnicianProfile_EmployeeId",
                schema: "setup",
                table: "FgsEmployeeTechnicianProfile",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEmployeeTechnicianProfile_TenantId_CompanyId",
                schema: "setup",
                table: "FgsEmployeeTechnicianProfile",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEmployeeTechnicianProfile_TenantId_CompanyId_CanBeScheduled",
                schema: "setup",
                table: "FgsEmployeeTechnicianProfile",
                columns: new[] { "TenantId", "CompanyId", "CanBeScheduled" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEmployeeTechnicianProfile_TenantId_CompanyId_DispatchZoneId",
                schema: "setup",
                table: "FgsEmployeeTechnicianProfile",
                columns: new[] { "TenantId", "CompanyId", "DispatchZoneId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEmployeeTechnicianProfile_TenantId_CompanyId_TechTradeId",
                schema: "setup",
                table: "FgsEmployeeTechnicianProfile",
                columns: new[] { "TenantId", "CompanyId", "TechTradeId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsEmployeeTechnicianProfile_EmployeeId",
                schema: "setup",
                table: "FgsEmployeeTechnicianProfile",
                columns: new[] { "TenantId", "CompanyId", "EmployeeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_FgsEmployeeTechnicianProfile_TenantId_CompanyId_TechCode",
                schema: "setup",
                table: "FgsEmployeeTechnicianProfile",
                columns: new[] { "TenantId", "CompanyId", "TechCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_GloInventoryTransactionSourceType_Code",
                schema: "glo",
                table: "GloInventoryTransactionSourceType",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloInventoryTransactionType_IsActive",
                schema: "glo",
                table: "GloInventoryTransactionType",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_GloInventoryTransactionType_SourceType",
                schema: "glo",
                table: "GloInventoryTransactionType",
                column: "InventoryTransactionSourceTypeId");

            migrationBuilder.CreateIndex(
                name: "UQ_GloInventoryTransactionType_Code",
                schema: "glo",
                table: "GloInventoryTransactionType",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsCrewMember",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "GloInventoryTransactionType",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "FgsCrew",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsEmployeeTechnicianProfile",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "GloInventoryTransactionSourceType",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "FgsEmployee",
                schema: "setup");

            migrationBuilder.AlterTable(
                name: "GloInventorySubCategory",
                schema: "glo",
                oldComment: "Global inventory sub-category catalog scoped to an inventory category.");

            migrationBuilder.AlterTable(
                name: "GloInventoryItemType",
                schema: "glo",
                oldComment: "Global inventory item type catalog (inventory, non-inventory, service, kit, tool).");

            migrationBuilder.AlterTable(
                name: "GloInventoryCategory",
                schema: "glo",
                oldComment: "Global inventory category catalog scoped to a business type.");

            migrationBuilder.AlterColumn<string>(
                name: "SubCategoryCode",
                schema: "glo",
                table: "GloInventorySubCategory",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldComment: "Unique sub-category code within the category.");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "glo",
                table: "GloInventorySubCategory",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150,
                oldComment: "Display name of the sub-category.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "glo",
                table: "GloInventorySubCategory",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true,
                oldComment: "Indicates whether the sub-category is active.");

            migrationBuilder.AlterColumn<int>(
                name: "InventoryCategoryId",
                schema: "glo",
                table: "GloInventorySubCategory",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Parent inventory category.");

            migrationBuilder.AlterColumn<short>(
                name: "DisplayOrder",
                schema: "glo",
                table: "GloInventorySubCategory",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)1,
                oldComment: "Display order.");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "glo",
                table: "GloInventorySubCategory",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Description of the sub-category.");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "glo",
                table: "GloInventorySubCategory",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "now()",
                oldComment: "Date and time the record was created.");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "glo",
                table: "GloInventorySubCategory",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Primary key.")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<bool>(
                name: "TracksQuantity",
                schema: "glo",
                table: "GloInventoryItemType",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false,
                oldComment: "Indicates whether items of this type track quantity on hand.");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "glo",
                table: "GloInventoryItemType",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldComment: "Display name of the item type.");

            migrationBuilder.AlterColumn<string>(
                name: "ItemTypeCode",
                schema: "glo",
                table: "GloInventoryItemType",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldComment: "Unique item type code.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "glo",
                table: "GloInventoryItemType",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true,
                oldComment: "Indicates whether the item type is active.");

            migrationBuilder.AlterColumn<short>(
                name: "DisplayOrder",
                schema: "glo",
                table: "GloInventoryItemType",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)1,
                oldComment: "Display order.");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "glo",
                table: "GloInventoryItemType",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Description of the item type.");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "glo",
                table: "GloInventoryItemType",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "now()",
                oldComment: "Date and time the record was created.");

            migrationBuilder.AlterColumn<short>(
                name: "Id",
                schema: "glo",
                table: "GloInventoryItemType",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldComment: "Primary key.")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "glo",
                table: "GloInventoryCategory",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150,
                oldComment: "Display name of the category.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "glo",
                table: "GloInventoryCategory",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true,
                oldComment: "Indicates whether the category is active.");

            migrationBuilder.AlterColumn<short>(
                name: "DisplayOrder",
                schema: "glo",
                table: "GloInventoryCategory",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)1,
                oldComment: "Display order.");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "glo",
                table: "GloInventoryCategory",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Description of the category.");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "glo",
                table: "GloInventoryCategory",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "now()",
                oldComment: "Date and time the record was created.");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryCode",
                schema: "glo",
                table: "GloInventoryCategory",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldComment: "Unique category code within the business type.");

            migrationBuilder.AlterColumn<int>(
                name: "BusinessTypeId",
                schema: "glo",
                table: "GloInventoryCategory",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Business type that owns this category.");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "glo",
                table: "GloInventoryCategory",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Primary key.")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
