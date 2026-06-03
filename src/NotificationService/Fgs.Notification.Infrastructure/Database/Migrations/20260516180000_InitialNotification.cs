using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Notification.Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class InitialPlatform : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(name: "dbo");

        migrationBuilder.CreateTable(
            name: "FgsNotificationHistory",
            schema: "dbo",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                Channel = table.Column<int>(type: "integer", nullable: false),
                TemplateName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                Recipient = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                Status = table.Column<int>(type: "integer", nullable: false),
                CorrelationId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                ProviderMessageId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                Error = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                SentOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FgsNotificationHistory", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "FgsProcessedIntegrationEvent",
            schema: "dbo",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                MessageId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                EventType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                ProcessedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FgsProcessedIntegrationEvent", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_FgsNotificationHistory_TenantId_CreatedOn",
            schema: "dbo",
            table: "FgsNotificationHistory",
            columns: new[] { "TenantId", "CreatedOn" });

        migrationBuilder.CreateIndex(
            name: "IX_FgsProcessedIntegrationEvent_MessageId",
            schema: "dbo",
            table: "FgsProcessedIntegrationEvent",
            column: "MessageId",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "FgsProcessedIntegrationEvent", schema: "dbo");
        migrationBuilder.DropTable(name: "FgsNotificationHistory", schema: "dbo");
    }
}
