using Fgs.Notification.Domain.Enums;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Notification.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class ReshapeEmailHistoryAddSmsHistoryDropNotificationHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsNotificationHistory",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "FgsEmailHistory",
                schema: "notification");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:notification.notification_status", "Queued,Sending,Sent,Delivered,Opened,Clicked,Failed,Undelivered,Bounced,Cancelled")
                .Annotation("Npgsql:Enum:notification.source_application", "FgsWeb,FgsMobile,CustomerPortal,TechnicianPortal,WorkflowEngine,Scheduler,CreditCard,CreditCardWidget,Api,FgsAddon");

            migrationBuilder.CreateTable(
                name: "FgsEmailHistory",
                schema: "notification",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    RecordType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Business record associated with the email such as Estimate, Invoice, WorkOrder, Opportunity, Customer, or User."),
                    RecordId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the associated business record."),
                    EmailTemplateId = table.Column<long>(type: "bigint", nullable: true, comment: "Email template used to generate the email."),
                    Status = table.Column<NotificationStatus>(type: "notification.notification_status", nullable: false, defaultValue: NotificationStatus.Queued, comment: "Current notification status."),
                    SourceApplication = table.Column<NotificationSourceApplication>(type: "notification.source_application", nullable: false, comment: "Application or component that originated the email."),
                    Subject = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, comment: "Email subject line."),
                    FromEmailAddress = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, comment: "Sender email address."),
                    FromDisplayName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Sender display name."),
                    ToEmailAddresses = table.Column<string>(type: "jsonb", nullable: false, comment: "JSON array containing recipient email addresses."),
                    CcEmailAddresses = table.Column<string>(type: "jsonb", nullable: true, comment: "JSON array containing carbon copy recipient email addresses."),
                    BccEmailAddresses = table.Column<string>(type: "jsonb", nullable: true, comment: "JSON array containing blind carbon copy recipient email addresses."),
                    Body = table.Column<string>(type: "text", nullable: false, comment: "Final email body that was sent to the recipient."),
                    ProviderName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Email provider used to send the message."),
                    ProviderMessageId = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Provider-specific message identifier used for troubleshooting and webhook tracking."),
                    SentOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the email was sent."),
                    DeliveredOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the email was confirmed as delivered by the provider."),
                    OpenedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the email was opened."),
                    FailedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the email failed to send or deliver."),
                    FailureReason = table.Column<string>(type: "text", nullable: true, comment: "Failure reason returned by the email provider when send fails."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true, comment: "User or process that created the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsEmailHistory", x => x.Id);
                },
                comment: "Stores outbound email history for business entities and provides a permanent audit trail of email communications.");

            migrationBuilder.CreateTable(
                name: "FgsSmsHistory",
                schema: "notification",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    RecordType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Business record associated with the SMS such as Estimate, Invoice, WorkOrder, Opportunity, Customer, or User."),
                    RecordId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifier of the associated business record."),
                    TemplateId = table.Column<long>(type: "bigint", nullable: true, comment: "SMS template used to generate the message."),
                    Status = table.Column<NotificationStatus>(type: "notification.notification_status", nullable: false, defaultValue: NotificationStatus.Queued, comment: "Current notification status."),
                    SourceApplication = table.Column<NotificationSourceApplication>(type: "notification.source_application", nullable: false, comment: "Application or component that originated the SMS."),
                    FromPhoneNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, comment: "Phone number or short code used to send the SMS."),
                    ToPhoneNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, comment: "Recipient mobile phone number."),
                    Message = table.Column<string>(type: "text", nullable: false, comment: "Final SMS message that was sent to the recipient."),
                    ProviderName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "SMS provider used to send the message."),
                    ProviderMessageId = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Provider-specific message identifier used for troubleshooting and webhook tracking."),
                    SegmentCount = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Number of SMS segments billed by the provider."),
                    SentOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the SMS was sent."),
                    DeliveredOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the SMS was confirmed as delivered by the provider."),
                    FailedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the SMS failed to send or deliver."),
                    FailureReason = table.Column<string>(type: "text", nullable: true, comment: "Failure reason returned by the SMS provider."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true, comment: "User or process that created the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSmsHistory", x => x.Id);
                },
                comment: "Stores outbound SMS history for business entities and provides a permanent audit trail of SMS communications.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEmailHistory_ProviderMessageId",
                schema: "notification",
                table: "FgsEmailHistory",
                column: "ProviderMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEmailHistory_Record",
                schema: "notification",
                table: "FgsEmailHistory",
                columns: new[] { "TenantId", "CompanyId", "RecordType", "RecordId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEmailHistory_SentOn",
                schema: "notification",
                table: "FgsEmailHistory",
                columns: new[] { "TenantId", "CompanyId", "SentOn" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEmailHistory_Status",
                schema: "notification",
                table: "FgsEmailHistory",
                columns: new[] { "TenantId", "CompanyId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEmailHistory_TenantCompany",
                schema: "notification",
                table: "FgsEmailHistory",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSmsHistory_ProviderMessageId",
                schema: "notification",
                table: "FgsSmsHistory",
                column: "ProviderMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSmsHistory_Record",
                schema: "notification",
                table: "FgsSmsHistory",
                columns: new[] { "TenantId", "CompanyId", "RecordType", "RecordId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSmsHistory_SentOn",
                schema: "notification",
                table: "FgsSmsHistory",
                columns: new[] { "TenantId", "CompanyId", "SentOn" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSmsHistory_Status",
                schema: "notification",
                table: "FgsSmsHistory",
                columns: new[] { "TenantId", "CompanyId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSmsHistory_TenantCompany",
                schema: "notification",
                table: "FgsSmsHistory",
                columns: new[] { "TenantId", "CompanyId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsSmsHistory",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "FgsEmailHistory",
                schema: "notification");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:notification.notification_status", "Queued,Sending,Sent,Delivered,Opened,Clicked,Failed,Undelivered,Bounced,Cancelled")
                .OldAnnotation("Npgsql:Enum:notification.source_application", "FgsWeb,FgsMobile,CustomerPortal,TechnicianPortal,WorkflowEngine,Scheduler,CreditCard,CreditCardWidget,Api,FgsAddon");

            migrationBuilder.CreateTable(
                name: "FgsEmailHistory",
                schema: "notification",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    EntityType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EntityId = table.Column<long>(type: "bigint", nullable: false),
                    EmailTemplateId = table.Column<long>(type: "bigint", nullable: true),
                    Subject = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FromEmailAddress = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FromDisplayName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ToEmailAddresses = table.Column<string>(type: "jsonb", nullable: false),
                    CcEmailAddresses = table.Column<string>(type: "jsonb", nullable: true),
                    BccEmailAddresses = table.Column<string>(type: "jsonb", nullable: true),
                    BodyHtml = table.Column<string>(type: "text", nullable: true),
                    BodyText = table.Column<string>(type: "text", nullable: true),
                    HasAttachments = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SentOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    FailureReason = table.Column<string>(type: "text", nullable: true),
                    ProviderMessageId = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsEmailHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsNotificationHistory",
                schema: "notification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Channel = table.Column<int>(type: "integer", nullable: false),
                    CorrelationId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    Error = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ProviderMessageId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Recipient = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    SentOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TemplateName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    TenantId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsNotificationHistory", x => x.Id);
                });

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
                name: "IX_FgsEmailHistory_TenantId_CompanyId_Status",
                schema: "notification",
                table: "FgsEmailHistory",
                columns: new[] { "TenantId", "CompanyId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEmailHistory_TenantId_CompanyId_SentOn",
                schema: "notification",
                table: "FgsEmailHistory",
                columns: new[] { "TenantId", "CompanyId", "SentOn" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsNotificationHistory_TenantId_CreatedOn",
                schema: "notification",
                table: "FgsNotificationHistory",
                columns: new[] { "TenantId", "CreatedOn" });
        }
    }
}
