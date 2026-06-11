using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddSalesPipelineEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE IF EXISTS setup."FgsEntityTag"
                    DROP CONSTRAINT IF EXISTS "FK_FgsEntityTag_GloMasterEntityType_MasterEntityTypeId";
                ALTER TABLE IF EXISTS setup."FgsSetupPricingMatrixLabor"
                    DROP CONSTRAINT IF EXISTS "FK_FgsSetupPricingMatrixLabor_LaborRateType";
                ALTER TABLE IF EXISTS setup."FgsVehicleMaintenance"
                    DROP CONSTRAINT IF EXISTS "FK_FgsVehicleMaintenance_GloVehicleMaintenanceType_VehicleMaintenanceTypeId";
                DROP INDEX IF EXISTS setup."IX_FgsVehicleMaintenance_VehicleMaintenanceTypeId";
                DROP INDEX IF EXISTS setup."IX_FgsSetupPricingMatrixLabor_LaborRateTypeId";
                DROP INDEX IF EXISTS setup."IX_FgsEntityTag_MasterEntityTypeId1";
                """);

            migrationBuilder.CreateTable(
                name: "FgsSalesActivityType",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier for the sales activity type.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier that owns the record."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier that owns the record."),
                    ActivityTypeCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Immutable business code for the sales activity type."),
                    ActivityTypeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "User-friendly name displayed throughout the application."),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Optional description explaining the sales activity type."),
                    AppliesToLead = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the activity type can be used by Leads."),
                    AppliesToOpportunity = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the activity type can be used by Opportunities."),
                    AllowManualSelection = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether users may manually select this activity type."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls the order in which activity types are displayed."),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the activity type was seeded by the system. System records should have immutable business codes."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the activity type is available for use."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSalesActivityType", x => x.Id);
                    table.CheckConstraint("CK_FgsSalesActivityType_AppliesToEntity", "\"AppliesToLead\" = true OR \"AppliesToOpportunity\" = true");
                    table.ForeignKey(
                        name: "FK_FgsSalesActivityType_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores tenant/company specific sales activity types used by Leads and Opportunities. Seeded from glo.GloSalesActivityType.");

            migrationBuilder.CreateTable(
                name: "FgsSalesDispositionReason",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier for the sales disposition reason.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier that owns the record."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier that owns the record."),
                    DispositionReasonCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Immutable business code for the disposition reason."),
                    DispositionReasonName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "User-friendly name displayed throughout the application."),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Optional description explaining the disposition reason."),
                    AppliesToLead = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the reason can be used when a Lead is Disqualified."),
                    AppliesToOpportunity = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the reason can be used when an Opportunity is Lost."),
                    RequireComment = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether users must provide additional comments when selecting this disposition reason."),
                    IsTerminal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether selecting this disposition reason should result in a terminal pipeline status."),
                    AllowManualSelection = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether users may manually select this disposition reason."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls the order in which disposition reasons are displayed."),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the disposition reason was seeded by the system. System records should have immutable business codes."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the disposition reason is available for use."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSalesDispositionReason", x => x.Id);
                    table.CheckConstraint("CK_FgsSalesDispositionReason_AppliesToEntity", "\"AppliesToLead\" = true OR \"AppliesToOpportunity\" = true");
                    table.ForeignKey(
                        name: "FK_FgsSalesDispositionReason_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores tenant/company specific sales disposition reasons used when a Lead is Disqualified or an Opportunity is Lost. Seeded from glo.GloSalesDispositionReason.");

            migrationBuilder.CreateTable(
                name: "FgsSalesPipelineStatus",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier for the sales pipeline status.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier that owns the record."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier that owns the record."),
                    StatusCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Immutable business code for the sales pipeline status."),
                    StatusName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "User-friendly name displayed throughout the application."),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Optional description explaining the purpose of the status."),
                    AppliesToLead = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the status can be used by Leads."),
                    AppliesToOpportunity = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the status can be used by Opportunities."),
                    IsTerminal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the status represents a terminal end state such as Won, Lost, or Disqualified."),
                    AllowManualSelection = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether users may manually select this status."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls the order in which statuses are displayed."),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the status was seeded by the system. System records should have immutable business codes."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the status is available for use."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSalesPipelineStatus", x => x.Id);
                    table.CheckConstraint("CK_FgsSalesPipelineStatus_AppliesToEntity", "\"AppliesToLead\" = true OR \"AppliesToOpportunity\" = true");
                    table.ForeignKey(
                        name: "FK_FgsSalesPipelineStatus_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores tenant/company specific sales pipeline statuses used by Leads and Opportunities. Seeded from glo.GloSalesPipelineStatus.");

            migrationBuilder.CreateTable(
                name: "GloSalesActivityOutcome",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false, comment: "Unique identifier for the sales activity outcome.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OutcomeCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Immutable business code for the sales activity outcome."),
                    OutcomeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "User-friendly name displayed throughout the application."),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Optional description explaining the sales activity outcome."),
                    AppliesToLead = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the outcome can be used by Leads."),
                    AppliesToOpportunity = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the outcome can be used by Opportunities."),
                    NextSalesPipelineStatusCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Suggested sales pipeline status code that should be applied when this outcome is selected."),
                    IsTerminal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether selecting this outcome typically results in a terminal sales pipeline status."),
                    RequireComment = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether users must provide additional comments when selecting this outcome."),
                    AllowManualSelection = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether users may manually select this outcome."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls the order in which outcomes are displayed."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloSalesActivityOutcome", x => x.Id);
                    table.CheckConstraint("CK_GloSalesActivityOutcome_AppliesToEntity", "\"AppliesToLead\" = true OR \"AppliesToOpportunity\" = true");
                },
                comment: "Master list of sales activity outcomes used by Leads and Opportunities. Outcomes represent the result of a sales interaction and may optionally suggest the next sales pipeline status. Seeded into setup.FgsSalesActivityOutcome.");

            migrationBuilder.CreateTable(
                name: "GloSalesActivityType",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false, comment: "Unique identifier for the sales activity type.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityTypeCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Immutable business code for the sales activity type."),
                    ActivityTypeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "User-friendly name displayed throughout the application."),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Optional description explaining the sales activity type."),
                    AppliesToLead = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the activity type can be used by Leads."),
                    AppliesToOpportunity = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the activity type can be used by Opportunities."),
                    AllowManualSelection = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether users may manually select this activity type."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls the order in which activity types are displayed."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloSalesActivityType", x => x.Id);
                    table.CheckConstraint("CK_GloSalesActivityType_AppliesToEntity", "\"AppliesToLead\" = true OR \"AppliesToOpportunity\" = true");
                },
                comment: "Master list of sales activity types used by Leads and Opportunities. Seeded into setup.FgsSalesActivityType.");

            migrationBuilder.CreateTable(
                name: "GloSalesDispositionReason",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false, comment: "Unique identifier for the sales disposition reason.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DispositionReasonCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Immutable business code for the disposition reason."),
                    DispositionReasonName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "User-friendly name displayed throughout the application."),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Optional description explaining the disposition reason."),
                    AppliesToLead = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the reason can be used when a Lead is Disqualified."),
                    AppliesToOpportunity = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the reason can be used when an Opportunity is Lost."),
                    RequireComment = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether users must provide additional comments when selecting this disposition reason."),
                    IsTerminal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether selecting this disposition reason should result in a terminal pipeline status such as Lost or Disqualified."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls the order in which reasons are displayed."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloSalesDispositionReason", x => x.Id);
                    table.CheckConstraint("CK_GloSalesDispositionReason_AppliesToEntity", "\"AppliesToLead\" = true OR \"AppliesToOpportunity\" = true");
                },
                comment: "Master list of sales disposition reasons used when a Lead is Disqualified or an Opportunity is Lost. Seeded into setup.FgsSalesDispositionReason.");

            migrationBuilder.CreateTable(
                name: "GloSalesPipelineStatus",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false, comment: "Unique identifier for the sales pipeline status.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StatusCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Immutable business code for the sales pipeline status."),
                    StatusName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "User-friendly name displayed throughout the application."),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Optional description explaining the purpose of the status."),
                    AppliesToLead = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the status can be used by Leads."),
                    AppliesToOpportunity = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the status can be used by Opportunities."),
                    IsTerminal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the status represents a terminal end state such as Won, Lost, or Disqualified."),
                    AllowManualSelection = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether users may manually select this status. When false, the status should be reached through workflow actions or automation."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls the order in which statuses are displayed."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloSalesPipelineStatus", x => x.Id);
                    table.CheckConstraint("CK_GloSalesPipelineStatus_AppliesToEntity", "\"AppliesToLead\" = true OR \"AppliesToOpportunity\" = true");
                },
                comment: "Master list of sales pipeline statuses used by Leads and Opportunities. Seeded into setup.FgsSalesPipelineStatus.");

            migrationBuilder.CreateTable(
                name: "FgsSalesActivityOutcome",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier for the sales activity outcome.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier that owns the record."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier that owns the record."),
                    OutcomeCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Immutable business code for the sales activity outcome."),
                    OutcomeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "User-friendly name displayed throughout the application."),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Optional description explaining the sales activity outcome."),
                    AppliesToLead = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the outcome can be used by Leads."),
                    AppliesToOpportunity = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the outcome can be used by Opportunities."),
                    NextSalesPipelineStatusId = table.Column<long>(type: "bigint", nullable: true, comment: "Suggested sales pipeline status that should be applied when this outcome is selected."),
                    IsTerminal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether selecting this outcome typically results in a terminal sales pipeline status."),
                    RequireComment = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether users must provide additional comments when selecting this outcome."),
                    AllowManualSelection = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether users may manually select this outcome."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls the order in which outcomes are displayed."),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the outcome was seeded by the system. System records should have immutable business codes."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the outcome is available for use."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSalesActivityOutcome", x => x.Id);
                    table.CheckConstraint("CK_FgsSalesActivityOutcome_AppliesToEntity", "\"AppliesToLead\" = true OR \"AppliesToOpportunity\" = true");
                    table.ForeignKey(
                        name: "FK_FgsSalesActivityOutcome_FgsSalesPipelineStatus_NextSalesPipelineStatusId",
                        column: x => x.NextSalesPipelineStatusId,
                        principalSchema: "setup",
                        principalTable: "FgsSalesPipelineStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsSalesActivityOutcome_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores tenant/company specific sales activity outcomes used by Leads and Opportunities. Outcomes represent the result of a sales interaction and may optionally suggest the next sales pipeline status. Seeded from glo.GloSalesActivityOutcome.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesActivityOutcome_NextSalesPipelineStatusId",
                schema: "setup",
                table: "FgsSalesActivityOutcome",
                column: "NextSalesPipelineStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesActivityOutcome_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSalesActivityOutcome",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesActivityOutcome_TenantId_CompanyId_DisplayOrder",
                schema: "setup",
                table: "FgsSalesActivityOutcome",
                columns: new[] { "TenantId", "CompanyId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesActivityOutcome_TenantId_CompanyId_IsActive",
                schema: "setup",
                table: "FgsSalesActivityOutcome",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesActivityOutcome_TenantId_CompanyId_NextStatusId",
                schema: "setup",
                table: "FgsSalesActivityOutcome",
                columns: new[] { "TenantId", "CompanyId", "NextSalesPipelineStatusId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsSalesActivityOutcome_TenantId_CompanyId_OutcomeCode",
                schema: "setup",
                table: "FgsSalesActivityOutcome",
                columns: new[] { "TenantId", "CompanyId", "OutcomeCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_FgsSalesActivityOutcome_TenantId_CompanyId_OutcomeName",
                schema: "setup",
                table: "FgsSalesActivityOutcome",
                columns: new[] { "TenantId", "CompanyId", "OutcomeName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesActivityType_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSalesActivityType",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesActivityType_TenantId_CompanyId_DisplayOrder",
                schema: "setup",
                table: "FgsSalesActivityType",
                columns: new[] { "TenantId", "CompanyId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesActivityType_TenantId_CompanyId_IsActive",
                schema: "setup",
                table: "FgsSalesActivityType",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsSalesActivityType_TenantId_CompanyId_ActivityTypeCode",
                schema: "setup",
                table: "FgsSalesActivityType",
                columns: new[] { "TenantId", "CompanyId", "ActivityTypeCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_FgsSalesActivityType_TenantId_CompanyId_ActivityTypeName",
                schema: "setup",
                table: "FgsSalesActivityType",
                columns: new[] { "TenantId", "CompanyId", "ActivityTypeName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesDispositionReason_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSalesDispositionReason",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesDispositionReason_TenantId_CompanyId_DisplayOrder",
                schema: "setup",
                table: "FgsSalesDispositionReason",
                columns: new[] { "TenantId", "CompanyId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesDispositionReason_TenantId_CompanyId_IsActive",
                schema: "setup",
                table: "FgsSalesDispositionReason",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsSalesDispReason_TenantId_CompanyId_ReasonCode",
                schema: "setup",
                table: "FgsSalesDispositionReason",
                columns: new[] { "TenantId", "CompanyId", "DispositionReasonCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_FgsSalesDispReason_TenantId_CompanyId_ReasonName",
                schema: "setup",
                table: "FgsSalesDispositionReason",
                columns: new[] { "TenantId", "CompanyId", "DispositionReasonName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesPipelineStatus_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSalesPipelineStatus",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesPipelineStatus_TenantId_CompanyId_DisplayOrder",
                schema: "setup",
                table: "FgsSalesPipelineStatus",
                columns: new[] { "TenantId", "CompanyId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesPipelineStatus_TenantId_CompanyId_IsActive",
                schema: "setup",
                table: "FgsSalesPipelineStatus",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsSalesPipelineStatus_TenantId_CompanyId_StatusCode",
                schema: "setup",
                table: "FgsSalesPipelineStatus",
                columns: new[] { "TenantId", "CompanyId", "StatusCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_FgsSalesPipelineStatus_TenantId_CompanyId_StatusName",
                schema: "setup",
                table: "FgsSalesPipelineStatus",
                columns: new[] { "TenantId", "CompanyId", "StatusName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloSalesActivityOutcome_DisplayOrder",
                schema: "glo",
                table: "GloSalesActivityOutcome",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "UX_GloSalesActivityOutcome_OutcomeCode",
                schema: "glo",
                table: "GloSalesActivityOutcome",
                column: "OutcomeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_GloSalesActivityOutcome_OutcomeName",
                schema: "glo",
                table: "GloSalesActivityOutcome",
                column: "OutcomeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloSalesActivityType_DisplayOrder",
                schema: "glo",
                table: "GloSalesActivityType",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "UX_GloSalesActivityType_ActivityTypeCode",
                schema: "glo",
                table: "GloSalesActivityType",
                column: "ActivityTypeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_GloSalesActivityType_ActivityTypeName",
                schema: "glo",
                table: "GloSalesActivityType",
                column: "ActivityTypeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloSalesDispositionReason_DisplayOrder",
                schema: "glo",
                table: "GloSalesDispositionReason",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "UX_GloSalesDispositionReason_DispositionReasonCode",
                schema: "glo",
                table: "GloSalesDispositionReason",
                column: "DispositionReasonCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_GloSalesDispositionReason_DispositionReasonName",
                schema: "glo",
                table: "GloSalesDispositionReason",
                column: "DispositionReasonName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloSalesPipelineStatus_DisplayOrder",
                schema: "glo",
                table: "GloSalesPipelineStatus",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "UX_GloSalesPipelineStatus_StatusCode",
                schema: "glo",
                table: "GloSalesPipelineStatus",
                column: "StatusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_GloSalesPipelineStatus_StatusName",
                schema: "glo",
                table: "GloSalesPipelineStatus",
                column: "StatusName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsSalesActivityOutcome",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSalesActivityType",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSalesDispositionReason",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "GloSalesActivityOutcome",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloSalesActivityType",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloSalesDispositionReason",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloSalesPipelineStatus",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "FgsSalesPipelineStatus",
                schema: "setup");

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicleMaintenance_VehicleMaintenanceTypeId",
                schema: "setup",
                table: "FgsVehicleMaintenance",
                column: "VehicleMaintenanceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPricingMatrixLabor_LaborRateTypeId",
                schema: "setup",
                table: "FgsSetupPricingMatrixLabor",
                column: "LaborRateTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEntityTag_MasterEntityTypeId1",
                schema: "setup",
                table: "FgsEntityTag",
                column: "MasterEntityTypeId");
        }
    }
}
