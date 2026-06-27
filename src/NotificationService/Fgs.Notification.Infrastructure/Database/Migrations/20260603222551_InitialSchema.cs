using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Notification.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "notification");

            migrationBuilder.CreateTable(
                name: "FgsNotificationHistory",
                schema: "notification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    Channel = table.Column<int>(type: "integer", nullable: false),
                    TemplateName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Recipient = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CorrelationId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ProviderMessageId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Error = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    SentOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsNotificationHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsProcessedIntegrationEvent",
                schema: "notification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MessageId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    EventType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ProcessedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsProcessedIntegrationEvent", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FgsNotificationHistory_TenantId_CreatedOn",
                schema: "notification",
                table: "FgsNotificationHistory",
                columns: new[] { "TenantId", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsProcessedIntegrationEvent_MessageId",
                schema: "notification",
                table: "FgsProcessedIntegrationEvent",
                column: "MessageId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsNotificationHistory",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "FgsProcessedIntegrationEvent",
                schema: "notification");
        }
    }
}
