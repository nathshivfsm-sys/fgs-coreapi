using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Crm.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCrmCoreEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CrmCustomer",
                schema: "crm",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    LastServiceLocationSequence = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AddressLine1 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AddressLine2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AddressLine3 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AddressLine4 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    County = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    FormattedAddress = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Latitude = table.Column<decimal>(type: "numeric(18,10)", nullable: true),
                    Longitude = table.Column<decimal>(type: "numeric(18,10)", nullable: true),
                    PlaceId = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DefaultPaymentTermId = table.Column<long>(type: "bigint", nullable: true),
                    DefaultMaterialPricingMatrixId = table.Column<long>(type: "bigint", nullable: true),
                    DefaultLaborPricingMatrixId = table.Column<long>(type: "bigint", nullable: true),
                    DefaultOtherPricingMatrixId = table.Column<long>(type: "bigint", nullable: true),
                    DefaultPORequired = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    TaxExempt = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    TaxExemptNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CustomerAccountNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ExternalEntityId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ExternalVersion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmCustomer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrmCustomer_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "crm",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrmEntityTag",
                schema: "crm",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TagId = table.Column<long>(type: "bigint", nullable: false),
                    EntityTypeId = table.Column<int>(type: "integer", nullable: false),
                    EntityId = table.Column<long>(type: "bigint", nullable: false),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmEntityTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrmEntityTag_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "crm",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrmLead",
                schema: "crm",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    LeadStatusId = table.Column<long>(type: "bigint", nullable: false, comment: "Current lead status selected from setup.FgsLeadStatus."),
                    LeadSourceId = table.Column<long>(type: "bigint", nullable: false, comment: "Source that generated the lead selected from setup.FgsLeadSource."),
                    CampaignId = table.Column<long>(type: "bigint", nullable: true, comment: "Marketing campaign associated with the lead."),
                    LeadSummary = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "Short summary describing the lead inquiry."),
                    LeadDescription = table.Column<string>(type: "text", nullable: true, comment: "Detailed description of the lead inquiry and customer requirements."),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Lead contact first name."),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Lead contact last name."),
                    CompanyName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Company or organization associated with the lead."),
                    CustomerTypeId = table.Column<long>(type: "bigint", nullable: true, comment: "Customer type associated with the lead."),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Primary email address for the lead."),
                    Phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Primary phone number for the lead."),
                    PrimaryContactMethodId = table.Column<long>(type: "bigint", nullable: true, comment: "Preferred or originating contact method for the lead."),
                    ServiceZipCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, comment: "ZIP or postal code where service is requested."),
                    AssignedToUserId = table.Column<long>(type: "bigint", nullable: true, comment: "User assigned to work the lead."),
                    LeadReceivedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "Date and time the lead was originally received."),
                    QualifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Date and time the lead was qualified."),
                    DisqualifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Date and time the lead was disqualified."),
                    DisqualificationReasonId = table.Column<long>(type: "bigint", nullable: true, comment: "Reason the lead was disqualified selected from setup.FgsLeadDisqualificationReason."),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true, comment: "Customer record created from this lead after conversion."),
                    ConvertedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "Date and time the lead was converted into a customer."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmLead", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrmLead_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "crm",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores inbound sales inquiries and prospects prior to qualification and conversion into customers and opportunities.");

            migrationBuilder.CreateTable(
                name: "CrmNote",
                schema: "crm",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntityTypeId = table.Column<int>(type: "integer", nullable: false),
                    EntityId = table.Column<long>(type: "bigint", nullable: false),
                    NoteTypeId = table.Column<short>(type: "smallint", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    NoteText = table.Column<string>(type: "text", nullable: false),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsPinned = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmNote", x => x.Id);
                    table.CheckConstraint("CK_CrmNote_EntityTypeId", "\"EntityTypeId\" BETWEEN 1 AND 5");
                    table.CheckConstraint("CK_CrmNote_NoteTypeId", "\"NoteTypeId\" BETWEEN 1 AND 5");
                    table.ForeignKey(
                        name: "FK_CrmNote_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "crm",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrmServiceLocation",
                schema: "crm",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    LocationSequence = table.Column<int>(type: "integer", nullable: false),
                    LocationNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmServiceLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrmServiceLocation_Customer",
                        column: x => x.CustomerId,
                        principalSchema: "crm",
                        principalTable: "CrmCustomer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrmServiceLocation_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "crm",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrmContact",
                schema: "crm",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    ServiceLocationId = table.Column<long>(type: "bigint", nullable: true),
                    DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DepartmentName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsDefaultContact = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CanReceiveEstimates = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CanReceiveInvoices = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CanReceiveAppointments = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmContact", x => x.Id);
                    table.CheckConstraint("CK_CrmContact_Owner", "(\"CustomerId\" IS NOT NULL AND \"ServiceLocationId\" IS NULL) OR (\"CustomerId\" IS NULL AND \"ServiceLocationId\" IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_CrmContact_Customer",
                        column: x => x.CustomerId,
                        principalSchema: "crm",
                        principalTable: "CrmCustomer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrmContact_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "crm",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrmContact_ServiceLocation",
                        column: x => x.ServiceLocationId,
                        principalSchema: "crm",
                        principalTable: "CrmServiceLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrmContactCommunication",
                schema: "crm",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContactId = table.Column<long>(type: "bigint", nullable: false),
                    CommunicationTypeId = table.Column<short>(type: "smallint", nullable: false),
                    Label = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Value = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Extension = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrmContactCommunication", x => x.Id);
                    table.CheckConstraint("CK_CrmContactCommunication_CommunicationTypeId", "\"CommunicationTypeId\" BETWEEN 1 AND 7");
                    table.ForeignKey(
                        name: "FK_CrmContactCommunication_Contact",
                        column: x => x.ContactId,
                        principalSchema: "crm",
                        principalTable: "CrmContact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrmContactCommunication_FgsTenantCompanyCache_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "crm",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CrmContact_CustomerId",
                schema: "crm",
                table: "CrmContact",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmContact_DisplayName",
                schema: "crm",
                table: "CrmContact",
                columns: new[] { "TenantId", "CompanyId", "DisplayName" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmContact_IsActive",
                schema: "crm",
                table: "CrmContact",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmContact_ServiceLocationId",
                schema: "crm",
                table: "CrmContact",
                column: "ServiceLocationId");

            migrationBuilder.CreateIndex(
                name: "UQ_CrmContact_DefaultCustomer",
                schema: "crm",
                table: "CrmContact",
                columns: new[] { "TenantId", "CompanyId", "CustomerId" },
                unique: true,
                filter: "\"IsDefaultContact\" = true AND \"CustomerId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ_CrmContact_DefaultServiceLocation",
                schema: "crm",
                table: "CrmContact",
                columns: new[] { "TenantId", "CompanyId", "ServiceLocationId" },
                unique: true,
                filter: "\"IsDefaultContact\" = true AND \"ServiceLocationId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CrmContactCommunication_CommunicationTypeId",
                schema: "crm",
                table: "CrmContactCommunication",
                columns: new[] { "TenantId", "CompanyId", "CommunicationTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmContactCommunication_ContactId",
                schema: "crm",
                table: "CrmContactCommunication",
                columns: new[] { "TenantId", "CompanyId", "ContactId" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmContactCommunication_ContactId1",
                schema: "crm",
                table: "CrmContactCommunication",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_CrmContactCommunication_IsActive",
                schema: "crm",
                table: "CrmContactCommunication",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmContactCommunication_Value",
                schema: "crm",
                table: "CrmContactCommunication",
                columns: new[] { "TenantId", "CompanyId", "Value" });

            migrationBuilder.CreateIndex(
                name: "UQ_CrmContactCommunication_Primary",
                schema: "crm",
                table: "CrmContactCommunication",
                columns: new[] { "TenantId", "CompanyId", "ContactId", "CommunicationTypeId" },
                unique: true,
                filter: "\"IsPrimary\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_CrmCustomer_CustomerAccountNumber",
                schema: "crm",
                table: "CrmCustomer",
                columns: new[] { "TenantId", "CompanyId", "CustomerAccountNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmCustomer_DisplayName",
                schema: "crm",
                table: "CrmCustomer",
                columns: new[] { "TenantId", "CompanyId", "DisplayName" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmCustomer_ExternalEntityId",
                schema: "crm",
                table: "CrmCustomer",
                columns: new[] { "TenantId", "CompanyId", "ExternalEntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmCustomer_IsActive",
                schema: "crm",
                table: "CrmCustomer",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "UQ_CrmCustomer_CustomerNumber",
                schema: "crm",
                table: "CrmCustomer",
                columns: new[] { "TenantId", "CompanyId", "CustomerNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CrmEntityTag_Entity",
                schema: "crm",
                table: "CrmEntityTag",
                columns: new[] { "TenantId", "CompanyId", "EntityTypeId", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmEntityTag_TagId",
                schema: "crm",
                table: "CrmEntityTag",
                columns: new[] { "TenantId", "CompanyId", "TagId" });

            migrationBuilder.CreateIndex(
                name: "UQ_CrmEntityTag",
                schema: "crm",
                table: "CrmEntityTag",
                columns: new[] { "TenantId", "CompanyId", "TagId", "EntityTypeId", "EntityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CrmLead_TenantId_CompanyId",
                schema: "crm",
                table: "CrmLead",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmLead_TenantId_CompanyId_AssignedToUserId",
                schema: "crm",
                table: "CrmLead",
                columns: new[] { "TenantId", "CompanyId", "AssignedToUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmLead_TenantId_CompanyId_CampaignId",
                schema: "crm",
                table: "CrmLead",
                columns: new[] { "TenantId", "CompanyId", "CampaignId" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmLead_TenantId_CompanyId_CustomerId",
                schema: "crm",
                table: "CrmLead",
                columns: new[] { "TenantId", "CompanyId", "CustomerId" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmLead_TenantId_CompanyId_CustomerTypeId",
                schema: "crm",
                table: "CrmLead",
                columns: new[] { "TenantId", "CompanyId", "CustomerTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmLead_TenantId_CompanyId_DisqualificationReasonId",
                schema: "crm",
                table: "CrmLead",
                columns: new[] { "TenantId", "CompanyId", "DisqualificationReasonId" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmLead_TenantId_CompanyId_LeadReceivedOn",
                schema: "crm",
                table: "CrmLead",
                columns: new[] { "TenantId", "CompanyId", "LeadReceivedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmLead_TenantId_CompanyId_LeadSourceId",
                schema: "crm",
                table: "CrmLead",
                columns: new[] { "TenantId", "CompanyId", "LeadSourceId" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmLead_TenantId_CompanyId_LeadStatusId",
                schema: "crm",
                table: "CrmLead",
                columns: new[] { "TenantId", "CompanyId", "LeadStatusId" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmLead_TenantId_CompanyId_PrimaryContactMethodId",
                schema: "crm",
                table: "CrmLead",
                columns: new[] { "TenantId", "CompanyId", "PrimaryContactMethodId" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmLead_TenantId_CompanyId_ServiceZipCode",
                schema: "crm",
                table: "CrmLead",
                columns: new[] { "TenantId", "CompanyId", "ServiceZipCode" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmNote_Entity",
                schema: "crm",
                table: "CrmNote",
                columns: new[] { "TenantId", "CompanyId", "EntityTypeId", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmNote_Entity_DisplayOrder",
                schema: "crm",
                table: "CrmNote",
                columns: new[] { "TenantId", "CompanyId", "EntityTypeId", "EntityId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmNote_IsActive",
                schema: "crm",
                table: "CrmNote",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmNote_IsPinned",
                schema: "crm",
                table: "CrmNote",
                columns: new[] { "TenantId", "CompanyId", "IsPinned" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmNote_NoteTypeId",
                schema: "crm",
                table: "CrmNote",
                columns: new[] { "TenantId", "CompanyId", "NoteTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmServiceLocation_CustomerId",
                schema: "crm",
                table: "CrmServiceLocation",
                columns: new[] { "TenantId", "CompanyId", "CustomerId" });

            migrationBuilder.CreateIndex(
                name: "IX_CrmServiceLocation_CustomerId1",
                schema: "crm",
                table: "CrmServiceLocation",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "UQ_CrmServiceLocation_Customer_LocationSequence",
                schema: "crm",
                table: "CrmServiceLocation",
                columns: new[] { "TenantId", "CompanyId", "CustomerId", "LocationSequence" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_CrmServiceLocation_LocationNumber",
                schema: "crm",
                table: "CrmServiceLocation",
                columns: new[] { "TenantId", "CompanyId", "LocationNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrmContactCommunication",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "CrmEntityTag",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "CrmLead",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "CrmNote",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "CrmContact",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "CrmServiceLocation",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "CrmCustomer",
                schema: "crm");
        }
    }
}
