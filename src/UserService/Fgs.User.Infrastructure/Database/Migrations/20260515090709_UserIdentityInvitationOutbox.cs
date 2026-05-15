using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class UserIdentityInvitationOutbox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsOutboxMessage",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventType = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Payload = table.Column<string>(type: "jsonb", nullable: false),
                    IdempotencyKey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CorrelationId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    LastError = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    ProcessedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsOutboxMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsUser",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    NormalizedEmail = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EntraObjectId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsUser_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyGuid" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsUser_FgsTenant_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "FgsTenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsInvitation",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    TokenHash = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ExpiresAtUtc = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    AcceptedAtUtc = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInvitation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsInvitation_FgsUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "FgsUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvitation_TenantId_Email_Status",
                schema: "dbo",
                table: "FgsInvitation",
                columns: new[] { "TenantId", "Email", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvitation_TokenHash",
                schema: "dbo",
                table: "FgsInvitation",
                column: "TokenHash");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvitation_UserId",
                schema: "dbo",
                table: "FgsInvitation",
                column: "UserId");

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

            migrationBuilder.CreateIndex(
                name: "IX_FgsUser_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsUser",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsUser_TenantId_NormalizedEmail",
                schema: "dbo",
                table: "FgsUser",
                columns: new[] { "TenantId", "NormalizedEmail" },
                unique: true,
                filter: "\"IsDeleted\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsInvitation",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsOutboxMessage",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsUser",
                schema: "dbo");
        }
    }
}
