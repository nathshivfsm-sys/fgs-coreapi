using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Notification.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddFgsEmailHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsEmailHistory",
                schema: "notification",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    EntityType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Entity associated with the email such as Estimate, Invoice, WorkOrder, Opportunity, or Customer."),
                    EntityId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the associated business entity."),
                    EmailTemplateId = table.Column<long>(type: "bigint", nullable: true, comment: "Email template used to generate the email."),
                    Subject = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, comment: "Email subject line."),
                    FromEmailAddress = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, comment: "Sender email address."),
                    FromDisplayName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Sender display name."),
                    ToEmailAddresses = table.Column<string>(type: "jsonb", nullable: false, comment: "JSON array containing recipient email addresses."),
                    CcEmailAddresses = table.Column<string>(type: "jsonb", nullable: true, comment: "JSON array containing carbon copy recipient email addresses."),
                    BccEmailAddresses = table.Column<string>(type: "jsonb", nullable: true, comment: "JSON array containing blind carbon copy recipient email addresses."),
                    BodyHtml = table.Column<string>(type: "text", nullable: true, comment: "Email body in HTML format."),
                    BodyText = table.Column<string>(type: "text", nullable: true, comment: "Email body in plain text format."),
                    HasAttachments = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether one or more attachments were included in the email."),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Email delivery status such as Queued, Sent, Delivered, Opened, Failed, or Bounced."),
                    SentOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the email was sent."),
                    FailureReason = table.Column<string>(type: "text", nullable: true, comment: "Failure reason returned by the email provider when send fails."),
                    ProviderMessageId = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Provider-specific message identifier used for troubleshooting and webhook tracking."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "text", nullable: true, comment: "User or process that created the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsEmailHistory", x => x.Id);
                },
                comment: "Stores outbound email history for business entities and provides a permanent audit trail of email communications.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEmailHistory_ProviderMessageId",
                schema: "notification",
                table: "FgsEmailHistory",
                column: "ProviderMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEmailHistory_TenantId_CompanyId",
                schema: "notification",
                table: "FgsEmailHistory",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEmailHistory_TenantId_CompanyId_EntityType_EntityId",
                schema: "notification",
                table: "FgsEmailHistory",
                columns: new[] { "TenantId", "CompanyId", "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEmailHistory_TenantId_CompanyId_SentOn",
                schema: "notification",
                table: "FgsEmailHistory",
                columns: new[] { "TenantId", "CompanyId", "SentOn" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEmailHistory_TenantId_CompanyId_Status",
                schema: "notification",
                table: "FgsEmailHistory",
                columns: new[] { "TenantId", "CompanyId", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsEmailHistory",
                schema: "notification");
        }
    }
}
