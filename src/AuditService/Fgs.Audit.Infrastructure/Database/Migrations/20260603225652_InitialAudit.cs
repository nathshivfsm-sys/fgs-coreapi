using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Audit.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "audit");

            migrationBuilder.CreateTable(
                name: "FgsCredentialAudit",
                schema: "audit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CredentialId = table.Column<Guid>(type: "uuid", nullable: false),
                    ActionType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Remarks = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    OldVersionNo = table.Column<int>(type: "integer", nullable: true),
                    NewVersionNo = table.Column<int>(type: "integer", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsCredentialAudit", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialAudit_CredentialId",
                schema: "audit",
                table: "FgsCredentialAudit",
                column: "CredentialId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialAudit_Tenant_Company",
                schema: "audit",
                table: "FgsCredentialAudit",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialAudit_Tenant_Company_Cred",
                schema: "audit",
                table: "FgsCredentialAudit",
                columns: new[] { "TenantId", "CompanyId", "CredentialId" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsCredentialAudit",
                schema: "audit",
                table: "FgsCredentialAudit",
                columns: new[] { "TenantId", "CompanyId", "CredentialId", "ActionType", "NewVersionNo" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsCredentialAudit",
                schema: "audit");
        }
    }
}
