using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Crm.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class ExpandCrmServiceLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "CrmServiceLocation",
                schema: "crm",
                comment: "Physical customer locations where field service work is performed.");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: false,
                comment: "Tenant identifier.",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "LocationSequence",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "integer",
                nullable: false,
                comment: "Sequential location number within a customer.",
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "LocationNumber",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                comment: "Business identifier for the service location.",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<long>(
                name: "CustomerId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: false,
                comment: "Customer that owns this service location.",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: false,
                comment: "Company identifier.",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: false,
                comment: "Primary key.",
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine1",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                comment: "Primary street address.");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                comment: "Secondary street address.");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine3",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                comment: "Additional address information.");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine4",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                comment: "Additional address information.");

            migrationBuilder.AddColumn<string>(
                name: "City",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "City.");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Country.");

            migrationBuilder.AddColumn<string>(
                name: "County",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "County or district.");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "User that created the record.");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()",
                comment: "Record creation timestamp.");

            migrationBuilder.AddColumn<long>(
                name: "DefaultLaborPricingMatrixId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Default labor pricing matrix.");

            migrationBuilder.AddColumn<long>(
                name: "DefaultMaterialPricingMatrixId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Default material pricing matrix.");

            migrationBuilder.AddColumn<long>(
                name: "DefaultOtherPricingMatrixId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Default miscellaneous pricing matrix.");

            migrationBuilder.AddColumn<long>(
                name: "DefaultPaymentMethodId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Default payment method for this location.");

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                comment: "Display name shown to users and customers.");

            migrationBuilder.AddColumn<bool>(
                name: "EmailAllowed",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                comment: "Whether email communication is permitted.");

            migrationBuilder.AddColumn<long>(
                name: "EstimateEmailTemplateId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Default estimate email template.");

            migrationBuilder.AddColumn<long>(
                name: "EstimateSmsTemplateId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Default estimate SMS template.");

            migrationBuilder.AddColumn<string>(
                name: "FormattedAddress",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "Formatted address returned by mapping provider.");

            migrationBuilder.AddColumn<long>(
                name: "InvoiceEmailTemplateId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Default invoice email template.");

            migrationBuilder.AddColumn<long>(
                name: "InvoiceSmsTemplateId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Default invoice SMS template.");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                comment: "Indicates whether this service location is active.");

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "numeric(18,10)",
                nullable: true,
                comment: "Latitude coordinate.");

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "numeric(18,10)",
                nullable: true,
                comment: "Longitude coordinate.");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                comment: "Internal service location name.");

            migrationBuilder.AddColumn<string>(
                name: "PlaceId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                comment: "Google or mapping provider Place Id.");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                comment: "Postal or ZIP code.");

            migrationBuilder.AddColumn<short>(
                name: "ServiceLocationTypeId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0,
                comment: "Lookup to service location type.");

            migrationBuilder.AddColumn<bool>(
                name: "SmsAllowed",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                comment: "Whether SMS communication is permitted.");

            migrationBuilder.AddColumn<string>(
                name: "State",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "State or province.");

            migrationBuilder.AddColumn<bool>(
                name: "TaxExempt",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Indicates whether this service location is tax exempt.");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "User that last updated the record.");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "timestamptz",
                nullable: true,
                comment: "Last update timestamp.");

            migrationBuilder.CreateIndex(
                name: "IX_CrmServiceLocation_City",
                schema: "crm",
                table: "CrmServiceLocation",
                columns: new[] { "TenantId", "CompanyId", "City" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmServiceLocation_DisplayName",
                schema: "crm",
                table: "CrmServiceLocation",
                columns: new[] { "TenantId", "CompanyId", "DisplayName" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmServiceLocation_IsActive",
                schema: "crm",
                table: "CrmServiceLocation",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmServiceLocation_Name",
                schema: "crm",
                table: "CrmServiceLocation",
                columns: new[] { "TenantId", "CompanyId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmServiceLocation_PlaceId",
                schema: "crm",
                table: "CrmServiceLocation",
                columns: new[] { "TenantId", "CompanyId", "PlaceId" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmServiceLocation_PostalCode",
                schema: "crm",
                table: "CrmServiceLocation",
                columns: new[] { "TenantId", "CompanyId", "PostalCode" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmServiceLocation_State",
                schema: "crm",
                table: "CrmServiceLocation",
                columns: new[] { "TenantId", "CompanyId", "State" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CrmServiceLocation_City",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropIndex(
                name: "IX_CrmServiceLocation_DisplayName",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropIndex(
                name: "IX_CrmServiceLocation_IsActive",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropIndex(
                name: "IX_CrmServiceLocation_Name",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropIndex(
                name: "IX_CrmServiceLocation_PlaceId",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropIndex(
                name: "IX_CrmServiceLocation_PostalCode",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropIndex(
                name: "IX_CrmServiceLocation_State",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "AddressLine1",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "AddressLine3",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "AddressLine4",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "City",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "Country",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "County",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "DefaultLaborPricingMatrixId",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "DefaultMaterialPricingMatrixId",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "DefaultOtherPricingMatrixId",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "DefaultPaymentMethodId",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "EmailAllowed",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "EstimateEmailTemplateId",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "EstimateSmsTemplateId",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "FormattedAddress",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "InvoiceEmailTemplateId",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "InvoiceSmsTemplateId",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "Latitude",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "Longitude",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "ServiceLocationTypeId",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "SmsAllowed",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "State",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "TaxExempt",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                schema: "crm",
                table: "CrmServiceLocation");

            migrationBuilder.AlterTable(
                name: "CrmServiceLocation",
                schema: "crm",
                oldComment: "Physical customer locations where field service work is performed.");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Tenant identifier.");

            migrationBuilder.AlterColumn<int>(
                name: "LocationSequence",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Sequential location number within a customer.");

            migrationBuilder.AlterColumn<string>(
                name: "LocationNumber",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldComment: "Business identifier for the service location.");

            migrationBuilder.AlterColumn<long>(
                name: "CustomerId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Customer that owns this service location.");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Company identifier.");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Primary key.")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
