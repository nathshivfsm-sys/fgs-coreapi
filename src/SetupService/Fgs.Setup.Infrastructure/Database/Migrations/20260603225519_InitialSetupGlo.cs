using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.Setup.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetupGlo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "setup");

            migrationBuilder.EnsureSchema(
                name: "glo");

            migrationBuilder.CreateTable(
                name: "FgsBillingCategory",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key identity of the billing category record.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier owning this billing category."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier within the tenant owning this billing category."),
                    BillingCategoryType = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false, comment: "Short billing category code such as IN, PM, SR, or other tenant-defined values."),
                    BillingCategoryName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Display name of the billing category shown throughout the application."),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Optional internal description or notes for the billing category."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls sorting/display order of billing categories in dropdowns and setup screens."),
                    IsSystemDefined = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the billing category was system seeded or manually created by the tenant/company."),
                    ShowToFieldTech = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the billing category is visible to field technicians in mobile and field service applications."),
                    AllowToPick = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Determines whether office users are allowed to manually select this billing category during estimate, invoice, or billing entry. Categories such as tax may be system controlled and not manually selectable."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the billing category record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User identifier that created the billing category record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the billing category record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User identifier that last updated the billing category record."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the billing category is active and available for use.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsBillingCategory", x => x.Id);
                    table.UniqueConstraint("UQ_FgsBillingCategory_TenantId_CompanyId_BillingCategoryType", x => new { x.TenantId, x.CompanyId, x.BillingCategoryType, x.BillingCategoryName });
                },
                comment: "Stores tenant/company specific billing categories used for invoicing, service billing, maintenance plans, and other billing operations. Seeded initially from GloBillingCategory but fully managed by each tenant/company independently.");

            migrationBuilder.CreateTable(
                name: "FgsBusinessType",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsBusinessType", x => x.Id);
                    table.UniqueConstraint("UQ_FgsBusinessType_TenantId_CompanyId_Code", x => new { x.TenantId, x.CompanyId, x.Code });
                });

            migrationBuilder.CreateTable(
                name: "FgsInventoryCategory",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CategoryCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventoryCategory", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventoryCategory_TenantId_CompanyId_CategoryCode", x => new { x.TenantId, x.CompanyId, x.CategoryCode });
                });

            migrationBuilder.CreateTable(
                name: "FgsInventoryItemType",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    ItemTypeCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    TracksQuantity = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventoryItemType", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventoryItemType_TenantId_CompanyId_ItemTypeCode", x => new { x.TenantId, x.CompanyId, x.ItemTypeCode });
                });

            migrationBuilder.CreateTable(
                name: "FgsJobTypeCategory",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CategoryCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsJobTypeCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsJobTypeSubCategory",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    SubCategoryCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsJobTypeSubCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsLeadSource",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    SourceCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SourceName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsLeadSource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupCommunicationTemplate",
                schema: "setup",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyId = table.Column<long>(type: "bigint", nullable: true),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CommunicationChannel = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    TemplateType = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: true),
                    Body = table.Column<string>(type: "text", nullable: false),
                    IsMobileVisible = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupCommunicationTemplate", x => x.Id);
                    table.CheckConstraint("CK_FgsSetupCommunicationTemplate_CommunicationChannel", "\"CommunicationChannel\" IN ('Email', 'SMS', 'PushNotification', 'SystemNotification')");
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupGLBreak",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Surrogate primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false, comment: "Unique GL break code within tenant, company, and break level scope."),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "Display name of the GL break."),
                    BreakLabel = table.Column<string>(type: "text", nullable: true, comment: "Optional label displayed in UI and financial documents."),
                    BreakLevel = table.Column<short>(type: "smallint", nullable: false, comment: "Break hierarchy level. Allowed values: 1 or 2."),
                    LogoFileId = table.Column<long>(type: "bigint", nullable: true, comment: "Optional reference to uploaded logo file in FgsFile."),
                    AddressId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Optional reference to branch or break address in FgsLocation."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, comment: "UTC timestamp when the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "UTC timestamp when the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that last updated the record."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, comment: "Indicates whether the GL break is active.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupGLBreak", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupGLBreak", x => new { x.TenantId, x.CompanyId, x.Code, x.BreakLevel });
                    table.CheckConstraint("CK_FgsSetupGLBreak_BreakLevel", "\"BreakLevel\" IN (1, 2)");
                },
                comment: "Stores GL break configuration for financial reporting segmentation by trade, division, branch, or organizational unit.");

            migrationBuilder.CreateTable(
                name: "FgsSetupLaborRateType",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupLaborRateType", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupLaborRateType_TenantId_CompanyId_Name", x => new { x.TenantId, x.CompanyId, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupPaymentMethod",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsMobileVisible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsCustomerPortalVisible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupPaymentMethod", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupPaymentMethod", x => new { x.TenantId, x.CompanyId, x.DisplayName });
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupPaymentTerm",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupPricingMatrix",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_FgsSetupPricingMatrix", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupPricingMatrix", x => new { x.TenantId, x.CompanyId, x.Code });
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupServiceAssetManufacturer",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupServiceAssetType",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupTax",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    TaxCode = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsExternalSystemRecord = table.Column<bool>(type: "boolean", nullable: false),
                    ExternalSystemId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    SyncToken = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ShowTaxDetail = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
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
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupTaxAuthority",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupTechSkillLevel",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupTechTrade",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupTitleOfCourtesy",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupZone",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "FgsTag",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    TagCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NormalizedName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    BackgroundColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    TextColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IconFileId = table.Column<long>(type: "bigint", nullable: true),
                    UsageCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsSystemGenerated = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsTag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FgsWarehouse",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    WarehouseCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Unique warehouse code within the tenant and company scope."),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Display name of the warehouse or inventory location."),
                    WarehouseType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, comment: "Type of inventory location. Allowed values: Warehouse, Truck, Trailer, JobSite, Consignment, Vendor."),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: true, comment: "Optional reference to the physical address or geo location in FgsLocation."),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Optional description or notes for the warehouse."),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether this warehouse is the default inventory location for the company."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last updated the record."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the warehouse is active and available for inventory operations.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsWarehouse", x => x.Id);
                    table.UniqueConstraint("UQ_FgsWarehouse_TenantId_CompanyId_WarehouseCode", x => new { x.TenantId, x.CompanyId, x.WarehouseCode });
                    table.CheckConstraint("CK_FgsWarehouse_WarehouseType", "\"WarehouseType\" IN ('Warehouse', 'Truck', 'Trailer', 'JobSite', 'Consignment', 'Vendor')");
                },
                comment: "Stores inventory warehouse, truck, trailer, job site, consignment, and vendor storage locations.");

            migrationBuilder.CreateTable(
                name: "GloAccountingIntegrationType",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloAccountingIntegrationType", x => x.Id);
                    table.UniqueConstraint("UX_AccountingIntegrationType_Code", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "GloBillingCategory",
                schema: "glo",
                columns: table => new
                {
                    BillingCategoryType = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false, comment: "Short billing category code (primary key), e.g. IN, LB, TX."),
                    BillingCategoryName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Display name of the billing category."),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Optional description of how the billing category is used."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls sorting/display order of billing categories in dropdowns and setup screens."),
                    ShowToFieldTech = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Determines whether field technicians can view/select this billing category in mobile and field workflows."),
                    AllowToPick = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Determines whether office users are allowed to manually select this billing category during estimate, invoice, or billing entry. Categories such as tax may be system controlled and not manually selectable.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloBillingCategory", x => x.BillingCategoryType);
                },
                comment: "Global billing line category lookup used during tenant provisioning (equipment, labor, tax, etc.).");

            migrationBuilder.CreateTable(
                name: "GloBusinessType",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloBusinessType", x => x.Id);
                    table.UniqueConstraint("UX_BusinessType_Code", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "GloCommunicationTemplate",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TemplateScope = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Tenant", comment: "Defines whether the template is system-managed or available for tenant customization."),
                    CommunicationChannel = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false, comment: "Communication delivery channel such as Email, SMS, PushNotification, or SystemNotification."),
                    TemplateCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Unique business event identifier such as INVOICE_SENT, PASSWORD_RESET, or WORKORDER_COMPLETED."),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Display name of the communication template."),
                    Subject = table.Column<string>(type: "text", nullable: true, comment: "Subject line used for communication channels that support a subject."),
                    Body = table.Column<string>(type: "text", nullable: false, comment: "Template content containing static text and communication tokens."),
                    IsMobileVisible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether the template is available within the mobile application."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Determines the display order of the template in user interfaces."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the communication template is active and available for use."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloCommunicationTemplate", x => x.Id);
                    table.CheckConstraint("CK_GloCommunicationTemplate_CommunicationChannel", "\"CommunicationChannel\" IN ('Email', 'SMS', 'PushNotification', 'SystemNotification')");
                    table.CheckConstraint("CK_GloCommunicationTemplate_TemplateScope", "\"TemplateScope\" IN ('Tenant', 'System')");
                },
                comment: "Stores FSM-provided communication templates available for system use or tenant customization.");

            migrationBuilder.CreateTable(
                name: "GloCommunicationToken",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TokenCode = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    SourceDatabaseName = table.Column<string>(type: "text", nullable: false),
                    SourceSchemaName = table.Column<string>(type: "text", nullable: false),
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
                schema: "glo",
                columns: table => new
                {
                    CountryCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    CountryName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CurrencyCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloCountry", x => x.CountryCode);
                });

            migrationBuilder.CreateTable(
                name: "GloCredentialProviderType",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    ProviderCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "System unique code used by application logic and integration services."),
                    ProviderName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "User friendly provider name displayed in setup screens."),
                    ConfigurationSchema = table.Column<string>(type: "jsonb", nullable: false, comment: "JSON schema used by the UI to dynamically render provider configuration fields and perform validation."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the provider can be selected for new credential configurations."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloCredentialProviderType", x => x.Id);
                },
                comment: "Master list of supported credential providers and integrations available within the FSM platform.");

            migrationBuilder.CreateTable(
                name: "GloInventoryItemType",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemTypeCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    TracksQuantity = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloInventoryItemType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloLanguage",
                schema: "glo",
                columns: table => new
                {
                    LanguageCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    LanguageName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CultureCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloLanguage", x => x.LanguageCode);
                });

            migrationBuilder.CreateTable(
                name: "GloLeadSource",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SourceCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SourceName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloLeadSource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloLocationType",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloLocationType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloMasterEntityType",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsDocumentAllowed = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloMasterEntityType", x => x.Id);
                    table.UniqueConstraint("UQ_GloMasterEntityType_Code", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "GloOutboxMessage",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyId = table.Column<long>(type: "bigint", nullable: true),
                    EventType = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    AggregateType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AggregateId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CausationId = table.Column<Guid>(type: "uuid", nullable: true),
                    ExchangeName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    RoutingKey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Payload = table.Column<string>(type: "jsonb", nullable: false),
                    Headers = table.Column<string>(type: "jsonb", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    RetryCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    MaxRetryCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 10),
                    NextRetryOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    ProcessedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    LastError = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloOutboxMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloPaymentMethodType",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloPaymentMethodType", x => x.Id);
                    table.UniqueConstraint("UQ_GloPaymentMethodType_Code", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "GloResolutionType",
                schema: "glo",
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
                schema: "glo",
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
                    table.CheckConstraint("CK_GloRole_Name_NotEmpty", "length(trim(\"Name\")) > 0");
                    table.CheckConstraint("CK_GloRole_RoleCode_NotEmpty", "length(trim(\"RoleCode\")) > 0");
                });

            migrationBuilder.CreateTable(
                name: "GloSeedTableMapping",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SeedCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SourceDatabaseName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    SourceSchemaName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "public"),
                    SourceTableName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    TargetDatabaseName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    TargetSchemaName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "public"),
                    TargetTableName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    SeedOrder = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloSeedTableMapping", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloSetupDescriptionType",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
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
                schema: "glo",
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
                name: "GloSetupPaymentTerm",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DueDateMethod = table.Column<string>(type: "text", nullable: false),
                    NumberOfDays = table.Column<int>(type: "integer", nullable: true),
                    IsAccountsReceivable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsAccountsPayable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsMobileVisible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloSetupPaymentTerm", x => x.Id);
                    table.UniqueConstraint("UQ_GloSetupPaymentTerm_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "GloSetupTenantStatus",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloSetupTenantStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloTag",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    TagCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NormalizedName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    BackgroundColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    TextColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IconFileId = table.Column<long>(type: "bigint", nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsSystemGenerated = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloTag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloTimeCardOption",
                schema: "glo",
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
                name: "GloTitleOfCourtesy",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloTitleOfCourtesy", x => x.Id);
                    table.CheckConstraint("CK_GloTitleOfCourtesy_Code_Upper", "\"Code\" = upper(\"Code\")");
                    table.CheckConstraint("CK_GloTitleOfCourtesy_SortOrder", "\"SortOrder\" >= 0");
                });

            migrationBuilder.CreateTable(
                name: "GloUnitOfMeasure",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UnitCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Abbreviation = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    UnitType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DecimalPlaces = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)2),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloUnitOfMeasure", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GloVehicleMaintenanceType",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaintenanceTypeCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Unique system code identifying the maintenance type."),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Display name of the maintenance type."),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Description of the maintenance type."),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1, comment: "Controls display order in lists and dropdowns."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the maintenance type is active and available for selection."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloVehicleMaintenanceType", x => x.Id);
                    table.UniqueConstraint("UQ_GloVehicleMaintenanceType_MaintenanceTypeCode", x => x.MaintenanceTypeCode);
                },
                comment: "Stores standard vehicle maintenance types used when recording maintenance activities for company vehicles.");

            migrationBuilder.CreateTable(
                name: "GloZone",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloZone", x => x.Id);
                    table.CheckConstraint("CK_GloZone_Code_Upper", "\"Code\" = upper(\"Code\")");
                });

            migrationBuilder.CreateTable(
                name: "FgsInventorySubCategory",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    InventoryCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    SubCategoryCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventorySubCategory", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventorySubCategory_TenantId_CompanyId_InventoryCategoryId_SubCategoryCode", x => new { x.TenantId, x.CompanyId, x.InventoryCategoryId, x.SubCategoryCode });
                    table.ForeignKey(
                        name: "FK_FgsInventorySubCategory_FgsInventoryCategory",
                        column: x => x.InventoryCategoryId,
                        principalSchema: "setup",
                        principalTable: "FgsInventoryCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsJobType",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    JobTypeCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    JobTypeSubCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    JobTypeCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TaskName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    UsedFor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Trade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EstimatedDurationMinutes = table.Column<int>(type: "integer", nullable: true),
                    BusinessUnit = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Priority = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)5),
                    BackgroundColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    TextColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ShowToFieldTech = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    ShowOnCustomerPortal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsJobType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsJobType_FgsJobTypeCategory",
                        column: x => x.JobTypeCategoryId,
                        principalSchema: "setup",
                        principalTable: "FgsJobTypeCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsJobType_FgsJobTypeSubCategory",
                        column: x => x.JobTypeSubCategoryId,
                        principalSchema: "setup",
                        principalTable: "FgsJobTypeSubCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupGLBreakTrade",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Surrogate primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Owning tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant-scoped company number."),
                    GLBreakId = table.Column<long>(type: "bigint", nullable: false, comment: "Reference to the associated GL break configuration."),
                    TradeCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Technician or operational trade code associated with the GL break such as HVAC, Plumbing, Electrical, or Drain."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "UTC timestamp when the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User or process that created the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupGLBreakTrade", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupGLBreakTrade", x => new { x.TenantId, x.CompanyId, x.GLBreakId, x.TradeCode });
                    table.ForeignKey(
                        name: "FK_FgsSetupGLBreakTrade_FgsSetupGLBreak_GLBreakId",
                        column: x => x.GLBreakId,
                        principalSchema: "setup",
                        principalTable: "FgsSetupGLBreak",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Stores trade-to-GL-break mappings used for financial segmentation and reporting.");

            migrationBuilder.CreateTable(
                name: "FgsVendor",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    VendorCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    LegalName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    VendorType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Allowed values: VENDOR, SUBCONTRACTOR"),
                    PaymentTermId = table.Column<long>(type: "bigint", nullable: true, comment: "References payment terms used for accounts payable due date calculation."),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    MobileNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Website = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    TaxIdentificationNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LicenseNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    InsurancePolicyNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Is1099Eligible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether vendor should be included in 1099 reporting."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsVendor", x => x.Id);
                    table.UniqueConstraint("UQ_FgsVendor_TenantId_CompanyId_VendorCode", x => new { x.TenantId, x.CompanyId, x.VendorCode });
                    table.CheckConstraint("CK_FgsVendor_VendorType", "\"VendorType\" IN ('VENDOR', 'SUBCONTRACTOR')");
                    table.ForeignKey(
                        name: "FK_FgsVendor_FgsSetupPaymentTerm",
                        column: x => x.PaymentTermId,
                        principalSchema: "setup",
                        principalTable: "FgsSetupPaymentTerm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores vendor and subcontractor master information for purchasing, AP, and subcontractor management.");

            migrationBuilder.CreateTable(
                name: "FgsSetupPricingMatrixMaterialTier",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    FgsSetupPricingMatrixId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ToCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MarkupPercent = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DiscountPercent = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupPricingMatrixMaterialTier", x => x.Id);
                    table.CheckConstraint("CK_FgsSetupPricingMatrixMaterialTier_DiscountPercent", "\"DiscountPercent\" IS NULL OR (\"DiscountPercent\" >= 0 AND \"DiscountPercent\" <= 100)");
                    table.CheckConstraint("CK_FgsSetupPricingMatrixMaterialTier_FromCost", "\"FromCost\" >= 0");
                    table.CheckConstraint("CK_FgsSetupPricingMatrixMaterialTier_MarkupPercent", "\"MarkupPercent\" >= 0");
                    table.CheckConstraint("CK_FgsSetupPricingMatrixMaterialTier_ToCost", "\"ToCost\" IS NULL OR \"ToCost\" >= \"FromCost\"");
                    table.ForeignKey(
                        name: "FK_FgsSetupPricingMatrixMaterialTier_PricingMatrix",
                        column: x => x.FgsSetupPricingMatrixId,
                        principalSchema: "setup",
                        principalTable: "FgsSetupPricingMatrix",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupPricingMatrixOther",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    FgsSetupPricingMatrixId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryCode = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    MarkupPercent = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    DiscountPercent = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupPricingMatrixOther", x => x.Id);
                    table.UniqueConstraint("UQ_FgsSetupPricingMatrixOther", x => new { x.TenantId, x.CompanyId, x.FgsSetupPricingMatrixId, x.CategoryCode });
                    table.CheckConstraint("CK_FgsSetupPricingMatrixOther_DiscountPercent", "\"DiscountPercent\" IS NULL OR (\"DiscountPercent\" >= 0 AND \"DiscountPercent\" <= 100)");
                    table.CheckConstraint("CK_FgsSetupPricingMatrixOther_MarkupPercent", "\"MarkupPercent\" IS NULL OR \"MarkupPercent\" >= 0");
                    table.ForeignKey(
                        name: "FK_FgsSetupPricingMatrixOther_PricingMatrix",
                        column: x => x.FgsSetupPricingMatrixId,
                        principalSchema: "setup",
                        principalTable: "FgsSetupPricingMatrix",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupServiceAssetModelReference",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
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
                        principalSchema: "setup",
                        principalTable: "FgsSetupServiceAssetType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsSvcAssetModelRef_Mfr",
                        column: x => x.FgsSetupServiceAssetManufacturerId,
                        principalSchema: "setup",
                        principalTable: "FgsSetupServiceAssetManufacturer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupTaxDetail",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
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
                        name: "FK_FgsSetupTaxDetail_Tax",
                        column: x => x.FgsSetupTaxId,
                        principalSchema: "setup",
                        principalTable: "FgsSetupTax",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsSetupTaxDetail_TaxAuth",
                        column: x => x.FgsSetupTaxAuthorityId,
                        principalSchema: "setup",
                        principalTable: "FgsSetupTaxAuthority",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupDescription",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
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
                        name: "FK_FgsSetupDescription_TechTrade",
                        column: x => x.FgsSetupTechTradeId,
                        principalSchema: "setup",
                        principalTable: "FgsSetupTechTrade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupPostalCode",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
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
                        name: "FK_FgsSetupPostalCode_Tax",
                        column: x => x.FgsSetupTaxId,
                        principalSchema: "setup",
                        principalTable: "FgsSetupTax",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsSetupPostalCode_Zone",
                        column: x => x.FgsSetupZoneId,
                        principalSchema: "setup",
                        principalTable: "FgsSetupZone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupTimeSlot",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
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
                        name: "FK_FgsSetupTimeSlot_Zone",
                        column: x => x.FgsSetupZoneId,
                        principalSchema: "setup",
                        principalTable: "FgsSetupZone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsVehicle",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    WarehouseId = table.Column<long>(type: "bigint", nullable: false, comment: "Associated truck warehouse used as the vehicle inventory location."),
                    OwnershipType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Owned", comment: "Indicates whether the vehicle is owned, leased, or rented."),
                    OwnershipCompany = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Name of the leasing company, rental provider, or other organization that owns the vehicle when it is not company-owned."),
                    Year = table.Column<short>(type: "smallint", nullable: true, comment: "Vehicle model year."),
                    Make = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Vehicle manufacturer such as Ford, Chevrolet, GMC, Ram, Toyota, etc."),
                    Model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Vehicle model such as F-150, Transit, Silverado, Express, etc."),
                    Color = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Vehicle exterior color."),
                    VIN = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Vehicle Identification Number assigned by the manufacturer."),
                    LicensePlate = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Vehicle registration plate number."),
                    LicensePlateState = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "State or province issuing the vehicle registration."),
                    PurchaseDate = table.Column<DateOnly>(type: "date", nullable: true, comment: "Date the vehicle was purchased or acquired."),
                    PurchasePrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true, comment: "Amount paid to acquire the vehicle."),
                    PurchasedFrom = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Name of the dealership, seller, auction, fleet provider, or other source from which the vehicle was acquired."),
                    IsPurchasedNew = table.Column<bool>(type: "boolean", nullable: true, comment: "Indicates whether the vehicle was purchased new or used."),
                    Notes = table.Column<string>(type: "text", nullable: true, comment: "Internal notes and remarks regarding the vehicle."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last updated the record."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the vehicle is active and available for service operations.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsVehicle", x => x.Id);
                    table.UniqueConstraint("UQ_FgsVehicle_WarehouseId", x => x.WarehouseId);
                    table.CheckConstraint("CK_FgsVehicle_OwnershipType", "\"OwnershipType\" IN ('Owned', 'Leased', 'Rented')");
                    table.ForeignKey(
                        name: "FK_FgsVehicle_FgsWarehouse_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "setup",
                        principalTable: "FgsWarehouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores company-owned or leased vehicles used for field service operations. Each vehicle is associated with a truck warehouse that serves as an inventory location.");

            migrationBuilder.CreateTable(
                name: "GloInventoryCategory",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BusinessTypeId = table.Column<int>(type: "integer", nullable: false),
                    CategoryCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloInventoryCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GloInventoryCategory_GloBusinessType",
                        column: x => x.BusinessTypeId,
                        principalSchema: "glo",
                        principalTable: "GloBusinessType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GloJobTypeCategory",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    BusinessTypeId = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloJobTypeCategory", x => x.Id);
                    table.CheckConstraint("CK_GloJobTypeCategory_Code_Upper", "\"Code\" = upper(\"Code\")");
                    table.ForeignKey(
                        name: "FK_GloJobTypeCategory_GloBusinessType_BusinessTypeId",
                        column: x => x.BusinessTypeId,
                        principalSchema: "glo",
                        principalTable: "GloBusinessType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GloJobTypeSubCategory",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    BusinessTypeId = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloJobTypeSubCategory", x => x.Id);
                    table.CheckConstraint("CK_GloJobTypeSubCategory_Code_Upper", "\"Code\" = upper(\"Code\")");
                    table.ForeignKey(
                        name: "FK_GloJobTypeSubCategory_GloBusinessType_BusinessTypeId",
                        column: x => x.BusinessTypeId,
                        principalSchema: "glo",
                        principalTable: "GloBusinessType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GloTrade",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BusinessTypeId = table.Column<int>(type: "integer", nullable: false),
                    TradeCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TradeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloTrade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GloTrade_GloBusinessType_BusinessTypeId",
                        column: x => x.BusinessTypeId,
                        principalSchema: "glo",
                        principalTable: "GloBusinessType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GloCommunicationTemplateToken",
                schema: "glo",
                columns: table => new
                {
                    CommunicationTemplateId = table.Column<long>(type: "bigint", nullable: false, comment: "Reference to the communication template."),
                    CommunicationTokenId = table.Column<int>(type: "integer", nullable: false, comment: "Reference to a communication token available for use within the template.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloCommunicationTemplateToken", x => new { x.CommunicationTemplateId, x.CommunicationTokenId });
                    table.ForeignKey(
                        name: "FK_GloCommunicationTemplateToken_CommunicationTemplateId",
                        column: x => x.CommunicationTemplateId,
                        principalSchema: "glo",
                        principalTable: "GloCommunicationTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GloCommunicationTemplateToken_CommunicationTokenId",
                        column: x => x.CommunicationTokenId,
                        principalSchema: "glo",
                        principalTable: "GloCommunicationToken",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Junction table defining the valid communication tokens that may be used within a communication template.");

            migrationBuilder.CreateTable(
                name: "GloStateProvince",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CountryCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    StateProvinceCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    StateProvinceName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloStateProvince", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GloStateProvince_Country",
                        column: x => x.CountryCode,
                        principalSchema: "glo",
                        principalTable: "GloCountry",
                        principalColumn: "CountryCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsCredential",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant that owns the credential."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company that owns the credential."),
                    CredentialProviderTypeId = table.Column<int>(type: "integer", nullable: false, comment: "Credential provider associated with this credential."),
                    CredentialName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "User friendly name displayed in tenant administration screens."),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Optional description of the credential usage."),
                    CredentialData = table.Column<byte[]>(type: "bytea", nullable: false, comment: "Provider credential JSON encrypted using a Data Encryption Key (DEK)."),
                    EncryptedDataKey = table.Column<byte[]>(type: "bytea", nullable: false, comment: "Data Encryption Key encrypted using AWS KMS."),
                    KeyIdentifier = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "AWS KMS key ARN or alias used to encrypt the Data Encryption Key."),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the credential is active and available for use."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsCredential", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsCredential_GloCredentialProviderType",
                        column: x => x.CredentialProviderTypeId,
                        principalSchema: "glo",
                        principalTable: "GloCredentialProviderType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores tenant-owned credentials encrypted using AWS KMS envelope encryption.");

            migrationBuilder.CreateTable(
                name: "GloCredential",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    CredentialProviderTypeId = table.Column<int>(type: "integer", nullable: false),
                    CredentialName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CredentialData = table.Column<byte[]>(type: "bytea", nullable: false),
                    EncryptedDataKey = table.Column<byte[]>(type: "bytea", nullable: false),
                    KeyIdentifier = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloCredential", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GloCredential_ProviderType",
                        column: x => x.CredentialProviderTypeId,
                        principalSchema: "glo",
                        principalTable: "GloCredentialProviderType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsEntityTag",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    TagId = table.Column<long>(type: "bigint", nullable: false),
                    MasterEntityTypeId = table.Column<int>(type: "integer", nullable: false),
                    EntityId = table.Column<long>(type: "bigint", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsEntityTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsEntityTag_FgsTag_TagId",
                        column: x => x.TagId,
                        principalSchema: "setup",
                        principalTable: "FgsTag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsEntityTag_GloMasterEntityType_MasterEntityTypeId",
                        column: x => x.MasterEntityTypeId,
                        principalSchema: "glo",
                        principalTable: "GloMasterEntityType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsTagEntityType",
                schema: "setup",
                columns: table => new
                {
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    TagId = table.Column<long>(type: "bigint", nullable: false),
                    MasterEntityTypeId = table.Column<int>(type: "integer", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsTagEntityType", x => new { x.TenantId, x.CompanyId, x.TagId, x.MasterEntityTypeId });
                    table.ForeignKey(
                        name: "FK_FgsTagEntityType_FgsTag_TagId",
                        column: x => x.TagId,
                        principalSchema: "setup",
                        principalTable: "FgsTag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FgsTagEntityType_GloMasterEntityType_MasterEntityTypeId",
                        column: x => x.MasterEntityTypeId,
                        principalSchema: "glo",
                        principalTable: "GloMasterEntityType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsResolutionCode",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
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
                        name: "FK_FgsResolutionCode_GloResType",
                        column: x => x.GloResolutionTypeId,
                        principalSchema: "glo",
                        principalTable: "GloResolutionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GloSeedTableColumnMapping",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SeedTableMappingId = table.Column<long>(type: "bigint", nullable: false),
                    SourceColumnName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    TargetColumnName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    TransformationType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    StaticValue = table.Column<string>(type: "text", nullable: true),
                    ColumnOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloSeedTableColumnMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GloSeedTableColumnMapping_GloSeedTableMapping",
                        column: x => x.SeedTableMappingId,
                        principalSchema: "glo",
                        principalTable: "GloSeedTableMapping",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupPricingMatrixLabor",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    FgsSetupPricingMatrixId = table.Column<Guid>(type: "uuid", nullable: false),
                    FgsSetupLaborRateTypeId = table.Column<int>(type: "integer", nullable: false),
                    FgsSetupTechSkillLevelId = table.Column<long>(type: "bigint", nullable: true),
                    BaseRate = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    OvertimeMultiplier = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    DoubleTimeMultiplier = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    DiscountPercent = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupPricingMatrixLabor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsSetupPricingMatrixLabor_LaborRateType",
                        column: x => x.FgsSetupLaborRateTypeId,
                        principalSchema: "glo",
                        principalTable: "GloSetupLaborRateType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsSetupPricingMatrixLabor_PricingMatrix",
                        column: x => x.FgsSetupPricingMatrixId,
                        principalSchema: "setup",
                        principalTable: "FgsSetupPricingMatrix",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsSetupPricingMatrixLabor_TechSkillLevel",
                        column: x => x.FgsSetupTechSkillLevelId,
                        principalSchema: "setup",
                        principalTable: "FgsSetupTechSkillLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsInventoryItem",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    InventoryItemTypeId = table.Column<long>(type: "bigint", nullable: false),
                    InventoryCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    InventorySubCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    ItemCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    PurchaseDescription = table.Column<string>(type: "text", nullable: true),
                    SalesDescription = table.Column<string>(type: "text", nullable: true),
                    ManufacturerPartNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UPCCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UnitOfMeasure = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TrackQuantity = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Cost = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    SalesPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    DefaultTaxable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventoryItem", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventoryItem_TenantId_CompanyId_ItemCode", x => new { x.TenantId, x.CompanyId, x.ItemCode });
                    table.ForeignKey(
                        name: "FK_FgsInventoryItem_FgsInventoryCategory",
                        column: x => x.InventoryCategoryId,
                        principalSchema: "setup",
                        principalTable: "FgsInventoryCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryItem_FgsInventoryItemType",
                        column: x => x.InventoryItemTypeId,
                        principalSchema: "setup",
                        principalTable: "FgsInventoryItemType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryItem_FgsInventorySubCategory",
                        column: x => x.InventorySubCategoryId,
                        principalSchema: "setup",
                        principalTable: "FgsInventorySubCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Inventory item master record for purchasing, sales, and stock tracking.");

            migrationBuilder.CreateTable(
                name: "FgsVehicleMaintenance",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Primary key.")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false, comment: "Tenant identifier."),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false, comment: "Company identifier."),
                    VehicleId = table.Column<long>(type: "bigint", nullable: false, comment: "Vehicle that received or is scheduled to receive maintenance service."),
                    VehicleMaintenanceTypeId = table.Column<int>(type: "integer", nullable: false, comment: "Type of maintenance activity being performed or scheduled."),
                    ServiceDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "Date the maintenance was performed or is scheduled to be performed."),
                    MileageAtService = table.Column<int>(type: "integer", nullable: true, comment: "Vehicle odometer reading at the time the maintenance was performed."),
                    ServiceProvider = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Name of the repair shop, dealership, service provider, or maintenance vendor."),
                    InvoiceNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Vendor invoice, receipt, repair order, or work order number associated with the maintenance activity."),
                    Cost = table.Column<decimal>(type: "numeric(18,2)", nullable: true, comment: "Total cost incurred for the maintenance activity."),
                    NextServiceDate = table.Column<DateOnly>(type: "date", nullable: true, comment: "Recommended next service date based on maintenance provider recommendations."),
                    NextServiceMileage = table.Column<int>(type: "integer", nullable: true, comment: "Recommended next service mileage based on maintenance provider recommendations."),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Indicates whether the maintenance activity has been completed. False indicates a scheduled or pending maintenance item."),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Short summary of the maintenance activity performed or scheduled."),
                    Notes = table.Column<string>(type: "text", nullable: true, comment: "Detailed notes, observations, recommendations, or repair information related to the maintenance activity."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()", comment: "Date and time the record was created."),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who created the record."),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Date and time the record was last updated."),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "User who last updated the record.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsVehicleMaintenance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsVehicleMaintenance_FgsVehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalSchema: "setup",
                        principalTable: "FgsVehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsVehicleMaintenance_GloVehicleMaintenanceType_VehicleMaintenanceTypeId",
                        column: x => x.VehicleMaintenanceTypeId,
                        principalSchema: "glo",
                        principalTable: "GloVehicleMaintenanceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores completed and scheduled maintenance activities, inspections, repairs, and service history for company vehicles.");

            migrationBuilder.CreateTable(
                name: "GloInventorySubCategory",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InventoryCategoryId = table.Column<int>(type: "integer", nullable: false),
                    SubCategoryCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloInventorySubCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GloInventorySubCategory_GloInventoryCategory",
                        column: x => x.InventoryCategoryId,
                        principalSchema: "glo",
                        principalTable: "GloInventoryCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GloSkill",
                schema: "glo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BusinessTypeId = table.Column<int>(type: "integer", nullable: false),
                    TradeId = table.Column<short>(type: "smallint", nullable: false),
                    SkillCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SkillName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    RequiresCertification = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloSkill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GloSkill_GloBusinessType_BusinessTypeId",
                        column: x => x.BusinessTypeId,
                        principalSchema: "glo",
                        principalTable: "GloBusinessType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GloSkill_GloTrade_TradeId",
                        column: x => x.TradeId,
                        principalSchema: "glo",
                        principalTable: "GloTrade",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupPricingMatrixLaborTier",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    FgsSetupPricingMatrixLaborId = table.Column<Guid>(type: "uuid", nullable: false),
                    SequenceOrder = table.Column<int>(type: "integer", nullable: false),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    Rate = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsSetupPricingMatrixLaborTier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgsSetupPricingMatrixLaborTier_Labor",
                        column: x => x.FgsSetupPricingMatrixLaborId,
                        principalSchema: "setup",
                        principalTable: "FgsSetupPricingMatrixLabor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsInventoryItemAlternate",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    AlternateInventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    AlternateType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PriorityOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventoryItemAlternate", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventoryItemAlternate_TenantId_CompanyId_InventoryItemId_AlternateInventoryItemId", x => new { x.TenantId, x.CompanyId, x.InventoryItemId, x.AlternateInventoryItemId });
                    table.ForeignKey(
                        name: "FK_FgsInventoryItemAlternate_FgsInventoryItem_AlternateInventoryItemId",
                        column: x => x.AlternateInventoryItemId,
                        principalSchema: "setup",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryItemAlternate_FgsInventoryItem_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalSchema: "setup",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsInventoryItemDependency",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    DependentInventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 1m),
                    DependencyType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventoryItemDependency", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventoryItemDependency_TenantId_CompanyId_InventoryItemId_DependentInventoryItemId", x => new { x.TenantId, x.CompanyId, x.InventoryItemId, x.DependentInventoryItemId });
                    table.ForeignKey(
                        name: "FK_FgsInventoryItemDependency_FgsInventoryItem_DependentInventoryItemId",
                        column: x => x.DependentInventoryItemId,
                        principalSchema: "setup",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsInventoryItemDependency_FgsInventoryItem_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalSchema: "setup",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsInventoryStock",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    QuantityOnHand = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 0m),
                    QuantityCommitted = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 0m),
                    QuantityAvailable = table.Column<decimal>(type: "numeric(18,4)", nullable: false, defaultValue: 0m),
                    AverageCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    LastCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    LastPurchaseDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    LastSoldDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsInventoryStock", x => x.Id);
                    table.UniqueConstraint("UQ_FgsInventoryStock_TenantId_CompanyId_InventoryItemId", x => new { x.TenantId, x.CompanyId, x.InventoryItemId });
                    table.ForeignKey(
                        name: "FK_FgsInventoryStock_FgsInventoryItem",
                        column: x => x.InventoryItemId,
                        principalSchema: "setup",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsVendorInventoryItem",
                schema: "setup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    VendorId = table.Column<long>(type: "bigint", nullable: false),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    VendorPartNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Vendor-specific part number for the inventory item."),
                    VendorPartName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    LastCost = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m, comment: "Last received cost from the vendor based on purchase order receiving."),
                    LastReceivedDate = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, comment: "Last date inventory was received from the vendor."),
                    PurchaseOrderComments = table.Column<string>(type: "text", nullable: true, comment: "Comments automatically copied to purchase orders for this vendor item combination."),
                    IsPreferredVendor = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Indicates whether this vendor is the preferred vendor for the inventory item."),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsVendorInventoryItem", x => x.Id);
                    table.UniqueConstraint("UQ_FgsVendorInventoryItem_TenantId_CompanyId_VendorId_InventoryItemId", x => new { x.TenantId, x.CompanyId, x.VendorId, x.InventoryItemId });
                    table.ForeignKey(
                        name: "FK_FgsVendorInventoryItem_FgsInventoryItem",
                        column: x => x.InventoryItemId,
                        principalSchema: "setup",
                        principalTable: "FgsInventoryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgsVendorInventoryItem_FgsVendor",
                        column: x => x.VendorId,
                        principalSchema: "setup",
                        principalTable: "FgsVendor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Stores vendor-specific inventory item relationships, vendor part information, pricing, and purchasing defaults.");

            migrationBuilder.CreateIndex(
                name: "IX_FgsBillingCategory_TenantId_CompanyId_IsActive",
                schema: "setup",
                table: "FgsBillingCategory",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsBusinessType_TenantId_CompanyId_IsActive",
                schema: "setup",
                table: "FgsBusinessType",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredential_CredentialProviderTypeId",
                schema: "setup",
                table: "FgsCredential",
                column: "CredentialProviderTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsCredential_Tenant_Company",
                schema: "setup",
                table: "FgsCredential",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UQ_FgsCredential_Tenant_Company_ProviderType",
                schema: "setup",
                table: "FgsCredential",
                columns: new[] { "TenantId", "CompanyId", "CredentialProviderTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsEntityTag_CreatedOn",
                schema: "setup",
                table: "FgsEntityTag",
                column: "CreatedOn",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_FgsEntityTag_Entity",
                schema: "setup",
                table: "FgsEntityTag",
                columns: new[] { "TenantId", "CompanyId", "MasterEntityTypeId", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEntityTag_MasterEntityTypeId",
                schema: "setup",
                table: "FgsEntityTag",
                columns: new[] { "TenantId", "CompanyId", "MasterEntityTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEntityTag_MasterEntityTypeId1",
                schema: "setup",
                table: "FgsEntityTag",
                column: "MasterEntityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsEntityTag_TagId",
                schema: "setup",
                table: "FgsEntityTag",
                columns: new[] { "TenantId", "CompanyId", "TagId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsEntityTag_TagId1",
                schema: "setup",
                table: "FgsEntityTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "UX_FgsEntityTag_TenantId_CompanyId_TagId_MasterEntityTypeId_EntityId",
                schema: "setup",
                table: "FgsEntityTag",
                columns: new[] { "TenantId", "CompanyId", "TagId", "MasterEntityTypeId", "EntityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryCategory_TenantId_CompanyId",
                schema: "setup",
                table: "FgsInventoryCategory",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_InventoryCategoryId",
                schema: "setup",
                table: "FgsInventoryItem",
                column: "InventoryCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_InventoryItemTypeId",
                schema: "setup",
                table: "FgsInventoryItem",
                column: "InventoryItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_InventorySubCategoryId",
                schema: "setup",
                table: "FgsInventoryItem",
                column: "InventorySubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_TenantId_CompanyId_InventoryCategoryId",
                schema: "setup",
                table: "FgsInventoryItem",
                columns: new[] { "TenantId", "CompanyId", "InventoryCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_TenantId_CompanyId_InventoryCategoryId_InventorySubCategoryId",
                schema: "setup",
                table: "FgsInventoryItem",
                columns: new[] { "TenantId", "CompanyId", "InventoryCategoryId", "InventorySubCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_TenantId_CompanyId_InventoryItemTypeId",
                schema: "setup",
                table: "FgsInventoryItem",
                columns: new[] { "TenantId", "CompanyId", "InventoryItemTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItem_TenantId_CompanyId_Name",
                schema: "setup",
                table: "FgsInventoryItem",
                columns: new[] { "TenantId", "CompanyId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemAlternate_AlternateInventoryItemId",
                schema: "setup",
                table: "FgsInventoryItemAlternate",
                column: "AlternateInventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemAlternate_InventoryItemId",
                schema: "setup",
                table: "FgsInventoryItemAlternate",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemAlternate_TenantId_CompanyId_InventoryItemId",
                schema: "setup",
                table: "FgsInventoryItemAlternate",
                columns: new[] { "TenantId", "CompanyId", "InventoryItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemDependency_DependentInventoryItemId",
                schema: "setup",
                table: "FgsInventoryItemDependency",
                column: "DependentInventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemDependency_InventoryItemId",
                schema: "setup",
                table: "FgsInventoryItemDependency",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemDependency_TenantId_CompanyId_InventoryItemId",
                schema: "setup",
                table: "FgsInventoryItemDependency",
                columns: new[] { "TenantId", "CompanyId", "InventoryItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryItemType_TenantId_CompanyId",
                schema: "setup",
                table: "FgsInventoryItemType",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventoryStock_InventoryItemId",
                schema: "setup",
                table: "FgsInventoryStock",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventorySubCategory_InventoryCategoryId",
                schema: "setup",
                table: "FgsInventorySubCategory",
                column: "InventoryCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventorySubCategory_TenantId_CompanyId",
                schema: "setup",
                table: "FgsInventorySubCategory",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsInventorySubCategory_TenantId_CompanyId_InventoryCategoryId",
                schema: "setup",
                table: "FgsInventorySubCategory",
                columns: new[] { "TenantId", "CompanyId", "InventoryCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobType_JobTypeCategoryId",
                schema: "setup",
                table: "FgsJobType",
                column: "JobTypeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobType_JobTypeSubCategoryId",
                schema: "setup",
                table: "FgsJobType",
                column: "JobTypeSubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobType_Tenant_Company",
                schema: "setup",
                table: "FgsJobType",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobType_Tenant_Company_BusinessUnit",
                schema: "setup",
                table: "FgsJobType",
                columns: new[] { "TenantId", "CompanyId", "BusinessUnit" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobType_Tenant_Company_Trade",
                schema: "setup",
                table: "FgsJobType",
                columns: new[] { "TenantId", "CompanyId", "Trade" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobType_Tenant_Company_UsedFor",
                schema: "setup",
                table: "FgsJobType",
                columns: new[] { "TenantId", "CompanyId", "UsedFor" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsJobType_Tenant_Company_JobTypeCode",
                schema: "setup",
                table: "FgsJobType",
                columns: new[] { "TenantId", "CompanyId", "JobTypeCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeCategory_Tenant_Company",
                schema: "setup",
                table: "FgsJobTypeCategory",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsJobTypeCategory_Tenant_Company_CategoryCode",
                schema: "setup",
                table: "FgsJobTypeCategory",
                columns: new[] { "TenantId", "CompanyId", "CategoryCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsJobTypeSubCategory_Tenant_Company",
                schema: "setup",
                table: "FgsJobTypeSubCategory",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "UX_FgsJobTypeSubCategory_Tenant_Company_SubCategoryCode",
                schema: "setup",
                table: "FgsJobTypeSubCategory",
                columns: new[] { "TenantId", "CompanyId", "SubCategoryCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_FgsLeadSource_TenantId_CompanyId_SourceCode",
                schema: "setup",
                table: "FgsLeadSource",
                columns: new[] { "TenantId", "CompanyId", "SourceCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsResolutionCode_GloResolutionTypeId",
                schema: "setup",
                table: "FgsResolutionCode",
                column: "GloResolutionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsResolutionCode_GloResType",
                schema: "setup",
                table: "FgsResolutionCode",
                columns: new[] { "TenantId", "CompanyId", "GloResolutionTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupCommunicationTemplate_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupCommunicationTemplate",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupCommunicationTemplate_TenantId_CompanyId_TemplateTyp",
                schema: "setup",
                table: "FgsSetupCommunicationTemplate",
                columns: new[] { "TenantId", "CompanyId", "CommunicationChannel", "TemplateType", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupDescription_TechTrade",
                schema: "setup",
                table: "FgsSetupDescription",
                column: "FgsSetupTechTradeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupDescription_Tenant_Company_Type",
                schema: "setup",
                table: "FgsSetupDescription",
                columns: new[] { "TenantId", "CompanyId", "DescriptionTypeCode" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupDescription_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupDescription",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupGLBreak_AddressId",
                schema: "setup",
                table: "FgsSetupGLBreak",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupGLBreak_LogoFileId",
                schema: "setup",
                table: "FgsSetupGLBreak",
                column: "LogoFileId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupGLBreak_TenantId_CompanyId_BreakLevel",
                schema: "setup",
                table: "FgsSetupGLBreak",
                columns: new[] { "TenantId", "CompanyId", "BreakLevel" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupGLBreakTrade_GLBreakId",
                schema: "setup",
                table: "FgsSetupGLBreakTrade",
                column: "GLBreakId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupGLBreakTrade_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupGLBreakTrade",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupGLBreakTrade_TradeCode",
                schema: "setup",
                table: "FgsSetupGLBreakTrade",
                columns: new[] { "TenantId", "CompanyId", "TradeCode" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupLaborRateType_TenantId_CompanyId_IsActive",
                schema: "setup",
                table: "FgsSetupLaborRateType",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPaymentMethod_TenantId_CompanyId_IsActive",
                schema: "setup",
                table: "FgsSetupPaymentMethod",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPaymentTerm_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupPaymentTerm",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPostalCode_TaxId",
                schema: "setup",
                table: "FgsSetupPostalCode",
                column: "FgsSetupTaxId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPostalCode_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupPostalCode",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPostalCode_ZoneId",
                schema: "setup",
                table: "FgsSetupPostalCode",
                column: "FgsSetupZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPricingMatrix_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupPricingMatrix",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPricingMatrixLabor_FgsSetupLaborRateTypeId",
                schema: "setup",
                table: "FgsSetupPricingMatrixLabor",
                column: "FgsSetupLaborRateTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPricingMatrixLabor_FgsSetupPricingMatrixId",
                schema: "setup",
                table: "FgsSetupPricingMatrixLabor",
                column: "FgsSetupPricingMatrixId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPricingMatrixLabor_FgsSetupTechSkillLevelId",
                schema: "setup",
                table: "FgsSetupPricingMatrixLabor",
                column: "FgsSetupTechSkillLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPricingMatrixLabor_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupPricingMatrixLabor",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPricingMatrixLaborTier_FgsSetupPricingMatrixLaborId",
                schema: "setup",
                table: "FgsSetupPricingMatrixLaborTier",
                column: "FgsSetupPricingMatrixLaborId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPricingMatrixLaborTier_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupPricingMatrixLaborTier",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPricingMatrixMaterialTier_FgsSetupPricingMatrixId",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier",
                column: "FgsSetupPricingMatrixId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPricingMatrixMaterialTier_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupPricingMatrixMaterialTier",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPricingMatrixOther_FgsSetupPricingMatrixId",
                schema: "setup",
                table: "FgsSetupPricingMatrixOther",
                column: "FgsSetupPricingMatrixId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPricingMatrixOther_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupPricingMatrixOther",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupServiceAssetManufacturer_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupServiceAssetManufacturer",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupServiceAssetModelReference_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupServiceAssetModelReference",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSvcAssetModelRef_Mfr",
                schema: "setup",
                table: "FgsSetupServiceAssetModelReference",
                columns: new[] { "TenantId", "CompanyId", "FgsSetupServiceAssetManufacturerId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSvcAssetModelRef_MfrId",
                schema: "setup",
                table: "FgsSetupServiceAssetModelReference",
                column: "FgsSetupServiceAssetManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSvcAssetModelRef_Type",
                schema: "setup",
                table: "FgsSetupServiceAssetModelReference",
                columns: new[] { "TenantId", "CompanyId", "FgsSetupServiceAssetTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSvcAssetModelRef_TypeId",
                schema: "setup",
                table: "FgsSetupServiceAssetModelReference",
                column: "FgsSetupServiceAssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSvcAssetModelRef_TypeMfr",
                schema: "setup",
                table: "FgsSetupServiceAssetModelReference",
                columns: new[] { "TenantId", "CompanyId", "FgsSetupServiceAssetTypeId", "FgsSetupServiceAssetManufacturerId" });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceAsset_UrlsJson",
                schema: "setup",
                table: "FgsSetupServiceAssetModelReference",
                column: "UrlsJson")
                .Annotation("Npgsql:IndexMethod", "gin");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupServiceAssetType_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupServiceAssetType",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTax_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupTax",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTaxAuthority_RegionCode",
                schema: "setup",
                table: "FgsSetupTaxAuthority",
                columns: new[] { "TenantId", "CompanyId", "RegionCode" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTaxAuthority_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupTaxAuthority",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTaxDetail_EffectiveDates",
                schema: "setup",
                table: "FgsSetupTaxDetail",
                columns: new[] { "EffectiveFromDate", "EffectiveToDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTaxDetail_Tax",
                schema: "setup",
                table: "FgsSetupTaxDetail",
                columns: new[] { "TenantId", "CompanyId", "FgsSetupTaxId", "EffectiveFromDate", "EffectiveToDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTaxDetail_TaxAuth",
                schema: "setup",
                table: "FgsSetupTaxDetail",
                columns: new[] { "TenantId", "CompanyId", "FgsSetupTaxAuthorityId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTaxDetail_TaxAuthId",
                schema: "setup",
                table: "FgsSetupTaxDetail",
                column: "FgsSetupTaxAuthorityId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTaxDetail_TaxId",
                schema: "setup",
                table: "FgsSetupTaxDetail",
                column: "FgsSetupTaxId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTaxDetail_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupTaxDetail",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTechSkillLevel_SortOrder",
                schema: "setup",
                table: "FgsSetupTechSkillLevel",
                columns: new[] { "TenantId", "CompanyId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTechSkillLevel_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupTechSkillLevel",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTechTrade_SortOrder",
                schema: "setup",
                table: "FgsSetupTechTrade",
                columns: new[] { "TenantId", "CompanyId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTechTrade_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupTechTrade",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTimeSlot_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupTimeSlot",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTimeSlot_Zone",
                schema: "setup",
                table: "FgsSetupTimeSlot",
                columns: new[] { "TenantId", "CompanyId", "FgsSetupZoneId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTimeSlot_ZoneId",
                schema: "setup",
                table: "FgsSetupTimeSlot",
                column: "FgsSetupZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTitleOfCourtesy_SortOrder",
                schema: "setup",
                table: "FgsSetupTitleOfCourtesy",
                columns: new[] { "TenantId", "CompanyId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupTitleOfCourtesy_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupTitleOfCourtesy",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupZone_TenantId_CompanyId",
                schema: "setup",
                table: "FgsSetupZone",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsTag_IconFileId",
                schema: "setup",
                table: "FgsTag",
                column: "IconFileId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsTag_IsActive",
                schema: "setup",
                table: "FgsTag",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsTag_Name",
                schema: "setup",
                table: "FgsTag",
                columns: new[] { "TenantId", "CompanyId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsTag_UsageCount",
                schema: "setup",
                table: "FgsTag",
                columns: new[] { "TenantId", "CompanyId", "UsageCount" },
                descending: new[] { false, false, true });

            migrationBuilder.CreateIndex(
                name: "UX_FgsTag_TenantId_CompanyId_NormalizedName",
                schema: "setup",
                table: "FgsTag",
                columns: new[] { "TenantId", "CompanyId", "NormalizedName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_FgsTag_TenantId_CompanyId_TagCode",
                schema: "setup",
                table: "FgsTag",
                columns: new[] { "TenantId", "CompanyId", "TagCode" },
                unique: true,
                filter: "\"TagCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FgsTagEntityType_IsDefault",
                schema: "setup",
                table: "FgsTagEntityType",
                columns: new[] { "TenantId", "CompanyId", "MasterEntityTypeId", "IsDefault" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsTagEntityType_MasterEntityTypeId",
                schema: "setup",
                table: "FgsTagEntityType",
                columns: new[] { "TenantId", "CompanyId", "MasterEntityTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsTagEntityType_MasterEntityTypeId1",
                schema: "setup",
                table: "FgsTagEntityType",
                column: "MasterEntityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsTagEntityType_TagId",
                schema: "setup",
                table: "FgsTagEntityType",
                columns: new[] { "TenantId", "CompanyId", "TagId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsTagEntityType_TagId1",
                schema: "setup",
                table: "FgsTagEntityType",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicle_TenantId_CompanyId_IsActive",
                schema: "setup",
                table: "FgsVehicle",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicle_TenantId_CompanyId_WarehouseId",
                schema: "setup",
                table: "FgsVehicle",
                columns: new[] { "TenantId", "CompanyId", "WarehouseId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicleMaintenance_TenantId_CompanyId_IsCompleted",
                schema: "setup",
                table: "FgsVehicleMaintenance",
                columns: new[] { "TenantId", "CompanyId", "IsCompleted" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicleMaintenance_TenantId_CompanyId_NextServiceDate",
                schema: "setup",
                table: "FgsVehicleMaintenance",
                columns: new[] { "TenantId", "CompanyId", "NextServiceDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicleMaintenance_TenantId_CompanyId_ServiceDate",
                schema: "setup",
                table: "FgsVehicleMaintenance",
                columns: new[] { "TenantId", "CompanyId", "ServiceDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicleMaintenance_TenantId_CompanyId_VehicleId",
                schema: "setup",
                table: "FgsVehicleMaintenance",
                columns: new[] { "TenantId", "CompanyId", "VehicleId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicleMaintenance_TenantId_CompanyId_VehicleMaintenanceTypeId",
                schema: "setup",
                table: "FgsVehicleMaintenance",
                columns: new[] { "TenantId", "CompanyId", "VehicleMaintenanceTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicleMaintenance_VehicleId",
                schema: "setup",
                table: "FgsVehicleMaintenance",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsVehicleMaintenance_VehicleMaintenanceTypeId",
                schema: "setup",
                table: "FgsVehicleMaintenance",
                column: "VehicleMaintenanceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendor_PaymentTermId",
                schema: "setup",
                table: "FgsVendor",
                column: "PaymentTermId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendor_TenantId_CompanyId",
                schema: "setup",
                table: "FgsVendor",
                columns: new[] { "TenantId", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendor_TenantId_CompanyId_Name",
                schema: "setup",
                table: "FgsVendor",
                columns: new[] { "TenantId", "CompanyId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendor_TenantId_CompanyId_VendorType",
                schema: "setup",
                table: "FgsVendor",
                columns: new[] { "TenantId", "CompanyId", "VendorType" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendorInventoryItem_InventoryItemId",
                schema: "setup",
                table: "FgsVendorInventoryItem",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendorInventoryItem_TenantId_CompanyId_InventoryItemId",
                schema: "setup",
                table: "FgsVendorInventoryItem",
                columns: new[] { "TenantId", "CompanyId", "InventoryItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendorInventoryItem_TenantId_CompanyId_VendorId",
                schema: "setup",
                table: "FgsVendorInventoryItem",
                columns: new[] { "TenantId", "CompanyId", "VendorId" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsVendorInventoryItem_VendorId",
                schema: "setup",
                table: "FgsVendorInventoryItem",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsWarehouse_TenantId_CompanyId_IsActive",
                schema: "setup",
                table: "FgsWarehouse",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsWarehouse_TenantId_CompanyId_WarehouseType",
                schema: "setup",
                table: "FgsWarehouse",
                columns: new[] { "TenantId", "CompanyId", "WarehouseType" });

            migrationBuilder.CreateIndex(
                name: "IX_GloCommunicationTemplate_CommunicationChannel",
                schema: "glo",
                table: "GloCommunicationTemplate",
                column: "CommunicationChannel");

            migrationBuilder.CreateIndex(
                name: "IX_GloCommunicationTemplate_IsActive",
                schema: "glo",
                table: "GloCommunicationTemplate",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_GloCommunicationTemplate_TemplateCode",
                schema: "glo",
                table: "GloCommunicationTemplate",
                column: "TemplateCode");

            migrationBuilder.CreateIndex(
                name: "IX_GloCommunicationTemplate_TemplateScope",
                schema: "glo",
                table: "GloCommunicationTemplate",
                column: "TemplateScope");

            migrationBuilder.CreateIndex(
                name: "UQ_GloCommunicationTemplate_CommunicationChannel_TemplateCode",
                schema: "glo",
                table: "GloCommunicationTemplate",
                columns: new[] { "CommunicationChannel", "TemplateCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloCommunicationTemplateToken_CommunicationTokenId",
                schema: "glo",
                table: "GloCommunicationTemplateToken",
                column: "CommunicationTokenId");

            migrationBuilder.CreateIndex(
                name: "IX_GloCredential_CredentialProviderTypeId",
                schema: "glo",
                table: "GloCredential",
                column: "CredentialProviderTypeId");

            migrationBuilder.CreateIndex(
                name: "UQ_GloCredentialProviderType_ProviderCode",
                schema: "glo",
                table: "GloCredentialProviderType",
                column: "ProviderCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloInventoryCategory_BusinessTypeId",
                schema: "glo",
                table: "GloInventoryCategory",
                column: "BusinessTypeId");

            migrationBuilder.CreateIndex(
                name: "UQ_GloInventoryCategory_BusinessTypeId_CategoryCode",
                schema: "glo",
                table: "GloInventoryCategory",
                columns: new[] { "BusinessTypeId", "CategoryCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_GloInventoryItemType_ItemTypeCode",
                schema: "glo",
                table: "GloInventoryItemType",
                column: "ItemTypeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloInventorySubCategory_InventoryCategoryId",
                schema: "glo",
                table: "GloInventorySubCategory",
                column: "InventoryCategoryId");

            migrationBuilder.CreateIndex(
                name: "UQ_GloInventorySubCategory_InventoryCategoryId_SubCategoryCode",
                schema: "glo",
                table: "GloInventorySubCategory",
                columns: new[] { "InventoryCategoryId", "SubCategoryCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_GloJobTypeCategory_BusinessTypeId_Code",
                schema: "glo",
                table: "GloJobTypeCategory",
                columns: new[] { "BusinessTypeId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloJobTypeSubCategory_BusinessTypeId",
                schema: "glo",
                table: "GloJobTypeSubCategory",
                column: "BusinessTypeId");

            migrationBuilder.CreateIndex(
                name: "UQ_GloJobTypeSubCategory_Code",
                schema: "glo",
                table: "GloJobTypeSubCategory",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_GloLeadSource_SourceCode",
                schema: "glo",
                table: "GloLeadSource",
                column: "SourceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloLocationType_Code",
                schema: "glo",
                table: "GloLocationType",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloOutboxMessage_CorrelationId",
                schema: "glo",
                table: "GloOutboxMessage",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_GloOutboxMessage_EventType",
                schema: "glo",
                table: "GloOutboxMessage",
                column: "EventType");

            migrationBuilder.CreateIndex(
                name: "IX_GloOutboxMessage_Status_NextRetryOn",
                schema: "glo",
                table: "GloOutboxMessage",
                columns: new[] { "Status", "NextRetryOn" });

            migrationBuilder.CreateIndex(
                name: "IX_GloOutboxMessage_TenantId",
                schema: "glo",
                table: "GloOutboxMessage",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_GloSeedTableColumnMapping_SeedTableMappingId",
                schema: "glo",
                table: "GloSeedTableColumnMapping",
                column: "SeedTableMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_GloSeedTableMapping_SeedCode",
                schema: "glo",
                table: "GloSeedTableMapping",
                column: "SeedCode");

            migrationBuilder.CreateIndex(
                name: "IX_GloSeedTableMapping_SeedOrder",
                schema: "glo",
                table: "GloSeedTableMapping",
                column: "SeedOrder");

            migrationBuilder.CreateIndex(
                name: "UX_GloSeedTableMapping_SeedCode_TargetTableName",
                schema: "glo",
                table: "GloSeedTableMapping",
                columns: new[] { "SeedCode", "TargetTableName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_GloSetupTenantStatus_Name",
                schema: "glo",
                table: "GloSetupTenantStatus",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloSkill_BusinessTypeId",
                schema: "glo",
                table: "GloSkill",
                column: "BusinessTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GloSkill_TradeId",
                schema: "glo",
                table: "GloSkill",
                column: "TradeId");

            migrationBuilder.CreateIndex(
                name: "UX_GloSkill_SkillCode",
                schema: "glo",
                table: "GloSkill",
                column: "SkillCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_GloStateProvince",
                schema: "glo",
                table: "GloStateProvince",
                columns: new[] { "CountryCode", "StateProvinceCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloTag_DisplayOrder",
                schema: "glo",
                table: "GloTag",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_GloTag_IconFileId",
                schema: "glo",
                table: "GloTag",
                column: "IconFileId");

            migrationBuilder.CreateIndex(
                name: "IX_GloTag_IsActive",
                schema: "glo",
                table: "GloTag",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_GloTag_Name",
                schema: "glo",
                table: "GloTag",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "UX_GloTag_NormalizedName",
                schema: "glo",
                table: "GloTag",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_GloTag_TagCode",
                schema: "glo",
                table: "GloTag",
                column: "TagCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloTitleOfCourtesy_SortOrder",
                schema: "glo",
                table: "GloTitleOfCourtesy",
                column: "SortOrder");

            migrationBuilder.CreateIndex(
                name: "UQ_GloTitleOfCourtesy_Code",
                schema: "glo",
                table: "GloTitleOfCourtesy",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloTrade_BusinessTypeId",
                schema: "glo",
                table: "GloTrade",
                column: "BusinessTypeId");

            migrationBuilder.CreateIndex(
                name: "UX_GloTrade_TradeCode",
                schema: "glo",
                table: "GloTrade",
                column: "TradeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloUnitOfMeasure_UnitType",
                schema: "glo",
                table: "GloUnitOfMeasure",
                column: "UnitType");

            migrationBuilder.CreateIndex(
                name: "UQ_GloUnitOfMeasure_UnitCode",
                schema: "glo",
                table: "GloUnitOfMeasure",
                column: "UnitCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloVehicleMaintenanceType_DisplayOrder",
                schema: "glo",
                table: "GloVehicleMaintenanceType",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "UQ_GloZone_Code",
                schema: "glo",
                table: "GloZone",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FgsBillingCategory",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsBusinessType",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsCredential",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsEntityTag",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsInventoryItemAlternate",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsInventoryItemDependency",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsInventoryStock",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsJobType",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsLeadSource",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsResolutionCode",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSetupCommunicationTemplate",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSetupDescription",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSetupGLBreakTrade",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSetupLaborRateType",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSetupPaymentMethod",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSetupPostalCode",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSetupPricingMatrixLaborTier",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSetupPricingMatrixMaterialTier",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSetupPricingMatrixOther",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSetupServiceAssetModelReference",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSetupTaxDetail",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSetupTimeSlot",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSetupTitleOfCourtesy",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsTagEntityType",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsVehicleMaintenance",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsVendorInventoryItem",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "GloAccountingIntegrationType",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloBillingCategory",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloCommunicationTemplateToken",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloCredential",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloInventoryItemType",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloInventorySubCategory",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloJobTypeCategory",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloJobTypeSubCategory",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloLanguage",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloLeadSource",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloLocationType",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloOutboxMessage",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloPaymentMethodType",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloRole",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloSeedTableColumnMapping",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloSetupDescriptionType",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloSetupPaymentTerm",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloSetupTenantStatus",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloSkill",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloStateProvince",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloTag",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloTimeCardOption",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloTitleOfCourtesy",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloUnitOfMeasure",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloZone",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "FgsJobTypeCategory",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsJobTypeSubCategory",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "GloResolutionType",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "FgsSetupTechTrade",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSetupGLBreak",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSetupPricingMatrixLabor",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSetupServiceAssetType",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSetupServiceAssetManufacturer",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSetupTax",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSetupTaxAuthority",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSetupZone",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsTag",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "GloMasterEntityType",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "FgsVehicle",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "GloVehicleMaintenanceType",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "FgsInventoryItem",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsVendor",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "GloCommunicationTemplate",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloCommunicationToken",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloCredentialProviderType",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloInventoryCategory",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloSeedTableMapping",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloTrade",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloCountry",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "GloSetupLaborRateType",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "FgsSetupPricingMatrix",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSetupTechSkillLevel",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsWarehouse",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsInventoryItemType",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsInventorySubCategory",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "FgsSetupPaymentTerm",
                schema: "setup");

            migrationBuilder.DropTable(
                name: "GloBusinessType",
                schema: "glo");

            migrationBuilder.DropTable(
                name: "FgsInventoryCategory",
                schema: "setup");
        }
    }
}
