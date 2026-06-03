using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddGloCommunicationTemplateAndSchemaComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "billing");
            migrationBuilder.EnsureSchema(name: "crm");
            migrationBuilder.EnsureSchema(name: "dispatch");
            migrationBuilder.EnsureSchema(name: "integration");
            migrationBuilder.EnsureSchema(name: "inventory");
            migrationBuilder.EnsureSchema(name: "notification");
            migrationBuilder.EnsureSchema(name: "reporting");
            migrationBuilder.EnsureSchema(name: "workflow");

            migrationBuilder.Sql(
                """
                COMMENT ON SCHEMA audit IS 'Stores audit logs, entity history, security events, and compliance records.';
                COMMENT ON SCHEMA billing IS 'Stores estimates, invoices, payments, taxes, and financial transactions.';
                COMMENT ON SCHEMA crm IS 'Stores leads, customers, contacts, opportunities, and customer-related data.';
                COMMENT ON SCHEMA dispatch IS 'Stores work orders, appointments, scheduling, routing, and service operations.';
                COMMENT ON SCHEMA glo IS 'Stores FSM platform-owned global master and reference data shared across all tenants.';
                COMMENT ON SCHEMA identity IS 'Stores users, roles, permissions, authentication, and security-related data.';
                COMMENT ON SCHEMA integration IS 'Stores external system integrations, credentials, webhooks, and synchronization data.';
                COMMENT ON SCHEMA inventory IS 'Stores inventory items, warehouses, stock transactions, and purchasing data.';
                COMMENT ON SCHEMA notification IS 'Stores notification templates, delivery queues, messages, and communication history.';
                COMMENT ON SCHEMA shared IS 'Stores reusable tenant-owned entities shared across multiple business modules.';
                COMMENT ON SCHEMA tenant IS 'Stores tenant organizational structure, companies, subscriptions, and ownership data.';
                COMMENT ON SCHEMA setup IS 'Stores tenant business configuration, operational settings, pricing, tax, and accounting setup.';
                COMMENT ON SCHEMA reporting IS 'Stores report definitions, dashboards, KPIs, and analytics configuration.';
                COMMENT ON SCHEMA workflow IS 'Stores workflow definitions, automation rules, triggers, and business processes.';
                """);

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupCommunicationTemplate_FgsTenantCompany_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupCommunicationTemplate");

            migrationBuilder.DropUniqueConstraint(
                name: "UQ_FgsSetupCommunicationTemplate",
                schema: "setup",
                table: "FgsSetupCommunicationTemplate");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "setup",
                table: "FgsSetupCommunicationTemplate",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "setup",
                table: "FgsSetupCommunicationTemplate",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "setup",
                table: "FgsSetupCommunicationTemplate",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddColumn<string>(
                name: "CommunicationChannel",
                schema: "setup",
                table: "FgsSetupCommunicationTemplate",
                type: "character varying(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "Email");

            migrationBuilder.CreateTable(
                name: "GloCommunicationTemplate",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TemplateScope = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Tenant", comment: "Defines whether the template is system-managed or available for tenant customization."),
                    CommunicationChannel = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false, comment: "Communication delivery channel such as Email, SMS, PushNotification, or SystemNotification."),
                    TemplateCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Unique business event identifier such as INVOICE_SENT, PASSWORD_RESET, or WORKORDER_COMPLETED."),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Display name of the communication template."),
                    Subject = table.Column<string>(type: "text", nullable: true, comment: "Subject line used for communication channels that support a subject."),
                    Body = table.Column<string>(type: "text", nullable: false, comment: "Template content containing static text and communication tokens."),
                    IsMobileVisible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the template is available within the mobile application."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Determines the display order of the template in user interfaces."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the communication template is active and available for use."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloCommunicationTemplate", x => x.Id);
                    table.CheckConstraint("CK_GloCommunicationTemplate_CommunicationChannel", "\"CommunicationChannel\" IN ('Email', 'SMS', 'PushNotification', 'SystemNotification')");
                    table.CheckConstraint("CK_GloCommunicationTemplate_TemplateScope", "\"TemplateScope\" IN ('Tenant', 'System')");
                },
                comment: "Stores FSM-provided communication templates available for system use or tenant customization.");

            migrationBuilder.CreateTable(
                name: "GloCommunicationTemplateToken",
                schema: "glo",
                columns: table => new
                {
                    CommunicationTemplateId = table.Column<long>(type: "bigint", nullable: false, comment: "Reference to the communication template."),
                    CommunicationTokenId = table.Column<int>(type: "integer", nullable: false, comment: "Reference to a communication token available for use within the template.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloCommunicationTemplateToken", x => new { x.CommunicationTemplateId, x.CommunicationTokenId });
                    table.ForeignKey(
                        name: "FK_GloCommunicationTemplateToken_CommunicationTemplateId",
                        column: x => x.CommunicationTemplateId,
                        principalSchema: "glo",
                        principalTable: "GloCommunicationTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GloCommunicationTemplateToken_CommunicationTokenId",
                        column: x => x.CommunicationTokenId,
                        principalSchema: "glo",
                        principalTable: "GloCommunicationToken",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Junction table defining the valid communication tokens that may be used within a communication template.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupCommunicationTemplate_TenantId_CompanyId_TemplateTyp",
                schema: "setup",
                table: "FgsSetupCommunicationTemplate",
                columns: new[] { "TenantId", "CompanyId", "CommunicationChannel", "TemplateType", "Code" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_FgsSetupCommunicationTemplate_CommunicationChannel",
                schema: "setup",
                table: "FgsSetupCommunicationTemplate",
                sql: "\"CommunicationChannel\" IN ('Email', 'SMS', 'PushNotification', 'SystemNotification')");

            migrationBuilder.CreateIndex(
                name: "IX_GloCommunicationTemplate_CommunicationChannel",
                schema: "glo",
                table: "GloCommunicationTemplate",
                column: "CommunicationChannel");

            migrationBuilder.CreateIndex(
                name: "IX_GloCommunicationTemplate_IsActive",
                schema: "glo",
                table: "GloCommunicationTemplate",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_GloCommunicationTemplate_TemplateCode",
                schema: "glo",
                table: "GloCommunicationTemplate",
                column: "TemplateCode");

            migrationBuilder.CreateIndex(
                name: "IX_GloCommunicationTemplate_TemplateScope",
                schema: "glo",
                table: "GloCommunicationTemplate",
                column: "TemplateScope");

            migrationBuilder.CreateIndex(
                name: "UQ_GloCommunicationTemplate_CommunicationChannel_TemplateCode",
                schema: "glo",
                table: "GloCommunicationTemplate",
                columns: new[] { "CommunicationChannel", "TemplateCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloCommunicationTemplateToken_CommunicationTokenId",
                schema: "glo",
                table: "GloCommunicationTemplateToken",
                column: "CommunicationTokenId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GloCommunicationTemplateToken",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloCommunicationTemplate",
                schema: "glo");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupCommunicationTemplate_TenantId_CompanyId_TemplateTyp",
                schema: "setup",
                table: "FgsSetupCommunicationTemplate");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FgsSetupCommunicationTemplate_CommunicationChannel",
                schema: "setup",
                table: "FgsSetupCommunicationTemplate");

            migrationBuilder.DropColumn(
                name: "CommunicationChannel",
                schema: "setup",
                table: "FgsSetupCommunicationTemplate");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "setup",
                table: "FgsSetupCommunicationTemplate",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "setup",
                table: "FgsSetupCommunicationTemplate",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "setup",
                table: "FgsSetupCommunicationTemplate",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddUniqueConstraint(
                name: "UQ_FgsSetupCommunicationTemplate",
                schema: "setup",
                table: "FgsSetupCommunicationTemplate",
                columns: new[] { "TenantId", "CompanyId", "TemplateType", "Code" });

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupCommunicationTemplate_FgsTenantCompany_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupCommunicationTemplate",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "tenant",
                principalTable: "FgsTenantCompany",
                principalColumns: new[] { "TenantId", "CompanyNumber" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
