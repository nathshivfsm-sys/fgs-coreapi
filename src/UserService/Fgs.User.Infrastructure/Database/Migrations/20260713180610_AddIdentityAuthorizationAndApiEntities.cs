using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentityAuthorizationAndApiEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FgsUserRole_GloRoleId",
                schema: "identity",
                table: "FgsUserRole");

            migrationBuilder.DropIndex(
                name: "IX_FgsUserRole_UserId_FgsRoleId",
                schema: "identity",
                table: "FgsUserRole");

            migrationBuilder.DropIndex(
                name: "IX_FgsUserRole_UserId_GloRoleId",
                schema: "identity",
                table: "FgsUserRole");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FgsUserRole_OnlyOneRole",
                schema: "identity",
                table: "FgsUserRole");

            migrationBuilder.DropColumn(
                name: "GloRoleId",
                schema: "identity",
                table: "FgsUserRole");

            migrationBuilder.DropColumn(
                name: "GloRoleId",
                schema: "identity",
                table: "FgsRole");

            migrationBuilder.AlterTable(
                name: "FgsUserRole",
                schema: "identity",
                comment: "Assigns one or more security roles to users within a company.");

            migrationBuilder.AlterTable(
                name: "FgsRole",
                schema: "identity",
                comment: "Stores built-in platform roles and company-defined custom roles used by the authorization system.");

            migrationBuilder.AlterColumn<long>(
                name: "FgsRoleId",
                schema: "identity",
                table: "FgsUserRole",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "identity",
                table: "FgsUserRole",
                type: "timestamptz",
                nullable: false,
                comment: "Date and time the role assignment was created.",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "identity",
                table: "FgsUserRole",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                comment: "User or system that assigned the role.");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "identity",
                table: "FgsRole",
                type: "timestamptz",
                nullable: true,
                comment: "Date and time the role was last modified.",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RoleCode",
                schema: "identity",
                table: "FgsRole",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                comment: "Unique system identifier for the role. Used internally by the application and should not be editable after creation.",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "identity",
                table: "FgsRole",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                comment: "Display name shown to administrators and users.",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "identity",
                table: "FgsRole",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                comment: "Indicates whether the role is available for assignment. Built-in roles should always remain active.",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "identity",
                table: "FgsRole",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                comment: "Optional description explaining the purpose of the role.",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "identity",
                table: "FgsRole",
                type: "timestamptz",
                nullable: false,
                comment: "Date and time the role was created.",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz");

            migrationBuilder.AddColumn<short>(
                name: "DisplayOrder",
                schema: "identity",
                table: "FgsRole",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1,
                comment: "Controls the display order of roles within the user interface.");

            migrationBuilder.AddColumn<bool>(
                name: "IsBuiltIn",
                schema: "identity",
                table: "FgsRole",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "Indicates whether the role is provided by the platform. Built-in roles cannot be edited, deleted, or deactivated but may be cloned.");

            migrationBuilder.AddColumn<long>(
                name: "ParentRoleId",
                schema: "identity",
                table: "FgsRole",
                type: "bigint",
                nullable: true,
                comment: "Original role from which this role was cloned. NULL for built-in roles or roles created from scratch.");

            migrationBuilder.CreateTable(
                name: "FgsApiClient",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()", comment: "Public client identifier used by external applications during authentication."),
                    ApplicationName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Display name of the application registered by the customer."),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Optional description explaining the purpose of the application."),
                    ContactName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Primary contact responsible for the application."),
                    ContactEmail = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true, comment: "Email address of the application owner or support contact."),
                    RateLimitPerMinute = table.Column<int>(type: "integer", nullable: false, defaultValue: 60, comment: "Maximum number of API requests permitted per minute for this application."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the application is permitted to authenticate and access the API.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsApiClient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsApiClient_FgsTenantCompanyCache",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "identity",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores developer applications created by tenant administrators for third-party integrations. Represents an application, not a credential.");

            migrationBuilder.CreateTable(
                name: "FgsApiEvent",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Unique event identifier exposed through the public API. Example: workorder.completed."),
                    EventCategory = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Logical category used to organize events, such as WorkOrder, Estimate, Invoice, Customer or Payment."),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Display name of the API event."),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Description of when the event is published."),
                    EventVersion = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Version number of the public event contract. Used to support backward-compatible changes to webhook payloads and API event schemas."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls the display order within the Developer Portal."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the event is available for webhook subscriptions."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, comment: "Date and time the API event was created.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsApiEvent", x => x.Id);
                },
                comment: "Master catalog of public API events that external applications may subscribe to through webhooks.");

            migrationBuilder.CreateTable(
                name: "FgsApiWebhook",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Display name of the webhook endpoint."),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Optional description explaining the purpose of the webhook endpoint."),
                    EndpointUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, comment: "HTTPS endpoint that receives webhook event notifications."),
                    AuthenticationType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, comment: "Authentication method used when invoking the webhook endpoint, such as None, BearerToken, BasicAuthentication or CustomHeader."),
                    AuthenticationValue = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Authentication value associated with the selected authentication type."),
                    Secret = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Shared secret used to sign webhook requests and verify message authenticity."),
                    TimeoutSeconds = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)30, comment: "Maximum number of seconds to wait for the webhook endpoint to respond before the request is considered failed."),
                    MaximumRetryCount = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)5, comment: "Maximum number of retry attempts after a webhook delivery failure."),
                    LastSuccessfulDeliveryOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the most recent webhook event was successfully delivered to this endpoint."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the webhook endpoint is enabled and eligible to receive events.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsApiWebhook", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsApiWebhook_FgsTenantCompanyCache",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "identity",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores webhook endpoints registered by tenant administrators for receiving API event notifications.");

            migrationBuilder.CreateTable(
                name: "FgsDataAccess",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    DataAccessCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Unique system identifier for the data access profile."),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Display name of the data access profile."),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Optional description explaining the purpose of the data access profile."),
                    IsBuiltIn = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the data access profile was provided by the platform. Built-in profiles cannot be edited but may be cloned."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls the display order within the user interface."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the data access profile is available for assignment.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsDataAccess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsDataAccess_FgsTenantCompanyCache",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "identity",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores reusable data access profiles that define the scope of data a role can access.");

            migrationBuilder.CreateTable(
                name: "FgsPermission",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PermissionCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Unique system identifier for the permission. Example: WORKORDER.CREATE."),
                    Module = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Functional module that owns the permission. Example: Work Orders, Billing, CRM."),
                    Resource = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Business resource protected by the permission. Example: WorkOrder, Invoice, Customer."),
                    Action = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Operation allowed by the permission. Example: View, Create, Edit, Delete, Approve, Dispatch."),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "User-friendly permission name displayed in the application."),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Optional description explaining the purpose of the permission."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls the display order of permissions within the user interface."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the permission is available for assignment."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, comment: "Date and time the permission was created.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsPermission", x => x.Id);
                },
                comment: "Master catalog of all permissions supported by the platform. Permissions are seeded by the application and assigned to security roles.");

            migrationBuilder.CreateTable(
                name: "FgsApiRequestLog",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    FgsApiClientId = table.Column<long>(type: "bigint", nullable: false),
                    RequestId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Unique identifier used to correlate request processing across services."),
                    Resource = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Business resource targeted by the API request, such as WorkOrder, Customer, Estimate or Invoice. Used for reporting, analytics, monitoring and rate limiting."),
                    HttpMethod = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, comment: "HTTP method used by the request, such as GET, POST, PUT or DELETE."),
                    Endpoint = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "API endpoint requested by the client."),
                    HttpStatusCode = table.Column<short>(type: "smallint", nullable: false, comment: "HTTP response status code returned to the client."),
                    DurationMilliseconds = table.Column<int>(type: "integer", nullable: false, comment: "Total request processing time in milliseconds."),
                    ClientIpAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "IP address from which the API request originated."),
                    UserAgent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "User-Agent header supplied by the client application."),
                    ErrorCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Application-specific error code returned for failed requests."),
                    ErrorMessage = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Brief error message associated with the failed request."),
                    RequestedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, comment: "Date and time the API request was received.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsApiRequestLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsApiRequestLog_FgsApiClient",
                        column: x => x.FgsApiClientId,
                        principalSchema: "identity",
                        principalTable: "FgsApiClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsApiRequestLog_FgsTenantCompanyCache",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "identity",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores API request metadata for monitoring, troubleshooting, rate limiting and analytics.");

            migrationBuilder.CreateTable(
                name: "FgsApiSecret",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    FgsApiClientId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "User-friendly name used to identify the secret, such as Production, Sandbox or July 2026 Rotation."),
                    SecretHash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, comment: "Cryptographic hash of the API secret. The original secret is never stored and cannot be recovered."),
                    ExpiresOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the secret expires. NULL indicates the secret does not expire."),
                    LastUsedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the secret was most recently used for successful authentication."),
                    RevokedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the secret was revoked."),
                    RevokedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or system that revoked the secret."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the secret is currently valid for API authentication."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, comment: "Date and time the secret was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "User or system that created the secret.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsApiSecret", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsApiSecret_FgsApiClient",
                        column: x => x.FgsApiClientId,
                        principalSchema: "identity",
                        principalTable: "FgsApiClient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsApiSecret_FgsTenantCompanyCache",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "identity",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores hashed API secrets associated with API clients. Supports secret rotation, expiration, revocation and auditing.");

            migrationBuilder.CreateTable(
                name: "FgsApiWebhookSubscription",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    FgsApiWebhookId = table.Column<long>(type: "bigint", nullable: false),
                    FgsApiEventId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, comment: "Date and time the webhook subscription was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "User or system that created the webhook subscription.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsApiWebhookSubscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsApiWebhookSubscription_FgsApiEvent",
                        column: x => x.FgsApiEventId,
                        principalSchema: "identity",
                        principalTable: "FgsApiEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsApiWebhookSubscription_FgsApiWebhook",
                        column: x => x.FgsApiWebhookId,
                        principalSchema: "identity",
                        principalTable: "FgsApiWebhook",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsApiWebhookSubscription_FgsTenantCompanyCache",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "identity",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Assigns one or more API events to webhook endpoints for event delivery.");

            migrationBuilder.CreateTable(
                name: "FgsDataAccessScope",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    FgsDataAccessId = table.Column<long>(type: "bigint", nullable: false),
                    ScopeType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Business entity used to restrict data, such as Company, BusinessUnit, Region, Warehouse, Technician or WorkOrder."),
                    Operator = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, comment: "Comparison operator used by the rule, such as ALL, IN, EQUALS, ASSIGNED_TO_CURRENT_USER or MANAGER_OF_CURRENT_USER."),
                    ScopeValue = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Comparison value used by the rule. NULL when the operator does not require a value."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls the order in which scope rules are evaluated and displayed."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, comment: "Date and time the scope rule was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "User or system that created the scope rule.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsDataAccessScope", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsDataAccessScope_FgsDataAccess",
                        column: x => x.FgsDataAccessId,
                        principalSchema: "identity",
                        principalTable: "FgsDataAccess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsDataAccessScope_FgsTenantCompanyCache",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "identity",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores one or more scope rules that define the records included in a Data Access profile.");

            migrationBuilder.CreateTable(
                name: "FgsRoleDataAccess",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    FgsRoleId = table.Column<long>(type: "bigint", nullable: false),
                    FgsDataAccessId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, comment: "Date and time the data access profile was assigned to the role."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "User or system that assigned the data access profile to the role.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsRoleDataAccess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsRoleDataAccess_FgsDataAccess",
                        column: x => x.FgsDataAccessId,
                        principalSchema: "identity",
                        principalTable: "FgsDataAccess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsRoleDataAccess_FgsRole",
                        column: x => x.FgsRoleId,
                        principalSchema: "identity",
                        principalTable: "FgsRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsRoleDataAccess_FgsTenantCompanyCache",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "identity",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Assigns one or more data access profiles to security roles within a company.");

            migrationBuilder.CreateTable(
                name: "FgsRolePermission",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    FgsRoleId = table.Column<long>(type: "bigint", nullable: false),
                    FgsPermissionId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, comment: "Date and time the permission was assigned to the role."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "User or system that assigned the permission to the role.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsRolePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsRolePermission_FgsPermission",
                        column: x => x.FgsPermissionId,
                        principalSchema: "identity",
                        principalTable: "FgsPermission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsRolePermission_FgsRole",
                        column: x => x.FgsRoleId,
                        principalSchema: "identity",
                        principalTable: "FgsRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsRolePermission_FgsTenantCompanyCache",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "identity",
                        principalTable: "FgsTenantCompanyCache",
                        principalColumns: new[] { "TenantId", "CompanyId" },
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Assigns permissions to security roles within a company.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsUserRole_TenantId_CompanyId_UserId_FgsRoleId",
                schema: "identity",
                table: "FgsUserRole",
                columns: new[] { "TenantId", "CompanyId", "UserId", "FgsRoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsRole_ParentRoleId",
                schema: "identity",
                table: "FgsRole",
                column: "ParentRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsRole_TenantId_CompanyId_IsBuiltIn",
                schema: "identity",
                table: "FgsRole",
                columns: new[] { "TenantId", "CompanyId", "IsBuiltIn" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsRole_TenantId_CompanyId_Name",
                schema: "identity",
                table: "FgsRole",
                columns: new[] { "TenantId", "CompanyId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsApiClient_TenantId_CompanyId",
                schema: "identity",
                table: "FgsApiClient",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsApiClient_ClientId",
                schema: "identity",
                table: "FgsApiClient",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_FgsApiClient_TenantId_CompanyId_ApplicationName",
                schema: "identity",
                table: "FgsApiClient",
                columns: new[] { "TenantId", "CompanyId", "ApplicationName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsApiEvent_EventCategory",
                schema: "identity",
                table: "FgsApiEvent",
                column: "EventCategory");

            migrationBuilder.CreateIndex(
                name: "IX_FgsApiEvent_EventCode",
                schema: "identity",
                table: "FgsApiEvent",
                column: "EventCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsApiRequestLog_FgsApiClientId",
                schema: "identity",
                table: "FgsApiRequestLog",
                column: "FgsApiClientId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsApiRequestLog_HttpStatusCode",
                schema: "identity",
                table: "FgsApiRequestLog",
                column: "HttpStatusCode");

            migrationBuilder.CreateIndex(
                name: "IX_FgsApiRequestLog_RequestedOn",
                schema: "identity",
                table: "FgsApiRequestLog",
                column: "RequestedOn");

            migrationBuilder.CreateIndex(
                name: "IX_FgsApiRequestLog_RequestId",
                schema: "identity",
                table: "FgsApiRequestLog",
                column: "RequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsApiRequestLog_TenantId_CompanyId",
                schema: "identity",
                table: "FgsApiRequestLog",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsApiSecret_FgsApiClientId",
                schema: "identity",
                table: "FgsApiSecret",
                column: "FgsApiClientId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsApiSecret_TenantId_CompanyId",
                schema: "identity",
                table: "FgsApiSecret",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsApiSecret_TenantId_CompanyId_Client_Name",
                schema: "identity",
                table: "FgsApiSecret",
                columns: new[] { "TenantId", "CompanyId", "FgsApiClientId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsApiWebhook_IsActive",
                schema: "identity",
                table: "FgsApiWebhook",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsApiWebhook_Name",
                schema: "identity",
                table: "FgsApiWebhook",
                columns: new[] { "TenantId", "CompanyId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsApiWebhook_TenantId_CompanyId",
                schema: "identity",
                table: "FgsApiWebhook",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsApiWebhookSubscription_FgsApiEventId",
                schema: "identity",
                table: "FgsApiWebhookSubscription",
                column: "FgsApiEventId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsApiWebhookSubscription_FgsApiWebhookId",
                schema: "identity",
                table: "FgsApiWebhookSubscription",
                column: "FgsApiWebhookId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsApiWebhookSubscription_TenantId_CompanyId",
                schema: "identity",
                table: "FgsApiWebhookSubscription",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsApiWebhookSubscription_TenantId_CompanyId_Webhook_Event",
                schema: "identity",
                table: "FgsApiWebhookSubscription",
                columns: new[] { "TenantId", "CompanyId", "FgsApiWebhookId", "FgsApiEventId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsDataAccess_TenantId_CompanyId_DataAccessCode",
                schema: "identity",
                table: "FgsDataAccess",
                columns: new[] { "TenantId", "CompanyId", "DataAccessCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsDataAccess_TenantId_CompanyId_IsBuiltIn",
                schema: "identity",
                table: "FgsDataAccess",
                columns: new[] { "TenantId", "CompanyId", "IsBuiltIn" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsDataAccess_TenantId_CompanyId_Name",
                schema: "identity",
                table: "FgsDataAccess",
                columns: new[] { "TenantId", "CompanyId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsDataAccessScope_FgsDataAccessId",
                schema: "identity",
                table: "FgsDataAccessScope",
                column: "FgsDataAccessId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsDataAccessScope_ScopeType",
                schema: "identity",
                table: "FgsDataAccessScope",
                column: "ScopeType");

            migrationBuilder.CreateIndex(
                name: "IX_FgsDataAccessScope_TenantId_CompanyId",
                schema: "identity",
                table: "FgsDataAccessScope",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPermission_Module",
                schema: "identity",
                table: "FgsPermission",
                column: "Module");

            migrationBuilder.CreateIndex(
                name: "IX_FgsPermission_Module_Resource",
                schema: "identity",
                table: "FgsPermission",
                columns: new[] { "Module", "Resource" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsPermission_PermissionCode",
                schema: "identity",
                table: "FgsPermission",
                column: "PermissionCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsPermission_Resource",
                schema: "identity",
                table: "FgsPermission",
                column: "Resource");

            migrationBuilder.CreateIndex(
                name: "IX_FgsRoleDataAccess_FgsDataAccessId",
                schema: "identity",
                table: "FgsRoleDataAccess",
                column: "FgsDataAccessId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsRoleDataAccess_FgsRoleId",
                schema: "identity",
                table: "FgsRoleDataAccess",
                column: "FgsRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsRoleDataAccess_TenantId_CompanyId",
                schema: "identity",
                table: "FgsRoleDataAccess",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsRoleDataAccess_TenantId_CompanyId_Role_DataAccess",
                schema: "identity",
                table: "FgsRoleDataAccess",
                columns: new[] { "TenantId", "CompanyId", "FgsRoleId", "FgsDataAccessId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsRolePermission_FgsPermissionId",
                schema: "identity",
                table: "FgsRolePermission",
                column: "FgsPermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsRolePermission_FgsRoleId",
                schema: "identity",
                table: "FgsRolePermission",
                column: "FgsRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsRolePermission_TenantId_CompanyId",
                schema: "identity",
                table: "FgsRolePermission",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsRolePermission_TenantId_CompanyId_Role_Permission",
                schema: "identity",
                table: "FgsRolePermission",
                columns: new[] { "TenantId", "CompanyId", "FgsRoleId", "FgsPermissionId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsRole_FgsTenantCompanyCache",
                schema: "identity",
                table: "FgsRole",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "identity",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsRole_ParentRole",
                schema: "identity",
                table: "FgsRole",
                column: "ParentRoleId",
                principalSchema: "identity",
                principalTable: "FgsRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsUser_FgsTenantCompanyCache",
                schema: "identity",
                table: "FgsUser",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "identity",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsUserRole_FgsTenantCompanyCache",
                schema: "identity",
                table: "FgsUserRole",
                columns: new[] { "TenantId", "CompanyId" },
                principalSchema: "identity",
                principalTable: "FgsTenantCompanyCache",
                principalColumns: new[] { "TenantId", "CompanyId" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FgsRole_FgsTenantCompanyCache",
                schema: "identity",
                table: "FgsRole");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsRole_ParentRole",
                schema: "identity",
                table: "FgsRole");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsUser_FgsTenantCompanyCache",
                schema: "identity",
                table: "FgsUser");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsUserRole_FgsTenantCompanyCache",
                schema: "identity",
                table: "FgsUserRole");

            migrationBuilder.DropTable(
                name: "FgsApiRequestLog",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "FgsApiSecret",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "FgsApiWebhookSubscription",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "FgsDataAccessScope",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "FgsRoleDataAccess",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "FgsRolePermission",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "FgsApiClient",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "FgsApiEvent",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "FgsApiWebhook",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "FgsDataAccess",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "FgsPermission",
                schema: "identity");

            migrationBuilder.DropIndex(
                name: "IX_FgsUserRole_TenantId_CompanyId_UserId_FgsRoleId",
                schema: "identity",
                table: "FgsUserRole");

            migrationBuilder.DropIndex(
                name: "IX_FgsRole_ParentRoleId",
                schema: "identity",
                table: "FgsRole");

            migrationBuilder.DropIndex(
                name: "IX_FgsRole_TenantId_CompanyId_IsBuiltIn",
                schema: "identity",
                table: "FgsRole");

            migrationBuilder.DropIndex(
                name: "IX_FgsRole_TenantId_CompanyId_Name",
                schema: "identity",
                table: "FgsRole");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "identity",
                table: "FgsUserRole");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                schema: "identity",
                table: "FgsRole");

            migrationBuilder.DropColumn(
                name: "IsBuiltIn",
                schema: "identity",
                table: "FgsRole");

            migrationBuilder.DropColumn(
                name: "ParentRoleId",
                schema: "identity",
                table: "FgsRole");

            migrationBuilder.AlterTable(
                name: "FgsUserRole",
                schema: "identity",
                oldComment: "Assigns one or more security roles to users within a company.");

            migrationBuilder.AlterTable(
                name: "FgsRole",
                schema: "identity",
                oldComment: "Stores built-in platform roles and company-defined custom roles used by the authorization system.");

            migrationBuilder.AlterColumn<long>(
                name: "FgsRoleId",
                schema: "identity",
                table: "FgsUserRole",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "identity",
                table: "FgsUserRole",
                type: "timestamptz",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldComment: "Date and time the role assignment was created.");

            migrationBuilder.AddColumn<short>(
                name: "GloRoleId",
                schema: "identity",
                table: "FgsUserRole",
                type: "smallint",
                nullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "identity",
                table: "FgsRole",
                type: "timestamptz",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldNullable: true,
                oldComment: "Date and time the role was last modified.");

            migrationBuilder.AlterColumn<string>(
                name: "RoleCode",
                schema: "identity",
                table: "FgsRole",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldComment: "Unique system identifier for the role. Used internally by the application and should not be editable after creation.");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "identity",
                table: "FgsRole",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldComment: "Display name shown to administrators and users.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "identity",
                table: "FgsRole",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true,
                oldComment: "Indicates whether the role is available for assignment. Built-in roles should always remain active.");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "identity",
                table: "FgsRole",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldComment: "Optional description explaining the purpose of the role.");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                schema: "identity",
                table: "FgsRole",
                type: "timestamptz",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldComment: "Date and time the role was created.");

            migrationBuilder.AddColumn<short>(
                name: "GloRoleId",
                schema: "identity",
                table: "FgsRole",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsUserRole_GloRoleId",
                schema: "identity",
                table: "FgsUserRole",
                column: "GloRoleId");

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

            migrationBuilder.AddCheckConstraint(
                name: "CK_FgsUserRole_OnlyOneRole",
                schema: "identity",
                table: "FgsUserRole",
                sql: "(\"GloRoleId\" IS NOT NULL AND \"FgsRoleId\" IS NULL) OR (\"GloRoleId\" IS NULL AND \"FgsRoleId\" IS NOT NULL)");
        }
    }
}
