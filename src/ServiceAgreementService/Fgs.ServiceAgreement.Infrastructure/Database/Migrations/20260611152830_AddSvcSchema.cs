using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.ServiceAgreement.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddSvcSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "svc");

            migrationBuilder.CreateTable(
                name: "FgsTenantCompanyCache",
                schema: "svc",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier mapped from tenant.FgsTenantCompany.CompanyNumber."),
                    CompanyGuid = table.Column<Guid>(type: "uuid", nullable: false, comment: "Globally unique company identifier used by integrations and external systems."),
                    CompanyCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Unique company code within a tenant."),
                    CompanyName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Display name of the company."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, comment: "Indicates whether the company is active."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Timestamp of the most recent synchronization from tenant.FgsTenantCompany.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsTenantCompanyCache", x => new { x.TenantId, x.CompanyId });
                },
                comment: "Local cache of tenant company information used by the svc schema to eliminate cross-schema dependencies on tenant.FgsTenantCompany.");

            migrationBuilder.CreateTable(
                name: "FgsServiceAgreement",
                schema: "svc",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AgreementNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "User visible service agreement number."),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false, comment: "Customer that owns the agreement."),
                    CustomerLocationId = table.Column<long>(type: "bigint", nullable: false, comment: "Service location covered by the agreement."),
                    EstimateId = table.Column<long>(type: "bigint", nullable: true, comment: "Estimate that was accepted and converted into this service agreement."),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Agreement name."),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Internal agreement description."),
                    Break1Id = table.Column<long>(type: "bigint", nullable: false, comment: "Business Unit classification for the agreement."),
                    Break2Id = table.Column<long>(type: "bigint", nullable: false, comment: "Secondary operational classification for the agreement."),
                    JobTypeId = table.Column<long>(type: "bigint", nullable: false, comment: "Job type associated with the agreement."),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "Agreement coverage start date."),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "Agreement coverage end date."),
                    ServiceAgreementStatusId = table.Column<short>(type: "smallint", nullable: false, comment: "Agreement lifecycle status. Values: 1=Draft, 2=Active, 3=Expired, 4=Cancelled."),
                    VisitFrequencyId = table.Column<short>(type: "smallint", nullable: false, comment: "Frequency used to generate service agreement visit schedules."),
                    BillingFrequencyId = table.Column<short>(type: "smallint", nullable: false, comment: "Frequency used to generate billing schedules."),
                    ContractAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, comment: "Total contract value. Billing schedule amounts are calculated from contract amount and billing frequency."),
                    LaborDiscountPercent = table.Column<decimal>(type: "numeric(5,2)", nullable: false, defaultValue: 0m, comment: "Labor discount percentage available under the agreement."),
                    MaterialDiscountPercent = table.Column<decimal>(type: "numeric(5,2)", nullable: false, defaultValue: 0m, comment: "Material discount percentage available under the agreement."),
                    AutoRenew = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the agreement should automatically renew at expiration."),
                    RenewedByServiceAgreementId = table.Column<long>(type: "bigint", nullable: true, comment: "Identifier of the agreement created when this agreement was renewed. Null indicates the agreement has not yet been renewed."),
                    SoldDate = table.Column<DateOnly>(type: "date", nullable: true, comment: "Date the agreement was sold."),
                    SoldByEmployeeId = table.Column<long>(type: "bigint", nullable: true, comment: "Employee that sold the agreement."),
                    ActivatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the agreement became active."),
                    CancelledOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the agreement was cancelled."),
                    ExternalEntityId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "External system identifier."),
                    ExternalVersion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "External synchronization token or version."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Record creation timestamp."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User that created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Record last update timestamp."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User that last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsServiceAgreement", x => x.Id);
                    table.CheckConstraint("CK_FgsServiceAgreement_Discounts", "\"LaborDiscountPercent\" BETWEEN 0 AND 100 AND \"MaterialDiscountPercent\" BETWEEN 0 AND 100");
                    table.CheckConstraint("CK_FgsServiceAgreement_EndDate", "\"EndDate\" >= \"StartDate\"");
                    table.CheckConstraint("CK_FgsServiceAgreement_Status", "\"ServiceAgreementStatusId\" IN (1, 2, 3, 4)");
                    table.ForeignKey(
                        name: "FK_FgsServiceAgreement_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "svc",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores recurring maintenance agreements, membership plans, preventive maintenance contracts, and service contracts. Visit schedules and billing schedules are generated into separate tables.");

            migrationBuilder.CreateTable(
                name: "FgsServiceAgreementCoveredAsset",
                schema: "svc",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceAgreementId = table.Column<long>(type: "bigint", nullable: false, comment: "Parent service agreement identifier."),
                    AssetId = table.Column<long>(type: "bigint", nullable: false, comment: "Covered customer asset identifier."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Record creation timestamp."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User that created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Record last update timestamp."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User that last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsServiceAgreementCoveredAsset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsServiceAgreementCoveredAsset_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "svc",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores assets covered under a service agreement. Covered assets are entitled to contract benefits such as labor discounts, material discounts, contract pricing, and service agreement coverage. Coverage is inherited from the parent agreement term.");

            migrationBuilder.CreateTable(
                name: "FgsServiceAgreementBillingSchedule",
                schema: "svc",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceAgreementId = table.Column<long>(type: "bigint", nullable: false, comment: "Parent service agreement identifier."),
                    BillingSequence = table.Column<int>(type: "integer", nullable: false, comment: "Sequential billing number within the service agreement."),
                    BillingDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "Scheduled billing date used for invoice generation."),
                    BillingAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, comment: "Amount expected to be billed for this billing event."),
                    BillingScheduleStatusId = table.Column<short>(type: "smallint", nullable: false, comment: "Billing schedule status. Values: 1=Pending, 2=InvoiceCreated, 3=Invoiced, 4=Skipped, 5=Cancelled."),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: true, comment: "Generated invoice identifier associated with the billing event."),
                    ExternalInvoiceNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Invoice number from a legacy or external system used during data migration."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Record creation timestamp."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User that created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Record last update timestamp."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User that last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsServiceAgreementBillingSchedule", x => x.Id);
                    table.CheckConstraint("CK_FgsServiceAgreementBillingSchedule_BillingAmount", "\"BillingAmount\" >= 0");
                    table.CheckConstraint("CK_FgsServiceAgreementBillingSchedule_Status", "\"BillingScheduleStatusId\" IN (1, 2, 3, 4, 5)");
                    table.ForeignKey(
                        name: "FK_FgsServiceAgreementBillingSchedule_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "svc",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsServiceAgreementBillingSchedule_ServiceAgreement",
                        column: x => x.ServiceAgreementId,
                        principalSchema: "svc",
                        principalTable: "FgsServiceAgreement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Stores future billing events generated from a service agreement. Billing schedules generate invoices but are not invoices themselves.");

            migrationBuilder.CreateTable(
                name: "FgsServiceAgreementVisit",
                schema: "svc",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceAgreementId = table.Column<long>(type: "bigint", nullable: false, comment: "Parent service agreement identifier."),
                    VisitNumber = table.Column<int>(type: "integer", nullable: false, comment: "Sequential visit number within the service agreement."),
                    JobTypeId = table.Column<long>(type: "bigint", nullable: false, comment: "Job type used when generating a work order from the service agreement visit."),
                    ExpectedServiceDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "Expected date the maintenance service should be performed according to the agreement."),
                    ServiceAgreementVisitStatusId = table.Column<short>(type: "smallint", nullable: false, comment: "Visit lifecycle status. Values: 1=Pending, 2=WorkOrderCreated, 3=Completed, 4=Skipped, 5=Cancelled."),
                    WorkOrderId = table.Column<long>(type: "bigint", nullable: true, comment: "Generated work order identifier associated with the visit."),
                    ExternalWorkOrderNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Work order number from a legacy or external system used during data migration."),
                    CompletedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the maintenance visit was completed."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Record creation timestamp."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User that created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Record last update timestamp."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User that last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsServiceAgreementVisit", x => x.Id);
                    table.CheckConstraint("CK_FgsServiceAgreementVisit_Status", "\"ServiceAgreementVisitStatusId\" IN (1, 2, 3, 4, 5)");
                    table.ForeignKey(
                        name: "FK_FgsServiceAgreementVisit_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "svc",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsServiceAgreementVisit_ServiceAgreement",
                        column: x => x.ServiceAgreementId,
                        principalSchema: "svc",
                        principalTable: "FgsServiceAgreement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Stores contractually required maintenance visits generated from a service agreement. Visits may later generate work orders to perform the required maintenance service.");

            migrationBuilder.CreateTable(
                name: "FgsServiceAgreementNote",
                schema: "svc",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceAgreementId = table.Column<long>(type: "bigint", nullable: true, comment: "Parent service agreement identifier."),
                    ServiceAgreementVisitId = table.Column<long>(type: "bigint", nullable: true, comment: "Parent service agreement visit identifier."),
                    ServiceAgreementBillingScheduleId = table.Column<long>(type: "bigint", nullable: true, comment: "Parent service agreement billing schedule identifier."),
                    NoteTypeId = table.Column<long>(type: "bigint", nullable: true, comment: "Optional note classification identifier."),
                    Note = table.Column<string>(type: "text", nullable: false, comment: "Note text."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Record creation timestamp."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User that created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Record last update timestamp."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User that last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsServiceAgreementNote", x => x.Id);
                    table.CheckConstraint("CK_FgsServiceAgreementNote_Parent", "(\"ServiceAgreementId\" IS NOT NULL AND \"ServiceAgreementVisitId\" IS NULL AND \"ServiceAgreementBillingScheduleId\" IS NULL)\r\nOR (\"ServiceAgreementId\" IS NULL AND \"ServiceAgreementVisitId\" IS NOT NULL AND \"ServiceAgreementBillingScheduleId\" IS NULL)\r\nOR (\"ServiceAgreementId\" IS NULL AND \"ServiceAgreementVisitId\" IS NULL AND \"ServiceAgreementBillingScheduleId\" IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_FgsServiceAgreementNote_BillingSchedule",
                        column: x => x.ServiceAgreementBillingScheduleId,
                        principalSchema: "svc",
                        principalTable: "FgsServiceAgreementBillingSchedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsServiceAgreementNote_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "svc",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsServiceAgreementNote_ServiceAgreement",
                        column: x => x.ServiceAgreementId,
                        principalSchema: "svc",
                        principalTable: "FgsServiceAgreement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsServiceAgreementNote_ServiceAgreementVisit",
                        column: x => x.ServiceAgreementVisitId,
                        principalSchema: "svc",
                        principalTable: "FgsServiceAgreementVisit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Stores notes related to service agreements, service agreement visits, and service agreement billing schedules. A note belongs to exactly one parent entity.");

            migrationBuilder.CreateTable(
                name: "FgsServiceAgreementVisitAsset",
                schema: "svc",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceAgreementId = table.Column<long>(type: "bigint", nullable: false, comment: "Parent service agreement identifier."),
                    ServiceAgreementVisitId = table.Column<long>(type: "bigint", nullable: false, comment: "Parent service agreement visit identifier."),
                    AssetId = table.Column<long>(type: "bigint", nullable: false, comment: "Asset associated with the service agreement visit."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Record creation timestamp."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User that created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Record last update timestamp."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User that last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsServiceAgreementVisitAsset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsServiceAgreementVisitAsset_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "svc",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsServiceAgreementVisitAsset_ServiceAgreementVisit",
                        column: x => x.ServiceAgreementVisitId,
                        principalSchema: "svc",
                        principalTable: "FgsServiceAgreementVisit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Stores assets associated with a service agreement maintenance visit. A visit may include one or more covered assets that require maintenance service.");

            migrationBuilder.CreateTable(
                name: "FgsServiceAgreementVisitItem",
                schema: "svc",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceAgreementId = table.Column<long>(type: "bigint", nullable: false, comment: "Parent service agreement identifier."),
                    ServiceAgreementVisitId = table.Column<long>(type: "bigint", nullable: false, comment: "Parent service agreement visit identifier."),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: true, comment: "Inventory item identifier. May be NULL when the item is manually entered."),
                    ItemName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Item name used when the item does not exist in the inventory catalog."),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Additional item description or maintenance instructions."),
                    Quantity = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 1m, comment: "Expected quantity required for the maintenance visit."),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the item is required for the maintenance visit."),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 1, comment: "Display order within the maintenance visit item list."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Record creation timestamp."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User that created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Record last update timestamp."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User that last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsServiceAgreementVisitItem", x => x.Id);
                    table.CheckConstraint("CK_FgsServiceAgreementVisitItem_Item", "\"InventoryItemId\" IS NOT NULL OR COALESCE(TRIM(\"ItemName\"), '') <> ''");
                    table.CheckConstraint("CK_FgsServiceAgreementVisitItem_Quantity", "\"Quantity\" > 0");
                    table.ForeignKey(
                        name: "FK_FgsServiceAgreementVisitItem_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "svc",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsServiceAgreementVisitItem_ServiceAgreementVisit",
                        column: x => x.ServiceAgreementVisitId,
                        principalSchema: "svc",
                        principalTable: "FgsServiceAgreementVisit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Stores recommended items associated with a service agreement maintenance visit. Items may come from the inventory catalog or be entered manually. Actual material usage is recorded on the work order.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreement_CustomerId",
                schema: "svc",
                table: "FgsServiceAgreement",
                columns: new[] { "TenantId", "CompanyId", "CustomerId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreement_CustomerLocationId",
                schema: "svc",
                table: "FgsServiceAgreement",
                columns: new[] { "TenantId", "CompanyId", "CustomerLocationId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreement_EndDate",
                schema: "svc",
                table: "FgsServiceAgreement",
                columns: new[] { "TenantId", "CompanyId", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreement_Status",
                schema: "svc",
                table: "FgsServiceAgreement",
                columns: new[] { "TenantId", "CompanyId", "ServiceAgreementStatusId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreement_TenantId_CompanyId",
                schema: "svc",
                table: "FgsServiceAgreement",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsServiceAgreement_AgreementNumber",
                schema: "svc",
                table: "FgsServiceAgreement",
                columns: new[] { "TenantId", "CompanyId", "AgreementNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementBillingSchedule_BillingDate",
                schema: "svc",
                table: "FgsServiceAgreementBillingSchedule",
                columns: new[] { "TenantId", "CompanyId", "BillingDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementBillingSchedule_InvoiceId",
                schema: "svc",
                table: "FgsServiceAgreementBillingSchedule",
                columns: new[] { "TenantId", "CompanyId", "InvoiceId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementBillingSchedule_ServiceAgreementId",
                schema: "svc",
                table: "FgsServiceAgreementBillingSchedule",
                columns: new[] { "TenantId", "CompanyId", "ServiceAgreementId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementBillingSchedule_ServiceAgreementId1",
                schema: "svc",
                table: "FgsServiceAgreementBillingSchedule",
                column: "ServiceAgreementId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementBillingSchedule_Status",
                schema: "svc",
                table: "FgsServiceAgreementBillingSchedule",
                columns: new[] { "TenantId", "CompanyId", "BillingScheduleStatusId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementBillingSchedule_TenantId_CompanyId",
                schema: "svc",
                table: "FgsServiceAgreementBillingSchedule",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsServiceAgreementBillingSchedule_Agreement_Sequence",
                schema: "svc",
                table: "FgsServiceAgreementBillingSchedule",
                columns: new[] { "TenantId", "CompanyId", "ServiceAgreementId", "BillingSequence" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementCoveredAsset_AssetId",
                schema: "svc",
                table: "FgsServiceAgreementCoveredAsset",
                columns: new[] { "TenantId", "CompanyId", "AssetId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementCoveredAsset_ServiceAgreementId",
                schema: "svc",
                table: "FgsServiceAgreementCoveredAsset",
                columns: new[] { "TenantId", "CompanyId", "ServiceAgreementId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementCoveredAsset_TenantId_CompanyId",
                schema: "svc",
                table: "FgsServiceAgreementCoveredAsset",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsServiceAgreementCoveredAsset_Agreement_Asset",
                schema: "svc",
                table: "FgsServiceAgreementCoveredAsset",
                columns: new[] { "TenantId", "CompanyId", "ServiceAgreementId", "AssetId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementNote_BillingScheduleId",
                schema: "svc",
                table: "FgsServiceAgreementNote",
                columns: new[] { "TenantId", "CompanyId", "ServiceAgreementBillingScheduleId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementNote_ServiceAgreementBillingScheduleId",
                schema: "svc",
                table: "FgsServiceAgreementNote",
                column: "ServiceAgreementBillingScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementNote_ServiceAgreementId",
                schema: "svc",
                table: "FgsServiceAgreementNote",
                columns: new[] { "TenantId", "CompanyId", "ServiceAgreementId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementNote_ServiceAgreementId1",
                schema: "svc",
                table: "FgsServiceAgreementNote",
                column: "ServiceAgreementId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementNote_ServiceAgreementVisitId",
                schema: "svc",
                table: "FgsServiceAgreementNote",
                columns: new[] { "TenantId", "CompanyId", "ServiceAgreementVisitId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementNote_ServiceAgreementVisitId1",
                schema: "svc",
                table: "FgsServiceAgreementNote",
                column: "ServiceAgreementVisitId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementNote_TenantId_CompanyId",
                schema: "svc",
                table: "FgsServiceAgreementNote",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementVisit_ExpectedServiceDate",
                schema: "svc",
                table: "FgsServiceAgreementVisit",
                columns: new[] { "TenantId", "CompanyId", "ExpectedServiceDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementVisit_JobTypeId",
                schema: "svc",
                table: "FgsServiceAgreementVisit",
                columns: new[] { "TenantId", "CompanyId", "JobTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementVisit_ServiceAgreementId",
                schema: "svc",
                table: "FgsServiceAgreementVisit",
                columns: new[] { "TenantId", "CompanyId", "ServiceAgreementId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementVisit_ServiceAgreementId1",
                schema: "svc",
                table: "FgsServiceAgreementVisit",
                column: "ServiceAgreementId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementVisit_Status",
                schema: "svc",
                table: "FgsServiceAgreementVisit",
                columns: new[] { "TenantId", "CompanyId", "ServiceAgreementVisitStatusId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementVisit_TenantId_CompanyId",
                schema: "svc",
                table: "FgsServiceAgreementVisit",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementVisit_WorkOrderId",
                schema: "svc",
                table: "FgsServiceAgreementVisit",
                columns: new[] { "TenantId", "CompanyId", "WorkOrderId" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsServiceAgreementVisit_Agreement_VisitNumber",
                schema: "svc",
                table: "FgsServiceAgreementVisit",
                columns: new[] { "TenantId", "CompanyId", "ServiceAgreementId", "VisitNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementVisitAsset_AssetId",
                schema: "svc",
                table: "FgsServiceAgreementVisitAsset",
                columns: new[] { "TenantId", "CompanyId", "AssetId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementVisitAsset_ServiceAgreementId",
                schema: "svc",
                table: "FgsServiceAgreementVisitAsset",
                columns: new[] { "TenantId", "CompanyId", "ServiceAgreementId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementVisitAsset_ServiceAgreementVisitId",
                schema: "svc",
                table: "FgsServiceAgreementVisitAsset",
                columns: new[] { "TenantId", "CompanyId", "ServiceAgreementVisitId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementVisitAsset_ServiceAgreementVisitId1",
                schema: "svc",
                table: "FgsServiceAgreementVisitAsset",
                column: "ServiceAgreementVisitId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementVisitAsset_TenantId_CompanyId",
                schema: "svc",
                table: "FgsServiceAgreementVisitAsset",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsServiceAgreementVisitAsset_Visit_Asset",
                schema: "svc",
                table: "FgsServiceAgreementVisitAsset",
                columns: new[] { "TenantId", "CompanyId", "ServiceAgreementVisitId", "AssetId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementVisitItem_DisplayOrder",
                schema: "svc",
                table: "FgsServiceAgreementVisitItem",
                columns: new[] { "TenantId", "CompanyId", "ServiceAgreementVisitId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementVisitItem_InventoryItemId",
                schema: "svc",
                table: "FgsServiceAgreementVisitItem",
                columns: new[] { "TenantId", "CompanyId", "InventoryItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementVisitItem_IsRequired",
                schema: "svc",
                table: "FgsServiceAgreementVisitItem",
                columns: new[] { "TenantId", "CompanyId", "IsRequired" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementVisitItem_ServiceAgreementId",
                schema: "svc",
                table: "FgsServiceAgreementVisitItem",
                columns: new[] { "TenantId", "CompanyId", "ServiceAgreementId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementVisitItem_ServiceAgreementVisitId",
                schema: "svc",
                table: "FgsServiceAgreementVisitItem",
                columns: new[] { "TenantId", "CompanyId", "ServiceAgreementVisitId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementVisitItem_ServiceAgreementVisitId1",
                schema: "svc",
                table: "FgsServiceAgreementVisitItem",
                column: "ServiceAgreementVisitId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsServiceAgreementVisitItem_TenantId_CompanyId",
                schema: "svc",
                table: "FgsServiceAgreementVisitItem",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsTenantCompanyCache_CompanyName",
                schema: "svc",
                table: "FgsTenantCompanyCache",
                column: "CompanyName");

            migrationBuilder.CreateIndex(
                name: "IX_FgsTenantCompanyCache_IsActive",
                schema: "svc",
                table: "FgsTenantCompanyCache",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "UQ_FgsTenantCompanyCache_CompanyGuid",
                schema: "svc",
                table: "FgsTenantCompanyCache",
                column: "CompanyGuid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsServiceAgreementCoveredAsset",
                schema: "svc");

            migrationBuilder.DropTable(
                name: "FgsServiceAgreementNote",
                schema: "svc");

            migrationBuilder.DropTable(
                name: "FgsServiceAgreementVisitAsset",
                schema: "svc");

            migrationBuilder.DropTable(
                name: "FgsServiceAgreementVisitItem",
                schema: "svc");

            migrationBuilder.DropTable(
                name: "FgsServiceAgreementBillingSchedule",
                schema: "svc");

            migrationBuilder.DropTable(
                name: "FgsServiceAgreementVisit",
                schema: "svc");

            migrationBuilder.DropTable(
                name: "FgsServiceAgreement",
                schema: "svc");

            migrationBuilder.DropTable(
                name: "FgsTenantCompanyCache",
                schema: "svc");
        }
    }
}
