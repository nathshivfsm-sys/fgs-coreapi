using System;
using Fgs.Crm.Domain.Enums;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Crm.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCrmSalesOpportunityAndActivityEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CrmLead_TenantId_CompanyId_CustomerTypeId",
                schema: "crm",
                table: "CrmLead");

            migrationBuilder.DropIndex(
                name: "IX_CrmLead_TenantId_CompanyId_PrimaryContactMethodId",
                schema: "crm",
                table: "CrmLead");

            migrationBuilder.DropIndex(
                name: "IX_CrmLead_TenantId_CompanyId_ServiceZipCode",
                schema: "crm",
                table: "CrmLead");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                schema: "crm",
                table: "CrmLead");

            migrationBuilder.DropColumn(
                name: "CustomerTypeId",
                schema: "crm",
                table: "CrmLead");

            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "crm",
                table: "CrmLead");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "crm",
                table: "CrmLead");

            migrationBuilder.DropColumn(
                name: "LeadSummary",
                schema: "crm",
                table: "CrmLead");

            migrationBuilder.DropColumn(
                name: "QualifiedOn",
                schema: "crm",
                table: "CrmLead");

            migrationBuilder.DropColumn(
                name: "ServiceZipCode",
                schema: "crm",
                table: "CrmLead");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:crm.SalesPriority", "LOW,NORMAL,HIGH");

            migrationBuilder.AlterTable(
                name: "CrmLead",
                schema: "crm",
                comment: "Stores sales leads/prospects received from the website, office users, technicians, referrals, campaigns, or other configured lead sources. A Lead may remain in the Lead pipeline, be disqualified/lost, be associated with an existing customer, or be converted into an Opportunity. Lead activities are stored separately in crm.FgsSalesActivity.",
                oldComment: "Stores inbound sales inquiries and prospects prior to qualification and conversion into customers and opportunities.");

            migrationBuilder.AlterColumn<long>(
                name: "LeadStatusId",
                schema: "crm",
                table: "CrmLead",
                type: "bigint",
                nullable: false,
                comment: "Current status of the lead selected from the configured sales pipeline statuses applicable to leads.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Current lead status selected from setup.FgsLeadStatus.");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LeadReceivedOn",
                schema: "crm",
                table: "CrmLead",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()",
                comment: "Date and time the lead was originally received.",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "Date and time the lead was originally received.");

            migrationBuilder.AlterColumn<string>(
                name: "LeadDescription",
                schema: "crm",
                table: "CrmLead",
                type: "text",
                nullable: true,
                comment: "Comments and details describing the customer inquiry, service need, or information provided with the lead.",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Detailed description of the lead inquiry and customer requirements.");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DisqualifiedOn",
                schema: "crm",
                table: "CrmLead",
                type: "timestamptz",
                nullable: true,
                comment: "Date and time the lead was disqualified.",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "Date and time the lead was disqualified.");

            migrationBuilder.AlterColumn<long>(
                name: "CustomerId",
                schema: "crm",
                table: "CrmLead",
                type: "bigint",
                nullable: true,
                comment: "Existing customer associated with the lead, when applicable.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Customer record created from this lead after conversion.");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ConvertedOn",
                schema: "crm",
                table: "CrmLead",
                type: "timestamptz",
                nullable: true,
                comment: "Date and time the lead was converted into an opportunity.",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "Date and time the lead was converted into a customer.");

            migrationBuilder.AddColumn<string>(
                name: "Address1",
                schema: "crm",
                table: "CrmLead",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                comment: "Primary street address where service is requested.");

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                schema: "crm",
                table: "CrmLead",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                comment: "Additional address information such as apartment, suite, unit, building, or floor.");

            migrationBuilder.AddColumn<string>(
                name: "City",
                schema: "crm",
                table: "CrmLead",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "City where service is requested.");

            migrationBuilder.AddColumn<long>(
                name: "ConvertedOpportunityId",
                schema: "crm",
                table: "CrmLead",
                type: "bigint",
                nullable: true,
                comment: "Opportunity created when the lead was converted.");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                schema: "crm",
                table: "CrmLead",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Country where service is requested.");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "crm",
                table: "CrmLead",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                comment: "Name of the person or contact submitting or associated with the lead.");

            migrationBuilder.Sql("""ALTER TABLE crm."CrmLead" ALTER COLUMN "Name" DROP DEFAULT;""");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                schema: "crm",
                table: "CrmLead",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                comment: "Postal or ZIP code where service is requested.");

            migrationBuilder.AddColumn<long>(
                name: "ServiceLocationId",
                schema: "crm",
                table: "CrmLead",
                type: "bigint",
                nullable: true,
                comment: "Optional service location associated with the lead.");

            migrationBuilder.AddColumn<string>(
                name: "State",
                schema: "crm",
                table: "CrmLead",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "State, province, or administrative region where service is requested.");

            migrationBuilder.CreateTable(
                name: "FgsOpportunity",
                schema: "crm",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant that owns the opportunity."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company within the tenant that owns the opportunity."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier for the opportunity.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    LeadId = table.Column<long>(type: "bigint", nullable: true, comment: "Optional Lead from which the opportunity was created. NULL when the opportunity was created directly without a Lead."),
                    OpportunityStatusId = table.Column<long>(type: "bigint", nullable: false, comment: "Current status of the opportunity selected from the configured sales pipeline statuses applicable to opportunities."),
                    LeadSourceId = table.Column<long>(type: "bigint", nullable: true, comment: "Optional source associated with the opportunity. When the opportunity originated from a Lead, this may be copied from the Lead source."),
                    CampaignId = table.Column<long>(type: "bigint", nullable: true, comment: "Optional marketing campaign associated with the opportunity."),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Name used to identify the sales opportunity."),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Detailed description of the opportunity, customer requirements, sales information, or other relevant comments."),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false, comment: "Customer associated with the opportunity. A customer is required for an active opportunity. The customer may be an existing customer or one created during Lead conversion."),
                    ServiceLocationId = table.Column<long>(type: "bigint", nullable: true, comment: "Optional service location associated with the opportunity."),
                    AssignedToUserId = table.Column<long>(type: "bigint", nullable: true, comment: "Salesperson or user currently responsible for working the opportunity."),
                    EstimatedAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: true, comment: "Current estimated sales value of the opportunity used for sales forecasting. This value may change as the opportunity progresses."),
                    SoldAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: true, comment: "Actual sales amount agreed upon when the opportunity is won. NULL until the opportunity is marked as won."),
                    ExpectedCloseOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Expected date and time when the opportunity is anticipated to close."),
                    WonOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time when the opportunity was marked as won."),
                    LostOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time when the opportunity was marked as lost."),
                    DispositionReasonId = table.Column<long>(type: "bigint", nullable: true, comment: "Reason the opportunity was lost, selected from the configured sales disposition reasons."),
                    EstimateId = table.Column<long>(type: "bigint", nullable: true, comment: "Estimate created from the opportunity when the sales process results in an Estimate."),
                    WorkOrderId = table.Column<long>(type: "bigint", nullable: true, comment: "Work Order created from the opportunity when the sales process results directly in a Work Order."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time when the opportunity record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that created the opportunity record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time when the opportunity record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that last updated the opportunity record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsOpportunity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsOpportunity_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "crm",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores qualified sales opportunities that originate from Leads or are created directly by users. An Opportunity represents an active sales pursuit and may ultimately result in an Estimate or Work Order.");

            migrationBuilder.CreateTable(
                name: "FgsSalesActivity",
                schema: "crm",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant that owns the sales activity."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company within the tenant that owns the sales activity."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier for the sales activity.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    LeadId = table.Column<long>(type: "bigint", nullable: true, comment: "Lead associated with the activity. Exactly one of LeadId or OpportunityId must be populated."),
                    OpportunityId = table.Column<long>(type: "bigint", nullable: true, comment: "Opportunity associated with the activity. Exactly one of OpportunityId or LeadId must be populated."),
                    ActivityTypeId = table.Column<long>(type: "bigint", nullable: false, comment: "Activity type selected from the configured sales activity types, such as Call, Email, Visit, Meeting, or Follow-up."),
                    AssignedToUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User responsible for performing the scheduled sales activity."),
                    ScheduledOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time when the activity is scheduled to occur. Used to place the activity on the dispatch board."),
                    EstimatedHours = table.Column<decimal>(type: "numeric(6,2)", nullable: true, comment: "Expected amount of time required to perform the scheduled activity, expressed in hours. Used for scheduling and dispatch capacity planning."),
                    StartedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Optional date and time when the user started performing the activity."),
                    CompletedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Optional date and time when the activity was completed. When StartedOn and CompletedOn are provided, ActualHours may be calculated from the elapsed time."),
                    ActualHours = table.Column<decimal>(type: "numeric(6,2)", nullable: true, comment: "Actual amount of time spent performing the activity, expressed in hours. The value may be calculated from StartedOn and CompletedOn or entered directly by the user when start and completion times are not tracked."),
                    PerformedByUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User who actually performed or completed the activity. This may differ from the user originally assigned to the activity."),
                    SalesActivityOutcomeId = table.Column<long>(type: "bigint", nullable: true, comment: "Outcome selected when the activity is completed. The outcome may determine the resulting pipeline status, whether another activity should be created, or whether the Lead should be converted to an Opportunity."),
                    OutcomeDetails = table.Column<string>(type: "text", nullable: true, comment: "Additional details describing the selected activity outcome, including specific results, customer response, information communicated, or other details associated with the outcome."),
                    Comments = table.Column<string>(type: "text", nullable: true, comment: "Comments or notes entered while scheduling, performing, or completing the activity."),
                    RequiresFollowUp = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether another sales activity is required after this activity."),
                    FollowUpOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time requested for the follow-up activity."),
                    FollowUpActivityId = table.Column<long>(type: "bigint", nullable: true, comment: "Activity created as the follow-up to this activity."),
                    Latitude = table.Column<decimal>(type: "numeric(10,7)", nullable: true, comment: "Latitude captured when the activity is performed, when location capture is enabled."),
                    Longitude = table.Column<decimal>(type: "numeric(10,7)", nullable: true, comment: "Longitude captured when the activity is performed, when location capture is enabled."),
                    IsSystemGenerated = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the activity was created automatically by the system rather than manually by a user."),
                    Priority = table.Column<SalesPriority>(type: "crm.\"SalesPriority\"", nullable: false, defaultValue: SalesPriority.NORMAL, comment: "Priority of the sales activity used to indicate the urgency with which the activity should be performed."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time when the sales activity record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that created the sales activity record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time when the sales activity record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that last updated the sales activity record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSalesActivity", x => x.Id);
                    table.CheckConstraint("CK_FgsSalesActivity_ActualHours", "\"ActualHours\" IS NULL OR \"ActualHours\" > 0");
                    table.CheckConstraint("CK_FgsSalesActivity_CompletedAfterStarted", "\"StartedOn\" IS NULL OR \"CompletedOn\" IS NULL OR \"CompletedOn\" >= \"StartedOn\"");
                    table.CheckConstraint("CK_FgsSalesActivity_CompletedRequiresStarted", "\"CompletedOn\" IS NULL OR \"StartedOn\" IS NOT NULL");
                    table.CheckConstraint("CK_FgsSalesActivity_EstimatedHours", "\"EstimatedHours\" IS NULL OR \"EstimatedHours\" > 0");
                    table.CheckConstraint("CK_FgsSalesActivity_LeadOrOpportunity", "(\"LeadId\" IS NOT NULL AND \"OpportunityId\" IS NULL) OR (\"LeadId\" IS NULL AND \"OpportunityId\" IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_FgsSalesActivity_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "crm",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsSalesActivity_FollowUpActivity",
                        column: x => x.FollowUpActivityId,
                        principalSchema: "crm",
                        principalTable: "FgsSalesActivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_FgsSalesActivity_Lead",
                        column: x => x.LeadId,
                        principalSchema: "crm",
                        principalTable: "CrmLead",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsSalesActivity_Opportunity",
                        column: x => x.OpportunityId,
                        principalSchema: "crm",
                        principalTable: "FgsOpportunity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Stores scheduled and completed sales activities for Leads and Opportunities, including calls, emails, meetings, site visits, follow-ups, and system-generated activities. Activities can be scheduled on the dispatch board and completed with an outcome, resulting pipeline status, comments, and optional follow-up activity.");

            migrationBuilder.CreateIndex(
                name: "IX_CrmLead_TenantId_CompanyId_ConvertedOpportunityId",
                schema: "crm",
                table: "CrmLead",
                columns: new[] { "TenantId", "CompanyId", "ConvertedOpportunityId" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmLead_TenantId_CompanyId_ServiceLocationId",
                schema: "crm",
                table: "CrmLead",
                columns: new[] { "TenantId", "CompanyId", "ServiceLocationId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsOpportunity_TenantId_CompanyId",
                schema: "crm",
                table: "FgsOpportunity",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsOpportunity_TenantId_CompanyId_AssignedToUserId",
                schema: "crm",
                table: "FgsOpportunity",
                columns: new[] { "TenantId", "CompanyId", "AssignedToUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsOpportunity_TenantId_CompanyId_CampaignId",
                schema: "crm",
                table: "FgsOpportunity",
                columns: new[] { "TenantId", "CompanyId", "CampaignId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsOpportunity_TenantId_CompanyId_CustomerId",
                schema: "crm",
                table: "FgsOpportunity",
                columns: new[] { "TenantId", "CompanyId", "CustomerId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsOpportunity_TenantId_CompanyId_DispositionReasonId",
                schema: "crm",
                table: "FgsOpportunity",
                columns: new[] { "TenantId", "CompanyId", "DispositionReasonId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsOpportunity_TenantId_CompanyId_EstimateId",
                schema: "crm",
                table: "FgsOpportunity",
                columns: new[] { "TenantId", "CompanyId", "EstimateId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsOpportunity_TenantId_CompanyId_ExpectedCloseOn",
                schema: "crm",
                table: "FgsOpportunity",
                columns: new[] { "TenantId", "CompanyId", "ExpectedCloseOn" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsOpportunity_TenantId_CompanyId_LeadId",
                schema: "crm",
                table: "FgsOpportunity",
                columns: new[] { "TenantId", "CompanyId", "LeadId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsOpportunity_TenantId_CompanyId_LeadSourceId",
                schema: "crm",
                table: "FgsOpportunity",
                columns: new[] { "TenantId", "CompanyId", "LeadSourceId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsOpportunity_TenantId_CompanyId_OpportunityStatusId",
                schema: "crm",
                table: "FgsOpportunity",
                columns: new[] { "TenantId", "CompanyId", "OpportunityStatusId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsOpportunity_TenantId_CompanyId_ServiceLocationId",
                schema: "crm",
                table: "FgsOpportunity",
                columns: new[] { "TenantId", "CompanyId", "ServiceLocationId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsOpportunity_TenantId_CompanyId_WorkOrderId",
                schema: "crm",
                table: "FgsOpportunity",
                columns: new[] { "TenantId", "CompanyId", "WorkOrderId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesActivity_FollowUpActivityId",
                schema: "crm",
                table: "FgsSalesActivity",
                column: "FollowUpActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesActivity_LeadId",
                schema: "crm",
                table: "FgsSalesActivity",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesActivity_OpportunityId",
                schema: "crm",
                table: "FgsSalesActivity",
                column: "OpportunityId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesActivity_TenantId_CompanyId",
                schema: "crm",
                table: "FgsSalesActivity",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesActivity_TenantId_CompanyId_ActivityTypeId",
                schema: "crm",
                table: "FgsSalesActivity",
                columns: new[] { "TenantId", "CompanyId", "ActivityTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesActivity_TenantId_CompanyId_AssignedToUserId",
                schema: "crm",
                table: "FgsSalesActivity",
                columns: new[] { "TenantId", "CompanyId", "AssignedToUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesActivity_TenantId_CompanyId_CompletedOn",
                schema: "crm",
                table: "FgsSalesActivity",
                columns: new[] { "TenantId", "CompanyId", "CompletedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesActivity_TenantId_CompanyId_FollowUpActivityId",
                schema: "crm",
                table: "FgsSalesActivity",
                columns: new[] { "TenantId", "CompanyId", "FollowUpActivityId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesActivity_TenantId_CompanyId_FollowUpOn",
                schema: "crm",
                table: "FgsSalesActivity",
                columns: new[] { "TenantId", "CompanyId", "FollowUpOn" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesActivity_TenantId_CompanyId_LeadId",
                schema: "crm",
                table: "FgsSalesActivity",
                columns: new[] { "TenantId", "CompanyId", "LeadId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesActivity_TenantId_CompanyId_OpportunityId",
                schema: "crm",
                table: "FgsSalesActivity",
                columns: new[] { "TenantId", "CompanyId", "OpportunityId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesActivity_TenantId_CompanyId_PerformedByUserId",
                schema: "crm",
                table: "FgsSalesActivity",
                columns: new[] { "TenantId", "CompanyId", "PerformedByUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesActivity_TenantId_CompanyId_SalesActivityOutcomeId",
                schema: "crm",
                table: "FgsSalesActivity",
                columns: new[] { "TenantId", "CompanyId", "SalesActivityOutcomeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSalesActivity_TenantId_CompanyId_ScheduledOn",
                schema: "crm",
                table: "FgsSalesActivity",
                columns: new[] { "TenantId", "CompanyId", "ScheduledOn" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsSalesActivity",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "FgsOpportunity",
                schema: "crm");

            migrationBuilder.DropIndex(
                name: "IX_CrmLead_TenantId_CompanyId_ConvertedOpportunityId",
                schema: "crm",
                table: "CrmLead");

            migrationBuilder.DropIndex(
                name: "IX_CrmLead_TenantId_CompanyId_ServiceLocationId",
                schema: "crm",
                table: "CrmLead");

            migrationBuilder.DropColumn(
                name: "Address1",
                schema: "crm",
                table: "CrmLead");

            migrationBuilder.DropColumn(
                name: "Address2",
                schema: "crm",
                table: "CrmLead");

            migrationBuilder.DropColumn(
                name: "City",
                schema: "crm",
                table: "CrmLead");

            migrationBuilder.DropColumn(
                name: "ConvertedOpportunityId",
                schema: "crm",
                table: "CrmLead");

            migrationBuilder.DropColumn(
                name: "Country",
                schema: "crm",
                table: "CrmLead");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "crm",
                table: "CrmLead");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                schema: "crm",
                table: "CrmLead");

            migrationBuilder.DropColumn(
                name: "ServiceLocationId",
                schema: "crm",
                table: "CrmLead");

            migrationBuilder.DropColumn(
                name: "State",
                schema: "crm",
                table: "CrmLead");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:crm.SalesPriority", "LOW,NORMAL,HIGH");

            migrationBuilder.AlterTable(
                name: "CrmLead",
                schema: "crm",
                comment: "Stores inbound sales inquiries and prospects prior to qualification and conversion into customers and opportunities.",
                oldComment: "Stores sales leads/prospects received from the website, office users, technicians, referrals, campaigns, or other configured lead sources. A Lead may remain in the Lead pipeline, be disqualified/lost, be associated with an existing customer, or be converted into an Opportunity. Lead activities are stored separately in crm.FgsSalesActivity.");

            migrationBuilder.AlterColumn<long>(
                name: "LeadStatusId",
                schema: "crm",
                table: "CrmLead",
                type: "bigint",
                nullable: false,
                comment: "Current lead status selected from setup.FgsLeadStatus.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Current status of the lead selected from the configured sales pipeline statuses applicable to leads.");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LeadReceivedOn",
                schema: "crm",
                table: "CrmLead",
                type: "timestamp with time zone",
                nullable: false,
                comment: "Date and time the lead was originally received.",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "now()",
                oldComment: "Date and time the lead was originally received.");

            migrationBuilder.AlterColumn<string>(
                name: "LeadDescription",
                schema: "crm",
                table: "CrmLead",
                type: "text",
                nullable: true,
                comment: "Detailed description of the lead inquiry and customer requirements.",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "Comments and details describing the customer inquiry, service need, or information provided with the lead.");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DisqualifiedOn",
                schema: "crm",
                table: "CrmLead",
                type: "timestamp with time zone",
                nullable: true,
                comment: "Date and time the lead was disqualified.",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldNullable: true,
                oldComment: "Date and time the lead was disqualified.");

            migrationBuilder.AlterColumn<long>(
                name: "CustomerId",
                schema: "crm",
                table: "CrmLead",
                type: "bigint",
                nullable: true,
                comment: "Customer record created from this lead after conversion.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Existing customer associated with the lead, when applicable.");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ConvertedOn",
                schema: "crm",
                table: "CrmLead",
                type: "timestamp with time zone",
                nullable: true,
                comment: "Date and time the lead was converted into a customer.",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldNullable: true,
                oldComment: "Date and time the lead was converted into an opportunity.");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                schema: "crm",
                table: "CrmLead",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                comment: "Company or organization associated with the lead.");

            migrationBuilder.AddColumn<long>(
                name: "CustomerTypeId",
                schema: "crm",
                table: "CrmLead",
                type: "bigint",
                nullable: true,
                comment: "Customer type associated with the lead.");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "crm",
                table: "CrmLead",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Lead contact first name.");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "crm",
                table: "CrmLead",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Lead contact last name.");

            migrationBuilder.AddColumn<string>(
                name: "LeadSummary",
                schema: "crm",
                table: "CrmLead",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                comment: "Short summary describing the lead inquiry.");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "QualifiedOn",
                schema: "crm",
                table: "CrmLead",
                type: "timestamp with time zone",
                nullable: true,
                comment: "Date and time the lead was qualified.");

            migrationBuilder.AddColumn<string>(
                name: "ServiceZipCode",
                schema: "crm",
                table: "CrmLead",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                comment: "ZIP or postal code where service is requested.");

            migrationBuilder.CreateIndex(
                name: "IX_CrmLead_TenantId_CompanyId_CustomerTypeId",
                schema: "crm",
                table: "CrmLead",
                columns: new[] { "TenantId", "CompanyId", "CustomerTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmLead_TenantId_CompanyId_PrimaryContactMethodId",
                schema: "crm",
                table: "CrmLead",
                columns: new[] { "TenantId", "CompanyId", "PrimaryContactMethodId" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmLead_TenantId_CompanyId_ServiceZipCode",
                schema: "crm",
                table: "CrmLead",
                columns: new[] { "TenantId", "CompanyId", "ServiceZipCode" });
        }
    }
}
