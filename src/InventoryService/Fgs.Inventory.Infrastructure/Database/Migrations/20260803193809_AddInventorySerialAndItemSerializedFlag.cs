using System;
using Fgs.Inventory.Domain.Enums;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Inventory.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddInventorySerialAndItemSerializedFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultTaxable",
                schema: "inventory",
                table: "FgsInventoryItem");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:inventory.FgsInventorySerialStatus", "Available,Reserved,Installed,InTransit,Returned,Scrapped,Lost,OnHold");

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                schema: "inventory",
                table: "FgsInventoryTransaction",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                comment: "Serial number of the inventory item involved in the transaction. Null for non-serialized inventory items.");

            migrationBuilder.AddColumn<bool>(
                name: "IsSerialized",
                schema: "inventory",
                table: "FgsInventoryItem",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Indicates whether individual serial numbers are tracked for this inventory item.");

            migrationBuilder.CreateTable(
                name: "FgsInventorySerial",
                schema: "inventory",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false, comment: "Inventory item this serialized unit belongs to."),
                    SerialNumber = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Serial number of the inventory unit."),
                    InventorySerialStatus = table.Column<FgsInventorySerialStatus>(type: "inventory.\"FgsInventorySerialStatus\"", nullable: false, defaultValue: FgsInventorySerialStatus.Available, comment: "Current lifecycle status of the serialized inventory unit."),
                    Notes = table.Column<string>(type: "text", nullable: true, comment: "Optional notes."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Creation timestamp."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Last update timestamp."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventorySerial", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventorySerial_TenantId_CompanyId_InventoryItemId_SerialNumber", x => new { x.TenantId, x.CompanyId, x.InventoryItemId, x.SerialNumber });
                    table.ForeignKey(
                        name: "FK_FgsInventorySerial_FgsInventoryItem",
                        column: x => x.InventoryItemId,
                        principalSchema: "inventory",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventorySerial_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "inventory",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores individual serialized inventory units and their current lifecycle status. Inventory movement history is maintained in FgsInventoryTransaction.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryTransaction_TenantId_CompanyId_SerialNumber",
                schema: "inventory",
                table: "FgsInventoryTransaction",
                columns: new[] { "TenantId", "CompanyId", "SerialNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventorySerial_InventoryItemId",
                schema: "inventory",
                table: "FgsInventorySerial",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventorySerial_TenantId_CompanyId_InventoryItemId",
                schema: "inventory",
                table: "FgsInventorySerial",
                columns: new[] { "TenantId", "CompanyId", "InventoryItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventorySerial_TenantId_CompanyId_InventorySerialStatus",
                schema: "inventory",
                table: "FgsInventorySerial",
                columns: new[] { "TenantId", "CompanyId", "InventorySerialStatus" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsInventorySerial",
                schema: "inventory");

            migrationBuilder.DropIndex(
                name: "IX_FgsInventoryTransaction_TenantId_CompanyId_SerialNumber",
                schema: "inventory",
                table: "FgsInventoryTransaction");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                schema: "inventory",
                table: "FgsInventoryTransaction");

            migrationBuilder.DropColumn(
                name: "IsSerialized",
                schema: "inventory",
                table: "FgsInventoryItem");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:inventory.FgsInventorySerialStatus", "Available,Reserved,Installed,InTransit,Returned,Scrapped,Lost,OnHold");

            migrationBuilder.AddColumn<bool>(
                name: "DefaultTaxable",
                schema: "inventory",
                table: "FgsInventoryItem",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }
    }
}
