using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UserService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithSetupSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var seedTime = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            migrationBuilder.EnsureSchema(
                name: "fgs");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.CreateTable(
                name: "FSGSetupAccountingIntegrationType",
                schema: "fgs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FSGSetupAccountingIntegrationType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FSGSetupBusinessType",
                schema: "fgs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FSGSetupBusinessType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FSGSetupLanguage",
                schema: "fgs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CultureCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FSGSetupLanguage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FSGSetupLocationType",
                schema: "fgs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FSGSetupLocationType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FSGSetupMasterEntityType",
                schema: "fgs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FSGSetupMasterEntityType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FSGSetupTimeCardOption",
                schema: "fgs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FSGSetupTimeCardOption", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenant",
                schema: "fgs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    LegalName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Email = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Website = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PrimaryLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    SubscriptionPlanId = table.Column<int>(type: "integer", nullable: true),
                    TimeZone = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DefaultCurrency = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DefaultLanguageId = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenant", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                schema: "fgs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyNumber = table.Column<long>(type: "bigint", nullable: false),
                    BusinessTypeId = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    LegalName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Email = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Website = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    TaxId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PrimaryLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    FullLogoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CompactLogoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IconLogoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    FaviconUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Company_FSGSetupBusinessType_BusinessTypeId",
                        column: x => x.BusinessTypeId,
                        principalSchema: "fgs",
                        principalTable: "FSGSetupBusinessType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Company_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "fgs",
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "fgs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "citext", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "fgs",
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "fgs",
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuthIdentity",
                schema: "fgs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Issuer = table.Column<string>(type: "text", nullable: false),
                    ObjectId = table.Column<string>(type: "text", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: true),
                    EmailSnapshot = table.Column<string>(type: "citext", nullable: true),
                    LinkedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthIdentity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthIdentity_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "fgs",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invite",
                schema: "fgs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    InvitedEmail = table.Column<string>(type: "citext", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    TokenHash = table.Column<byte[]>(type: "bytea", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    AcceptedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RevokedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invite_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "fgs",
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invite_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "fgs",
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invite_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "fgs",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IxAuthIdentityIssuerObjectId",
                schema: "fgs",
                table: "AuthIdentity",
                columns: new[] { "Issuer", "ObjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IxAuthIdentityUser",
                schema: "fgs",
                table: "AuthIdentity",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IxCompanyBusinessTypeId",
                schema: "fgs",
                table: "Company",
                column: "BusinessTypeId");

            migrationBuilder.CreateIndex(
                name: "IxCompanyCompanyGuid",
                schema: "fgs",
                table: "Company",
                column: "CompanyGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IxCompanyTenant",
                schema: "fgs",
                table: "Company",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IxFSGSetupAccountingIntegrationTypeCode",
                schema: "fgs",
                table: "FSGSetupAccountingIntegrationType",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IxFSGSetupBusinessTypeCode",
                schema: "fgs",
                table: "FSGSetupBusinessType",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IxFSGSetupLanguageCode",
                schema: "fgs",
                table: "FSGSetupLanguage",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IxFSGSetupLocationTypeCode",
                schema: "fgs",
                table: "FSGSetupLocationType",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IxFSGSetupMasterEntityTypeCode",
                schema: "fgs",
                table: "FSGSetupMasterEntityType",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IxFSGSetupTimeCardOptionCode",
                schema: "fgs",
                table: "FSGSetupTimeCardOption",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IxInviteCompanyId",
                schema: "fgs",
                table: "Invite",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IxInvitePending",
                schema: "fgs",
                table: "Invite",
                column: "TenantId",
                filter: "\"Status\" = 'pending'");

            migrationBuilder.CreateIndex(
                name: "IxInviteTokenHash",
                schema: "fgs",
                table: "Invite",
                column: "TokenHash");

            migrationBuilder.CreateIndex(
                name: "IxInviteUser",
                schema: "fgs",
                table: "Invite",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IxUsersCompanyId",
                schema: "fgs",
                table: "Users",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IxUsersTenant",
                schema: "fgs",
                table: "Users",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IxUsersTenantEmail",
                schema: "fgs",
                table: "Users",
                columns: new[] { "TenantId", "Email" },
                unique: true);

            migrationBuilder.InsertData(
                schema: "fgs",
                table: "FSGSetupBusinessType",
                columns: new[] { "Id", "Code", "Name", "SortOrder", "IsActive", "CreatedOn" },
                values: new object[,]
                {
                    { 1, "HVAC", "HVAC", 1, true, seedTime },
                    { 2, "PLUMBING", "Plumbing", 2, true, seedTime },
                    { 3, "ELECTRICAL", "Electrical", 3, true, seedTime },
                    { 4, "PESTCONTROL", "Pest Control", 4, true, seedTime },
                    { 5, "LAWNCARE", "Lawn Care", 5, true, seedTime },
                    { 6, "TRASHPICKUP", "Trash Pickup", 6, true, seedTime },
                    { 7, "GARAGEDOOR", "Garage Door", 7, true, seedTime },
                    { 8, "HOUSECLEANING", "House Cleaning", 8, true, seedTime },
                    { 9, "PAINTING", "Painting", 9, true, seedTime }
                });

            migrationBuilder.InsertData(
                schema: "fgs",
                table: "FSGSetupTimeCardOption",
                columns: new[] { "Id", "Code", "Description", "IsActive", "CreatedOn" },
                values: new object[,]
                {
                    { 1, "NONE", "No formal technician time tracking workflow", true, seedTime },
                    { 2, "DISPATCHARRIVECOMPLETE", "Tracks dispatch, arrival, and completion timestamps", true, seedTime },
                    { 3, "CHECKINCHECKOUT", "Technician manually checks in and checks out", true, seedTime }
                });

            migrationBuilder.InsertData(
                schema: "fgs",
                table: "FSGSetupAccountingIntegrationType",
                columns: new[] { "Id", "Code", "Name", "IsActive", "CreatedOn" },
                values: new object[,]
                {
                    { 1, "NONE", "No Accounting", true, seedTime },
                    { 2, "QUICKBOOKSONLINE", "QuickBooks Online", true, seedTime },
                    { 3, "SAGEINTACCT", "Sage Intacct", true, seedTime }
                });

            migrationBuilder.InsertData(
                schema: "fgs",
                table: "FSGSetupLanguage",
                columns: new[] { "Id", "Code", "Name", "CultureCode", "IsDefault", "SortOrder", "IsActive", "CreatedOn" },
                values: new object[,]
                {
                    { 1, "EN", "English", "en-US", true, 1, true, seedTime },
                    { 2, "ES", "Spanish", "es-US", false, 2, true, seedTime },
                    { 3, "FR", "French", "fr-FR", false, 3, true, seedTime }
                });

            migrationBuilder.InsertData(
                schema: "fgs",
                table: "FSGSetupMasterEntityType",
                columns: new[] { "Id", "Code", "Name", "IsActive", "CreatedOn" },
                values: new object[,]
                {
                    { 1, "TENANT", "TENANT", true, seedTime },
                    { 2, "COMPANY", "COMPANY", true, seedTime },
                    { 3, "SERVICELOCATION", "SERVICELOCATION", true, seedTime },
                    { 4, "BILLTO", "BILLTO", true, seedTime },
                    { 5, "VENDOR", "VENDOR", true, seedTime },
                    { 6, "SUBCONTRACTOR", "SUBCONTRACTOR", true, seedTime },
                    { 7, "LEAD", "LEAD", true, seedTime },
                    { 8, "PROPOSAL", "PROPOSAL", true, seedTime },
                    { 9, "CUSTOMER", "CUSTOMER", true, seedTime },
                    { 10, "WORKORDER", "WORKORDER", true, seedTime },
                    { 11, "INVOICE", "INVOICE", true, seedTime }
                });

            migrationBuilder.InsertData(
                schema: "fgs",
                table: "FSGSetupLocationType",
                columns: new[] { "Id", "Code", "Name", "IsActive", "CreatedOn" },
                values: new object[,]
                {
                    { 1, "BILLING", "BILLING", true, seedTime },
                    { 2, "SHIPPING", "SHIPPING", true, seedTime },
                    { 3, "PHYSICAL", "PHYSICAL", true, seedTime },
                    { 4, "SERVICE", "SERVICE", true, seedTime },
                    { 5, "MAILING", "MAILING", true, seedTime },
                    { 6, "HQ", "HQ", true, seedTime },
                    { 7, "REMITTO", "REMITTO", true, seedTime },
                    { 8, "JOBSITE", "JOBSITE", true, seedTime }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthIdentity",
                schema: "fgs");

            migrationBuilder.DropTable(
                name: "FSGSetupAccountingIntegrationType",
                schema: "fgs");

            migrationBuilder.DropTable(
                name: "FSGSetupLanguage",
                schema: "fgs");

            migrationBuilder.DropTable(
                name: "FSGSetupLocationType",
                schema: "fgs");

            migrationBuilder.DropTable(
                name: "FSGSetupMasterEntityType",
                schema: "fgs");

            migrationBuilder.DropTable(
                name: "FSGSetupTimeCardOption",
                schema: "fgs");

            migrationBuilder.DropTable(
                name: "Invite",
                schema: "fgs");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "fgs");

            migrationBuilder.DropTable(
                name: "Company",
                schema: "fgs");

            migrationBuilder.DropTable(
                name: "FSGSetupBusinessType",
                schema: "fgs");

            migrationBuilder.DropTable(
                name: "Tenant",
                schema: "fgs");
        }
    }
}
