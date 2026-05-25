using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fgs.User.Infrastructure.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddBillingBusinessTypeLaborRatePaymentTermAndSchemaUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupPaymentMethod_GloPayType",
                schema: "dbo",
                table: "FgsSetupPaymentMethod");

            migrationBuilder.DropTable(
                name: "FgsSetupGLBreakTechTrade",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloJobTypeCategorySubCategory",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "UX_GloSeedTableMapping_SeedCode",
                schema: "dbo",
                table: "GloSeedTableMapping");

            migrationBuilder.DropUniqueConstraint(
                name: "UQ_FgsSetupPaymentMethod",
                schema: "dbo",
                table: "FgsSetupPaymentMethod");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupPaymentMethod_GloPaymentMethodTypeId",
                schema: "dbo",
                table: "FgsSetupPaymentMethod");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupPaymentMethod_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupPaymentMethod");

            migrationBuilder.DropUniqueConstraint(
                name: "UQ_FgsSetupGLBreak",
                schema: "dbo",
                table: "FgsSetupGLBreak");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupGLBreak_TenantId_CompanyId",
                schema: "dbo",
                table: "FgsSetupGLBreak");

            migrationBuilder.DropColumn(
                name: "GloPaymentMethodTypeId",
                schema: "dbo",
                table: "FgsSetupPaymentMethod");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                schema: "dbo",
                table: "FgsSetupGLBreak");

            migrationBuilder.DropTable(
                name: "GloSetupDescriptionType",
                schema: "dbo");

            migrationBuilder.CreateTable(
                name: "GloSetupDescriptionType",
                schema: "dbo",
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

            migrationBuilder.AddColumn<int>(
                name: "BusinessTypeId",
                schema: "dbo",
                table: "GloJobTypeSubCategory",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "FgsTenantStatusId",
                schema: "dbo",
                table: "FgsTenant",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)1)
                .Annotation("Relational:ColumnOrder", 2)
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantGuid",
                schema: "dbo",
                table: "FgsTenant",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<bool>(
                name: "IsMobileVisible",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCustomerPortalVisible",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<short>(
                name: "BreakLevel",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<Guid>(
                name: "AddressId",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LogoFileId",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "Trades",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "UQ_FgsSetupPaymentMethod",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                columns: new[] { "TenantId", "CompanyId", "DisplayName" });

            migrationBuilder.AddUniqueConstraint(
                name: "UQ_FgsSetupGLBreak",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                columns: new[] { "TenantId", "CompanyId", "Code", "BreakLevel" });

            migrationBuilder.CreateTable(
                name: "FgsBillingCategory",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BillingCategoryType = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    BillingCategoryName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    IsSystemDefined = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ShowToFieldTech = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgsBillingCategory", x => x.Id);
                    table.UniqueConstraint("UQ_FgsBillingCategory_TenantId_CompanyId_BillingCategoryType", x => new { x.TenantId, x.CompanyId, x.BillingCategoryType });
                    table.ForeignKey(
                        name: "FK_FgsBillingCategory_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsBusinessType",
                schema: "dbo",
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
                    table.ForeignKey(
                        name: "FK_FgsBusinessType_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgsSetupLaborRateType",
                schema: "dbo",
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
                    table.ForeignKey(
                        name: "FK_FgsSetupLaborRateType_FgsTenantCompany_TenantId_CompanyId",
                        columns: x => new { x.TenantId, x.CompanyId },
                        principalSchema: "dbo",
                        principalTable: "FgsTenantCompany",
                        principalColumns: new[] { "TenantId", "CompanyNumber" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GloSetupPaymentTerm",
                schema: "dbo",
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

            migrationBuilder.CreateIndex(
                name: "IX_GloSeedTableMapping_SeedCode",
                schema: "dbo",
                table: "GloSeedTableMapping",
                column: "SeedCode");

            migrationBuilder.CreateIndex(
                name: "UX_GloSeedTableMapping_SeedCode_TargetTableName",
                schema: "dbo",
                table: "GloSeedTableMapping",
                columns: new[] { "SeedCode", "TargetTableName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GloJobTypeSubCategory_BusinessTypeId",
                schema: "dbo",
                table: "GloJobTypeSubCategory",
                column: "BusinessTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsTenant_TenantGuid",
                schema: "dbo",
                table: "FgsTenant",
                column: "TenantGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupPaymentMethod_TenantId_CompanyId_IsActive",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupGLBreak_AddressId",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupGLBreak_LogoFileId",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                column: "LogoFileId");

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupGLBreak_TenantId_CompanyId_BreakLevel",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                columns: new[] { "TenantId", "CompanyId", "BreakLevel" });

            migrationBuilder.AddCheckConstraint(
                name: "CK_FgsSetupGLBreak_BreakLevel",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                sql: "\"BreakLevel\" IN (1, 2)");

            migrationBuilder.CreateIndex(
                name: "IX_FgsBillingCategory_TenantId_CompanyId_IsActive",
                schema: "dbo",
                table: "FgsBillingCategory",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsBusinessType_TenantId_CompanyId_IsActive",
                schema: "dbo",
                table: "FgsBusinessType",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_FgsSetupLaborRateType_TenantId_CompanyId_IsActive",
                schema: "dbo",
                table: "FgsSetupLaborRateType",
                columns: new[] { "TenantId", "CompanyId", "IsActive" });

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupGLBreak_FgsFile_LogoFileId",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                column: "LogoFileId",
                principalSchema: "dbo",
                principalTable: "FgsFile",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupGLBreak_FgsLocation_AddressId",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                column: "AddressId",
                principalSchema: "dbo",
                principalTable: "FgsLocation",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_GloJobTypeSubCategory_GloBusinessType_BusinessTypeId",
                schema: "dbo",
                table: "GloJobTypeSubCategory",
                column: "BusinessTypeId",
                principalSchema: "dbo",
                principalTable: "GloBusinessType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql(
                """
                COMMENT ON TABLE dbo."FgsSetupGLBreak" IS 'Stores GL break configuration for financial reporting segmentation by trade, location, or organizational unit.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."Id" IS 'Surrogate primary key.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."TenantId" IS 'Owning tenant identifier.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."CompanyId" IS 'Tenant-scoped company number (maps to FgsTenantCompany.CompanyNumber).';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."Code" IS 'Unique GL break code within tenant, company, and break level scope.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."Name" IS 'Display name of the GL break.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."BreakLabel" IS 'Optional label shown in UI for the break.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."BreakLevel" IS 'Break hierarchy level. Allowed values: 1, 2.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."Trades" IS 'Optional array of technician trade codes associated with this GL break.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."LogoFileId" IS 'Optional reference to uploaded logo file in FgsFile.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."AddressId" IS 'Optional reference to break address in FgsLocation.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."IsActive" IS 'Indicates whether the GL break is active.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."CreatedOn" IS 'UTC timestamp when the record was created.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."CreatedBy" IS 'User or process that created the record.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."UpdatedOn" IS 'UTC timestamp of the last update.';
                COMMENT ON COLUMN dbo."FgsSetupGLBreak"."UpdatedBy" IS 'User or process that last updated the record.';

                COMMENT ON TABLE dbo."FgsBillingCategory" IS 'Stores tenant/company specific billing categories used for invoicing, service billing, maintenance plans, and other billing operations. Seeded initially from GloBillingCategory but fully managed by each tenant/company independently.';
                COMMENT ON COLUMN dbo."FgsBillingCategory"."Id" IS 'Primary key identity of the billing category record.';
                COMMENT ON COLUMN dbo."FgsBillingCategory"."TenantId" IS 'Tenant identifier owning this billing category.';
                COMMENT ON COLUMN dbo."FgsBillingCategory"."CompanyId" IS 'Company identifier within the tenant owning this billing category.';
                COMMENT ON COLUMN dbo."FgsBillingCategory"."BillingCategoryType" IS 'Short billing category code such as IN, PM, SR, or other tenant-defined values.';
                COMMENT ON COLUMN dbo."FgsBillingCategory"."BillingCategoryName" IS 'Display name of the billing category shown throughout the application.';
                COMMENT ON COLUMN dbo."FgsBillingCategory"."Description" IS 'Optional internal description or notes for the billing category.';
                COMMENT ON COLUMN dbo."FgsBillingCategory"."DisplayOrder" IS 'Controls sorting/display order of billing categories in dropdowns and setup screens.';
                COMMENT ON COLUMN dbo."FgsBillingCategory"."IsSystemDefined" IS 'Indicates whether the billing category was system seeded or manually created by the tenant/company.';
                COMMENT ON COLUMN dbo."FgsBillingCategory"."ShowToFieldTech" IS 'Indicates whether the billing category is visible to field technicians in mobile and field service applications.';
                COMMENT ON COLUMN dbo."FgsBillingCategory"."IsActive" IS 'Indicates whether the billing category is active and available for use.';
                COMMENT ON COLUMN dbo."FgsBillingCategory"."CreatedOn" IS 'Date and time the billing category record was created.';
                COMMENT ON COLUMN dbo."FgsBillingCategory"."CreatedBy" IS 'User identifier that created the billing category record.';
                COMMENT ON COLUMN dbo."FgsBillingCategory"."UpdatedOn" IS 'Date and time the billing category record was last updated.';
                COMMENT ON COLUMN dbo."FgsBillingCategory"."UpdatedBy" IS 'User identifier that last updated the billing category record.';

                COMMENT ON TABLE dbo."FgsBusinessType" IS 'Stores tenant/company specific business types used throughout the application. Seeded initially from GloBusinessType but independently managed by each tenant/company.';
                COMMENT ON COLUMN dbo."FgsBusinessType"."Id" IS 'Primary key identity of the business type record.';
                COMMENT ON COLUMN dbo."FgsBusinessType"."TenantId" IS 'Tenant identifier owning this business type.';
                COMMENT ON COLUMN dbo."FgsBusinessType"."CompanyId" IS 'Company identifier within the tenant owning this business type.';
                COMMENT ON COLUMN dbo."FgsBusinessType"."Code" IS 'Unique business type code within the tenant/company.';
                COMMENT ON COLUMN dbo."FgsBusinessType"."Name" IS 'Display name of the business type shown throughout the application.';
                COMMENT ON COLUMN dbo."FgsBusinessType"."Description" IS 'Optional internal description or notes for the business type.';
                COMMENT ON COLUMN dbo."FgsBusinessType"."DisplayOrder" IS 'Controls sorting/display order of business types in dropdowns and setup screens.';
                COMMENT ON COLUMN dbo."FgsBusinessType"."IsActive" IS 'Indicates whether the business type is active and available for use.';
                COMMENT ON COLUMN dbo."FgsBusinessType"."CreatedOn" IS 'Date and time the business type record was created.';
                COMMENT ON COLUMN dbo."FgsBusinessType"."UpdatedOn" IS 'Date and time the business type record was last updated.';
                COMMENT ON COLUMN dbo."FgsBusinessType"."CreatedBy" IS 'User identifier that created the business type record.';
                COMMENT ON COLUMN dbo."FgsBusinessType"."UpdatedBy" IS 'User identifier that last updated the business type record.';

                COMMENT ON COLUMN dbo."GloJobTypeSubCategory"."BusinessTypeId" IS 'Optional business type associated with this job type subcategory. NULL means shared across all business types.';

                COMMENT ON TABLE dbo."GloSetupDescriptionType" IS 'Stores global setup description types used throughout the system for organizing setup descriptions and configuration text.';
                COMMENT ON COLUMN dbo."GloSetupDescriptionType"."Id" IS 'Primary key identity of the setup description type record.';
                COMMENT ON COLUMN dbo."GloSetupDescriptionType"."Code" IS 'Unique code representing the setup description type.';
                COMMENT ON COLUMN dbo."GloSetupDescriptionType"."Name" IS 'Display name of the setup description type.';
                COMMENT ON COLUMN dbo."GloSetupDescriptionType"."Description" IS 'Optional description or notes for the setup description type.';
                COMMENT ON COLUMN dbo."GloSetupDescriptionType"."IsActive" IS 'Indicates whether the setup description type is active and available for use.';
                COMMENT ON COLUMN dbo."GloSetupDescriptionType"."CreatedOn" IS 'Date and time the setup description type record was created.';
                COMMENT ON COLUMN dbo."GloSetupDescriptionType"."UpdatedOn" IS 'Date and time the setup description type record was last updated.';

                COMMENT ON TABLE dbo."FgsSetupLaborRateType" IS 'Stores tenant/company specific labor rate types used for pricing, billing, overtime, emergency rates, and other labor configurations. Seeded initially from GloSetupLaborRateType but independently managed by each tenant/company.';
                COMMENT ON COLUMN dbo."FgsSetupLaborRateType"."Id" IS 'Primary key identity of the labor rate type record.';
                COMMENT ON COLUMN dbo."FgsSetupLaborRateType"."TenantId" IS 'Tenant identifier owning this labor rate type.';
                COMMENT ON COLUMN dbo."FgsSetupLaborRateType"."CompanyId" IS 'Company identifier within the tenant owning this labor rate type.';
                COMMENT ON COLUMN dbo."FgsSetupLaborRateType"."Name" IS 'Display name of the labor rate type.';
                COMMENT ON COLUMN dbo."FgsSetupLaborRateType"."Description" IS 'Optional description or notes for the labor rate type.';
                COMMENT ON COLUMN dbo."FgsSetupLaborRateType"."SortOrder" IS 'Controls sorting/display order of labor rate types in dropdowns and setup screens.';
                COMMENT ON COLUMN dbo."FgsSetupLaborRateType"."IsSystem" IS 'Indicates whether the labor rate type was seeded by the system or manually created by the tenant/company.';
                COMMENT ON COLUMN dbo."FgsSetupLaborRateType"."IsActive" IS 'Indicates whether the labor rate type is active and available for use.';
                COMMENT ON COLUMN dbo."FgsSetupLaborRateType"."CreatedOn" IS 'Date and time the labor rate type record was created.';
                COMMENT ON COLUMN dbo."FgsSetupLaborRateType"."CreatedBy" IS 'User identifier that created the labor rate type record.';
                COMMENT ON COLUMN dbo."FgsSetupLaborRateType"."UpdatedOn" IS 'Date and time the labor rate type record was last updated.';
                COMMENT ON COLUMN dbo."FgsSetupLaborRateType"."UpdatedBy" IS 'User identifier that last updated the labor rate type record.';

                COMMENT ON TABLE dbo."GloSetupPaymentTerm" IS 'Stores global payment term master data used to seed tenant/company payment terms for accounts receivable and accounts payable operations.';
                COMMENT ON COLUMN dbo."GloSetupPaymentTerm"."Id" IS 'Primary key identity of the payment term record.';
                COMMENT ON COLUMN dbo."GloSetupPaymentTerm"."Name" IS 'Display name of the payment term.';
                COMMENT ON COLUMN dbo."GloSetupPaymentTerm"."DueDateMethod" IS 'Defines how the due date is calculated such as NetDays, EndOfMonth, DueOnReceipt, or FixedDayOfMonth.';
                COMMENT ON COLUMN dbo."GloSetupPaymentTerm"."NumberOfDays" IS 'Number of days used for due date calculations when applicable.';
                COMMENT ON COLUMN dbo."GloSetupPaymentTerm"."IsAccountsReceivable" IS 'Indicates whether the payment term is available for customer invoicing/accounts receivable.';
                COMMENT ON COLUMN dbo."GloSetupPaymentTerm"."IsAccountsPayable" IS 'Indicates whether the payment term is available for vendor billing/accounts payable.';
                COMMENT ON COLUMN dbo."GloSetupPaymentTerm"."IsMobileVisible" IS 'Indicates whether the payment term is visible in mobile applications.';
                COMMENT ON COLUMN dbo."GloSetupPaymentTerm"."CreatedOn" IS 'Date and time the payment term record was created.';
                COMMENT ON COLUMN dbo."GloSetupPaymentTerm"."CreatedBy" IS 'User identifier that created the payment term record.';
                COMMENT ON COLUMN dbo."GloSetupPaymentTerm"."UpdatedOn" IS 'Date and time the payment term record was last updated.';
                COMMENT ON COLUMN dbo."GloSetupPaymentTerm"."UpdatedBy" IS 'User identifier that last updated the payment term record.';
                COMMENT ON COLUMN dbo."GloSetupPaymentTerm"."IsActive" IS 'Indicates whether the payment term is active and available for use.';

                COMMENT ON TABLE dbo."FgsSetupPaymentMethod" IS 'Stores tenant/company specific payment methods used throughout invoicing, customer payments, vendor payments, mobile applications, and customer portals.';
                COMMENT ON COLUMN dbo."FgsSetupPaymentMethod"."SortOrder" IS 'Controls sorting/display order of payment methods in dropdowns and setup screens.';

                COMMENT ON COLUMN dbo."FgsTenant"."TenantGuid" IS 'Stable UUID identifier for the tenant used in external integrations and cross-service references.';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupGLBreak_FgsFile_LogoFileId",
                schema: "dbo",
                table: "FgsSetupGLBreak");

            migrationBuilder.DropForeignKey(
                name: "FK_FgsSetupGLBreak_FgsLocation_AddressId",
                schema: "dbo",
                table: "FgsSetupGLBreak");

            migrationBuilder.DropForeignKey(
                name: "FK_GloJobTypeSubCategory_GloBusinessType_BusinessTypeId",
                schema: "dbo",
                table: "GloJobTypeSubCategory");

            migrationBuilder.DropTable(
                name: "FgsBillingCategory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsBusinessType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FgsSetupLaborRateType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GloSetupPaymentTerm",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_GloSeedTableMapping_SeedCode",
                schema: "dbo",
                table: "GloSeedTableMapping");

            migrationBuilder.DropIndex(
                name: "UX_GloSeedTableMapping_SeedCode_TargetTableName",
                schema: "dbo",
                table: "GloSeedTableMapping");

            migrationBuilder.DropIndex(
                name: "IX_GloJobTypeSubCategory_BusinessTypeId",
                schema: "dbo",
                table: "GloJobTypeSubCategory");

            migrationBuilder.DropIndex(
                name: "IX_FgsTenant_TenantGuid",
                schema: "dbo",
                table: "FgsTenant");

            migrationBuilder.DropUniqueConstraint(
                name: "UQ_FgsSetupPaymentMethod",
                schema: "dbo",
                table: "FgsSetupPaymentMethod");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupPaymentMethod_TenantId_CompanyId_IsActive",
                schema: "dbo",
                table: "FgsSetupPaymentMethod");

            migrationBuilder.DropUniqueConstraint(
                name: "UQ_FgsSetupGLBreak",
                schema: "dbo",
                table: "FgsSetupGLBreak");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupGLBreak_AddressId",
                schema: "dbo",
                table: "FgsSetupGLBreak");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupGLBreak_LogoFileId",
                schema: "dbo",
                table: "FgsSetupGLBreak");

            migrationBuilder.DropIndex(
                name: "IX_FgsSetupGLBreak_TenantId_CompanyId_BreakLevel",
                schema: "dbo",
                table: "FgsSetupGLBreak");

            migrationBuilder.DropCheckConstraint(
                name: "CK_FgsSetupGLBreak_BreakLevel",
                schema: "dbo",
                table: "FgsSetupGLBreak");

            migrationBuilder.DropColumn(
                name: "BusinessTypeId",
                schema: "dbo",
                table: "GloJobTypeSubCategory");

            migrationBuilder.DropColumn(
                name: "TenantGuid",
                schema: "dbo",
                table: "FgsTenant");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                schema: "dbo",
                table: "FgsSetupPaymentMethod");

            migrationBuilder.DropColumn(
                name: "AddressId",
                schema: "dbo",
                table: "FgsSetupGLBreak");

            migrationBuilder.DropColumn(
                name: "LogoFileId",
                schema: "dbo",
                table: "FgsSetupGLBreak");

            migrationBuilder.DropColumn(
                name: "Trades",
                schema: "dbo",
                table: "FgsSetupGLBreak");

            migrationBuilder.DropTable(
                name: "GloSetupDescriptionType",
                schema: "dbo");

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

            migrationBuilder.AlterColumn<short>(
                name: "FgsTenantStatusId",
                schema: "dbo",
                table: "FgsTenant",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)1)
                .Annotation("Relational:ColumnOrder", 1)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<bool>(
                name: "IsMobileVisible",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsCustomerPortalVisible",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "GloPaymentMethodTypeId",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "BreakLevel",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "integer",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                type: "text",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "UQ_FgsSetupPaymentMethod",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                columns: new[] { "TenantId", "CompanyId", "GloPaymentMethodTypeId" });

            migrationBuilder.AddUniqueConstraint(
                name: "UQ_FgsSetupGLBreak",
                schema: "dbo",
                table: "FgsSetupGLBreak",
                columns: new[] { "TenantId", "CompanyId", "Code" });

            migrationBuilder.CreateTable(
                name: "FgsSetupGLBreakTechTrade",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    FgsSetupGLBreakId = table.Column<long>(type: "bigint", nullable: false),
                    FgsSetupTechTradeId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
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
                name: "GloJobTypeCategorySubCategory",
                schema: "dbo",
                columns: table => new
                {
                    BusinessTypeId = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<short>(type: "smallint", nullable: false),
                    SubCategoryId = table.Column<short>(type: "smallint", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GloJobTypeCategorySubCategory", x => new { x.BusinessTypeId, x.CategoryId, x.SubCategoryId });
                    table.ForeignKey(
                        name: "FK_GloJobTypeCategorySubCategory_GloBusinessType_BusinessTypeId",
                        column: x => x.BusinessTypeId,
                        principalSchema: "dbo",
                        principalTable: "GloBusinessType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GloJobTypeCategorySubCategory_GloJobTypeCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "dbo",
                        principalTable: "GloJobTypeCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GloJobTypeCategorySubCategory_GloJobTypeSubCategory_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalSchema: "dbo",
                        principalTable: "GloJobTypeSubCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "UX_GloSeedTableMapping_SeedCode",
                schema: "dbo",
                table: "GloSeedTableMapping",
                column: "SeedCode",
                unique: true);

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
                name: "IX_GloJobTypeCategorySubCategory_CategoryId",
                schema: "dbo",
                table: "GloJobTypeCategorySubCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_GloJobTypeCategorySubCategory_SubCategoryId",
                schema: "dbo",
                table: "GloJobTypeCategorySubCategory",
                column: "SubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_FgsSetupPaymentMethod_GloPayType",
                schema: "dbo",
                table: "FgsSetupPaymentMethod",
                column: "GloPaymentMethodTypeId",
                principalSchema: "dbo",
                principalTable: "GloPaymentMethodType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
