using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceFgsOutboxMessageWithGloOutboxMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsOutboxMessage",
                schema: "dbo");

            migrationBuilder.CreateTable(
                name: "GloOutboxMessage",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyId = table.Column<long>(type: "bigint", nullable: true),
                    EventType = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    AggregateType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AggregateId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CausationId = table.Column<Guid>(type: "uuid", nullable: true),
                    ExchangeName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    RoutingKey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Payload = table.Column<string>(type: "jsonb", nullable: false),
                    Headers = table.Column<string>(type: "jsonb", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    RetryCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    MaxRetryCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 10),
                    NextRetryOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    ProcessedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    LastError = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloOutboxMessage", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GloOutboxMessage_CorrelationId",
                schema: "dbo",
                table: "GloOutboxMessage",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_GloOutboxMessage_EventType",
                schema: "dbo",
                table: "GloOutboxMessage",
                column: "EventType");

            migrationBuilder.CreateIndex(
                name: "IX_GloOutboxMessage_Status_NextRetryOn",
                schema: "dbo",
                table: "GloOutboxMessage",
                columns: new[] { "Status", "NextRetryOn" });

            migrationBuilder.CreateIndex(
                name: "IX_GloOutboxMessage_TenantId",
                schema: "dbo",
                table: "GloOutboxMessage",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GloOutboxMessage",
                schema: "dbo");

            migrationBuilder.CreateTable(
                name: "FgsOutboxMessage",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CorrelationId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    EventType = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IdempotencyKey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastError = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Payload = table.Column<string>(type: "jsonb", nullable: false),
                    ProcessedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsOutboxMessage", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FgsOutboxMessage_IdempotencyKey",
                schema: "dbo",
                table: "FgsOutboxMessage",
                column: "IdempotencyKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsOutboxMessage_Status_CreatedOn",
                schema: "dbo",
                table: "FgsOutboxMessage",
                columns: new[] { "Status", "CreatedOn" });
        }
    }
}
