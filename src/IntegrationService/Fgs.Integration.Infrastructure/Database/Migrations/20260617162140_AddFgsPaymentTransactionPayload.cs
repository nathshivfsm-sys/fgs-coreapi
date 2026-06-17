using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Integration.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFgsPaymentTransactionPayload : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsPaymentTransactionPayload",
                schema: "integration",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    PaymentTransactionId = table.Column<long>(type: "bigint", nullable: false),
                    RequestJson = table.Column<string>(type: "jsonb", nullable: true),
                    ResponseJson = table.Column<string>(type: "jsonb", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsPaymentTransactionPayload", x => x.Id);
                },
                comment: "Stores optional payment processor request and response payloads for troubleshooting, support, auditing, and integration diagnostics.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsPaymentTransactionPayload_TenantCompany",
                schema: "integration",
                table: "FgsPaymentTransactionPayload",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsPaymentTransactionPayload_PaymentTransaction",
                schema: "integration",
                table: "FgsPaymentTransactionPayload",
                column: "PaymentTransactionId",
                unique: true);

            migrationBuilder.Sql(
                """
                ALTER TABLE integration."FgsPaymentTransactionPayload"
                    ADD CONSTRAINT "FK_FgsPaymentTransactionPayload_TenantCompany"
                    FOREIGN KEY ("TenantId", "CompanyId")
                    REFERENCES billing."FgsTenantCompanyCache" ("TenantId", "CompanyId")
                    ON DELETE RESTRICT;

                ALTER TABLE integration."FgsPaymentTransactionPayload"
                    ADD CONSTRAINT "FK_FgsPaymentTransactionPayload_PaymentTransaction"
                    FOREIGN KEY ("PaymentTransactionId")
                    REFERENCES billing."FgsPaymentTransaction" ("Id")
                    ON DELETE CASCADE;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsPaymentTransactionPayload",
                schema: "integration");
        }
    }
}
