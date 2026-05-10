using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "FgsCredentialAudit",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CredentialSecretId = table.Column<Guid>(type: "uuid", nullable: false),
                    ActionType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Remarks = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    OldVersionNo = table.Column<int>(type: "integer", nullable: true),
                    NewVersionNo = table.Column<int>(type: "integer", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsCredentialAudit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsCredentialProvider",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    CompanyId = table.Column<long>(type: "bigint", nullable: true),
                    CredentialProviderTypeId = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Environment = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsCredentialProvider", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsCredentialProviderConfiguration",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CredentialProviderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConfigurationKey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ConfigurationValue = table.Column<string>(type: "text", nullable: true),
                    Environment = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsCredentialProviderConfiguration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsCredentialSecret",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CredentialProviderId = table.Column<Guid>(type: "uuid", nullable: false),
                    VaultProvider = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SecretName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SecretArn = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    RegionName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    KmsKeyArn = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    RotationEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    VersionNo = table.Column<int>(type: "integer", nullable: false),
                    RotatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    LastValidatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    Remarks = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsCredentialSecret", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsLocation",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityTypeId = table.Column<int>(type: "integer", nullable: false),
                    EntityNumber = table.Column<long>(type: "bigint", nullable: true),
                    AddressLine1 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AddressLine2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
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
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsLocation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupCommunicationTemplate",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TemplateType = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: true),
                    Body = table.Column<string>(type: "text", nullable: false),
                    IsMobileVisible = table.Column<bool>(type: "boolean", nullable: false),
                    GloMasterEntityTypeId = table.Column<int>(type: "integer", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupCommunicationTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupCommunicationToken",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TokenCode = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SampleValue = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupCommunicationToken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupDescription",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DescriptionTypeCode = table.Column<string>(type: "text", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    FgsSetupTechTradeId = table.Column<long>(type: "bigint", nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupDescription", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupGLBreak",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    BreakLevel = table.Column<int>(type: "integer", nullable: false),
                    FgsSetupTechTradeId = table.Column<long>(type: "bigint", nullable: true),
                    LogoLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupGLBreak", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupPaymentMethod",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PaymentMethodType = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    IsMobileVisible = table.Column<bool>(type: "boolean", nullable: false),
                    IsCustomerPortalVisible = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupPaymentMethod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupPaymentTerm",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DueDateMethod = table.Column<string>(type: "text", nullable: false),
                    NumberOfDays = table.Column<int>(type: "integer", nullable: true),
                    IsAccountsReceivable = table.Column<bool>(type: "boolean", nullable: false),
                    IsAccountsPayable = table.Column<bool>(type: "boolean", nullable: false),
                    IsMobileVisible = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupPaymentTerm", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupPostalCode",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PostalCode = table.Column<string>(type: "text", nullable: false),
                    FgsSetupZoneId = table.Column<long>(type: "bigint", nullable: true),
                    FgsSetupTaxId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupPostalCode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupPriceSheet",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    EffectiveFrom = table.Column<DateOnly>(type: "date", nullable: false),
                    EffectiveTo = table.Column<DateOnly>(type: "date", nullable: true),
                    IsMobileVisible = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupPriceSheet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupPriceSheetLabor",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FgsSetupPriceSheetId = table.Column<long>(type: "bigint", nullable: false),
                    FgsSetupTechSkillLevelId = table.Column<long>(type: "bigint", nullable: true),
                    RateType = table.Column<string>(type: "text", nullable: false),
                    BaseRate = table.Column<decimal>(type: "numeric", nullable: false),
                    OvertimeMultiplier = table.Column<decimal>(type: "numeric", nullable: true),
                    DoubleTimeMultiplier = table.Column<decimal>(type: "numeric", nullable: true),
                    DiscountPercent = table.Column<decimal>(type: "numeric", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupPriceSheetLabor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupPriceSheetLaborTier",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FgsSetupPriceSheetLaborId = table.Column<long>(type: "bigint", nullable: false),
                    SequenceOrder = table.Column<int>(type: "integer", nullable: false),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    Rate = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupPriceSheetLaborTier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupPriceSheetMaterial",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FgsSetupPriceSheetId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DefaultMarkupPercent = table.Column<decimal>(type: "numeric", nullable: true),
                    DefaultDiscountPercent = table.Column<decimal>(type: "numeric", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupPriceSheetMaterial", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupPriceSheetMaterialRange",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FgsSetupPriceSheetMaterialId = table.Column<long>(type: "bigint", nullable: false),
                    CostFrom = table.Column<decimal>(type: "numeric", nullable: false),
                    CostTo = table.Column<decimal>(type: "numeric", nullable: false),
                    MarkupPercent = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupPriceSheetMaterialRange", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupPriceSheetOther",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FgsSetupPriceSheetId = table.Column<long>(type: "bigint", nullable: false),
                    CategoryCode = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    MarkupPercent = table.Column<decimal>(type: "numeric", nullable: true),
                    DiscountPercent = table.Column<decimal>(type: "numeric", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupPriceSheetOther", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupServiceAssetManufacturer",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupServiceAssetManufacturer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupServiceAssetMedia",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FgsSetupServiceAssetTypeId = table.Column<long>(type: "bigint", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    MediaUrl = table.Column<string>(type: "text", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupServiceAssetMedia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupServiceAssetModelSerialDescription",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FgsSetupServiceAssetManufacturerId = table.Column<long>(type: "bigint", nullable: true),
                    ModelDescription = table.Column<string>(type: "text", nullable: false),
                    SerialNumberPattern = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupServiceAssetModelSerialDescription", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupServiceAssetType",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupServiceAssetType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupTax",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TaxCode = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupTax", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupTaxAuthority",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RegionCode = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupTaxAuthority", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupTaxDetail",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FgsSetupTaxId = table.Column<long>(type: "bigint", nullable: false),
                    FgsSetupTaxAuthorityId = table.Column<long>(type: "bigint", nullable: false),
                    EffectiveFromDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EffectiveToDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TaxPercent = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupTaxDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupTechSkillLevel",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupTechSkillLevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupTechTrade",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TradeCode = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupTechTrade", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupTimeSlot",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    BeginTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    MarkTechArrivedLateAfter = table.Column<TimeSpan>(type: "interval", nullable: true),
                    MarkWorkOrderDelayedCompletionAfter = table.Column<TimeSpan>(type: "interval", nullable: true),
                    IsMobileVisible = table.Column<bool>(type: "boolean", nullable: false),
                    IsCustomerPortalVisible = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupTimeSlot", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupTitleOfCourtesy",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupTitleOfCourtesy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupZone",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupZone", x => x.Id);
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
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
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
                    PhysicalLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    BillingLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    FullLogoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CompactLogoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IconLogoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    FaviconUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsTenantCompany", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsTenantCompanyConfiguration",
                schema: "dbo",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    TimeCardOptionId = table.Column<int>(type: "integer", nullable: false),
                    AccountingIntegrationTypeId = table.Column<int>(type: "integer", nullable: true),
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
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsTenantCompanyConfiguration", x => new { x.TenantId, x.CompanyId });
                });

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredentialProvider_TenantId_Code",
                schema: "dbo",
                table: "FgsCredentialProvider",
                columns: new[] { "TenantId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsLocation_EntityTypeId",
                schema: "dbo",
                table: "FgsLocation",
                column: "EntityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupCommunicationTemplate_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupCommunicationTemplate",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupCommunicationToken_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupCommunicationToken",
                columns: new[] { "TenantId", "CompanyId" });

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
                name: "IX_FgsSetupPostalCode_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupPostalCode",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPriceSheet_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheet",
                columns: new[] { "TenantId", "CompanyId" });

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
                name: "IX_FgsSetupPriceSheetMaterialRange_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupPriceSheetMaterialRange",
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
                name: "IX_FgsSetupServiceAssetMedia_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupServiceAssetMedia",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupServiceAssetModelSerialDescription_TenantId_Company~",
                schema: "dbo",
                table: "FgsSetupServiceAssetModelSerialDescription",
                columns: new[] { "TenantId", "CompanyId" });

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
                name: "IX_FgsSetupTaxAuthority_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupTaxAuthority",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTaxDetail_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupTaxDetail",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTechSkillLevel_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupTechSkillLevel",
                columns: new[] { "TenantId", "CompanyId" });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsCredentialAudit",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsCredentialProvider",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsCredentialProviderConfiguration",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsCredentialSecret",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsLocation",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupCommunicationTemplate",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupCommunicationToken",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupDescription",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupGLBreak",
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
                name: "FgsSetupPriceSheetMaterialRange",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupPriceSheetOther",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupServiceAssetManufacturer",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupServiceAssetMedia",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupServiceAssetModelSerialDescription",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupServiceAssetType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupTax",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupTaxAuthority",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupTaxDetail",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupTechSkillLevel",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupTechTrade",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupTimeSlot",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupTitleOfCourtesy",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupZone",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsTenant",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsTenantCompany",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsTenantCompanyConfiguration",
                schema: "dbo");
        }
    }
}
