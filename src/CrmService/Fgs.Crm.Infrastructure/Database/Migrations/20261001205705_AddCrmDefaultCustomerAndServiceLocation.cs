using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Crm.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCrmDefaultCustomerAndServiceLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CrmDefaultCustomer",
                schema: "crm",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DefaultPaymentTermId = table.Column<long>(type: "bigint", nullable: true, comment: "Default payment term applied to a new customer."),
                    DefaultMaterialPricingMatrixId = table.Column<long>(type: "bigint", nullable: true, comment: "Default material pricing matrix applied to a new customer."),
                    DefaultLaborPricingMatrixId = table.Column<long>(type: "bigint", nullable: true, comment: "Default labor pricing matrix applied to a new customer."),
                    DefaultOtherPricingMatrixId = table.Column<long>(type: "bigint", nullable: true, comment: "Default miscellaneous pricing matrix applied to a new customer."),
                    DefaultPORequired = table.Column<bool>(type: "boolean", nullable: false, comment: "Indicates whether a purchase order is required by default for a new customer."),
                    TaxExempt = table.Column<bool>(type: "boolean", nullable: false, comment: "Indicates whether a new customer is tax exempt by default."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Record creation timestamp."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User that created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Last update timestamp."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User that last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmDefaultCustomer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrmDefaultCustomer_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "crm",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores default values used when creating new customers.");

            migrationBuilder.CreateTable(
                name: "CrmDefaultServiceLocation",
                schema: "crm",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DefaultLaborPricingMatrixId = table.Column<long>(type: "bigint", nullable: true, comment: "Default labor pricing matrix applied to a new service location."),
                    DefaultMaterialPricingMatrixId = table.Column<long>(type: "bigint", nullable: true, comment: "Default material pricing matrix applied to a new service location."),
                    DefaultOtherPricingMatrixId = table.Column<long>(type: "bigint", nullable: true, comment: "Default miscellaneous pricing matrix applied to a new service location."),
                    DefaultPaymentMethodId = table.Column<long>(type: "bigint", nullable: true, comment: "Default payment method applied to a new service location."),
                    EmailAllowed = table.Column<bool>(type: "boolean", nullable: false, comment: "Indicates whether email communication is enabled by default for a new service location."),
                    EstimateEmailTemplateId = table.Column<long>(type: "bigint", nullable: true, comment: "Default estimate email template applied to a new service location."),
                    EstimateSmsTemplateId = table.Column<long>(type: "bigint", nullable: true, comment: "Default estimate SMS template applied to a new service location."),
                    InvoiceEmailTemplateId = table.Column<long>(type: "bigint", nullable: true, comment: "Default invoice email template applied to a new service location."),
                    InvoiceSmsTemplateId = table.Column<long>(type: "bigint", nullable: true, comment: "Default invoice SMS template applied to a new service location."),
                    SmsAllowed = table.Column<bool>(type: "boolean", nullable: false, comment: "Indicates whether SMS communication is enabled by default for a new service location."),
                    TaxExempt = table.Column<bool>(type: "boolean", nullable: false, comment: "Indicates whether a new service location is tax exempt by default."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Record creation timestamp."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "User that created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Last update timestamp."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User that last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmDefaultServiceLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrmDefaultServiceLocation_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "crm",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores default values used when creating new customer service locations.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrmDefaultCustomer",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "CrmDefaultServiceLocation",
                schema: "crm");
        }
    }
}
