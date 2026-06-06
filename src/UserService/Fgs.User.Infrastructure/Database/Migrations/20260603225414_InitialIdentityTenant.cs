using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialIdentityTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "identity");

            migrationBuilder.EnsureSchema(
                name: "tenant");

            migrationBuilder.CreateTable(
                name: "FgsLocation",
                schema: "tenant",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    MasterEntityTypeId = table.Column<int>(type: "integer", nullable: false),
                    EntityNumber = table.Column<long>(type: "bigint", nullable: true),
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
                    Latitude = table.Column<decimal>(type: "numeric(18,10)", precision: 18, scale: 10, nullable: true),
                    Longitude = table.Column<decimal>(type: "numeric(18,10)", precision: 18, scale: 10, nullable: true),
                    PlaceId = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsLocation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsRole",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    RoleCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    GloRoleId = table.Column<short>(type: "smallint", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsTenant",
                schema: "tenant",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantGuid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    FgsTenantStatusId = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    TenantCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    LegalName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Email = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Website = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PhysicalLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    BillingLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    SubscriptionPlanId = table.Column<int>(type: "integer", nullable: true),
                    TimeZone = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DefaultCurrency = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DefaultLanguageId = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    StorageBucketName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsTenant", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsTenantCompany",
                schema: "tenant",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyNumber = table.Column<long>(type: "bigint", nullable: false),
                    BusinessTypeId = table.Column<int>(type: "integer", nullable: false),
                    CompanySize = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    LegalName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Email = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Website = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    TaxId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PhysicalLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    BillingLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    FullLogoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CompactLogoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IconLogoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    FaviconUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsTenantCompany", x => x.Id);
                    table.UniqueConstraint("AK_FgsTenantCompany_TenantId_CompanyGuid", x => new { x.TenantId, x.CompanyGuid });
                    table.UniqueConstraint("UX_Company_Tenant_Code", x => new { x.TenantId, x.Code });
                    table.UniqueConstraint("UX_Company_Tenant_CompanyNumber", x => new { x.TenantId, x.CompanyNumber });
                });

            migrationBuilder.CreateTable(
                name: "FgsTenantServiceSetup",
                schema: "tenant",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    GloTimeCardOptionId = table.Column<int>(type: "integer", nullable: false),
                    AccountingIntegrationTypeId = table.Column<int>(type: "integer", nullable: true),
                    UseExternalTaxCalculationProvider = table.Column<bool>(type: "boolean", nullable: false),
                    EnableCallBookingWidget = table.Column<bool>(type: "boolean", nullable: false),
                    EnablePaymentWidget = table.Column<bool>(type: "boolean", nullable: false),
                    EnableCustomerPortal = table.Column<bool>(type: "boolean", nullable: false),
                    EnableRulesManagement = table.Column<bool>(type: "boolean", nullable: false),
                    EnableAutoArrive = table.Column<bool>(type: "boolean", nullable: false),
                    WorkLocationRadiusForAutoArrive = table.Column<int>(type: "integer", nullable: true),
                    OTStartTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    OTEndTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    DTStartTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    DTEndTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    BillHoursFromDispatchOrArrive = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SourceCodeRequiredOnWorkOrder = table.Column<bool>(type: "boolean", nullable: false),
                    SourceCodeRequiredOnServiceLocation = table.Column<bool>(type: "boolean", nullable: false),
                    BillToStartNumber = table.Column<long>(type: "bigint", nullable: false),
                    POStartNumber = table.Column<long>(type: "bigint", nullable: false),
                    QuoteStartNumber = table.Column<long>(type: "bigint", nullable: false),
                    WorkOrderStartNumber = table.Column<long>(type: "bigint", nullable: false),
                    InvoiceNumberPrefix = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    QuoteNumberPrefix = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PONumberPrefix = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    WorkOrderNumberPrefix = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    InvoiceBatchNumberFormat = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsTenantServiceSetup", x => new { x.TenantId, x.CompanyId });
                    table.CheckConstraint("CK_FgsTenantServiceSetup_DTRange", "\"DTStartTime\" IS NULL OR \"DTEndTime\" IS NULL OR \"DTEndTime\" > \"DTStartTime\"");
                    table.CheckConstraint("CK_FgsTenantServiceSetup_OTRange", "\"OTStartTime\" IS NULL OR \"OTEndTime\" IS NULL OR \"OTEndTime\" > \"OTStartTime\"");
                    table.CheckConstraint("CK_FgsTenantServiceSetup_WorkLocationRadius", "\"WorkLocationRadiusForAutoArrive\" IS NULL OR \"WorkLocationRadiusForAutoArrive\" >= 0");
                });

            migrationBuilder.CreateTable(
                name: "FgsUser",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EntraObjectId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsUser_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "tenant",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsUser_FgsTenant_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "tenant",
                        principalTable: "FgsTenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsInvitation",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    TokenHash = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ExpiresAtUtc = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    AcceptedAtUtc = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInvitation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsInvitation_FgsUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "FgsUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsUserRole",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    GloRoleId = table.Column<short>(type: "smallint", nullable: true),
                    FgsRoleId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsUserRole", x => x.Id);
                    table.CheckConstraint("CK_FgsUserRole_OnlyOneRole", "(\"GloRoleId\" IS NOT NULL AND \"FgsRoleId\" IS NULL) OR (\"GloRoleId\" IS NULL AND \"FgsRoleId\" IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_FgsUserRole_FgsRole_FgsRoleId",
                        column: x => x.FgsRoleId,
                        principalSchema: "identity",
                        principalTable: "FgsRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsUserRole_FgsUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "FgsUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvitation_TenantId_Email_Status",
                schema: "identity",
                table: "FgsInvitation",
                columns: new[] { "TenantId", "Email", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvitation_TokenHash",
                schema: "identity",
                table: "FgsInvitation",
                column: "TokenHash");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvitation_UserId",
                schema: "identity",
                table: "FgsInvitation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsLocation_Tenant_Company_Entity",
                schema: "tenant",
                table: "FgsLocation",
                columns: new[] { "TenantId", "CompanyId", "MasterEntityTypeId", "EntityNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsRole_TenantId_CompanyId_RoleCode",
                schema: "identity",
                table: "FgsRole",
                columns: new[] { "TenantId", "CompanyId", "RoleCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsTenant_TenantCode",
                schema: "tenant",
                table: "FgsTenant",
                column: "TenantCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsTenant_TenantGuid",
                schema: "tenant",
                table: "FgsTenant",
                column: "TenantGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsUser_TenantId_CompanyId",
                schema: "identity",
                table: "FgsUser",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsUser_TenantId_Email",
                schema: "identity",
                table: "FgsUser",
                columns: new[] { "TenantId", "Email" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_FgsUserRole_FgsRoleId",
                schema: "identity",
                table: "FgsUserRole",
                column: "FgsRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsUserRole_GloRoleId",
                schema: "identity",
                table: "FgsUserRole",
                column: "GloRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsUserRole_TenantId_CompanyId",
                schema: "identity",
                table: "FgsUserRole",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsUserRole_UserId",
                schema: "identity",
                table: "FgsUserRole",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsUserRole_UserId_FgsRoleId",
                schema: "identity",
                table: "FgsUserRole",
                columns: new[] { "UserId", "FgsRoleId" },
                unique: true,
                filter: "\"FgsRoleId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FgsUserRole_UserId_GloRoleId",
                schema: "identity",
                table: "FgsUserRole",
                columns: new[] { "UserId", "GloRoleId" },
                unique: true,
                filter: "\"GloRoleId\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsInvitation",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "FgsLocation",
                schema: "tenant");

            migrationBuilder.DropTable(
                name: "FgsTenantServiceSetup",
                schema: "tenant");

            migrationBuilder.DropTable(
                name: "FgsUserRole",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "FgsRole",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "FgsUser",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "FgsTenantCompany",
                schema: "tenant");

            migrationBuilder.DropTable(
                name: "FgsTenant",
                schema: "tenant");
        }
    }
}
