using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.ServiceAgreement.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "svc");

            migrationBuilder.Sql(
                """
                COMMENT ON SCHEMA svc IS $comment$Service Agreement Domain

                Stores recurring maintenance agreements, membership plans, preventive maintenance contracts and service contracts.

                Responsible for:
                - Service Agreements
                - Covered Assets
                - Visit Scheduling
                - Billing Scheduling
                - Renewals

                Typical lifecycle:

                Lead
                -> Opportunity
                -> Service Agreement
                -> Scheduled Visits
                -> Work Orders
                -> Billing
                -> Renewal

                CRM owns the sales process.
                SVC owns the contract lifecycle after the sale$comment$;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("COMMENT ON SCHEMA svc IS NULL;");
        }
    }
}
