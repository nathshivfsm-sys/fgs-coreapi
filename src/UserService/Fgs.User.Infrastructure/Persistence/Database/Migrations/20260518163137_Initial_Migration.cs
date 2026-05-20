using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "FgsOutboxMessage",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventType = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Payload = table.Column<string>(type: "jsonb", nullable: false),
                    IdempotencyKey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CorrelationId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    LastError = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    ProcessedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsOutboxMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsTenant",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
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
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.UniqueConstraint("AK_FgsTenantCompany_TenantId_CompanyNumber", x => new { x.TenantId, x.CompanyNumber });
                });

            migrationBuilder.CreateTable(
                name: "GloAccountingIntegrationType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloAccountingIntegrationType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloBillingCategory",
                schema: "dbo",
                columns: table => new
                {
                    BillingCategoryType = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    BillingCategoryName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloBillingCategory", x => x.BillingCategoryType);
                });

            migrationBuilder.CreateTable(
                name: "GloBusinessType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloBusinessType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloCommunicationToken",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TokenCode = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    SourceTableName = table.Column<string>(type: "text", nullable: false),
                    SourceColumnName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloCommunicationToken", x => x.Id);
                    table.UniqueConstraint("UQ_GloCommunicationToken_TokenCode", x => x.TokenCode);
                });

            migrationBuilder.CreateTable(
                name: "GloCountry",
                schema: "dbo",
                columns: table => new
                {
                    CountryCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    CountryName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CurrencyCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloCountry", x => x.CountryCode);
                });

            migrationBuilder.CreateTable(
                name: "GloCredentialCategory",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloCredentialCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloCredentialProviderType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloCredentialProviderType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloLanguage",
                schema: "dbo",
                columns: table => new
                {
                    LanguageCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    LanguageName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CultureCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloLanguage", x => x.LanguageCode);
                });

            migrationBuilder.CreateTable(
                name: "GloLocationType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloLocationType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloMasterEntityType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsDocumentAllowed = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloMasterEntityType", x => x.Id);
                    table.UniqueConstraint("UQ_GloMasterEntityType_Code", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "GloPaymentMethodType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloPaymentMethodType", x => x.Id);
                    table.UniqueConstraint("UQ_GloPaymentMethodType_Code", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "GloResolutionType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResolutionTypeCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ResolutionTypeName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloResolutionType", x => x.Id);
                    table.UniqueConstraint("UQ_GloResolutionType_Code", x => x.ResolutionTypeCode);
                });

            migrationBuilder.CreateTable(
                name: "GloRole",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    RoleLevel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsAssignable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsSystemRole = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    SortOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloRole", x => x.Id);
                    table.UniqueConstraint("UX_GloRole_RoleCode", x => x.RoleCode);
                    table.CheckConstraint("CK_GloRole_RoleCode_NotEmpty", "length(trim(\"RoleCode\")) > 0");
                    table.CheckConstraint("CK_GloRole_Name_NotEmpty", "length(trim(\"Name\")) > 0");
                });

            migrationBuilder.CreateTable(
                name: "GloSetupDescriptionType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloSetupDescriptionType", x => x.Id);
                    table.UniqueConstraint("UQ_GloSetupDescriptionType_Code", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "GloSetupLaborRateType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloSetupLaborRateType", x => x.Id);
                    table.UniqueConstraint("UQ_GloSetupLaborRateType_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "GloTimeCardOption",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloTimeCardOption", x => x.Id);
                    table.UniqueConstraint("UQ_GloTimeCardOption_Code", x => x.Code);
                    table.CheckConstraint("CK_GloTimeCardOption_Code_Upper", "\"Code\" = UPPER(\"Code\")");
                });

            migrationBuilder.CreateTable(
                name: "FgsCredentialProvider",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CredentialProviderTypeId = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Environment = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsCredentialProvider", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsCredentialProvider_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupCommunicationTemplate",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    TemplateType = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: true),
                    Body = table.Column<string>(type: "text", nullable: false),
                    IsMobileVisible = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupCommunicationTemplate", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupCommunicationTemplate", x => new { x.TenantId, x.CompanyId, x.TemplateType, x.Code });
                    table.ForeignKey(
                        name: "FK_FgsSetupCommunicationTemplate_FgsTenantCompany_TenantId_Com~",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupGLBreak",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    BreakLabel = table.Column<string>(type: "text", nullable: true),
                    BreakLevel = table.Column<int>(type: "integer", nullable: false),
                    LogoUrl = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupGLBreak", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupGLBreak", x => new { x.TenantId, x.CompanyId, x.Code });
                    table.ForeignKey(
                        name: "FK_FgsSetupGLBreak_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupPaymentTerm",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DueDateMethod = table.Column<string>(type: "text", nullable: false),
                    NumberOfDays = table.Column<int>(type: "integer", nullable: true),
                    IsAccountsReceivable = table.Column<bool>(type: "boolean", nullable: false),
                    IsAccountsPayable = table.Column<bool>(type: "boolean", nullable: false),
                    IsMobileVisible = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupPaymentTerm", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupPaymentTerm", x => new { x.TenantId, x.CompanyId, x.Name });
                    table.ForeignKey(
                        name: "FK_FgsSetupPaymentTerm_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupPriceSheet",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsLaborTierStructure = table.Column<bool>(type: "boolean", nullable: false),
                    IsLaborRateBySkillLevel = table.Column<bool>(type: "boolean", nullable: false),
                    EffectiveFrom = table.Column<DateOnly>(type: "date", nullable: false),
                    EffectiveTo = table.Column<DateOnly>(type: "date", nullable: true),
                    IsMobileVisible = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupPriceSheet", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupPricingMatrix", x => new { x.TenantId, x.CompanyId, x.Code });
                    table.ForeignKey(
                        name: "FK_FgsSetupPriceSheet_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupPriceSheetLaborTier",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    FgsSetupPriceSheetLaborId = table.Column<long>(type: "bigint", nullable: false),
                    SequenceOrder = table.Column<int>(type: "integer", nullable: false),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    Rate = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupPriceSheetLaborTier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsSetupPriceSheetLaborTier_FgsTenantCompany_TenantId_Compa~",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupPriceSheetMaterial",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    FgsSetupPriceSheetId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DefaultMarkupPercent = table.Column<decimal>(type: "numeric", nullable: true),
                    DefaultDiscountPercent = table.Column<decimal>(type: "numeric", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupPriceSheetMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsSetupPriceSheetMaterial_FgsTenantCompany_TenantId_Compan~",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupPriceSheetOther",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    FgsSetupPriceSheetId = table.Column<long>(type: "bigint", nullable: false),
                    CategoryCode = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    MarkupPercent = table.Column<decimal>(type: "numeric", nullable: true),
                    DiscountPercent = table.Column<decimal>(type: "numeric", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupPriceSheetOther", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupPricingMatrixOther", x => new { x.TenantId, x.CompanyId, x.FgsSetupPriceSheetId, x.CategoryCode });
                    table.CheckConstraint("CK_FgsSetupPricingMatrixOther_DiscountPercent", "\"DiscountPercent\" IS NULL OR (\"DiscountPercent\" >= 0 AND \"DiscountPercent\" <= 100)");
                    table.CheckConstraint("CK_FgsSetupPricingMatrixOther_MarkupPercent", "\"MarkupPercent\" IS NULL OR (\"MarkupPercent\" >= 0 AND \"MarkupPercent\" <= 100)");
                    table.ForeignKey(
                        name: "FK_FgsSetupPriceSheetOther_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupServiceAssetManufacturer",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupServiceAssetManufacturer", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupServiceAssetManufacturer", x => new { x.TenantId, x.CompanyId, x.Code });
                    table.CheckConstraint("CK_FgsSetupServiceAssetManufacturer_Code_Upper", "\"Code\" = UPPER(\"Code\")");
                    table.ForeignKey(
                        name: "FK_FgsSetupServiceAssetManufacturer_FgsTenantCompany_TenantId_~",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupServiceAssetType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupServiceAssetType", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupServiceAssetType", x => new { x.TenantId, x.CompanyId, x.Code });
                    table.CheckConstraint("CK_FgsSetupServiceAssetType_Code_Upper", "\"Code\" = UPPER(\"Code\")");
                    table.ForeignKey(
                        name: "FK_FgsSetupServiceAssetType_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupTax",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    TaxCode = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsExternalSystemRecord = table.Column<bool>(type: "boolean", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupTax", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupTax", x => new { x.TenantId, x.CompanyId, x.TaxCode });
                    table.CheckConstraint("CK_FgsSetupTax_TaxCode_Upper", "\"TaxCode\" = UPPER(\"TaxCode\")");
                    table.ForeignKey(
                        name: "FK_FgsSetupTax_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupTaxAuthority",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RegionCode = table.Column<string>(type: "text", nullable: true),
                    IsExternalSystemRecord = table.Column<bool>(type: "boolean", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupTaxAuthority", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupTaxAuthority", x => new { x.TenantId, x.CompanyId, x.Code });
                    table.CheckConstraint("CK_FgsSetupTaxAuthority_Code_Upper", "\"Code\" = UPPER(\"Code\")");
                    table.CheckConstraint("CK_FgsSetupTaxAuthority_RegionCode_Upper", "\"RegionCode\" IS NULL OR \"RegionCode\" = UPPER(\"RegionCode\")");
                    table.ForeignKey(
                        name: "FK_FgsSetupTaxAuthority_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupTechSkillLevel",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupTechSkillLevel", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupTechSkillLevel", x => new { x.TenantId, x.CompanyId, x.Code });
                    table.CheckConstraint("CK_FgsSetupTechSkillLevel_Code_Upper", "\"Code\" = UPPER(\"Code\")");
                    table.CheckConstraint("CK_FgsSetupTechSkillLevel_SortOrder", "\"SortOrder\" >= 0");
                    table.ForeignKey(
                        name: "FK_FgsSetupTechSkillLevel_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupTechTrade",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    TradeCode = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupTechTrade", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupTechTrade", x => new { x.TenantId, x.CompanyId, x.TradeCode });
                    table.CheckConstraint("CK_FgsSetupTechTrade_SortOrder", "\"SortOrder\" >= 0");
                    table.CheckConstraint("CK_FgsSetupTechTrade_TradeCode_Upper", "\"TradeCode\" = UPPER(\"TradeCode\")");
                    table.ForeignKey(
                        name: "FK_FgsSetupTechTrade_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupTitleOfCourtesy",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupTitleOfCourtesy", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupTitleOfCourtesy", x => new { x.TenantId, x.CompanyId, x.Code });
                    table.CheckConstraint("CK_FgsSetupTitleOfCourtesy_Code_Upper", "\"Code\" = UPPER(\"Code\")");
                    table.CheckConstraint("CK_FgsSetupTitleOfCourtesy_SortOrder", "\"SortOrder\" >= 0");
                    table.ForeignKey(
                        name: "FK_FgsSetupTitleOfCourtesy_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupZone",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupZone", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupZone", x => new { x.TenantId, x.CompanyId, x.Code });
                    table.CheckConstraint("CK_FgsSetupZone_Code_Upper", "\"Code\" = UPPER(\"Code\")");
                    table.ForeignKey(
                        name: "FK_FgsSetupZone_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsUser",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EntraObjectId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
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
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsUser_FgsTenant_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "dbo",
                        principalTable: "FgsTenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GloStateProvince",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CountryCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    StateProvinceCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    StateProvinceName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloStateProvince", x => x.Id);
                    table.UniqueConstraint("UQ_GloStateProvince", x => new { x.CountryCode, x.StateProvinceCode });
                    table.ForeignKey(
                        name: "FK_GloStateProvince_Country",
                        column: x => x.CountryCode,
                        principalSchema: "dbo",
                        principalTable: "GloCountry",
                        principalColumn: "CountryCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsLocation",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_FgsLocation_Company",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsLocation_MasterEntityType",
                        column: x => x.MasterEntityTypeId,
                        principalSchema: "dbo",
                        principalTable: "GloMasterEntityType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupPaymentMethod",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    GloPaymentMethodTypeId = table.Column<int>(type: "integer", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    IsMobileVisible = table.Column<bool>(type: "boolean", nullable: false),
                    IsCustomerPortalVisible = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupPaymentMethod", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupPaymentMethod", x => new { x.TenantId, x.CompanyId, x.GloPaymentMethodTypeId });
                    table.ForeignKey(
                        name: "FK_FgsSetupPaymentMethod_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsSetupPaymentMethod_GloPayType",
                        column: x => x.GloPaymentMethodTypeId,
                        principalSchema: "dbo",
                        principalTable: "GloPaymentMethodType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsResolutionCode",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    GloResolutionTypeId = table.Column<int>(type: "integer", nullable: false),
                    ResolutionCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ResolutionName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsMobileVisible = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsResolutionCode", x => x.Id);
                    table.UniqueConstraint("UQ_FgsResolutionCode_Code", x => new { x.TenantId, x.CompanyId, x.ResolutionCode });
                    table.CheckConstraint("CK_FgsResolutionCode_Code_Upper", "\"ResolutionCode\" = UPPER(\"ResolutionCode\")");
                    table.ForeignKey(
                        name: "FK_FgsResolutionCode_Company",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsResolutionCode_GloResType",
                        column: x => x.GloResolutionTypeId,
                        principalSchema: "dbo",
                        principalTable: "GloResolutionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsRole",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_FgsRole_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsRole_GloRole_GloRoleId",
                        column: x => x.GloRoleId,
                        principalSchema: "dbo",
                        principalTable: "GloRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupPriceSheetLabor",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    FgsSetupPriceSheetId = table.Column<long>(type: "bigint", nullable: false),
                    FgsSetupLaborRateTypeId = table.Column<int>(type: "integer", nullable: true),
                    FgsSetupTechSkillLevelId = table.Column<long>(type: "bigint", nullable: true),
                    RateType = table.Column<string>(type: "text", nullable: false),
                    BaseRate = table.Column<decimal>(type: "numeric", nullable: false),
                    OvertimeMultiplier = table.Column<decimal>(type: "numeric", nullable: true),
                    DoubleTimeMultiplier = table.Column<decimal>(type: "numeric", nullable: true),
                    DiscountPercent = table.Column<decimal>(type: "numeric", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupPriceSheetLabor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsSetupPriceSheetLabor_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsSetupPriceSheetLabor_GloSetupLaborRateType_FgsSetupLabor~",
                        column: x => x.FgsSetupLaborRateTypeId,
                        principalSchema: "dbo",
                        principalTable: "GloSetupLaborRateType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsTenantServiceSetup",
                schema: "dbo",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_FgsTenantServiceSetup_Company",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsTenantServiceSetup_TimeCardOption",
                        column: x => x.GloTimeCardOptionId,
                        principalSchema: "dbo",
                        principalTable: "GloTimeCardOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsCredentialProviderConfiguration",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CredentialProviderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConfigurationKey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ConfigurationValue = table.Column<string>(type: "text", nullable: true),
                    Environment = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsCredentialProviderConfiguration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsCredProvCfg_Company",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsCredProvCfg_Provider",
                        column: x => x.CredentialProviderId,
                        principalSchema: "dbo",
                        principalTable: "FgsCredentialProvider",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsCredentialSecret",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CredentialProviderId = table.Column<Guid>(type: "uuid", nullable: false),
                    SecretName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EncryptedSecretValue = table.Column<string>(type: "text", nullable: false),
                    EncryptedDek = table.Column<string>(type: "text", nullable: false),
                    EncryptionKeyId = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    VersionNo = table.Column<int>(type: "integer", nullable: false),
                    LastRotatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    ExpiresOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsCredentialSecret", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsCredentialSecret_Company",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsCredentialSecret_Provider",
                        column: x => x.CredentialProviderId,
                        principalSchema: "dbo",
                        principalTable: "FgsCredentialProvider",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupServiceAssetModelReference",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    FgsSetupServiceAssetTypeId = table.Column<long>(type: "bigint", nullable: false),
                    FgsSetupServiceAssetManufacturerId = table.Column<long>(type: "bigint", nullable: false),
                    ModelNumber = table.Column<string>(type: "text", nullable: true),
                    ModelDescription = table.Column<string>(type: "text", nullable: false),
                    SerialNumberPattern = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    UrlsJson = table.Column<string>(type: "jsonb", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupServiceAssetModelReference", x => x.Id);
                    table.CheckConstraint("CK_FgsSvcAssetModelRef_UrlsJson", "\"UrlsJson\" IS NULL OR jsonb_typeof(\"UrlsJson\") = 'array'");
                    table.ForeignKey(
                        name: "FK_FgsSvcAssetModelRef_AssetType",
                        column: x => x.FgsSetupServiceAssetTypeId,
                        principalSchema: "dbo",
                        principalTable: "FgsSetupServiceAssetType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsSvcAssetModelRef_Company",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsSvcAssetModelRef_Mfr",
                        column: x => x.FgsSetupServiceAssetManufacturerId,
                        principalSchema: "dbo",
                        principalTable: "FgsSetupServiceAssetManufacturer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupTaxDetail",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    FgsSetupTaxId = table.Column<long>(type: "bigint", nullable: false),
                    FgsSetupTaxAuthorityId = table.Column<long>(type: "bigint", nullable: false),
                    EffectiveFromDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EffectiveToDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TaxPercent = table.Column<decimal>(type: "numeric", nullable: false),
                    IsExternalSystemRecord = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupTaxDetail", x => x.Id);
                    table.CheckConstraint("CK_FgsSetupTaxDetail_EffectiveDates", "\"EffectiveToDate\" IS NULL OR \"EffectiveToDate\" >= \"EffectiveFromDate\"");
                    table.CheckConstraint("CK_FgsSetupTaxDetail_TaxPercent", "\"TaxPercent\" >= 0 AND \"TaxPercent\" <= 100");
                    table.ForeignKey(
                        name: "FK_FgsSetupTaxDetail_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsSetupTaxDetail_Tax",
                        column: x => x.FgsSetupTaxId,
                        principalSchema: "dbo",
                        principalTable: "FgsSetupTax",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsSetupTaxDetail_TaxAuth",
                        column: x => x.FgsSetupTaxAuthorityId,
                        principalSchema: "dbo",
                        principalTable: "FgsSetupTaxAuthority",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupDescription",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    DescriptionTypeCode = table.Column<string>(type: "text", nullable: false),
                    ShortNote = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Body = table.Column<string>(type: "text", nullable: false),
                    FgsSetupTechTradeId = table.Column<long>(type: "bigint", nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupDescription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsSetupDescription_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsSetupDescription_TechTrade",
                        column: x => x.FgsSetupTechTradeId,
                        principalSchema: "dbo",
                        principalTable: "FgsSetupTechTrade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupGLBreakTechTrade",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    FgsSetupGLBreakId = table.Column<long>(type: "bigint", nullable: false),
                    FgsSetupTechTradeId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupGLBreakTechTrade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsSetupGLBreakTechTrade_FgsSetupGLBreak_FgsSetupGLBreakId",
                        column: x => x.FgsSetupGLBreakId,
                        principalSchema: "dbo",
                        principalTable: "FgsSetupGLBreak",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsSetupGLBreakTechTrade_FgsSetupTechTrade_FgsSetupTechTrad~",
                        column: x => x.FgsSetupTechTradeId,
                        principalSchema: "dbo",
                        principalTable: "FgsSetupTechTrade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsSetupGLBreakTechTrade_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupPostalCode",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    PostalCode = table.Column<string>(type: "text", nullable: false),
                    FgsSetupZoneId = table.Column<long>(type: "bigint", nullable: true),
                    FgsSetupTaxId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupPostalCode", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupPostalCode", x => new { x.TenantId, x.CompanyId, x.PostalCode });
                    table.ForeignKey(
                        name: "FK_FgsSetupPostalCode_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsSetupPostalCode_Tax",
                        column: x => x.FgsSetupTaxId,
                        principalSchema: "dbo",
                        principalTable: "FgsSetupTax",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsSetupPostalCode_Zone",
                        column: x => x.FgsSetupZoneId,
                        principalSchema: "dbo",
                        principalTable: "FgsSetupZone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupTimeSlot",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    FgsSetupZoneId = table.Column<long>(type: "bigint", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    BeginTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    MarkTechArrivedLateAfter = table.Column<TimeSpan>(type: "interval", nullable: true),
                    MarkWorkOrderDelayedCompletionAfter = table.Column<TimeSpan>(type: "interval", nullable: true),
                    IsMobileVisible = table.Column<bool>(type: "boolean", nullable: false),
                    IsCustomerPortalVisible = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupTimeSlot", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupTimeSlot", x => new { x.TenantId, x.CompanyId, x.Code });
                    table.CheckConstraint("CK_FgsSetupTimeSlot_Code_Upper", "\"Code\" = UPPER(\"Code\")");
                    table.CheckConstraint("CK_FgsSetupTimeSlot_TimeRange", "\"EndTime\" > \"BeginTime\"");
                    table.ForeignKey(
                        name: "FK_FgsSetupTimeSlot_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsSetupTimeSlot_Zone",
                        column: x => x.FgsSetupZoneId,
                        principalSchema: "dbo",
                        principalTable: "FgsSetupZone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsInvitation",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
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
                        principalSchema: "dbo",
                        principalTable: "FgsUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsUserRole",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
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
                        principalSchema: "dbo",
                        principalTable: "FgsRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsUserRole_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsUserRole_FgsUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "FgsUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsUserRole_GloRole_GloRoleId",
                        column: x => x.GloRoleId,
                        principalSchema: "dbo",
                        principalTable: "GloRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsCredentialAudit",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CredentialSecretId = table.Column<Guid>(type: "uuid", nullable: false),
                    ActionType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Remarks = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    OldVersionNo = table.Column<int>(type: "integer", nullable: true),
                    NewVersionNo = table.Column<int>(type: "integer", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsCredentialAudit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsCredentialAudit_Company",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsCredentialAudit_CredentialSecret",
                        column: x => x.CredentialSecretId,
                        principalSchema: "dbo",
                        principalTable: "FgsCredentialSecret",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialAudit_CredentialSecretId",
                schema: "dbo",
                table: "FgsCredentialAudit",
                column: "CredentialSecretId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialAudit_Tenant_Company",
                schema: "dbo",
                table: "FgsCredentialAudit",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialAudit_Tenant_Company_Cred",
                schema: "dbo",
                table: "FgsCredentialAudit",
                columns: new[] { "TenantId", "CompanyId", "CredentialSecretId" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsCredentialAudit",
                schema: "dbo",
                table: "FgsCredentialAudit",
                columns: new[] { "TenantId", "CompanyId", "CredentialSecretId", "ActionType", "NewVersionNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialProvider_TenantId_CompanyId_Code",
                schema: "dbo",
                table: "FgsCredentialProvider",
                columns: new[] { "TenantId", "CompanyId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialProviderConfiguration_CredentialProviderId",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration",
                column: "CredentialProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredProvCfg_Tenant_Company",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredProvCfg_Tenant_Company_Prov",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration",
                columns: new[] { "TenantId", "CompanyId", "CredentialProviderId" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsCredentialProviderConfiguration",
                schema: "dbo",
                table: "FgsCredentialProviderConfiguration",
                columns: new[] { "TenantId", "CompanyId", "CredentialProviderId", "ConfigurationKey", "Environment" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialSecret_CredentialProviderId",
                schema: "dbo",
                table: "FgsCredentialSecret",
                column: "CredentialProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialSecret_IsActive",
                schema: "dbo",
                table: "FgsCredentialSecret",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialSecret_Tenant_Company",
                schema: "dbo",
                table: "FgsCredentialSecret",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialSecret_Tenant_Company_Prov",
                schema: "dbo",
                table: "FgsCredentialSecret",
                columns: new[] { "TenantId", "CompanyId", "CredentialProviderId" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsCredentialSecret",
                schema: "dbo",
                table: "FgsCredentialSecret",
                columns: new[] { "TenantId", "CompanyId", "CredentialProviderId", "SecretName", "VersionNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvitation_TenantId_Email_Status",
                schema: "dbo",
                table: "FgsInvitation",
                columns: new[] { "TenantId", "Email", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvitation_TokenHash",
                schema: "dbo",
                table: "FgsInvitation",
                column: "TokenHash");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInvitation_UserId",
                schema: "dbo",
                table: "FgsInvitation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsLocation_MasterEntityTypeId",
                schema: "dbo",
                table: "FgsLocation",
                column: "MasterEntityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsLocation_Tenant_Company_Entity",
                schema: "dbo",
                table: "FgsLocation",
                columns: new[] { "TenantId", "CompanyId", "MasterEntityTypeId", "EntityNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsOutboxMessage_IdempotencyKey",
                schema: "dbo",
                table: "FgsOutboxMessage",
                column: "IdempotencyKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsOutboxMessage_Status_CreatedOn",
                schema: "dbo",
                table: "FgsOutboxMessage",
                columns: new[] { "Status", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsResolutionCode_GloResolutionTypeId",
                schema: "dbo",
                table: "FgsResolutionCode",
                column: "GloResolutionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsResolutionCode_GloResType",
                schema: "dbo",
                table: "FgsResolutionCode",
                columns: new[] { "TenantId", "CompanyId", "GloResolutionTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsRole_GloRoleId",
                schema: "dbo",
                table: "FgsRole",
                column: "GloRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsRole_TenantId_CompanyId_RoleCode",
                schema: "dbo",
                table: "FgsRole",
                columns: new[] { "TenantId", "CompanyId", "RoleCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupCommunicationTemplate_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupCommunicationTemplate",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupDescription_TechTrade",
                schema: "dbo",
                table: "FgsSetupDescription",
                column: "FgsSetupTechTradeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupDescription_Tenant_Company_Type",
                schema: "dbo",
                table: "FgsSetupDescription",
                columns: new[] { "TenantId", "CompanyId", "DescriptionTypeCode" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupDescription_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupDescription",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupGLBreak_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupGLBreakTechTrade_FgsSetupGLBreakId_FgsSetupTechTrad~",
                schema: "dbo",
                table: "FgsSetupGLBreakTechTrade",
                columns: new[] { "FgsSetupGLBreakId", "FgsSetupTechTradeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupGLBreakTechTrade_FgsSetupTechTradeId",
                schema: "dbo",
                table: "FgsSetupGLBreakTechTrade",
                column: "FgsSetupTechTradeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupGLBreakTechTrade_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupGLBreakTechTrade",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPaymentMethod_GloPaymentMethodTypeId",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                column: "GloPaymentMethodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPaymentMethod_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPaymentTerm_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupPaymentTerm",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPostalCode_TaxId",
                schema: "dbo",
                table: "FgsSetupPostalCode",
                column: "FgsSetupTaxId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPostalCode_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupPostalCode",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPostalCode_ZoneId",
                schema: "dbo",
                table: "FgsSetupPostalCode",
                column: "FgsSetupZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPriceSheet_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheet",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPriceSheetLabor_FgsSetupLaborRateTypeId",
                schema: "dbo",
                table: "FgsSetupPriceSheetLabor",
                column: "FgsSetupLaborRateTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPriceSheetLabor_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheetLabor",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPriceSheetLaborTier_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheetLaborTier",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPriceSheetMaterial_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheetMaterial",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPriceSheetOther_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheetOther",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupServiceAssetManufacturer_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupServiceAssetManufacturer",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSvcAssetModelRef_Mfr",
                schema: "dbo",
                table: "FgsSetupServiceAssetModelReference",
                columns: new[] { "TenantId", "CompanyId", "FgsSetupServiceAssetManufacturerId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSvcAssetModelRef_MfrId",
                schema: "dbo",
                table: "FgsSetupServiceAssetModelReference",
                column: "FgsSetupServiceAssetManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSvcAssetModelRef_Type",
                schema: "dbo",
                table: "FgsSetupServiceAssetModelReference",
                columns: new[] { "TenantId", "CompanyId", "FgsSetupServiceAssetTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSvcAssetModelRef_TypeId",
                schema: "dbo",
                table: "FgsSetupServiceAssetModelReference",
                column: "FgsSetupServiceAssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSvcAssetModelRef_TypeMfr",
                schema: "dbo",
                table: "FgsSetupServiceAssetModelReference",
                columns: new[] { "TenantId", "CompanyId", "FgsSetupServiceAssetTypeId", "FgsSetupServiceAssetManufacturerId" });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAsset_UrlsJson",
                schema: "dbo",
                table: "FgsSetupServiceAssetModelReference",
                column: "UrlsJson")
                .Annotation("Npgsql:IndexMethod", "gin");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupServiceAssetType_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupServiceAssetType",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTax_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupTax",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTaxAuthority_RegionCode",
                schema: "dbo",
                table: "FgsSetupTaxAuthority",
                columns: new[] { "TenantId", "CompanyId", "RegionCode" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTaxAuthority_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupTaxAuthority",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTaxDetail_EffectiveDates",
                schema: "dbo",
                table: "FgsSetupTaxDetail",
                columns: new[] { "EffectiveFromDate", "EffectiveToDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTaxDetail_Tax",
                schema: "dbo",
                table: "FgsSetupTaxDetail",
                columns: new[] { "TenantId", "CompanyId", "FgsSetupTaxId", "EffectiveFromDate", "EffectiveToDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTaxDetail_TaxAuth",
                schema: "dbo",
                table: "FgsSetupTaxDetail",
                columns: new[] { "TenantId", "CompanyId", "FgsSetupTaxAuthorityId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTaxDetail_TaxAuthId",
                schema: "dbo",
                table: "FgsSetupTaxDetail",
                column: "FgsSetupTaxAuthorityId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTaxDetail_TaxId",
                schema: "dbo",
                table: "FgsSetupTaxDetail",
                column: "FgsSetupTaxId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTaxDetail_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupTaxDetail",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTechSkillLevel_SortOrder",
                schema: "dbo",
                table: "FgsSetupTechSkillLevel",
                columns: new[] { "TenantId", "CompanyId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTechSkillLevel_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupTechSkillLevel",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTechTrade_SortOrder",
                schema: "dbo",
                table: "FgsSetupTechTrade",
                columns: new[] { "TenantId", "CompanyId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTechTrade_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupTechTrade",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTimeSlot_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupTimeSlot",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTimeSlot_Zone",
                schema: "dbo",
                table: "FgsSetupTimeSlot",
                columns: new[] { "TenantId", "CompanyId", "FgsSetupZoneId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTimeSlot_ZoneId",
                schema: "dbo",
                table: "FgsSetupTimeSlot",
                column: "FgsSetupZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTitleOfCourtesy_SortOrder",
                schema: "dbo",
                table: "FgsSetupTitleOfCourtesy",
                columns: new[] { "TenantId", "CompanyId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTitleOfCourtesy_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupTitleOfCourtesy",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupZone_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupZone",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsTenant_TenantCode",
                schema: "dbo",
                table: "FgsTenant",
                column: "TenantCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsTenantCompany_TenantId_Code",
                schema: "dbo",
                table: "FgsTenantCompany",
                columns: new[] { "TenantId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsTenantCompany_TenantId_CompanyNumber",
                schema: "dbo",
                table: "FgsTenantCompany",
                columns: new[] { "TenantId", "CompanyNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsTenantServiceSetup_GloTimeCardOptionId",
                schema: "dbo",
                table: "FgsTenantServiceSetup",
                column: "GloTimeCardOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsUser_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsUser",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsUser_TenantId_Email",
                schema: "dbo",
                table: "FgsUser",
                columns: new[] { "TenantId", "Email" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_FgsUserRole_FgsRoleId",
                schema: "dbo",
                table: "FgsUserRole",
                column: "FgsRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsUserRole_GloRoleId",
                schema: "dbo",
                table: "FgsUserRole",
                column: "GloRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsUserRole_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsUserRole",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsUserRole_UserId",
                schema: "dbo",
                table: "FgsUserRole",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsUserRole_UserId_FgsRoleId",
                schema: "dbo",
                table: "FgsUserRole",
                columns: new[] { "UserId", "FgsRoleId" },
                unique: true,
                filter: "\"FgsRoleId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FgsUserRole_UserId_GloRoleId",
                schema: "dbo",
                table: "FgsUserRole",
                columns: new[] { "UserId", "GloRoleId" },
                unique: true,
                filter: "\"GloRoleId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GloAccountingIntegrationType_Code",
                schema: "dbo",
                table: "GloAccountingIntegrationType",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloBusinessType_Code",
                schema: "dbo",
                table: "GloBusinessType",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloCredentialCategory_Code",
                schema: "dbo",
                table: "GloCredentialCategory",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloCredentialProviderType_Code",
                schema: "dbo",
                table: "GloCredentialProviderType",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloLocationType_Code",
                schema: "dbo",
                table: "GloLocationType",
                column: "Code",
                unique: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsCredentialAudit",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsCredentialProviderConfiguration",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsInvitation",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsLocation",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsOutboxMessage",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsResolutionCode",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupCommunicationTemplate",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupDescription",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupGLBreakTechTrade",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupPaymentMethod",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupPaymentTerm",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupPostalCode",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupPriceSheet",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupPriceSheetLabor",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupPriceSheetLaborTier",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupPriceSheetMaterial",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupPriceSheetOther",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupServiceAssetModelReference",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupTaxDetail",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupTechSkillLevel",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupTimeSlot",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupTitleOfCourtesy",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsTenantServiceSetup",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsUserRole",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloAccountingIntegrationType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloBillingCategory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloBusinessType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloCommunicationToken",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloCredentialCategory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloCredentialProviderType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloLanguage",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloLocationType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloSetupDescriptionType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloStateProvince",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsCredentialSecret",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloMasterEntityType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloResolutionType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupGLBreak",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupTechTrade",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloPaymentMethodType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloSetupLaborRateType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupServiceAssetType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupServiceAssetManufacturer",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupTax",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupTaxAuthority",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupZone",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloTimeCardOption",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsRole",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsUser",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloCountry",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsCredentialProvider",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloRole",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsTenant",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsTenantCompany",
                schema: "dbo");
        }
    }
}
