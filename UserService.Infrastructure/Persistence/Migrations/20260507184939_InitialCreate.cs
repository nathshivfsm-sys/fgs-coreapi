using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "fgs");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.CreateTable(
                name: "tenant",
                schema: "fgs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tenant", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tenant_company",
                schema: "fgs",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    company_id = table.Column<short>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tenant_company", x => new { x.tenant_id, x.company_id });
                    table.ForeignKey(
                        name: "FK_tenant_company_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalSchema: "fgs",
                        principalTable: "tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "fgs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "citext", nullable: false),
                    display_name = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    company_id = table.Column<short>(type: "smallint", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_tenant_company_tenant_id_company_id",
                        columns: x => new { x.tenant_id, x.company_id },
                        principalSchema: "fgs",
                        principalTable: "tenant_company",
                        principalColumns: new[] { "tenant_id", "company_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_users_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalSchema: "fgs",
                        principalTable: "tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "auth_identity",
                schema: "fgs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    issuer = table.Column<string>(type: "text", nullable: false),
                    object_id = table.Column<string>(type: "text", nullable: false),
                    subject = table.Column<string>(type: "text", nullable: true),
                    email_snapshot = table.Column<string>(type: "citext", nullable: true),
                    linked_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auth_identity", x => x.id);
                    table.ForeignKey(
                        name: "FK_auth_identity_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "fgs",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invite",
                schema: "fgs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    invited_email = table.Column<string>(type: "citext", nullable: false),
                    company_id = table.Column<short>(type: "smallint", nullable: false),
                    token_hash = table.Column<byte[]>(type: "bytea", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    expires_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    accepted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    revoked_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invite", x => x.id);
                    table.ForeignKey(
                        name: "FK_invite_tenant_company_tenant_id_company_id",
                        columns: x => new { x.tenant_id, x.company_id },
                        principalSchema: "fgs",
                        principalTable: "tenant_company",
                        principalColumns: new[] { "tenant_id", "company_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_invite_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalSchema: "fgs",
                        principalTable: "tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_invite_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "fgs",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_auth_identity_issuer_object_id",
                schema: "fgs",
                table: "auth_identity",
                columns: new[] { "issuer", "object_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_auth_identity_user",
                schema: "fgs",
                table: "auth_identity",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_invite_pending",
                schema: "fgs",
                table: "invite",
                column: "tenant_id",
                filter: "status = 'pending'");

            migrationBuilder.CreateIndex(
                name: "IX_invite_tenant_id_company_id",
                schema: "fgs",
                table: "invite",
                columns: new[] { "tenant_id", "company_id" });

            migrationBuilder.CreateIndex(
                name: "ix_invite_token_hash",
                schema: "fgs",
                table: "invite",
                column: "token_hash");

            migrationBuilder.CreateIndex(
                name: "ix_invite_user",
                schema: "fgs",
                table: "invite",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_tenant_company_tenant",
                schema: "fgs",
                table: "tenant_company",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_tenant",
                schema: "fgs",
                table: "users",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_tenant_id_company_id",
                schema: "fgs",
                table: "users",
                columns: new[] { "tenant_id", "company_id" });

            migrationBuilder.CreateIndex(
                name: "IX_users_tenant_id_email",
                schema: "fgs",
                table: "users",
                columns: new[] { "tenant_id", "email" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "auth_identity",
                schema: "fgs");

            migrationBuilder.DropTable(
                name: "invite",
                schema: "fgs");

            migrationBuilder.DropTable(
                name: "users",
                schema: "fgs");

            migrationBuilder.DropTable(
                name: "tenant_company",
                schema: "fgs");

            migrationBuilder.DropTable(
                name: "tenant",
                schema: "fgs");
        }
    }
}
