using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Crm.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFgsEstimateEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsEstimateClause",
                schema: "crm",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    ClauseTypeId = table.Column<long>(type: "bigint", nullable: false, comment: "Clause type such as Inclusion, Exclusion, or Terms and Conditions."),
                    ClauseName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "User-friendly clause name."),
                    ClauseText = table.Column<string>(type: "text", nullable: false, comment: "Customer-facing clause text displayed on estimate documents."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Default display order."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the clause is available for use."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsEstimateClause", x => x.Id);
                    table.CheckConstraint("CK_FgsEstimateClause_DisplayOrder", "\"DisplayOrder\" > 0");
                    table.ForeignKey(
                        name: "FK_FgsEstimateClause_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "crm",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores reusable estimate clauses that may be used across estimates and estimate templates.");

            migrationBuilder.CreateTable(
                name: "FgsEstimateFlavor",
                schema: "crm",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    FlavorCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BackgroundColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TextColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsEstimateFlavor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsEstimateFlavor_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "crm",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores estimate flavor definitions used to visually categorize estimate options such as Good, Better, Best, Popular, Premium, Bronze, Silver, and Gold.");

            migrationBuilder.CreateTable(
                name: "FgsEstimateStatus",
                schema: "crm",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    StatusCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Immutable system status code used by application business logic."),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "User-facing display name that may be customized by the tenant."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsEstimateStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsEstimateStatus_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "crm",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores estimate statuses available to a specific tenant/company. StatusCode is immutable and used by application business logic.");

            migrationBuilder.CreateTable(
                name: "FgsEstimateTemplateCategory",
                schema: "crm",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    CategoryCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Unique internal category code within a company."),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "User-facing category name."),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Optional category description."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls display sequence of categories."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsEstimateTemplateCategory", x => x.Id);
                    table.CheckConstraint("CK_FgsEstimateTemplateCategory_DisplayOrder", "\"DisplayOrder\" > 0");
                    table.ForeignKey(
                        name: "FK_FgsEstimateTemplateCategory_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "crm",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores estimate template categories used to organize estimate templates into logical groups.");

            migrationBuilder.CreateTable(
                name: "FgsEstimate",
                schema: "crm",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    EstimateNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "User-facing estimate number."),
                    EstimateStatusId = table.Column<long>(type: "bigint", nullable: false, comment: "Current estimate status."),
                    EstimateTypeId = table.Column<long>(type: "bigint", nullable: false, comment: "Estimate presentation style such as Single Option or Good Better Best."),
                    EstimateSourceId = table.Column<long>(type: "bigint", nullable: true, comment: "Source that originated the estimate."),
                    OpportunityId = table.Column<long>(type: "bigint", nullable: true, comment: "Associated opportunity."),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false, comment: "Associated customer."),
                    ServiceLocationId = table.Column<long>(type: "bigint", nullable: false, comment: "Service location where work will be performed."),
                    WorkOrderId = table.Column<long>(type: "bigint", nullable: true, comment: "Work order generated from the estimate."),
                    JobTypeId = table.Column<long>(type: "bigint", nullable: true, comment: "Job type associated with the estimate."),
                    PaymentTermId = table.Column<long>(type: "bigint", nullable: true, comment: "Payment terms applicable to the estimate."),
                    PaymentMethodId = table.Column<long>(type: "bigint", nullable: true, comment: "Preferred payment method for the estimate."),
                    Break1Id = table.Column<long>(type: "bigint", nullable: true, comment: "First accounting segment used for GL exports and reporting."),
                    Break2Id = table.Column<long>(type: "bigint", nullable: true, comment: "Second accounting segment used for GL exports and reporting."),
                    QuoteName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "User-facing quote name."),
                    QuoteDescription = table.Column<string>(type: "text", nullable: true, comment: "Detailed quote description presented to the customer."),
                    EstimateDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "Date estimate was created."),
                    ExpirationDate = table.Column<DateOnly>(type: "date", nullable: true, comment: "Date estimate expires."),
                    QuotedByEmployeeId = table.Column<long>(type: "bigint", nullable: true, comment: "Employee who prepared or presented the estimate."),
                    SoldByEmployeeId = table.Column<long>(type: "bigint", nullable: true, comment: "Employee credited with the sale."),
                    SelectedEstimateOptionId = table.Column<long>(type: "bigint", nullable: true, comment: "Estimate option selected by the customer."),
                    SignedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Name entered by the person signing the estimate."),
                    SignedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the estimate was signed."),
                    SignatureFileId = table.Column<long>(type: "bigint", nullable: true, comment: "File identifier pointing to the signature image stored in file.FgsFile."),
                    TaxAuthoritySnapshotJson = table.Column<string>(type: "jsonb", nullable: true, comment: "Historical snapshot of tax authority codes, names, and rates used for tax calculations."),
                    MaterialPricingMatrixId = table.Column<long>(type: "bigint", nullable: true, comment: "Material pricing matrix used for pricing calculations."),
                    LaborPricingMatrixId = table.Column<long>(type: "bigint", nullable: true, comment: "Labor pricing matrix used for pricing calculations."),
                    OtherPricingMatrixId = table.Column<long>(type: "bigint", nullable: true, comment: "Other pricing matrix used for pricing calculations."),
                    SubtotalAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Subtotal before discounts and taxes."),
                    DiscountAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Total discount amount."),
                    TaxAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Total tax amount."),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Final estimate amount."),
                    GrossProfitAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Gross profit amount."),
                    GrossProfitPercent = table.Column<decimal>(type: "numeric(9,4)", nullable: false, defaultValue: 0m, comment: "Gross profit percentage."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsEstimate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsEstimate_EstimateStatus",
                        column: x => x.EstimateStatusId,
                        principalSchema: "crm",
                        principalTable: "FgsEstimateStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsEstimate_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "crm",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores estimate header information and pricing totals for the selected/sold estimate option.");

            migrationBuilder.CreateTable(
                name: "FgsEstimateTemplate",
                schema: "crm",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false, comment: "Template category."),
                    TemplateCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Unique internal template code within a company."),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "User-facing template name."),
                    TemplateDescription = table.Column<string>(type: "text", nullable: true, comment: "Description copied into estimate description when estimate is generated from template."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls display sequence within a category."),
                    ShowToFieldTechnician = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether template-generated content should be visible to field technicians."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether template is available for use."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsEstimateTemplate", x => x.Id);
                    table.CheckConstraint("CK_FgsEstimateTemplate_DisplayOrder", "\"DisplayOrder\" > 0");
                    table.ForeignKey(
                        name: "FK_FgsEstimateTemplate_Category",
                        column: x => x.CategoryId,
                        principalSchema: "crm",
                        principalTable: "FgsEstimateTemplateCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsEstimateTemplate_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "crm",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores reusable estimate templates used to generate estimate options and pricing lines.");

            migrationBuilder.CreateTable(
                name: "FgsEstimateClauseItem",
                schema: "crm",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    EstimateId = table.Column<long>(type: "bigint", nullable: false, comment: "Parent estimate."),
                    ClauseId = table.Column<long>(type: "bigint", nullable: true, comment: "Source clause from crm.FgsEstimateClause."),
                    ClauseTypeId = table.Column<long>(type: "bigint", nullable: false, comment: "Snapshot of clause type such as Inclusion, Exclusion, or Terms and Conditions."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls display sequence on estimate documents."),
                    ClauseName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "Snapshot of clause name at the time it was added to the estimate."),
                    ClauseText = table.Column<string>(type: "text", nullable: false, comment: "Snapshot of clause text at the time it was added to the estimate."),
                    ShowOnProposal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the clause should be displayed on customer-facing proposal documents."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsEstimateClauseItem", x => x.Id);
                    table.CheckConstraint("CK_FgsEstimateClauseItem_DisplayOrder", "\"DisplayOrder\" > 0");
                    table.ForeignKey(
                        name: "FK_FgsEstimateClauseItem_Clause",
                        column: x => x.ClauseId,
                        principalSchema: "crm",
                        principalTable: "FgsEstimateClause",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_FgsEstimateClauseItem_Estimate",
                        column: x => x.EstimateId,
                        principalSchema: "crm",
                        principalTable: "FgsEstimate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsEstimateClauseItem_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "crm",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores estimate-specific clause snapshots. Changes to the clause library do not affect existing estimates.");

            migrationBuilder.CreateTable(
                name: "FgsEstimateOption",
                schema: "crm",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    EstimateId = table.Column<long>(type: "bigint", nullable: false, comment: "Parent estimate."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Display order within the estimate."),
                    OptionName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "Customer-facing option name."),
                    OptionDescription = table.Column<string>(type: "text", nullable: true, comment: "Detailed customer-facing option description."),
                    IsRecommended = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the option is highlighted as the recommended option."),
                    IsSelected = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the customer selected this option."),
                    SelectedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the option was selected by the customer."),
                    SubtotalAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Option subtotal amount."),
                    DiscountAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Option discount amount."),
                    TaxAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Option tax amount."),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Option total amount."),
                    InternalNotes = table.Column<string>(type: "text", nullable: true, comment: "Internal notes not visible to customers."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsEstimateOption", x => x.Id);
                    table.CheckConstraint("CK_FgsEstimateOption_DiscountAmount", "\"DiscountAmount\" >= 0");
                    table.CheckConstraint("CK_FgsEstimateOption_DisplayOrder", "\"DisplayOrder\" > 0");
                    table.CheckConstraint("CK_FgsEstimateOption_SubtotalAmount", "\"SubtotalAmount\" >= 0");
                    table.CheckConstraint("CK_FgsEstimateOption_TaxAmount", "\"TaxAmount\" >= 0");
                    table.CheckConstraint("CK_FgsEstimateOption_TotalAmount", "\"TotalAmount\" >= 0");
                    table.ForeignKey(
                        name: "FK_FgsEstimateOption_Estimate",
                        column: x => x.EstimateId,
                        principalSchema: "crm",
                        principalTable: "FgsEstimate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsEstimateOption_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "crm",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores sellable estimate options/packages belonging to an estimate. Detailed pricing is stored in crm.FgsEstimateOptionLine.");

            migrationBuilder.CreateTable(
                name: "FgsEstimateTemplateOption",
                schema: "crm",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    EstimateTemplateId = table.Column<long>(type: "bigint", nullable: false, comment: "Parent estimate template."),
                    EstimateFlavorId = table.Column<long>(type: "bigint", nullable: false, comment: "Flavor assigned to the option such as Standard, Good, Better, Best, or Add-On."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls display sequence within the template."),
                    OptionName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "Customer-facing option name copied to the estimate option."),
                    OptionDescription = table.Column<string>(type: "text", nullable: true, comment: "Customer-facing option description copied to the estimate option."),
                    ShowOnProposal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the option should be displayed on customer-facing proposals."),
                    ShowPriceOnProposal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether pricing should be displayed on customer-facing proposals."),
                    IsSelectedByDefault = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the option should be selected by default when the template is applied."),
                    AllowQuantityChange = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether quantity may be modified after template application."),
                    AllowPriceChange = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether pricing may be modified after template application."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsEstimateTemplateOption", x => x.Id);
                    table.CheckConstraint("CK_FgsEstimateTemplateOption_DisplayOrder", "\"DisplayOrder\" > 0");
                    table.ForeignKey(
                        name: "FK_FgsEstimateTemplateOption_EstimateFlavor",
                        column: x => x.EstimateFlavorId,
                        principalSchema: "crm",
                        principalTable: "FgsEstimateFlavor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsEstimateTemplateOption_EstimateTemplate",
                        column: x => x.EstimateTemplateId,
                        principalSchema: "crm",
                        principalTable: "FgsEstimateTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsEstimateTemplateOption_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "crm",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores reusable estimate options belonging to an estimate template. Template options are copied into estimate options when a template is applied.");

            migrationBuilder.CreateTable(
                name: "FgsEstimateOptionLine",
                schema: "crm",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    EstimateOptionId = table.Column<long>(type: "bigint", nullable: false, comment: "Parent estimate option."),
                    ParentLineId = table.Column<long>(type: "bigint", nullable: true, comment: "Parent estimate option line used for service breakdowns, discounts, taxes, bundles, rebates, and other hierarchical structures."),
                    TemplateId = table.Column<long>(type: "bigint", nullable: true, comment: "Source estimate template."),
                    TemplateLineId = table.Column<long>(type: "bigint", nullable: true, comment: "Source estimate template option line."),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 1, comment: "Display sequence within the estimate option."),
                    BillingCategoryId = table.Column<long>(type: "bigint", nullable: false, comment: "Billing category such as Material, Labor, Service, Equipment, Discount, Tax, Fee, or Other."),
                    ItemCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Associated item identifier. May represent inventory, non-inventory, service, labor, fee, or miscellaneous items."),
                    RateOfDayId = table.Column<long>(type: "bigint", nullable: true, comment: "Rate of day applied to labor pricing such as Regular, Overtime, Double Time, Weekend, Holiday, or Emergency."),
                    Description = table.Column<string>(type: "text", nullable: false, comment: "Customer-facing description, service description, tax authority name, or other detail text."),
                    ShowOnProposal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the line should be displayed on customer-facing proposal documents."),
                    ShowPriceOnProposal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether price and amount should be displayed on customer-facing proposal documents."),
                    ShowToFieldTechnician = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the line should be visible to field technicians."),
                    Source = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Indicates where the line originated such as Manual, Template, ServiceItem, PricingMatrix, Bundle, Import, Clone, or System."),
                    Quantity = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 1m, comment: "Quantity associated with the line."),
                    UnitCost = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 0m, comment: "Cost per unit."),
                    ExtendedCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Quantity multiplied by UnitCost."),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 0m, comment: "Selling price per unit."),
                    ExtendedPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Quantity multiplied by UnitPrice."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsEstimateOptionLine", x => x.Id);
                    table.CheckConstraint("CK_FgsEstimateOptionLine_Quantity", "\"Quantity\" >= 0");
                    table.CheckConstraint("CK_FgsEstimateOptionLine_UnitCost", "\"UnitCost\" >= 0");
                    table.CheckConstraint("CK_FgsEstimateOptionLine_UnitPrice", "\"UnitPrice\" >= 0");
                    table.ForeignKey(
                        name: "FK_FgsEstimateOptionLine_EstimateOption",
                        column: x => x.EstimateOptionId,
                        principalSchema: "crm",
                        principalTable: "FgsEstimateOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsEstimateOptionLine_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "crm",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsEstimateOptionLine_ParentLine",
                        column: x => x.ParentLineId,
                        principalSchema: "crm",
                        principalTable: "FgsEstimateOptionLine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores detailed pricing lines belonging to an estimate option. Supports materials, labor, services, discounts, taxes, fees, and hierarchical pricing structures.");

            migrationBuilder.CreateTable(
                name: "FgsEstimateTemplateOptionLine",
                schema: "crm",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    EstimateTemplateOptionId = table.Column<long>(type: "bigint", nullable: false, comment: "Parent estimate template option."),
                    ParentLineId = table.Column<long>(type: "bigint", nullable: true, comment: "Parent template option line used for service breakdowns, bundles, discounts, rebates, credits, and other hierarchical pricing structures."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Display sequence within the template option."),
                    BillingCategoryId = table.Column<long>(type: "bigint", nullable: false, comment: "Billing category such as Material, Labor, Service, Equipment, Discount, Tax, or Other."),
                    ItemId = table.Column<long>(type: "bigint", nullable: true, comment: "Item associated with the line."),
                    RateOfDayId = table.Column<long>(type: "bigint", nullable: true, comment: "Rate of day applied to labor pricing such as Regular, Overtime, Double Time, Weekend, Holiday, or Emergency."),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, comment: "Customer-facing description or tax authority name."),
                    ShowOnProposal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the line should be displayed on customer-facing proposals."),
                    ShowPriceOnProposal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether pricing amounts should be displayed on customer-facing proposals."),
                    AllowQuantityChange = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether quantity may be modified after template application."),
                    AllowPriceChange = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether pricing may be modified after template application."),
                    Source = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Identifies where the line originated such as Manual, ServiceItem, PricingMatrix, Bundle, Import, or Clone."),
                    Quantity = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 1m, comment: "Default quantity applied when template is used."),
                    UnitCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Default cost per unit."),
                    ExtendedCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Quantity multiplied by UnitCost."),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Default selling price per unit."),
                    ExtendedPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Quantity multiplied by UnitPrice."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsEstimateTemplateOptionLine", x => x.Id);
                    table.CheckConstraint("CK_FgsEstimateTemplateOptionLine_DisplayOrder", "\"DisplayOrder\" > 0");
                    table.CheckConstraint("CK_FgsEstimateTemplateOptionLine_Quantity", "\"Quantity\" >= 0");
                    table.CheckConstraint("CK_FgsEstimateTemplateOptionLine_UnitCost", "\"UnitCost\" >= 0");
                    table.ForeignKey(
                        name: "FK_FgsEstimateTemplateOptionLine_EstimateTemplateOption",
                        column: x => x.EstimateTemplateOptionId,
                        principalSchema: "crm",
                        principalTable: "FgsEstimateTemplateOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsEstimateTemplateOptionLine_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "crm",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsEstimateTemplateOptionLine_ParentLine",
                        column: x => x.ParentLineId,
                        principalSchema: "crm",
                        principalTable: "FgsEstimateTemplateOptionLine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores detailed pricing lines belonging to an estimate template option and are copied into estimate option lines when a template is applied.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimate_EstimateStatusId",
                schema: "crm",
                table: "FgsEstimate",
                column: "EstimateStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimate_TenantId_CompanyId",
                schema: "crm",
                table: "FgsEstimate",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimate_TenantId_CompanyId_CustomerId",
                schema: "crm",
                table: "FgsEstimate",
                columns: new[] { "TenantId", "CompanyId", "CustomerId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimate_TenantId_CompanyId_EstimateStatusId",
                schema: "crm",
                table: "FgsEstimate",
                columns: new[] { "TenantId", "CompanyId", "EstimateStatusId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimate_TenantId_CompanyId_ServiceLocationId",
                schema: "crm",
                table: "FgsEstimate",
                columns: new[] { "TenantId", "CompanyId", "ServiceLocationId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsEstimate_TenantId_CompanyId_EstimateNumber",
                schema: "crm",
                table: "FgsEstimate",
                columns: new[] { "TenantId", "CompanyId", "EstimateNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_FgsEstimate_TenantId_CompanyId_OpportunityId",
                schema: "crm",
                table: "FgsEstimate",
                columns: new[] { "TenantId", "CompanyId", "OpportunityId" },
                unique: true,
                filter: "\"OpportunityId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UX_FgsEstimate_TenantId_CompanyId_WorkOrderId",
                schema: "crm",
                table: "FgsEstimate",
                columns: new[] { "TenantId", "CompanyId", "WorkOrderId" },
                unique: true,
                filter: "\"WorkOrderId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateClause_TenantId_CompanyId",
                schema: "crm",
                table: "FgsEstimateClause",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateClause_TenantId_CompanyId_ClauseTypeId",
                schema: "crm",
                table: "FgsEstimateClause",
                columns: new[] { "TenantId", "CompanyId", "ClauseTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateClause_TenantId_CompanyId_DisplayOrder",
                schema: "crm",
                table: "FgsEstimateClause",
                columns: new[] { "TenantId", "CompanyId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsEstimateClause_TenantId_CompanyId_ClauseTypeId_ClauseName",
                schema: "crm",
                table: "FgsEstimateClause",
                columns: new[] { "TenantId", "CompanyId", "ClauseTypeId", "ClauseName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateClauseItem_ClauseId",
                schema: "crm",
                table: "FgsEstimateClauseItem",
                column: "ClauseId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateClauseItem_EstimateId",
                schema: "crm",
                table: "FgsEstimateClauseItem",
                column: "EstimateId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateClauseItem_TenantId_CompanyId",
                schema: "crm",
                table: "FgsEstimateClauseItem",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateClauseItem_TenantId_CompanyId_ClauseTypeId",
                schema: "crm",
                table: "FgsEstimateClauseItem",
                columns: new[] { "TenantId", "CompanyId", "ClauseTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateClauseItem_TenantId_CompanyId_DisplayOrder",
                schema: "crm",
                table: "FgsEstimateClauseItem",
                columns: new[] { "TenantId", "CompanyId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateClauseItem_TenantId_CompanyId_EstimateId",
                schema: "crm",
                table: "FgsEstimateClauseItem",
                columns: new[] { "TenantId", "CompanyId", "EstimateId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsEstimateClauseItem_TenantId_CompanyId_EstimateId_DisplayOrder",
                schema: "crm",
                table: "FgsEstimateClauseItem",
                columns: new[] { "TenantId", "CompanyId", "EstimateId", "DisplayOrder" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateFlavor_TenantId_CompanyId",
                schema: "crm",
                table: "FgsEstimateFlavor",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsEstimateFlavor_TenantId_CompanyId_FlavorCode",
                schema: "crm",
                table: "FgsEstimateFlavor",
                columns: new[] { "TenantId", "CompanyId", "FlavorCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateOption_EstimateId",
                schema: "crm",
                table: "FgsEstimateOption",
                column: "EstimateId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateOption_TenantId_CompanyId",
                schema: "crm",
                table: "FgsEstimateOption",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateOption_TenantId_CompanyId_EstimateId",
                schema: "crm",
                table: "FgsEstimateOption",
                columns: new[] { "TenantId", "CompanyId", "EstimateId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateOptionLine_EstimateOptionId",
                schema: "crm",
                table: "FgsEstimateOptionLine",
                column: "EstimateOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateOptionLine_ParentLineId",
                schema: "crm",
                table: "FgsEstimateOptionLine",
                column: "ParentLineId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateOptionLine_TenantId_CompanyId",
                schema: "crm",
                table: "FgsEstimateOptionLine",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateOptionLine_TenantId_CompanyId_DisplayOrder",
                schema: "crm",
                table: "FgsEstimateOptionLine",
                columns: new[] { "TenantId", "CompanyId", "EstimateOptionId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateOptionLine_TenantId_CompanyId_EstimateOptionId",
                schema: "crm",
                table: "FgsEstimateOptionLine",
                columns: new[] { "TenantId", "CompanyId", "EstimateOptionId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateOptionLine_TenantId_CompanyId_ParentLineId",
                schema: "crm",
                table: "FgsEstimateOptionLine",
                columns: new[] { "TenantId", "CompanyId", "ParentLineId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateStatus_TenantId_CompanyId",
                schema: "crm",
                table: "FgsEstimateStatus",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsEstimateStatus_TenantId_CompanyId_Name",
                schema: "crm",
                table: "FgsEstimateStatus",
                columns: new[] { "TenantId", "CompanyId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_FgsEstimateStatus_TenantId_CompanyId_StatusCode",
                schema: "crm",
                table: "FgsEstimateStatus",
                columns: new[] { "TenantId", "CompanyId", "StatusCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateTemplate_CategoryId",
                schema: "crm",
                table: "FgsEstimateTemplate",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateTemplate_TenantId_CompanyId",
                schema: "crm",
                table: "FgsEstimateTemplate",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateTemplate_TenantId_CompanyId_CategoryId",
                schema: "crm",
                table: "FgsEstimateTemplate",
                columns: new[] { "TenantId", "CompanyId", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsEstimateTemplate_TenantId_CompanyId_CategoryId_Name",
                schema: "crm",
                table: "FgsEstimateTemplate",
                columns: new[] { "TenantId", "CompanyId", "CategoryId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_FgsEstimateTemplate_TenantId_CompanyId_TemplateCode",
                schema: "crm",
                table: "FgsEstimateTemplate",
                columns: new[] { "TenantId", "CompanyId", "TemplateCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateTemplateCategory_TenantId_CompanyId",
                schema: "crm",
                table: "FgsEstimateTemplateCategory",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsEstimateTemplateCategory_TenantId_CompanyId_CategoryCode",
                schema: "crm",
                table: "FgsEstimateTemplateCategory",
                columns: new[] { "TenantId", "CompanyId", "CategoryCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_FgsEstimateTemplateCategory_TenantId_CompanyId_Name",
                schema: "crm",
                table: "FgsEstimateTemplateCategory",
                columns: new[] { "TenantId", "CompanyId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateTemplateOption_EstimateFlavorId",
                schema: "crm",
                table: "FgsEstimateTemplateOption",
                column: "EstimateFlavorId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateTemplateOption_EstimateTemplateId",
                schema: "crm",
                table: "FgsEstimateTemplateOption",
                column: "EstimateTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateTemplateOption_TenantId_CompanyId",
                schema: "crm",
                table: "FgsEstimateTemplateOption",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateTemplateOption_TenantId_CompanyId_EstimateFlavorId",
                schema: "crm",
                table: "FgsEstimateTemplateOption",
                columns: new[] { "TenantId", "CompanyId", "EstimateFlavorId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateTemplateOption_TenantId_CompanyId_EstimateTemplateId",
                schema: "crm",
                table: "FgsEstimateTemplateOption",
                columns: new[] { "TenantId", "CompanyId", "EstimateTemplateId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsEstimateTemplateOption_TenantId_CompanyId_EstimateTemplateId_DisplayOrder",
                schema: "crm",
                table: "FgsEstimateTemplateOption",
                columns: new[] { "TenantId", "CompanyId", "EstimateTemplateId", "DisplayOrder" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateTemplateOptionLine_EstimateTemplateOptionId",
                schema: "crm",
                table: "FgsEstimateTemplateOptionLine",
                column: "EstimateTemplateOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateTemplateOptionLine_ParentLineId",
                schema: "crm",
                table: "FgsEstimateTemplateOptionLine",
                column: "ParentLineId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateTemplateOptionLine_TenantId_CompanyId",
                schema: "crm",
                table: "FgsEstimateTemplateOptionLine",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateTemplateOptionLine_TenantId_CompanyId_EstimateTemplateOptionId",
                schema: "crm",
                table: "FgsEstimateTemplateOptionLine",
                columns: new[] { "TenantId", "CompanyId", "EstimateTemplateOptionId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEstimateTemplateOptionLine_TenantId_CompanyId_ParentLineId",
                schema: "crm",
                table: "FgsEstimateTemplateOptionLine",
                columns: new[] { "TenantId", "CompanyId", "ParentLineId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsEstimateClauseItem",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "FgsEstimateOptionLine",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "FgsEstimateTemplateOptionLine",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "FgsEstimateClause",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "FgsEstimateOption",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "FgsEstimateTemplateOption",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "FgsEstimate",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "FgsEstimateFlavor",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "FgsEstimateTemplate",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "FgsEstimateStatus",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "FgsEstimateTemplateCategory",
                schema: "crm");
        }
    }
}
