using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UserService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyConfiguration",
                schema: "fgs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    TimeCardOptionId = table.Column<int>(type: "integer", nullable: false),
                    AccountingIntegrationTypeId = table.Column<int>(type: "integer", nullable: true),
                    EnableCallBookingWidget = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    EnablePaymentWidget = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    EnableCustomerPortal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    EnableRulesManagement = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    EnableAutoArrive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    WorkLocationRadiusForAutoArrive = table.Column<int>(type: "integer", nullable: true),
                    OTStartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    OTEndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    DTStartTime = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    DTEndTime = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    BillHoursFromDispatchOrArrive = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "DISPATCH"),
                    SourceCodeRequiredOnWorkOrder = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    SourceCodeRequiredOnServiceLocation = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    BillToStartNumber = table.Column<long>(type: "bigint", nullable: false, defaultValue: 100L),
                    POStartNumber = table.Column<long>(type: "bigint", nullable: false, defaultValue: 100L),
                    QuoteStartNumber = table.Column<long>(type: "bigint", nullable: false, defaultValue: 100L),
                    WorkOrderStartNumber = table.Column<long>(type: "bigint", nullable: false, defaultValue: 100L),
                    InvoiceNumberPrefix = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    QuoteNumberPrefix = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PONumberPrefix = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    WorkOrderNumberPrefix = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    InvoiceBatchNumberFormat = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyConfiguration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyConfiguration_AccountingIntegrationType",
                        column: x => x.AccountingIntegrationTypeId,
                        principalSchema: "fgs",
                        principalTable: "FSGSetupAccountingIntegrationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CompanyConfiguration_Company",
                        column: x => x.CompanyId,
                        principalSchema: "fgs",
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyConfiguration_TimeCardOption",
                        column: x => x.TimeCardOptionId,
                        principalSchema: "fgs",
                        principalTable: "FSGSetupTimeCardOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IxCompanyConfigurationAccountingIntegrationTypeId",
                schema: "fgs",
                table: "CompanyConfiguration",
                column: "AccountingIntegrationTypeId");

            migrationBuilder.CreateIndex(
                name: "IxCompanyConfigurationTimeCardOptionId",
                schema: "fgs",
                table: "CompanyConfiguration",
                column: "TimeCardOptionId");

            migrationBuilder.CreateIndex(
                name: "IxCompanyConfigurationCompany",
                schema: "fgs",
                table: "CompanyConfiguration",
                column: "CompanyId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyConfiguration",
                schema: "fgs");
        }
    }
}
