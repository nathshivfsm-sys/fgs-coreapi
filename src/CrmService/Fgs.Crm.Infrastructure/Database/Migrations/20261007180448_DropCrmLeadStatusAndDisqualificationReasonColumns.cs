using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fgs.Crm.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class DropCrmLeadStatusAndDisqualificationReasonColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CrmLead_TenantId_CompanyId_DisqualificationReasonId",
                schema: "crm",
                table: "CrmLead");

            migrationBuilder.DropIndex(
                name: "IX_CrmLead_TenantId_CompanyId_LeadStatusId",
                schema: "crm",
                table: "CrmLead");

            migrationBuilder.DropColumn(
                name: "DisqualificationReasonId",
                schema: "crm",
                table: "CrmLead");

            migrationBuilder.DropColumn(
                name: "LeadStatusId",
                schema: "crm",
                table: "CrmLead");

            migrationBuilder.AlterColumn<long>(
                name: "LeadSourceId",
                schema: "crm",
                table: "CrmLead",
                type: "bigint",
                nullable: false,
                comment: "Source that generated the lead selected from setup.FgsSource.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Source that generated the lead selected from setup.FgsLeadSource.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "LeadSourceId",
                schema: "crm",
                table: "CrmLead",
                type: "bigint",
                nullable: false,
                comment: "Source that generated the lead selected from setup.FgsLeadSource.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Source that generated the lead selected from setup.FgsSource.");

            migrationBuilder.AddColumn<long>(
                name: "DisqualificationReasonId",
                schema: "crm",
                table: "CrmLead",
                type: "bigint",
                nullable: true,
                comment: "Reason the lead was disqualified selected from setup.FgsLeadDisqualificationReason.");

            migrationBuilder.AddColumn<long>(
                name: "LeadStatusId",
                schema: "crm",
                table: "CrmLead",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "Current status of the lead selected from the configured sales pipeline statuses applicable to leads.");

            migrationBuilder.CreateIndex(
                name: "IX_CrmLead_TenantId_CompanyId_DisqualificationReasonId",
                schema: "crm",
                table: "CrmLead",
                columns: new[] { "TenantId", "CompanyId", "DisqualificationReasonId" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmLead_TenantId_CompanyId_LeadStatusId",
                schema: "crm",
                table: "CrmLead",
                columns: new[] { "TenantId", "CompanyId", "LeadStatusId" });
        }
    }
}
