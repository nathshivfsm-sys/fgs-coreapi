using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Platform.Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class ChangeTenantAndCompanyIdsToBigint : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_FgsSetupCommunicationTemplate_TenantId_CompanyId_TemplateType_Code",
            schema: "dbo",
            table: "FgsSetupCommunicationTemplate");

        migrationBuilder.DropIndex(
            name: "IX_FgsSetupCommunicationTemplate_TenantId_CompanyId",
            schema: "dbo",
            table: "FgsSetupCommunicationTemplate");

        migrationBuilder.DropColumn(
            name: "TenantId",
            schema: "dbo",
            table: "FgsSetupCommunicationTemplate");

        migrationBuilder.DropColumn(
            name: "CompanyId",
            schema: "dbo",
            table: "FgsSetupCommunicationTemplate");

        migrationBuilder.AddColumn<long>(
            name: "TenantId",
            schema: "dbo",
            table: "FgsSetupCommunicationTemplate",
            type: "bigint",
            nullable: true);

        migrationBuilder.AddColumn<long>(
            name: "CompanyId",
            schema: "dbo",
            table: "FgsSetupCommunicationTemplate",
            type: "bigint",
            nullable: true);

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

        migrationBuilder.DropIndex(
            name: "IX_FgsNotificationHistory_TenantId_CreatedOn",
            schema: "dbo",
            table: "FgsNotificationHistory");

        migrationBuilder.DropColumn(
            name: "TenantId",
            schema: "dbo",
            table: "FgsNotificationHistory");

        migrationBuilder.AddColumn<long>(
            name: "TenantId",
            schema: "dbo",
            table: "FgsNotificationHistory",
            type: "bigint",
            nullable: false,
            defaultValue: 0L);

        migrationBuilder.CreateIndex(
            name: "IX_FgsNotificationHistory_TenantId_CreatedOn",
            schema: "dbo",
            table: "FgsNotificationHistory",
            columns: new[] { "TenantId", "CreatedOn" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_FgsSetupCommunicationTemplate_TenantId_CompanyId_TemplateType_Code",
            schema: "dbo",
            table: "FgsSetupCommunicationTemplate");

        migrationBuilder.DropIndex(
            name: "IX_FgsSetupCommunicationTemplate_TenantId_CompanyId",
            schema: "dbo",
            table: "FgsSetupCommunicationTemplate");

        migrationBuilder.DropColumn(
            name: "TenantId",
            schema: "dbo",
            table: "FgsSetupCommunicationTemplate");

        migrationBuilder.DropColumn(
            name: "CompanyId",
            schema: "dbo",
            table: "FgsSetupCommunicationTemplate");

        migrationBuilder.AddColumn<Guid>(
            name: "TenantId",
            schema: "dbo",
            table: "FgsSetupCommunicationTemplate",
            type: "uuid",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "CompanyId",
            schema: "dbo",
            table: "FgsSetupCommunicationTemplate",
            type: "uuid",
            nullable: true);

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

        migrationBuilder.DropIndex(
            name: "IX_FgsNotificationHistory_TenantId_CreatedOn",
            schema: "dbo",
            table: "FgsNotificationHistory");

        migrationBuilder.DropColumn(
            name: "TenantId",
            schema: "dbo",
            table: "FgsNotificationHistory");

        migrationBuilder.AddColumn<Guid>(
            name: "TenantId",
            schema: "dbo",
            table: "FgsNotificationHistory",
            type: "uuid",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.CreateIndex(
            name: "IX_FgsNotificationHistory_TenantId_CreatedOn",
            schema: "dbo",
            table: "FgsNotificationHistory",
            columns: new[] { "TenantId", "CreatedOn" });
    }
}
