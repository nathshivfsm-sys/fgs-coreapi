using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Crm.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AlignCrmCustomerLocationContactComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "CrmServiceLocation",
                schema: "crm",
                comment: "Physical customer location where field service work is performed.",
                oldComment: "Physical customer locations where field service work is performed.");

            migrationBuilder.AlterTable(
                name: "CrmCustomer",
                schema: "crm",
                comment: "Represents a customer account that can own one or more service locations and is responsible for billing.");

            migrationBuilder.AlterTable(
                name: "CrmContact",
                schema: "crm",
                comment: "Contact associated with either a customer or a service location. A contact belongs to one owner: either a customer or a service location.");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "timestamptz",
                nullable: true,
                comment: "Timestamp when the service location was last updated.",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldNullable: true,
                oldComment: "Last update timestamp.");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "User or process that last updated the service location.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "User that last updated the record.");

            migrationBuilder.AlterColumn<bool>(
                name: "TaxExempt",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Indicates whether the service location is tax exempt.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false,
                oldComment: "Indicates whether this service location is tax exempt.");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Service location state or province.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "State or province.");

            migrationBuilder.AlterColumn<bool>(
                name: "SmsAllowed",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                comment: "Indicates whether SMS communication is permitted for the service location.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true,
                oldComment: "Whether SMS communication is permitted.");

            migrationBuilder.AlterColumn<short>(
                name: "ServiceLocationTypeId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0,
                comment: "Identifier of the service location type.",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)0,
                oldComment: "Lookup to service location type.");

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                comment: "Service location postal or ZIP code.",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldComment: "Postal or ZIP code.");

            migrationBuilder.AlterColumn<string>(
                name: "PlaceId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                comment: "Place identifier returned by the address or mapping provider.",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true,
                oldComment: "Google or mapping provider Place Id.");

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "numeric(18,10)",
                nullable: true,
                comment: "Longitude coordinate associated with the service location.",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,10)",
                oldNullable: true,
                oldComment: "Longitude coordinate.");

            migrationBuilder.AlterColumn<int>(
                name: "LocationSequence",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "integer",
                nullable: false,
                comment: "Sequential service location number within a customer.",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Sequential location number within a customer.");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "numeric(18,10)",
                nullable: true,
                comment: "Latitude coordinate associated with the service location.",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,10)",
                oldNullable: true,
                oldComment: "Latitude coordinate.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                comment: "Indicates whether the service location is active.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true,
                oldComment: "Indicates whether this service location is active.");

            migrationBuilder.AlterColumn<long>(
                name: "InvoiceSmsTemplateId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Default SMS template used when sending invoices for the service location.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Default invoice SMS template.");

            migrationBuilder.AlterColumn<long>(
                name: "InvoiceEmailTemplateId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Default email template used when sending invoices for the service location.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Default invoice email template.");

            migrationBuilder.AlterColumn<string>(
                name: "FormattedAddress",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "Formatted service location address returned or constructed by the address or mapping provider.",
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldComment: "Formatted address returned by mapping provider.");

            migrationBuilder.AlterColumn<long>(
                name: "EstimateSmsTemplateId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Default SMS template used when sending estimates for the service location.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Default estimate SMS template.");

            migrationBuilder.AlterColumn<long>(
                name: "EstimateEmailTemplateId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Default email template used when sending estimates for the service location.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Default estimate email template.");

            migrationBuilder.AlterColumn<bool>(
                name: "EmailAllowed",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                comment: "Indicates whether email communication is permitted for the service location.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true,
                oldComment: "Whether email communication is permitted.");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                comment: "Service location name displayed to users and customers.",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldDefaultValue: "",
                oldComment: "Display name shown to users and customers.");

            migrationBuilder.AlterColumn<long>(
                name: "DefaultPaymentMethodId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Default payment method assigned to the service location.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Default payment method for this location.");

            migrationBuilder.AlterColumn<long>(
                name: "DefaultOtherPricingMatrixId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Default miscellaneous or other pricing matrix assigned to the service location.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Default miscellaneous pricing matrix.");

            migrationBuilder.AlterColumn<long>(
                name: "DefaultMaterialPricingMatrixId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Default material pricing matrix assigned to the service location.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Default material pricing matrix.");

            migrationBuilder.AlterColumn<long>(
                name: "DefaultLaborPricingMatrixId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Default labor pricing matrix assigned to the service location.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Default labor pricing matrix.");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()",
                comment: "Timestamp when the service location was created.",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "now()",
                oldComment: "Record creation timestamp.");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "User or process that created the service location.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "User that created the record.");

            migrationBuilder.AlterColumn<string>(
                name: "County",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Service location county or district.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "County or district.");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Service location country.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Country.");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: false,
                comment: "Company identifier within the tenant.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Company identifier.");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Service location city.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "City.");

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine4",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                comment: "Additional service location address information.",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "Additional address information.");

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine3",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                comment: "Additional service location address information.",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "Additional address information.");

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine2",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                comment: "Secondary service location address line.",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "Secondary street address.");

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine1",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                comment: "Primary service location address line.",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "Primary street address.");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: false,
                comment: "Primary key for the service location.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Primary key.")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "crm",
                table: "CrmCustomer",
                type: "timestamptz",
                nullable: true,
                comment: "Timestamp when the customer was last updated.",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "User or process that last updated the customer.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "crm",
                table: "CrmCustomer",
                type: "bigint",
                nullable: false,
                comment: "Tenant identifier.",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "TaxExemptNumber",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Tax exemption certificate or reference number associated with the customer.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "TaxExempt",
                schema: "crm",
                table: "CrmCustomer",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Indicates whether the customer is tax exempt.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Customer state or province.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                comment: "Customer postal or ZIP code.",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlaceId",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                comment: "Place identifier returned by the address or mapping provider.",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                comment: "Customer name.",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                schema: "crm",
                table: "CrmCustomer",
                type: "numeric(18,10)",
                nullable: true,
                comment: "Longitude coordinate associated with the customer address.",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                schema: "crm",
                table: "CrmCustomer",
                type: "numeric(18,10)",
                nullable: true,
                comment: "Latitude coordinate associated with the customer address.",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LastServiceLocationSequence",
                schema: "crm",
                table: "CrmCustomer",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Last service location sequence number assigned to this customer. Used to generate the next service location sequence.",
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "crm",
                table: "CrmCustomer",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                comment: "Indicates whether the customer is active.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "FormattedAddress",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "Formatted customer address returned or constructed by the address or mapping provider.",
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExternalVersion",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Version or synchronization version associated with the customer in an external system.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExternalEntityId",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                comment: "Identifier of the customer in an external system.",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                comment: "Customer name displayed to users and customers.",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<long>(
                name: "DefaultPaymentTermId",
                schema: "crm",
                table: "CrmCustomer",
                type: "bigint",
                nullable: true,
                comment: "Default payment term assigned to the customer.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "DefaultPORequired",
                schema: "crm",
                table: "CrmCustomer",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Indicates whether a purchase order is required by default for transactions for this customer.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<long>(
                name: "DefaultOtherPricingMatrixId",
                schema: "crm",
                table: "CrmCustomer",
                type: "bigint",
                nullable: true,
                comment: "Default miscellaneous or other pricing matrix assigned to the customer.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "DefaultMaterialPricingMatrixId",
                schema: "crm",
                table: "CrmCustomer",
                type: "bigint",
                nullable: true,
                comment: "Default material pricing matrix assigned to the customer.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "DefaultLaborPricingMatrixId",
                schema: "crm",
                table: "CrmCustomer",
                type: "bigint",
                nullable: true,
                comment: "Default labor pricing matrix assigned to the customer.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerNumber",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                comment: "Unique business identifier assigned to the customer.",
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerAccountNumber",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Customer account number maintained by the customer or an external accounting system.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "crm",
                table: "CrmCustomer",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()",
                comment: "Timestamp when the customer was created.",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "User or process that created the customer.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "County",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Customer county or district.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Customer country.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "crm",
                table: "CrmCustomer",
                type: "bigint",
                nullable: false,
                comment: "Company identifier within the tenant.",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Customer city.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine4",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                comment: "Additional customer address information.",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine3",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                comment: "Additional customer address information.",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine2",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                comment: "Secondary customer address line.",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine1",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                comment: "Primary customer address line.",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "crm",
                table: "CrmCustomer",
                type: "bigint",
                nullable: false,
                comment: "Primary key for the customer.",
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "crm",
                table: "CrmContact",
                type: "timestamptz",
                nullable: true,
                comment: "Timestamp when the contact was last updated.",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                schema: "crm",
                table: "CrmContact",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "User or process that last updated the contact.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "crm",
                table: "CrmContact",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Job title or position of the contact.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "crm",
                table: "CrmContact",
                type: "bigint",
                nullable: false,
                comment: "Tenant identifier.",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "ServiceLocationId",
                schema: "crm",
                table: "CrmContact",
                type: "bigint",
                nullable: true,
                comment: "Service location associated with the contact. NULL when the contact belongs to a customer.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDefaultContact",
                schema: "crm",
                table: "CrmContact",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Indicates whether the contact is the default contact for the associated customer or service location.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "crm",
                table: "CrmContact",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                comment: "Indicates whether the contact is active.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<short>(
                name: "DisplayOrder",
                schema: "crm",
                table: "CrmContact",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1,
                comment: "Order in which the contact is displayed relative to other contacts for the same owner.",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)1);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                schema: "crm",
                table: "CrmContact",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                comment: "Contact name displayed to users and customers.",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentName",
                schema: "crm",
                table: "CrmContact",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Department or organizational area of the contact.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CustomerId",
                schema: "crm",
                table: "CrmContact",
                type: "bigint",
                nullable: true,
                comment: "Customer associated with the contact. NULL when the contact belongs to a service location.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "crm",
                table: "CrmContact",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()",
                comment: "Timestamp when the contact was created.",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                schema: "crm",
                table: "CrmContact",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "User or process that created the contact.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "crm",
                table: "CrmContact",
                type: "bigint",
                nullable: false,
                comment: "Company identifier within the tenant.",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<bool>(
                name: "CanReceiveInvoices",
                schema: "crm",
                table: "CrmContact",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Indicates whether the contact is permitted to receive invoices.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "CanReceiveEstimates",
                schema: "crm",
                table: "CrmContact",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Indicates whether the contact is permitted to receive estimates.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "CanReceiveAppointments",
                schema: "crm",
                table: "CrmContact",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                comment: "Indicates whether the contact is permitted to receive appointment notifications.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "crm",
                table: "CrmContact",
                type: "bigint",
                nullable: false,
                comment: "Primary key for the contact.",
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "CrmServiceLocation",
                schema: "crm",
                comment: "Physical customer locations where field service work is performed.",
                oldComment: "Physical customer location where field service work is performed.");

            migrationBuilder.AlterTable(
                name: "CrmCustomer",
                schema: "crm",
                oldComment: "Represents a customer account that can own one or more service locations and is responsible for billing.");

            migrationBuilder.AlterTable(
                name: "CrmContact",
                schema: "crm",
                oldComment: "Contact associated with either a customer or a service location. A contact belongs to one owner: either a customer or a service location.");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "timestamptz",
                nullable: true,
                comment: "Last update timestamp.",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldNullable: true,
                oldComment: "Timestamp when the service location was last updated.");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "User that last updated the record.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "User or process that last updated the service location.");

            migrationBuilder.AlterColumn<bool>(
                name: "TaxExempt",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Indicates whether this service location is tax exempt.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false,
                oldComment: "Indicates whether the service location is tax exempt.");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "State or province.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Service location state or province.");

            migrationBuilder.AlterColumn<bool>(
                name: "SmsAllowed",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                comment: "Whether SMS communication is permitted.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true,
                oldComment: "Indicates whether SMS communication is permitted for the service location.");

            migrationBuilder.AlterColumn<short>(
                name: "ServiceLocationTypeId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0,
                comment: "Lookup to service location type.",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)0,
                oldComment: "Identifier of the service location type.");

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                comment: "Postal or ZIP code.",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldComment: "Service location postal or ZIP code.");

            migrationBuilder.AlterColumn<string>(
                name: "PlaceId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                comment: "Google or mapping provider Place Id.",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true,
                oldComment: "Place identifier returned by the address or mapping provider.");

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "numeric(18,10)",
                nullable: true,
                comment: "Longitude coordinate.",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,10)",
                oldNullable: true,
                oldComment: "Longitude coordinate associated with the service location.");

            migrationBuilder.AlterColumn<int>(
                name: "LocationSequence",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "integer",
                nullable: false,
                comment: "Sequential location number within a customer.",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Sequential service location number within a customer.");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "numeric(18,10)",
                nullable: true,
                comment: "Latitude coordinate.",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,10)",
                oldNullable: true,
                oldComment: "Latitude coordinate associated with the service location.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                comment: "Indicates whether this service location is active.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true,
                oldComment: "Indicates whether the service location is active.");

            migrationBuilder.AlterColumn<long>(
                name: "InvoiceSmsTemplateId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Default invoice SMS template.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Default SMS template used when sending invoices for the service location.");

            migrationBuilder.AlterColumn<long>(
                name: "InvoiceEmailTemplateId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Default invoice email template.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Default email template used when sending invoices for the service location.");

            migrationBuilder.AlterColumn<string>(
                name: "FormattedAddress",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "Formatted address returned by mapping provider.",
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldComment: "Formatted service location address returned or constructed by the address or mapping provider.");

            migrationBuilder.AlterColumn<long>(
                name: "EstimateSmsTemplateId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Default estimate SMS template.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Default SMS template used when sending estimates for the service location.");

            migrationBuilder.AlterColumn<long>(
                name: "EstimateEmailTemplateId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Default estimate email template.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Default email template used when sending estimates for the service location.");

            migrationBuilder.AlterColumn<bool>(
                name: "EmailAllowed",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                comment: "Whether email communication is permitted.",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true,
                oldComment: "Indicates whether email communication is permitted for the service location.");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                comment: "Display name shown to users and customers.",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldDefaultValue: "",
                oldComment: "Service location name displayed to users and customers.");

            migrationBuilder.AlterColumn<long>(
                name: "DefaultPaymentMethodId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Default payment method for this location.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Default payment method assigned to the service location.");

            migrationBuilder.AlterColumn<long>(
                name: "DefaultOtherPricingMatrixId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Default miscellaneous pricing matrix.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Default miscellaneous or other pricing matrix assigned to the service location.");

            migrationBuilder.AlterColumn<long>(
                name: "DefaultMaterialPricingMatrixId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Default material pricing matrix.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Default material pricing matrix assigned to the service location.");

            migrationBuilder.AlterColumn<long>(
                name: "DefaultLaborPricingMatrixId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: true,
                comment: "Default labor pricing matrix.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Default labor pricing matrix assigned to the service location.");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()",
                comment: "Record creation timestamp.",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "now()",
                oldComment: "Timestamp when the service location was created.");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "User that created the record.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "User or process that created the service location.");

            migrationBuilder.AlterColumn<string>(
                name: "County",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "County or district.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Service location county or district.");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Country.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Service location country.");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: false,
                comment: "Company identifier.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Company identifier within the tenant.");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "City.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Service location city.");

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine4",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                comment: "Additional address information.",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "Additional service location address information.");

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine3",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                comment: "Additional address information.",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "Additional service location address information.");

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine2",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                comment: "Secondary street address.",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "Secondary service location address line.");

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine1",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                comment: "Primary street address.",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "Primary service location address line.");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "crm",
                table: "CrmServiceLocation",
                type: "bigint",
                nullable: false,
                comment: "Primary key.",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Primary key for the service location.")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "crm",
                table: "CrmCustomer",
                type: "timestamptz",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldNullable: true,
                oldComment: "Timestamp when the customer was last updated.");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "User or process that last updated the customer.");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "crm",
                table: "CrmCustomer",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Tenant identifier.");

            migrationBuilder.AlterColumn<string>(
                name: "TaxExemptNumber",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Tax exemption certificate or reference number associated with the customer.");

            migrationBuilder.AlterColumn<bool>(
                name: "TaxExempt",
                schema: "crm",
                table: "CrmCustomer",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false,
                oldComment: "Indicates whether the customer is tax exempt.");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Customer state or province.");

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldComment: "Customer postal or ZIP code.");

            migrationBuilder.AlterColumn<string>(
                name: "PlaceId",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true,
                oldComment: "Place identifier returned by the address or mapping provider.");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldComment: "Customer name.");

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                schema: "crm",
                table: "CrmCustomer",
                type: "numeric(18,10)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,10)",
                oldNullable: true,
                oldComment: "Longitude coordinate associated with the customer address.");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                schema: "crm",
                table: "CrmCustomer",
                type: "numeric(18,10)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,10)",
                oldNullable: true,
                oldComment: "Latitude coordinate associated with the customer address.");

            migrationBuilder.AlterColumn<int>(
                name: "LastServiceLocationSequence",
                schema: "crm",
                table: "CrmCustomer",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0,
                oldComment: "Last service location sequence number assigned to this customer. Used to generate the next service location sequence.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "crm",
                table: "CrmCustomer",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true,
                oldComment: "Indicates whether the customer is active.");

            migrationBuilder.AlterColumn<string>(
                name: "FormattedAddress",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldComment: "Formatted customer address returned or constructed by the address or mapping provider.");

            migrationBuilder.AlterColumn<string>(
                name: "ExternalVersion",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Version or synchronization version associated with the customer in an external system.");

            migrationBuilder.AlterColumn<string>(
                name: "ExternalEntityId",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "Identifier of the customer in an external system.");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldComment: "Customer name displayed to users and customers.");

            migrationBuilder.AlterColumn<long>(
                name: "DefaultPaymentTermId",
                schema: "crm",
                table: "CrmCustomer",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Default payment term assigned to the customer.");

            migrationBuilder.AlterColumn<bool>(
                name: "DefaultPORequired",
                schema: "crm",
                table: "CrmCustomer",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false,
                oldComment: "Indicates whether a purchase order is required by default for transactions for this customer.");

            migrationBuilder.AlterColumn<long>(
                name: "DefaultOtherPricingMatrixId",
                schema: "crm",
                table: "CrmCustomer",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Default miscellaneous or other pricing matrix assigned to the customer.");

            migrationBuilder.AlterColumn<long>(
                name: "DefaultMaterialPricingMatrixId",
                schema: "crm",
                table: "CrmCustomer",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Default material pricing matrix assigned to the customer.");

            migrationBuilder.AlterColumn<long>(
                name: "DefaultLaborPricingMatrixId",
                schema: "crm",
                table: "CrmCustomer",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Default labor pricing matrix assigned to the customer.");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerNumber",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldComment: "Unique business identifier assigned to the customer.");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerAccountNumber",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Customer account number maintained by the customer or an external accounting system.");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "crm",
                table: "CrmCustomer",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "now()",
                oldComment: "Timestamp when the customer was created.");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "User or process that created the customer.");

            migrationBuilder.AlterColumn<string>(
                name: "County",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Customer county or district.");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Customer country.");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "crm",
                table: "CrmCustomer",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Company identifier within the tenant.");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Customer city.");

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine4",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "Additional customer address information.");

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine3",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "Additional customer address information.");

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine2",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "Secondary customer address line.");

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine1",
                schema: "crm",
                table: "CrmCustomer",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "Primary customer address line.");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "crm",
                table: "CrmCustomer",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Primary key for the customer.")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "crm",
                table: "CrmContact",
                type: "timestamptz",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldNullable: true,
                oldComment: "Timestamp when the contact was last updated.");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                schema: "crm",
                table: "CrmContact",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "User or process that last updated the contact.");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "crm",
                table: "CrmContact",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Job title or position of the contact.");

            migrationBuilder.AlterColumn<long>(
                name: "TenantId",
                schema: "crm",
                table: "CrmContact",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Tenant identifier.");

            migrationBuilder.AlterColumn<long>(
                name: "ServiceLocationId",
                schema: "crm",
                table: "CrmContact",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Service location associated with the contact. NULL when the contact belongs to a customer.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDefaultContact",
                schema: "crm",
                table: "CrmContact",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false,
                oldComment: "Indicates whether the contact is the default contact for the associated customer or service location.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "crm",
                table: "CrmContact",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true,
                oldComment: "Indicates whether the contact is active.");

            migrationBuilder.AlterColumn<short>(
                name: "DisplayOrder",
                schema: "crm",
                table: "CrmContact",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)1,
                oldComment: "Order in which the contact is displayed relative to other contacts for the same owner.");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                schema: "crm",
                table: "CrmContact",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldComment: "Contact name displayed to users and customers.");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentName",
                schema: "crm",
                table: "CrmContact",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Department or organizational area of the contact.");

            migrationBuilder.AlterColumn<long>(
                name: "CustomerId",
                schema: "crm",
                table: "CrmContact",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Customer associated with the contact. NULL when the contact belongs to a service location.");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "crm",
                table: "CrmContact",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "now()",
                oldComment: "Timestamp when the contact was created.");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                schema: "crm",
                table: "CrmContact",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "User or process that created the contact.");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                schema: "crm",
                table: "CrmContact",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Company identifier within the tenant.");

            migrationBuilder.AlterColumn<bool>(
                name: "CanReceiveInvoices",
                schema: "crm",
                table: "CrmContact",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false,
                oldComment: "Indicates whether the contact is permitted to receive invoices.");

            migrationBuilder.AlterColumn<bool>(
                name: "CanReceiveEstimates",
                schema: "crm",
                table: "CrmContact",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false,
                oldComment: "Indicates whether the contact is permitted to receive estimates.");

            migrationBuilder.AlterColumn<bool>(
                name: "CanReceiveAppointments",
                schema: "crm",
                table: "CrmContact",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true,
                oldComment: "Indicates whether the contact is permitted to receive appointment notifications.");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                schema: "crm",
                table: "CrmContact",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Primary key for the contact.")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
