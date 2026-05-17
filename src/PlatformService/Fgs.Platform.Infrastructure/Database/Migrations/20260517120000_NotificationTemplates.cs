using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Platform.Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class NotificationTemplates : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "FgsSetupCommunicationTemplate",
            schema: "dbo",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                TemplateType = table.Column<string>(type: "text", nullable: false),
                Code = table.Column<string>(type: "text", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                Subject = table.Column<string>(type: "text", nullable: true),
                Body = table.Column<string>(type: "text", nullable: false),
                IsMobileVisible = table.Column<bool>(type: "boolean", nullable: false),
                IsActive = table.Column<bool>(type: "boolean", nullable: false),
                CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FgsSetupCommunicationTemplate", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_FgsSetupCommunicationTemplate_TenantId_CompanyId",
            schema: "dbo",
            table: "FgsSetupCommunicationTemplate",
            columns: new[] { "TenantId", "CompanyId" });

        migrationBuilder.CreateIndex(
            name: "IX_FgsSetupCommunicationTemplate_TenantId_CompanyId_TemplateType_Code",
            schema: "dbo",
            table: "FgsSetupCommunicationTemplate",
            columns: new[] { "TenantId", "CompanyId", "TemplateType", "Code" },
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "FgsSetupCommunicationTemplate",
            schema: "dbo");
    }
}
