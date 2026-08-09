using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Inventory.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddTruckStockTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FgsTruckStockTemplate",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier for the truck stock template.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifies the tenant that owns this truck stock template."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifies the company that owns this truck stock template."),
                    TemplateCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Unique business code used to identify the truck stock template within a company."),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "User-friendly name of the truck stock template."),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Optional description explaining the intended purpose or usage of the truck stock template."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time when the truck stock template was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the truck stock template."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time when the truck stock template was last modified."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last modified the truck stock template."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the truck stock template is available for use when commissioning or synchronizing truck inventory.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsTruckStockTemplate", x => x.Id);
                    table.UniqueConstraint("UQ_FgsTruckStockTemplate_TenantId_CompanyId_TemplateCode", x => new { x.TenantId, x.CompanyId, x.TemplateCode });
                    table.ForeignKey(
                        name: "FK_FgsTruckStockTemplate_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "inventory",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Defines reusable truck stock templates that specify the desired inventory configuration for service vehicles. Templates are used during truck commissioning and synchronization and do not store or create inventory themselves.");

            migrationBuilder.CreateTable(
                name: "FgsTruckStockTemplateItem",
                schema: "inventory",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifies the tenant that owns this truck stock template item."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Identifies the company that owns this truck stock template item."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Unique identifier for the truck stock template item.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TruckStockTemplateId = table.Column<long>(type: "bigint", nullable: false, comment: "References the truck stock template that includes this inventory item."),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false, comment: "References the inventory item included in the truck stock template."),
                    TargetQuantity = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0.00m, comment: "Desired quantity of the inventory item to stock on trucks using this template."),
                    MinimumQuantity = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0.00m, comment: "Minimum warehouse quantity required before inventory can be transferred during truck commissioning or synchronization."),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 1, comment: "Controls the display order of inventory items within the truck stock template."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time when the truck stock template item was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the truck stock template item."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time when the truck stock template item was last modified."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last modified the truck stock template item.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsTruckStockTemplateItem", x => x.Id);
                    table.UniqueConstraint("UQ_FgsTruckStockTemplateItem_Template_Item", x => new { x.TenantId, x.CompanyId, x.TruckStockTemplateId, x.InventoryItemId });
                    table.CheckConstraint("CK_FgsTruckStockTemplateItem_MinimumQuantity", "\"MinimumQuantity\" >= 0");
                    table.CheckConstraint("CK_FgsTruckStockTemplateItem_TargetGreaterThanMinimum", "\"TargetQuantity\" >= \"MinimumQuantity\"");
                    table.CheckConstraint("CK_FgsTruckStockTemplateItem_TargetQuantity", "\"TargetQuantity\" >= 0");
                    table.ForeignKey(
                        name: "FK_FgsTruckStockTemplateItem_FgsInventoryItem",
                        column: x => x.InventoryItemId,
                        principalSchema: "inventory",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsTruckStockTemplateItem_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "inventory",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsTruckStockTemplateItem_FgsTruckStockTemplate",
                        column: x => x.TruckStockTemplateId,
                        principalSchema: "inventory",
                        principalTable: "FgsTruckStockTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Defines the inventory items and desired stocking quantities for a truck stock template.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsTruckStockTemplate_IsActive",
                schema: "inventory",
                table: "FgsTruckStockTemplate",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsTruckStockTemplate_TenantId_CompanyId_Name",
                schema: "inventory",
                table: "FgsTruckStockTemplate",
                columns: new[] { "TenantId", "CompanyId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsTruckStockTemplateItem_InventoryItemId",
                schema: "inventory",
                table: "FgsTruckStockTemplateItem",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsTruckStockTemplateItem_TenantId_CompanyId_TruckStockTemplateId",
                schema: "inventory",
                table: "FgsTruckStockTemplateItem",
                columns: new[] { "TenantId", "CompanyId", "TruckStockTemplateId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsTruckStockTemplateItem_TruckStockTemplateId",
                schema: "inventory",
                table: "FgsTruckStockTemplateItem",
                column: "TruckStockTemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsTruckStockTemplateItem",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "FgsTruckStockTemplate",
                schema: "inventory");
        }
    }
}
