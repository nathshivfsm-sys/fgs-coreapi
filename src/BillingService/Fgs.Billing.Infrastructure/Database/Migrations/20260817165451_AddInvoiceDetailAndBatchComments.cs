using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Billing.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoiceDetailAndBatchComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "FgsInvoiceDetail",
                schema: "billing",
                comment: "Stores individual invoice line items, including labor, service, equipment, material, and other billable items, along with pricing, cost, tax, accounting, technician, and source information.");

            migrationBuilder.AlterTable(
                name: "FgsInvoiceBatch",
                schema: "billing",
                comment: "Stores invoice batch records used to group and summarize invoices for a tenant and company.");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedOn",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "timestamp",
                nullable: true,
                comment: "Date and time when the invoice detail line was last updated.",
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "UpdatedBy",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "bigint",
                nullable: true,
                comment: "Identifies the user who last updated the invoice detail line.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "numeric(18,4)",
                nullable: false,
                defaultValue: 0m,
                comment: "Sales price per unit, hour, or other quantity basis for the invoice line.",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,4)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitCost",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "numeric(18,4)",
                nullable: false,
                defaultValue: 0m,
                comment: "Cost per unit, hour, or other quantity basis for the invoice line.",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,4)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "bigint",
                nullable: false,
                comment: "Identifies the tenant that owns the invoice detail.",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "TechnicianId",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "bigint",
                nullable: true,
                comment: "Identifies the technician associated with the invoice line, when applicable.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "numeric(18,4)",
                nullable: false,
                defaultValue: 1m,
                comment: "Quantity used to calculate the extended cost and extended sales price of the invoice line. For labor, this represents the number of hours.",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,4)",
                oldDefaultValue: 1m);

            migrationBuilder.AlterColumn<long>(
                name: "PriceBookItemId",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "bigint",
                nullable: true,
                comment: "Identifies the Price Book item from which the invoice line was selected or populated, when applicable.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ParentLineId",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "bigint",
                nullable: true,
                comment: "Identifies the parent invoice detail line when this line is associated with another invoice line, such as a child or related line.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MasterPartNum",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Master part number associated with the item when applicable.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LineNumber",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "integer",
                nullable: false,
                comment: "Sequential line number used to identify and order the detail lines within an invoice.",
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "LineAddedFromId",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "bigint",
                nullable: true,
                comment: "Identifies the specific source record from which the invoice line was added.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LineAddedFrom",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                comment: "Identifies the type of source document or transaction from which the invoice line was added, such as an Estimate or Work Order.",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LaborRateTypeId",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "integer",
                nullable: true,
                comment: "Identifies the labor rate type used to determine labor pricing when the invoice line is a labor item.",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ItemDescription",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "text",
                nullable: false,
                comment: "Description of the item, service, labor, or charge displayed on the invoice.",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ItemCode",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Code identifying the service, material, equipment, or other item associated with the invoice line.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsTaxable",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Indicates whether the invoice line is subject to applicable sales tax calculation.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsInventory",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Indicates whether the invoice line represents an inventory item.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<long>(
                name: "InvoiceId",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "bigint",
                nullable: false,
                comment: "Identifies the invoice to which this detail line belongs.",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "InventoryItemId",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "bigint",
                nullable: true,
                comment: "Identifies the inventory item associated with the invoice detail when the line represents an inventory item.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GLBreak2Id",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "integer",
                nullable: true,
                comment: "Identifies the second general ledger break or accounting classification assigned to the invoice line.",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GLBreak1Id",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "integer",
                nullable: true,
                comment: "Identifies the first general ledger break or accounting classification assigned to the invoice line.",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ExtendedPrice",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m,
                comment: "Total sales price of the invoice line calculated from the applicable quantity and unit price.",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "ExtendedCost",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m,
                comment: "Total cost of the invoice line calculated from the applicable quantity and unit cost.",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "now()",
                comment: "Date and time when the invoice detail line was created.",
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<long>(
                name: "CreatedBy",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "bigint",
                nullable: false,
                comment: "Identifies the user who created the invoice detail line.",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "bigint",
                nullable: false,
                comment: "Identifies the company within the tenant that owns the invoice detail.",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "BillingCategoryId",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "integer",
                nullable: false,
                comment: "Identifies the billing category that determines the type and behavior of the invoice line, such as Labor, Service, Equipment, Material, or Other.",
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "AddedSource",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                comment: "Identifies the source or mechanism through which the invoice line was added to the invoice.",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "bigint",
                nullable: false,
                comment: "Unique identifier for the invoice detail line.",
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedOn",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "timestamp",
                nullable: true,
                comment: "Date and time when the invoice batch was last updated.",
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "UpdatedBy",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "bigint",
                nullable: true,
                comment: "Identifies the user who last updated the invoice batch.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalTax",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m,
                comment: "Total tax amount across all invoices included in the batch.",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "bigint",
                nullable: false,
                comment: "Identifies the tenant that owns the invoice batch.",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<bool>(
                name: "IsClosed",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Indicates whether the invoice batch has been closed and is no longer available for further batch processing.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "InvoiceTotal",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m,
                comment: "Total invoice amount across all invoices included in the batch, including applicable taxes.",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "InvoiceSubtotal",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m,
                comment: "Sum of the subtotals for all invoices included in the batch before tax.",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "InvoiceCount",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Number of invoices included in the batch.",
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "now()",
                comment: "Date and time when the invoice batch was created.",
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<long>(
                name: "CreatedBy",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "bigint",
                nullable: false,
                comment: "Identifies the user who created the invoice batch.",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "bigint",
                nullable: false,
                comment: "Identifies the company within the tenant that owns the invoice batch.",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ClosedOn",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "timestamp",
                nullable: true,
                comment: "Date and time when the invoice batch was closed.",
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ClosedBy",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "bigint",
                nullable: true,
                comment: "Identifies the user who closed the invoice batch.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BatchNumber",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                comment: "Unique batch number used to identify the invoice batch within the tenant and company.",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "BatchDate",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "date",
                nullable: false,
                comment: "Date assigned to the invoice batch.",
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "bigint",
                nullable: false,
                comment: "Unique identifier for the invoice batch.",
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "FgsInvoiceDetail",
                schema: "billing",
                oldComment: "Stores individual invoice line items, including labor, service, equipment, material, and other billable items, along with pricing, cost, tax, accounting, technician, and source information.");

            migrationBuilder.AlterTable(
                name: "FgsInvoiceBatch",
                schema: "billing",
                oldComment: "Stores invoice batch records used to group and summarize invoices for a tenant and company.");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedOn",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true,
                oldComment: "Date and time when the invoice detail line was last updated.");

            migrationBuilder.AlterColumn<long>(
                name: "UpdatedBy",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Identifies the user who last updated the invoice detail line.");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "numeric(18,4)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,4)",
                oldDefaultValue: 0m,
                oldComment: "Sales price per unit, hour, or other quantity basis for the invoice line.");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitCost",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "numeric(18,4)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,4)",
                oldDefaultValue: 0m,
                oldComment: "Cost per unit, hour, or other quantity basis for the invoice line.");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Identifies the tenant that owns the invoice detail.");

            migrationBuilder.AlterColumn<long>(
                name: "TechnicianId",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Identifies the technician associated with the invoice line, when applicable.");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "numeric(18,4)",
                nullable: false,
                defaultValue: 1m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,4)",
                oldDefaultValue: 1m,
                oldComment: "Quantity used to calculate the extended cost and extended sales price of the invoice line. For labor, this represents the number of hours.");

            migrationBuilder.AlterColumn<long>(
                name: "PriceBookItemId",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Identifies the Price Book item from which the invoice line was selected or populated, when applicable.");

            migrationBuilder.AlterColumn<long>(
                name: "ParentLineId",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Identifies the parent invoice detail line when this line is associated with another invoice line, such as a child or related line.");

            migrationBuilder.AlterColumn<string>(
                name: "MasterPartNum",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Master part number associated with the item when applicable.");

            migrationBuilder.AlterColumn<int>(
                name: "LineNumber",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Sequential line number used to identify and order the detail lines within an invoice.");

            migrationBuilder.AlterColumn<long>(
                name: "LineAddedFromId",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Identifies the specific source record from which the invoice line was added.");

            migrationBuilder.AlterColumn<string>(
                name: "LineAddedFrom",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "Identifies the type of source document or transaction from which the invoice line was added, such as an Estimate or Work Order.");

            migrationBuilder.AlterColumn<int>(
                name: "LaborRateTypeId",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "Identifies the labor rate type used to determine labor pricing when the invoice line is a labor item.");

            migrationBuilder.AlterColumn<string>(
                name: "ItemDescription",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "Description of the item, service, labor, or charge displayed on the invoice.");

            migrationBuilder.AlterColumn<string>(
                name: "ItemCode",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Code identifying the service, material, equipment, or other item associated with the invoice line.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsTaxable",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false,
                oldComment: "Indicates whether the invoice line is subject to applicable sales tax calculation.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsInventory",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false,
                oldComment: "Indicates whether the invoice line represents an inventory item.");

            migrationBuilder.AlterColumn<long>(
                name: "InvoiceId",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Identifies the invoice to which this detail line belongs.");

            migrationBuilder.AlterColumn<long>(
                name: "InventoryItemId",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Identifies the inventory item associated with the invoice detail when the line represents an inventory item.");

            migrationBuilder.AlterColumn<int>(
                name: "GLBreak2Id",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "Identifies the second general ledger break or accounting classification assigned to the invoice line.");

            migrationBuilder.AlterColumn<int>(
                name: "GLBreak1Id",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "Identifies the first general ledger break or accounting classification assigned to the invoice line.");

            migrationBuilder.AlterColumn<decimal>(
                name: "ExtendedPrice",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldDefaultValue: 0m,
                oldComment: "Total sales price of the invoice line calculated from the applicable quantity and unit price.");

            migrationBuilder.AlterColumn<decimal>(
                name: "ExtendedCost",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldDefaultValue: 0m,
                oldComment: "Total cost of the invoice line calculated from the applicable quantity and unit cost.");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldDefaultValueSql: "now()",
                oldComment: "Date and time when the invoice detail line was created.");

            migrationBuilder.AlterColumn<long>(
                name: "CreatedBy",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Identifies the user who created the invoice detail line.");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Identifies the company within the tenant that owns the invoice detail.");

            migrationBuilder.AlterColumn<int>(
                name: "BillingCategoryId",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Identifies the billing category that determines the type and behavior of the invoice line, such as Labor, Service, Equipment, Material, or Other.");

            migrationBuilder.AlterColumn<string>(
                name: "AddedSource",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "Identifies the source or mechanism through which the invoice line was added to the invoice.");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "billing",
                table: "FgsInvoiceDetail",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Unique identifier for the invoice detail line.")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedOn",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true,
                oldComment: "Date and time when the invoice batch was last updated.");

            migrationBuilder.AlterColumn<long>(
                name: "UpdatedBy",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Identifies the user who last updated the invoice batch.");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalTax",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldDefaultValue: 0m,
                oldComment: "Total tax amount across all invoices included in the batch.");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Identifies the tenant that owns the invoice batch.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsClosed",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false,
                oldComment: "Indicates whether the invoice batch has been closed and is no longer available for further batch processing.");

            migrationBuilder.AlterColumn<decimal>(
                name: "InvoiceTotal",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldDefaultValue: 0m,
                oldComment: "Total invoice amount across all invoices included in the batch, including applicable taxes.");

            migrationBuilder.AlterColumn<decimal>(
                name: "InvoiceSubtotal",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldDefaultValue: 0m,
                oldComment: "Sum of the subtotals for all invoices included in the batch before tax.");

            migrationBuilder.AlterColumn<int>(
                name: "InvoiceCount",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0,
                oldComment: "Number of invoices included in the batch.");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldDefaultValueSql: "now()",
                oldComment: "Date and time when the invoice batch was created.");

            migrationBuilder.AlterColumn<long>(
                name: "CreatedBy",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Identifies the user who created the invoice batch.");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Identifies the company within the tenant that owns the invoice batch.");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ClosedOn",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true,
                oldComment: "Date and time when the invoice batch was closed.");

            migrationBuilder.AlterColumn<long>(
                name: "ClosedBy",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Identifies the user who closed the invoice batch.");

            migrationBuilder.AlterColumn<string>(
                name: "BatchNumber",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldComment: "Unique batch number used to identify the invoice batch within the tenant and company.");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "BatchDate",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldComment: "Date assigned to the invoice batch.");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "billing",
                table: "FgsInvoiceBatch",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Unique identifier for the invoice batch.")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);
        }
    }
}
