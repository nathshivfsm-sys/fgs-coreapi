using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceAgreementEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsSetupServiceAgreementPricingComponent",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    PricingComponentCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PricingComponentTypeCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "FIXED, FIRST_SYSTEM, SECOND_SYSTEM, THIRD_SYSTEM, FOURTH_SYSTEM, FIFTH_SYSTEM, or ADDITIONAL_SYSTEM."),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupServiceAgreementPricingComponent", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupServiceAgreementPricingComponent_TenantId_CompanyId_PricingComponentCode", x => new { x.TenantId, x.CompanyId, x.PricingComponentCode });
                    table.CheckConstraint("CK_FgsSetupServiceAgreementPricingComponent_Amount", "\"Amount\" >= 0");
                    table.CheckConstraint("CK_FgsSetupServiceAgreementPricingComponent_TypeCode", "\"PricingComponentTypeCode\" IN ('FIXED','FIRST_SYSTEM','SECOND_SYSTEM','THIRD_SYSTEM','FOURTH_SYSTEM','FIFTH_SYSTEM','ADDITIONAL_SYSTEM')");
                    table.ForeignKey(
                        name: "FK_FgsSetupServiceAgreementPricingComponent_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores reusable service agreement pricing components and their default pricing for a tenant-company. Component types: FIXED (applied once), FIRST_SYSTEM through FIFTH_SYSTEM (per-system tier), ADDITIONAL_SYSTEM (sixth and subsequent systems).");

            migrationBuilder.CreateTable(
                name: "FgsSetupServiceAgreementTemplate",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    TemplateCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    BillingFrequencyMonths = table.Column<short>(type: "smallint", nullable: false, comment: "Billing frequency in months. 1 = Monthly, 3 = Quarterly, 6 = Semi-Annual, 12 = Annual."),
                    MaintenanceFrequencyMonths = table.Column<short>(type: "smallint", nullable: false, comment: "Maintenance frequency in months. 1 = Monthly, 3 = Quarterly, 6 = Semi-Annual, 12 = Annual."),
                    RepairDiscountPercent = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false, defaultValue: 0m, comment: "Discount given to service agreement customers on additional repairs."),
                    IsAutoRenew = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupServiceAgreementTemplate", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupServiceAgreementTemplate_TenantId_CompanyId_TemplateCode", x => new { x.TenantId, x.CompanyId, x.TemplateCode });
                    table.CheckConstraint("CK_FgsSetupServiceAgreementTemplate_BillingFrequencyMonths", "\"BillingFrequencyMonths\" IN (1,3,6,12)");
                    table.CheckConstraint("CK_FgsSetupServiceAgreementTemplate_MaintenanceFrequencyMonths", "\"MaintenanceFrequencyMonths\" IN (1,3,6,12)");
                    table.CheckConstraint("CK_FgsSetupServiceAgreementTemplate_RepairDiscountPercent", "\"RepairDiscountPercent\" >= 0 AND \"RepairDiscountPercent\" <= 100");
                    table.ForeignKey(
                        name: "FK_FgsSetupServiceAgreementTemplate_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores service agreement templates defining billing frequency, maintenance frequency, repair discounts, and default terms for a tenant-company.");

            migrationBuilder.CreateTable(
                name: "FgsSetupServiceAgreementTemplateCoverage",
                schema: "setup",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceAgreementTemplateId = table.Column<long>(type: "bigint", nullable: false, comment: "Service agreement template that this coverage item belongs to."),
                    CoverageTypeCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, comment: "INCLUDE or EXCLUDE."),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupServiceAgreementTemplateCoverage", x => x.Id);
                    table.CheckConstraint("CK_FgsSetupServiceAgreementTemplateCoverage_CoverageTypeCode", "\"CoverageTypeCode\" IN ('INCLUDE','EXCLUDE')");
                    table.ForeignKey(
                        name: "FK_FgsSetupServiceAgreementTemplateCoverage_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsSetupServiceAgreementTemplateCoverage_ServiceAgreementTemplateId",
                        column: x => x.ServiceAgreementTemplateId,
                        principalSchema: "setup",
                        principalTable: "FgsSetupServiceAgreementTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores included or excluded coverage items for a service agreement template.");

            migrationBuilder.CreateTable(
                name: "FgsSetupServiceAgreementTemplatePricingComponent",
                schema: "setup",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceAgreementTemplateId = table.Column<long>(type: "bigint", nullable: false, comment: "Service agreement template that includes this pricing component snapshot."),
                    PricingComponentCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupServiceAgreementTemplatePricingComponent", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupServiceAgreementTemplatePricingComponent_TenantId_CompanyId_TemplateId_ComponentCode", x => new { x.TenantId, x.CompanyId, x.ServiceAgreementTemplateId, x.PricingComponentCode });
                    table.CheckConstraint("CK_FgsSetupServiceAgreementTemplatePricingComponent_Amount", "\"Amount\" >= 0");
                    table.ForeignKey(
                        name: "FK_FgsSetupServiceAgreementTemplatePricingComponent_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "setup",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsSetupServiceAgreementTemplatePricingComponent_ServiceAgreementTemplateId",
                        column: x => x.ServiceAgreementTemplateId,
                        principalSchema: "setup",
                        principalTable: "FgsSetupServiceAgreementTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores pricing components included in a service agreement template. Rows are a snapshot copied from FgsSetupServiceAgreementPricingComponent when the template is configured and become independent of the master pricing component.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupServiceAgreementTemplate_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupServiceAgreementTemplate",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupServiceAgreementTemplate_TenantId_CompanyId_Name",
                schema: "setup",
                table: "FgsSetupServiceAgreementTemplate",
                columns: new[] { "TenantId", "CompanyId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupServiceAgreementTemplateCoverage_ServiceAgreementTe~",
                schema: "setup",
                table: "FgsSetupServiceAgreementTemplateCoverage",
                column: "ServiceAgreementTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupServiceAgreementTemplateCoverage_TenantId_CompanyId_TemplateId",
                schema: "setup",
                table: "FgsSetupServiceAgreementTemplateCoverage",
                columns: new[] { "TenantId", "CompanyId", "ServiceAgreementTemplateId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupServiceAgreementTemplatePricingComponent_ServiceAgr~",
                schema: "setup",
                table: "FgsSetupServiceAgreementTemplatePricingComponent",
                column: "ServiceAgreementTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupServiceAgreementTemplatePricingComponent_TenantId_CompanyId_TemplateId",
                schema: "setup",
                table: "FgsSetupServiceAgreementTemplatePricingComponent",
                columns: new[] { "TenantId", "CompanyId", "ServiceAgreementTemplateId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsSetupServiceAgreementPricingComponent",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSetupServiceAgreementTemplateCoverage",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSetupServiceAgreementTemplatePricingComponent",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSetupServiceAgreementTemplate",
                schema: "setup");
        }
    }
}
