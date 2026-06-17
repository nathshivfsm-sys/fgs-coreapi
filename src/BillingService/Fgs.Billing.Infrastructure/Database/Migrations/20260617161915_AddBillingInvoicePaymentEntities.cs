using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Billing.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddBillingInvoicePaymentEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsInvoiceBatch",
                schema: "billing",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    BatchNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BatchDate = table.Column<DateOnly>(type: "date", nullable: false),
                    InvoiceCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    InvoiceSubtotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    TotalTax = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    InvoiceTotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    IsClosed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ClosedOn = table.Column<DateTime>(type: "timestamp", nullable: true),
                    ClosedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInvoiceBatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsInvoiceBatch_TenantCompany",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "billing",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsPayment",
                schema: "billing",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    PaymentNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceLocationId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentTypeId = table.Column<int>(type: "integer", nullable: false),
                    PaymentMethodId = table.Column<int>(type: "integer", nullable: false),
                    PaymentStatusId = table.Column<int>(type: "integer", nullable: false),
                    SourceType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SourceId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    AccountingDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    BankAccountId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    AppliedAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    DepositDate = table.Column<DateOnly>(type: "date", nullable: true),
                    PaymentNote = table.Column<string>(type: "text", nullable: true),
                    ExternalAccountingId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ExternalAccountingSyncToken = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsPayment_TenantCompany",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "billing",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores customer payment transactions received for invoices, estimates, service agreements, deposits, refunds, and other billing-related activities.");

            migrationBuilder.CreateTable(
                name: "FgsInvoice",
                schema: "billing",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    InvoiceNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    InvoiceTypeId = table.Column<short>(type: "smallint", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceLocationId = table.Column<long>(type: "bigint", nullable: false),
                    WorkOrderId = table.Column<long>(type: "bigint", nullable: true),
                    ProjectId = table.Column<long>(type: "bigint", nullable: true),
                    ServiceAgreementId = table.Column<long>(type: "bigint", nullable: true),
                    MaintenanceVisitId = table.Column<long>(type: "bigint", nullable: true),
                    ServiceJobNum = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsAgreementBilling = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsRecurringInvoice = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    RecurringScheduleId = table.Column<long>(type: "bigint", nullable: true),
                    WorkOrderNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    JobTypeId = table.Column<int>(type: "integer", nullable: true),
                    LeadEmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerPONumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    InvoiceDate = table.Column<DateOnly>(type: "date", nullable: false),
                    AccountingDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    NetTermId = table.Column<int>(type: "integer", nullable: true),
                    PreferredPaymentMethodId = table.Column<int>(type: "integer", nullable: true),
                    LaborPricingMatrixId = table.Column<long>(type: "bigint", nullable: true),
                    MaterialPricingMatrixId = table.Column<long>(type: "bigint", nullable: true),
                    OtherPricingMatrixId = table.Column<long>(type: "bigint", nullable: true),
                    GLBreak1Id = table.Column<int>(type: "integer", nullable: true),
                    GLBreak2Id = table.Column<int>(type: "integer", nullable: true),
                    TaxingAuthorityJson = table.Column<string>(type: "jsonb", nullable: true),
                    BillToAddressJson = table.Column<string>(type: "jsonb", nullable: true),
                    ServiceLocationAddressJson = table.Column<string>(type: "jsonb", nullable: true),
                    CompanyAddressJson = table.Column<string>(type: "jsonb", nullable: true),
                    InvoiceTemplateId = table.Column<long>(type: "bigint", nullable: true),
                    IsSigned = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    SignedOn = table.Column<DateTime>(type: "timestamp", nullable: true),
                    InvoiceSubtotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    TotalDiscount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    TaxableAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    TotalTax = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    InvoiceTotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    AppliedAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    BalanceDue = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ApprovedBy = table.Column<long>(type: "bigint", nullable: true),
                    ApprovedOn = table.Column<DateTime>(type: "timestamp", nullable: true),
                    IsPosted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    PostedBy = table.Column<long>(type: "bigint", nullable: true),
                    PostedOn = table.Column<DateTime>(type: "timestamp", nullable: true),
                    InvoiceBatchId = table.Column<long>(type: "bigint", nullable: true),
                    ExternalAccountingId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ExternalAccountingSyncToken = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsInvoice_InvoiceBatch",
                        column: x => x.InvoiceBatchId,
                        principalSchema: "billing",
                        principalTable: "FgsInvoiceBatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInvoice_TenantCompany",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "billing",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsPaymentTransaction",
                schema: "billing",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    PaymentId = table.Column<long>(type: "bigint", nullable: false),
                    TransactionTypeId = table.Column<int>(type: "integer", nullable: false),
                    TransactionMethodId = table.Column<int>(type: "integer", nullable: false),
                    PaymentProcessorId = table.Column<int>(type: "integer", nullable: false),
                    TransactionId = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    OriginalTransactionId = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    AuthorizationCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ProcessorStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CardHolderName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CardLast4 = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    BankAccountLast4 = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    TransactionAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    UserName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Source = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TransactionDataJson = table.Column<string>(type: "jsonb", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsPaymentTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsPaymentTransaction_Payment",
                        column: x => x.PaymentId,
                        principalSchema: "billing",
                        principalTable: "FgsPayment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsPaymentTransaction_TenantCompany",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "billing",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores payment processor transaction records associated with customer payments.");

            migrationBuilder.CreateTable(
                name: "FgsInvoiceDetail",
                schema: "billing",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: false),
                    ParentLineId = table.Column<long>(type: "bigint", nullable: true),
                    LineNumber = table.Column<int>(type: "integer", nullable: false),
                    BillingCategoryId = table.Column<int>(type: "integer", nullable: false),
                    ItemCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ItemDescription = table.Column<string>(type: "text", nullable: false),
                    IsInventory = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    MasterPartNum = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: true),
                    PriceBookItemId = table.Column<long>(type: "bigint", nullable: true),
                    LaborRateTypeId = table.Column<int>(type: "integer", nullable: true),
                    TechnicianId = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 1m),
                    UnitCost = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 0m),
                    ExtendedCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 0m),
                    ExtendedPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    IsTaxable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    GLBreak1Id = table.Column<int>(type: "integer", nullable: true),
                    GLBreak2Id = table.Column<int>(type: "integer", nullable: true),
                    LineAddedFrom = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LineAddedFromId = table.Column<long>(type: "bigint", nullable: true),
                    AddedSource = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInvoiceDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsInvoiceDetail_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "billing",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInvoiceDetail_Invoice",
                        column: x => x.InvoiceId,
                        principalSchema: "billing",
                        principalTable: "FgsInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsInvoiceDetail_ParentLine",
                        column: x => x.ParentLineId,
                        principalSchema: "billing",
                        principalTable: "FgsInvoiceDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsInvoicePaymentApplication",
                schema: "billing",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    PaymentId = table.Column<long>(type: "bigint", nullable: false),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: false),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    AppliedAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    AppliedOn = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    ApplicationNote = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInvoicePaymentApplication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsInvoicePaymentApplication_Invoice",
                        column: x => x.InvoiceId,
                        principalSchema: "billing",
                        principalTable: "FgsInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInvoicePaymentApplication_Payment",
                        column: x => x.PaymentId,
                        principalSchema: "billing",
                        principalTable: "FgsPayment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInvoicePaymentApplication_TenantCompany",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "billing",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores payment allocation records between payments and invoices.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvoice_AccountingDate",
                schema: "billing",
                table: "FgsInvoice",
                columns: new[] { "TenantId", "CompanyId", "AccountingDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvoice_CustomerId",
                schema: "billing",
                table: "FgsInvoice",
                columns: new[] { "TenantId", "CompanyId", "CustomerId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvoice_InvoiceBatchId",
                schema: "billing",
                table: "FgsInvoice",
                columns: new[] { "TenantId", "CompanyId", "InvoiceBatchId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvoice_InvoiceBatchId1",
                schema: "billing",
                table: "FgsInvoice",
                column: "InvoiceBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvoice_InvoiceDate",
                schema: "billing",
                table: "FgsInvoice",
                columns: new[] { "TenantId", "CompanyId", "InvoiceDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvoice_ServiceAgreementId",
                schema: "billing",
                table: "FgsInvoice",
                columns: new[] { "TenantId", "CompanyId", "ServiceAgreementId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvoice_ServiceLocationId",
                schema: "billing",
                table: "FgsInvoice",
                columns: new[] { "TenantId", "CompanyId", "ServiceLocationId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvoice_WorkOrderId",
                schema: "billing",
                table: "FgsInvoice",
                columns: new[] { "TenantId", "CompanyId", "WorkOrderId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsInvoice_TenantCompany_InvoiceNumber",
                schema: "billing",
                table: "FgsInvoice",
                columns: new[] { "TenantId", "CompanyId", "InvoiceNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvoiceBatch_BatchDate",
                schema: "billing",
                table: "FgsInvoiceBatch",
                columns: new[] { "TenantId", "CompanyId", "BatchDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvoiceBatch_TenantCompany",
                schema: "billing",
                table: "FgsInvoiceBatch",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsInvoiceBatch_TenantCompany_BatchNumber",
                schema: "billing",
                table: "FgsInvoiceBatch",
                columns: new[] { "TenantId", "CompanyId", "BatchNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvoiceDetail_InvoiceId",
                schema: "billing",
                table: "FgsInvoiceDetail",
                columns: new[] { "TenantId", "CompanyId", "InvoiceId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvoiceDetail_InvoiceId_LineNumber",
                schema: "billing",
                table: "FgsInvoiceDetail",
                columns: new[] { "TenantId", "CompanyId", "InvoiceId", "LineNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvoiceDetail_InvoiceId1",
                schema: "billing",
                table: "FgsInvoiceDetail",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvoiceDetail_ParentLineId",
                schema: "billing",
                table: "FgsInvoiceDetail",
                column: "ParentLineId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvoicePaymentApplication_AppliedOn",
                schema: "billing",
                table: "FgsInvoicePaymentApplication",
                columns: new[] { "TenantId", "CompanyId", "AppliedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvoicePaymentApplication_Invoice",
                schema: "billing",
                table: "FgsInvoicePaymentApplication",
                columns: new[] { "TenantId", "CompanyId", "InvoiceId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvoicePaymentApplication_InvoiceId",
                schema: "billing",
                table: "FgsInvoicePaymentApplication",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvoicePaymentApplication_Payment",
                schema: "billing",
                table: "FgsInvoicePaymentApplication",
                columns: new[] { "TenantId", "CompanyId", "PaymentId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvoicePaymentApplication_TenantCompany",
                schema: "billing",
                table: "FgsInvoicePaymentApplication",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsInvoicePaymentApplication_PaymentInvoice",
                schema: "billing",
                table: "FgsInvoicePaymentApplication",
                columns: new[] { "PaymentId", "InvoiceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsPayment_AccountingDate",
                schema: "billing",
                table: "FgsPayment",
                columns: new[] { "TenantId", "CompanyId", "AccountingDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPayment_BankAccount",
                schema: "billing",
                table: "FgsPayment",
                columns: new[] { "TenantId", "CompanyId", "BankAccountId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPayment_Customer",
                schema: "billing",
                table: "FgsPayment",
                columns: new[] { "TenantId", "CompanyId", "CustomerId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPayment_PaymentDate",
                schema: "billing",
                table: "FgsPayment",
                columns: new[] { "TenantId", "CompanyId", "PaymentDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPayment_ServiceLocation",
                schema: "billing",
                table: "FgsPayment",
                columns: new[] { "TenantId", "CompanyId", "ServiceLocationId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPayment_Source",
                schema: "billing",
                table: "FgsPayment",
                columns: new[] { "TenantId", "CompanyId", "SourceType", "SourceId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPayment_Status",
                schema: "billing",
                table: "FgsPayment",
                columns: new[] { "TenantId", "CompanyId", "PaymentStatusId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPayment_TenantCompany",
                schema: "billing",
                table: "FgsPayment",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsPayment_TenantCompany_PaymentNumber",
                schema: "billing",
                table: "FgsPayment",
                columns: new[] { "TenantId", "CompanyId", "PaymentNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsPaymentTransaction_OriginalTransactionId",
                schema: "billing",
                table: "FgsPaymentTransaction",
                columns: new[] { "TenantId", "CompanyId", "OriginalTransactionId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPaymentTransaction_Payment",
                schema: "billing",
                table: "FgsPaymentTransaction",
                columns: new[] { "TenantId", "CompanyId", "PaymentId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPaymentTransaction_PaymentId",
                schema: "billing",
                table: "FgsPaymentTransaction",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsPaymentTransaction_Processor",
                schema: "billing",
                table: "FgsPaymentTransaction",
                columns: new[] { "TenantId", "CompanyId", "PaymentProcessorId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPaymentTransaction_ProcessorStatus",
                schema: "billing",
                table: "FgsPaymentTransaction",
                columns: new[] { "TenantId", "CompanyId", "ProcessorStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPaymentTransaction_TenantCompany",
                schema: "billing",
                table: "FgsPaymentTransaction",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPaymentTransaction_TransactionDate",
                schema: "billing",
                table: "FgsPaymentTransaction",
                columns: new[] { "TenantId", "CompanyId", "TransactionDate" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsPaymentTransaction_TransactionId",
                schema: "billing",
                table: "FgsPaymentTransaction",
                columns: new[] { "TenantId", "CompanyId", "TransactionId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsInvoiceDetail",
                schema: "billing");

            migrationBuilder.DropTable(
                name: "FgsInvoicePaymentApplication",
                schema: "billing");

            migrationBuilder.DropTable(
                name: "FgsPaymentTransaction",
                schema: "billing");

            migrationBuilder.DropTable(
                name: "FgsInvoice",
                schema: "billing");

            migrationBuilder.DropTable(
                name: "FgsPayment",
                schema: "billing");

            migrationBuilder.DropTable(
                name: "FgsInvoiceBatch",
                schema: "billing");
        }
    }
}
